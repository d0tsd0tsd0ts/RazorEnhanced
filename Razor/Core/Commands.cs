using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Assistant
{
	internal class Commands
	{
		public static void Initialize()
		{
			Command.Register("where", new CommandCallback(Where));
			Command.Register("ping", new CommandCallback(Ping));
			Command.Register("reducecpu", new CommandCallback(ReNice));
			Command.Register("renice", new CommandCallback(ReNice));
			Command.Register("listcommand", new CommandCallback(Command.ListCommands));
			Command.Register("echo", new CommandCallback(Echo));
			Command.Register("getserial", new CommandCallback(GetSerial));
			Command.Register("inspect", new CommandCallback(GetInfo));
		}

		private static void GetSerial(string[] param)
		{
			ClientCommunication.ForceSendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 0x25, 3, Language.CliLocName, "System", "Target a player or item to get their serial number."));
			Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(GetSerialTarget_Callback));
		}

		private static void GetSerialTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			ClientCommunication.ForceSendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 0x25, 3, Language.CliLocName, "System", "Serial: 0x" + serial.Value.ToString("X8")));
		}

		private static void GetInfo(string[] param)
		{
			ClientCommunication.ForceSendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 0x25, 3, Language.CliLocName, "System", "Target a player or item to open object inspect."));
			Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(GetInfoTarget_Callback));
		}

		private static void GetInfoTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Item assistantItem = Assistant.World.FindItem(serial);
			if (assistantItem != null && assistantItem.Serial.IsItem)
			{
				Assistant.Engine.MainWindow.BeginInvoke((MethodInvoker)delegate
				{
					RazorEnhanced.UI.EnhancedItemInspector inspector = new RazorEnhanced.UI.EnhancedItemInspector(assistantItem);
					inspector.TopMost = true;
					inspector.Show();
				});
			}
			else
			{
				Assistant.Mobile assistantMobile = Assistant.World.FindMobile(serial);
				if (assistantMobile != null && assistantMobile.Serial.IsMobile)
				{
					Assistant.Engine.MainWindow.BeginInvoke((MethodInvoker)delegate
					{
						RazorEnhanced.UI.EnhancedMobileInspector inspector = new RazorEnhanced.UI.EnhancedMobileInspector(assistantMobile);
						inspector.TopMost = true;
						inspector.Show();
					});
				}
			}
		}

		private static void Echo(string[] param)
		{
			StringBuilder sb = new StringBuilder("Note To Self: ");
			for (int i = 0; i < param.Length; i++)
				sb.Append(param[i]);
			ClientCommunication.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 0x3B2, 3, Language.CliLocName, "System", sb.ToString()));
		}

		private static void ReNice(string[] param)
		{
			try
			{
				System.Diagnostics.ProcessPriorityClass prio;
				if (param.Length < 1)
					prio = System.Diagnostics.ProcessPriorityClass.BelowNormal;
				else
					prio = (System.Diagnostics.ProcessPriorityClass)Enum.Parse(typeof(System.Diagnostics.ProcessPriorityClass), param[0], true);

				ClientCommunication.ClientProcess.PriorityClass = prio;
				World.Player.SendMessage(MsgLevel.Force, LocString.PrioSet, prio);
			}
			catch (Exception e)
			{
				World.Player.SendMessage(MsgLevel.Force, LocString.PrioSet, String.Format("Error: {0}", e.Message));
			}
		}

		private static void Where(string[] param)
		{
			string mapStr;
			switch (World.Player.Map)
			{
				case 0:
					mapStr = "Felucca";
					break;

				case 1:
					mapStr = "Trammel";
					break;

				case 2:
					mapStr = "Ilshenar";
					break;

				case 3:
					mapStr = "Malas";
					break;

				case 4:
					mapStr = "Tokuno";
					break;

				case 5:
					mapStr = "Ter Mur";
					break;

				case 0x7F:
					mapStr = "Internal";
					break;

				default:
					mapStr = String.Format("Unknown (#{0})", World.Player.Map);
					break;
			}
			World.Player.SendMessage(MsgLevel.Force, LocString.CurLoc, World.Player.Position, mapStr);
		}

		private static void Ping(string[] param)
		{
			int count = 5;
			if (param.Length > 0)
				count = Utility.ToInt32(param[0], 5);
			Assistant.Ping.StartPing(count);
		}
	}

	internal delegate void CommandCallback(string[] param);

	internal class Command
	{
		private static Dictionary<string, CommandCallback> m_List;

		static Command()
		{
			m_List = new Dictionary<string, CommandCallback>();
			PacketHandler.RegisterClientToServerFilter(0xAD, new PacketFilterCallback(OnSpeech));
		}

		internal static void ListCommands(string[] param)
		{
			RazorEnhanced.Misc.SendMessage("Command List:");
			foreach (string cmd in m_List.Keys)
			{
				RazorEnhanced.Misc.SendMessage("-" + cmd);
			}
		}

		internal static void Register(string cmd, CommandCallback callback)
		{
			m_List.Add(cmd, callback);
		}

		internal static CommandCallback FindCommand(string cmd)
		{
			CommandCallback callback;
			m_List.TryGetValue(cmd, out callback);
			return callback;
		}

		internal static void RemoveCommand(string cmd)
		{
			m_List.Remove(cmd);
		}

		internal static Dictionary<string, CommandCallback> List { get { return m_List; } }

		internal static void OnSpeech(Packet pvSrc, PacketHandlerEventArgs args)
		{
			MessageType type = (MessageType)pvSrc.ReadByte();
			ushort hue = pvSrc.ReadUInt16();
			ushort font = pvSrc.ReadUInt16();
			string lang = pvSrc.ReadString(4);
			string text = "";
			List<ushort> keys = null;
			long txtOffset = 0;

			World.Player.SpeechHue = hue;

			if ((type & MessageType.Encoded) != 0)
			{
				int value = pvSrc.ReadInt16();
				int count = (value & 0xFFF0) >> 4;
				keys = new List<ushort>();
				keys.Add((ushort)value);

				for (int i = 0; i < count; ++i)
				{
					if ((i & 1) == 0)
					{
						keys.Add(pvSrc.ReadByte());
					}
					else
					{
						keys.Add(pvSrc.ReadByte());
						keys.Add(pvSrc.ReadByte());
					}
				}

				txtOffset = pvSrc.Position;
				text = pvSrc.ReadUTF8StringSafe();
				type &= ~MessageType.Encoded;
			}
			else
			{
				txtOffset = pvSrc.Position;
				text = pvSrc.ReadUnicodeStringSafe();
			}

			text = text.Trim();

			if (text.Length > 0)
			{
				// Enanched Map Chat
				if (Map.MapNetwork.Connected)
					if (text.StartsWith(RazorEnhanced.Settings.General.ReadString("MapChatPrefixTextBox")))
					{
						string message = text.Replace(RazorEnhanced.Settings.General.ReadString("MapChatPrefixTextBox"), "");
						Map.MapNetworkOut.SendChatMessageQueue.Enqueue(new Map.MapNetworkOut.SendChatMessage(message.Length, RazorEnhanced.Settings.General.ReadInt("MapChatColor"), message));
						args.Block = true;
					}

				if (text[0] == '-')
				{
					text = text.Substring(1).ToLower();
					RazorEnhanced.Misc.SendMessage(text);
					string[] split = text.Split(' ', '\t');
					if (m_List.ContainsKey(split[0]))
					{
						CommandCallback call = (CommandCallback)m_List[split[0]];
						if (call != null)
						{
							string[] param = new String[split.Length - 1];
							for (int i = 0; i < param.Length; i++)
								param[i] = split[i + 1];
							call(param);

							args.Block = true;
						}
					}
				}
			}
		}
	}
}