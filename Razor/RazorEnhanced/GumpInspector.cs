﻿using System;
using System.Collections;
using System.Collections.Generic;
using Assistant;

namespace RazorEnhanced
{
    public class GumpInspector
    {

        internal static void GumpResponseAddLogMain(Assistant.Serial ser, uint tid, int bid)
        {
            if (!Assistant.Engine.MainWindow.GumpInspectorEnable)
            return;

            AddLog("----------- Response Recevied START -----------");
            AddLog("Gump Operation: " + ser.ToString());
            AddLog("Gump ID: " + tid.ToString(""));
            AddLog("Gump Button: " + bid.ToString());
        }

        internal static void GumpResponseAddLogSwitchID(List<int> switchid)
        {
            if (!Assistant.Engine.MainWindow.GumpInspectorEnable)
                return;
            int i = 0;
            foreach (int sid in switchid)
            {
                AddLog("Switch ID: " + i + " Value: +" + sid.ToString());
                i++;
            }
        }

        internal static void GumpResponseAddLogTextID(List<string> texts)
        {
            if (!Assistant.Engine.MainWindow.GumpInspectorEnable)
                return;
            int i = 0;
            foreach (string stext in texts)
            {
                AddLog("Text ID: " + i + " String: +" + stext);
                i++;
            }
        }

        internal static void GumpResponseAddLogEnd()
        {
            if (!Assistant.Engine.MainWindow.GumpInspectorEnable)
                return;
            AddLog("----------- Response Recevied END -----------");
        }


        internal static void GumpCloseAddLog(Packet p, PacketHandlerEventArgs args)
        {
            if (!Assistant.Engine.MainWindow.GumpInspectorEnable)
                return;
            AddLog("----------- Close Recevied START -----------");
            ushort ext = p.ReadUInt16(); // Scarto primo uint
            uint gid = p.ReadUInt32();
            AddLog("Gump ID: " + gid.ToString());
            int bid = p.ReadInt32();
            AddLog("Gump Close Button: " + bid.ToString());
            AddLog("----------- Close Recevied END -----------");
            return;
        }

        internal static void NewGumpStandardAddLog(PacketReader p, PacketHandlerEventArgs args)
        {
            if (!Assistant.Engine.MainWindow.GumpInspectorEnable)
                return;

            AddLog("----------- New Recevied START -----------");
            
            Assistant.Serial ser = p.ReadUInt32();
            AddLog("Gump Operation: " + ser.ToString());

            uint gid = p.ReadUInt32();
            AddLog("Gump ID: " + gid.ToString());

            AddLog("----------- New Recevied END -----------");
            return;
        }

        internal static void NewGumpCompressedAddLog(uint GumpS, uint GumpI, List<string> stringlist)
        {
            if (!Assistant.Engine.MainWindow.GumpInspectorEnable)
                return;

            AddLog("----------- New Recevied START -----------");

            AddLog("Gump Operation: " + GumpS.ToString());
            AddLog("Gump ID: " + GumpI.ToString());

            foreach (string text in stringlist)
                AddLog("Gump Text Data: " + text);

            AddLog("----------- New Recevied END -----------");
            return;
        }
        internal static void AddLog(string addlog)
        {
            RazorEnhanced.UI.EnhancedGumpInspector.EnhancedGumpInspectorListBox.Invoke(new Action(() => RazorEnhanced.UI.EnhancedGumpInspector.EnhancedGumpInspectorListBox.Items.Add(addlog)));           
        }
    }
}