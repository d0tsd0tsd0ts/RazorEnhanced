﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using ScintillaNET;
using Assistant;
using RazorEnhanced;

using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using IronPython.Runtime.Exceptions;
using IronPython.Runtime;


namespace RazorEnhanced.UI
{
	internal partial class EnhancedScriptEditor : Form
	{
		delegate void SetHighlightLineDelegate(bool highlight, int linenum, Color color);
		delegate void SetStatusLabelDelegate(string text);
		delegate string GetScintillaTextDelegate();
		delegate void SetTracebackDelegate(string text);

		private enum Command
		{
			None = 0,
			Line,
			Call,
			Return
		}

		private static Thread m_Thread;
		private static EnhancedScriptEditor m_EnhancedScriptEditor;
		private static ConcurrentQueue<Command> m_Queue = new ConcurrentQueue<Command>();
		private static Command m_CurrentCommand = Command.None;
		private static AutoResetEvent m_DebugContinue = new AutoResetEvent(false);

		private const string m_Title = "Enhanced Script Editor";
		private string m_Filename = "";

		private ScriptEngine m_Engine;
		private ScriptSource m_Source;
		private ScriptScope m_Scope;

		private TraceBackFrame m_CurrentFrame;
		private FunctionCode m_CurrentCode;
		private string m_CurrentResult;
		private object m_CurrentPayload;

		private volatile bool m_Breaktrace = false;

		internal static void Init()
		{
			ScriptEngine engine = Python.CreateEngine();
			m_EnhancedScriptEditor = new EnhancedScriptEditor(engine);
			m_EnhancedScriptEditor.Show();
		}

		internal static void End()
		{
			if (m_EnhancedScriptEditor != null)
			{
				m_EnhancedScriptEditor.Stop();
				m_EnhancedScriptEditor.Close();
				m_EnhancedScriptEditor.Dispose();
			}
		}

		internal EnhancedScriptEditor(ScriptEngine engine)
		{
			InitializeComponent();

			this.Text = m_Title;
			this.m_Engine = engine;
			this.m_Engine.SetTrace(null);
		}

		private TracebackDelegate OnTraceback(TraceBackFrame frame, string result, object payload)
		{
			if (m_Breaktrace)
			{
				CheckCurrentCommand();

				if (m_CurrentCommand == Command.None)
				{
					SetTraceback("");
					ResetCurrentCommand();
				}
				else if (result == "call" && m_CurrentCommand == Command.Call)
				{
					UpdateCurrentState(frame, result, payload);
					TracebackCall();
					ResetCurrentCommand();
				}
				else if (result == "line" && m_CurrentCommand == Command.Line)
				{
					UpdateCurrentState(frame, result, payload);
					TracebackLine();
					ResetCurrentCommand();
				}

				else if (result == "return" && m_CurrentCommand == Command.Return)
				{
					UpdateCurrentState(frame, result, payload);
					TracebackReturn();
					ResetCurrentCommand();
				}

				return OnTraceback;
			}
			else
				return null;
		}

		private void TracebackCall()
		{
			SetStatusLabel("DEBUGGER ACTIVE - " + string.Format("Call {0}", m_CurrentCode.co_name));
			SetHighlightLine(true, (int)m_CurrentFrame.f_lineno, Color.LightGreen);
			string locals = GetLocalsText(m_CurrentFrame);
			SetTraceback(locals);
		}

		private void TracebackReturn()
		{
			SetStatusLabel("DEBUGGER ACTIVE - " + string.Format("Return {0}", m_CurrentCode.co_name));
			SetHighlightLine(true, m_CurrentCode.co_firstlineno, Color.LightPink);
			string locals = GetLocalsText(m_CurrentFrame);
			SetTraceback(locals);
		}

		private void TracebackLine()
		{
			SetStatusLabel("DEBUGGER ACTIVE - " + string.Format("Line {0}", m_CurrentFrame.f_lineno));
			SetHighlightLine(true, (int)m_CurrentFrame.f_lineno, Color.Yellow);
			string locals = GetLocalsText(m_CurrentFrame);
			SetTraceback(locals);
		}

		private void EnqueueCommand(Command command)
		{
			m_Queue.Enqueue(command);
			m_DebugContinue.Set();
		}

		private bool CheckCurrentCommand()
		{
			bool result = m_Queue.TryDequeue(out m_CurrentCommand);
			return result;
		}

		private void UpdateCurrentState(TraceBackFrame frame, string result, object payload)
		{
			m_CurrentFrame = frame;
			m_CurrentCode = frame.f_code;
			m_CurrentResult = result;
			m_CurrentPayload = payload;
		}

		private void ResetCurrentCommand()
		{
			m_CurrentCommand = Command.None;
			m_DebugContinue.WaitOne();
		}

		private void Start(bool debug)
		{
			if (m_Thread == null ||
					(m_Thread != null && m_Thread.ThreadState != ThreadState.Running &&
					m_Thread.ThreadState != ThreadState.Unstarted &&
					m_Thread.ThreadState != ThreadState.WaitSleepJoin)
				)
			{
				m_Thread = new Thread(() => AsyncStart(debug));
				m_Thread.Start();
			}
		}

		private void AsyncStart(bool debug)
		{
			if (debug)
				SetStatusLabel("DEBUGGER ACTIVE");
			else
				SetStatusLabel("");

			try
			{
				m_Breaktrace = debug;
				string text = GetScintillaText();
				m_Source = m_Engine.CreateScriptSourceFromString(text);
				m_Scope = RazorEnhanced.Scripts.GetRazorScope(m_Engine);
				m_Engine.SetTrace(m_EnhancedScriptEditor.OnTraceback);
				m_Source.Execute(m_Scope);
			}
			catch (Exception ex)
			{
				if (m_Thread != null)
					m_Thread.Abort();
			}
		}

		private void Stop()
		{
			m_Breaktrace = false;
			m_DebugContinue.Set();
			SetHighlightLine(false, 1, Color.White);
			SetStatusLabel("");

			if (m_Thread != null && m_Thread.ThreadState != ThreadState.Stopped)
			{
				m_Thread.Abort();
				m_Thread = null;
			}
		}

		private void SetHighlightLine(bool highlight, int linenum, Color background)
		{
			if (this.scintillaEditor.InvokeRequired)
			{
				SetHighlightLineDelegate d = new SetHighlightLineDelegate(SetHighlightLine);
				this.Invoke(d, new object[] { highlight, linenum, background });
			}
			else
			{
				this.scintillaEditor.Caret.HighlightCurrentLine = highlight;

				if (highlight)
				{
					this.scintillaEditor.GoTo.Line(linenum - 1);
					this.scintillaEditor.Caret.CurrentLineBackgroundColor = background;
					this.scintillaEditor.Focus();
				}
			}
		}

		private void SetStatusLabel(string text)
		{
			if (this.InvokeRequired)
			{
				SetStatusLabelDelegate d = new SetStatusLabelDelegate(SetStatusLabel);
				this.Invoke(d, new object[] { text });
			}
			else
			{
				this.toolStripStatusLabelScript.Text = text;
			}
		}

		private string GetScintillaText()
		{

			if (this.scintillaEditor.InvokeRequired)
			{
				GetScintillaTextDelegate d = new GetScintillaTextDelegate(GetScintillaText);
				return (string)this.Invoke(d, null);
			}
			else
			{
				return scintillaEditor.Text;
			}
		}

		private string GetLocalsText(TraceBackFrame frame)
		{
			string result = "";

			PythonDictionary locals = frame.f_locals as PythonDictionary;
			if (locals != null)
			{
				foreach (KeyValuePair<object, object> pair in locals)
				{
					if (!(pair.Key.ToString().StartsWith("__") && pair.Key.ToString().EndsWith("__")))
					{
						string line = pair.Key.ToString() + ": " + (pair.Value != null ? pair.Value.ToString() : "") + "\n";
						result += line;
					}
				}
			}

			return result;
		}

		private void SetTraceback(string text)
		{
			if (this.textBoxDebug.InvokeRequired)
			{
				SetTracebackDelegate d = new SetTracebackDelegate(SetTraceback);
				this.Invoke(d, new object[] { text });
			}
			else
			{
				this.textBoxDebug.Text = text;
			}
		}

		private void EnhancedScriptEditor_Load(object sender, EventArgs e)
		{
			scintillaEditor.Margins[0].Width = 20;
		}

		private void scintillaEditor_TextChanged(object sender, EventArgs e)
		{
			Stop();
		}

		private void EnhancedScriptEditor_FormClosing(object sender, FormClosingEventArgs e)
		{
			Stop();
		}

		private void toolStripButtonPlay_Click(object sender, EventArgs e)
		{
			Start(false);
		}

		private void toolStripButtonDebug_Click(object sender, EventArgs e)
		{
			Start(true);
		}

		private void toolStripNextCall_Click(object sender, EventArgs e)
		{
			EnqueueCommand(Command.Call);
		}

		private void toolStripButtonNextLine_Click(object sender, EventArgs e)
		{
			EnqueueCommand(Command.Line);
		}

		private void toolStripButtonNextReturn_Click(object sender, EventArgs e)
		{
			EnqueueCommand(Command.Return);
		}

		private void toolStripButtonStop_Click(object sender, EventArgs e)
		{
			Stop();
		}

		private void toolStripButtonAddBreakpoint_Click(object sender, EventArgs e)
		{
		}

		private void toolStripButtonRemoveBreakpoints_Click(object sender, EventArgs e)
		{
		}

		private void toolStripButtonOpen_Click(object sender, EventArgs e)
		{
			OpenFileDialog open = new OpenFileDialog();
			open.Filter = "Script Files|*.py";

			if (open.ShowDialog() == DialogResult.OK)
			{
				m_Filename = Path.GetFileNameWithoutExtension(open.FileName);
				this.Text = m_Title + " - " + m_Filename + ".cs";
				scintillaEditor.Text = System.IO.File.ReadAllText(open.FileName);
			}
		}

		private void toolStripButtonSave_Click(object sender, EventArgs e)
		{
			SaveFileDialog save = new SaveFileDialog();
			save.Filter = "Script Files|*.py";

			if (save.ShowDialog() == DialogResult.OK)
			{
				m_Filename = Path.GetFileNameWithoutExtension(save.FileName);
				this.Text = m_Title + " - " + m_Filename + ".cs";
				System.IO.File.WriteAllText(save.FileName, scintillaEditor.Text);
			}
		}

		private void toolStripButtonClose_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Save current file?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
			{
				SaveFileDialog save = new SaveFileDialog();
				save.Filter = "Script Files|*.py";

				if (save.ShowDialog() == DialogResult.OK)
				{
					System.IO.File.WriteAllText(save.FileName, scintillaEditor.Text);
				}
			}

			scintillaEditor.Text = "";
			m_Filename = "";
			this.Text = m_Title;
		}

		private void toolStripButtonInspect_Click(object sender, EventArgs e)
		{
			Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(InspectItemTarget_Callback));
		}

		private void InspectItemTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{

			Assistant.Item assistantItem = Assistant.World.FindItem(serial);
			if (assistantItem != null && assistantItem.Serial.IsItem)
			{
				EnhancedItemInspector inspector = new EnhancedItemInspector(assistantItem);
				inspector.TopMost = true;
				inspector.Show();
			}

			else
			{
				Assistant.Mobile assistantMobile = Assistant.World.FindMobile(serial);
				if (assistantMobile != null && assistantMobile.Serial.IsMobile)
				{
					EnhancedMobileInspector inspector = new EnhancedMobileInspector(assistantMobile);
					inspector.TopMost = true;
					inspector.Show();
				}
			}
		}
	}
}
