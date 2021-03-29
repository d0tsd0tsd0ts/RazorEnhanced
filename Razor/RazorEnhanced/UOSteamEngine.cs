﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IronPython.Runtime;
using IronPython.Hosting;
using IronPython.Runtime.Exceptions;
using Microsoft.Scripting.Hosting;
using IronPython.Compiler;

namespace RazorEnhanced
{
    //
    class UOSteamEngine
    {
        private int m_lastMount;
        private int m_toggle_LeftSave;
        private int m_toggle_RightSave;
        // useOnceIgnoreList
        private List<int> m_serialUseOnceIgnoreList;


        private static UOSteamEngine instance = null;
        public static UOSteamEngine Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UOSteamEngine();
                    instance.RegisterCommands();
                }
                return instance;
            }
        }

        private UOSteamEngine()
        {
            m_serialUseOnceIgnoreList = new List<int>();
            UOScript.Interpreter.SetAlias("backpack", (uint)Player.Backpack.Serial);
            UOScript.Interpreter.SetAlias("self", (uint)Player.Serial);
            UOScript.Interpreter.SetAlias("found", 0);
            UOScript.Interpreter.SetAlias("enemy", 0);
            UOScript.Interpreter.SetAlias("bank", 0);
            UOScript.Interpreter.SetAlias("friend", 0);
            UOScript.Interpreter.RegisterAliasHandler("ground", AliasHandler);
            UOScript.Interpreter.RegisterAliasHandler("any", AliasHandler);
            UOScript.Interpreter.SetAlias("last", 0);
            UOScript.Interpreter.SetAlias("lasttarget", 0);
            UOScript.Interpreter.SetAlias("lastobject", 0);
            UOScript.Interpreter.SetAlias("lefthand", 0);
            UOScript.Interpreter.SetAlias("mount", 0);
            UOScript.Interpreter.SetAlias("righthand", 0);

            m_toggle_LeftSave = 0;
            m_toggle_RightSave = 0;
            m_lastMount = 0;
        }

        uint AliasHandler(string alias)
        {
            int everywhere = -1;
            if (0 == String.Compare(alias, "ground", true))
                return (uint)everywhere;
            if (0 == String.Compare(alias, "any", true))
                return (uint)everywhere;


            return 0;
        }

        public void Execute(string filename)
        {
            if (filename == null) return;
            var root = Lexer.Lex(filename);
            UOScript.Script script = new UOScript.Script(root);
            UOScript.Interpreter.StartScript(script);
            try
            {
                while (UOScript.Interpreter.ExecuteScript()) { };
            }
            catch (Exception e)
            {
                UOScript.Interpreter.StopScript();
                throw;
            }

        }
        public void Execute(string[] textLines)
        {
            var root = Lexer.Lex(textLines);
            UOScript.Script script = new UOScript.Script(root);
            UOScript.Interpreter.StartScript(script);
            try
            {
                while (UOScript.Interpreter.ExecuteScript()) { };
            }
            catch (Exception e)
            {
                UOScript.Interpreter.StopScript();
                throw;
            }
        }

        // Abstract Placeholders

        public void RegisterCommands()
        {
            // Commands. From UOSteam Documentation
            UOScript.Interpreter.RegisterCommandHandler("fly", this.FlyCommand);
            UOScript.Interpreter.RegisterCommandHandler("land", this.LandCommand);
            UOScript.Interpreter.RegisterCommandHandler("setability", this.SetAbility);
            UOScript.Interpreter.RegisterCommandHandler("attack", this.Attack);
            UOScript.Interpreter.RegisterCommandHandler("clearhands", this.ClearHands);
            UOScript.Interpreter.RegisterCommandHandler("clickobject", this.ClickObject);
            UOScript.Interpreter.RegisterCommandHandler("bandageself", this.BandageSelf);
            UOScript.Interpreter.RegisterCommandHandler("usetype", this.UseType);
            UOScript.Interpreter.RegisterCommandHandler("useobject", this.UseObject);
            UOScript.Interpreter.RegisterCommandHandler("useonce", this.UseOnce);
            UOScript.Interpreter.RegisterCommandHandler("cleanusequeue", this.CleanUseQueue);
            UOScript.Interpreter.RegisterCommandHandler("moveitem", this.MoveItem);
            UOScript.Interpreter.RegisterCommandHandler("moveitemoffset", this.MoveItemOffset);
            UOScript.Interpreter.RegisterCommandHandler("movetype", this.MoveType);
            UOScript.Interpreter.RegisterCommandHandler("movetypeoffset", this.MoveTypeOffset);
            UOScript.Interpreter.RegisterCommandHandler("walk", this.Walk);
            UOScript.Interpreter.RegisterCommandHandler("turn", this.Turn);
            UOScript.Interpreter.RegisterCommandHandler("run", this.Walk); // I dunno how to make him run
            UOScript.Interpreter.RegisterCommandHandler("useskill", this.UseSkill);
            UOScript.Interpreter.RegisterCommandHandler("feed", this.Feed);
            UOScript.Interpreter.RegisterCommandHandler("rename", this.RenamePet);
            UOScript.Interpreter.RegisterCommandHandler("shownames", this.ShowNames);
            UOScript.Interpreter.RegisterCommandHandler("togglehands", this.ToggleHands);
            UOScript.Interpreter.RegisterCommandHandler("equipitem", this.EquipItem); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("togglemounted", this.ToggleMounted); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("equipwand", this.EquipWand); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("buy", this.Buy); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("sell", this.Sell); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("clearbuy", this.ClearBuy); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("clearsell", this.ClearSell); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("organizer", this.Organizer); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("autoloot", this.Autoloot); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("autotargetobject", this.AutoTargetObject);
            UOScript.Interpreter.RegisterCommandHandler("dress", this.Dress); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("undress", this.Undress); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("dressconfig", this.DressConfig); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("toggleautoloot", this.ToggleAutoloot); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("togglescavenger", this.ToggleScavenger); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("counter", this.Counter); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("unsetalias", this.UnSetAlias);
            UOScript.Interpreter.RegisterCommandHandler("setalias", this.SetAlias);
            UOScript.Interpreter.RegisterCommandHandler("promptalias", this.PromptAlias);
            UOScript.Interpreter.RegisterCommandHandler("waitforgump", this.WaitForGump);
            UOScript.Interpreter.RegisterCommandHandler("replygump", this.ReplyGump);
            UOScript.Interpreter.RegisterCommandHandler("closegump", this.CloseGump); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("clearjournal", this.ClearJournal); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("waitforjournal", this.WaitForJournal); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("poplist", this.PopList); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("pushlist", this.PushList);
            UOScript.Interpreter.RegisterCommandHandler("removelist", this.RemoveList); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("createlist", this.CreateList);
            UOScript.Interpreter.RegisterCommandHandler("clearlist", this.ClearList); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("info", this.Info);
            UOScript.Interpreter.RegisterCommandHandler("pause", this.Pause);
            UOScript.Interpreter.RegisterCommandHandler("ping", this.Ping); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("playmacro", this.PlayMacro); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("playsound", this.PlaySound); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("resync", this.Resync); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("snapshot", this.Snapshot); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("hotkeys", this.Hotkeys); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("where", this.Where); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("messagebox", this.MessageBox); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("mapuo", this.MapUO); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("clickscreen", this.ClickScreen); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("paperdoll", this.Paperdoll); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("helpbutton", this.HelpButton); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("guildbutton", this.GuildButton); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("questsbutton", this.QuestsButton); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("logoutbutton", this.LogoutButton); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("virtue", this.Virtue); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("msg", this.MsgCommand);
            UOScript.Interpreter.RegisterCommandHandler("headmsg", this.HeadMsg);
            UOScript.Interpreter.RegisterCommandHandler("partymsg", this.PartyMsg);
            UOScript.Interpreter.RegisterCommandHandler("guildmsg", this.GuildMsg); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("allymsg", this.AllyMsg); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("whispermsg", this.WhisperMsg); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("yellmsg", this.YellMsg); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("sysmsg", this.SysMsg); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("chatmsg", this.ChatMsg); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("emotemsg", this.EmoteMsg); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("promptmsg", this.PromptMsg); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("timermsg", this.TimerMsg); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("waitforprompt", this.WaitForPrompt); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("cancelprompt", this.CancelPrompt); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("addfriend", this.AddFriend); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("removefriend", this.RemoveFriend); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("contextmenu", this.ContextMenu); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("waitforcontext", this.WaitForContext); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("ignoreobject", this.IgnoreObject); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("clearignorelist", this.ClearIgnoreList); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("setskill", this.SetSkill); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("waitforproperties", this.WaitForProperties); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("autocolorpick", this.AutoColorPick); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("waitforcontents", this.WaitForContents); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("miniheal", this.MiniHeal); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("bigheal", this.BigHeal); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("cast", this.Cast); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("chivalryheal", this.ChivalryHeal); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("waitfortarget", this.WaitForTarget); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("canceltarget", this.CancelTarget); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("cancelautotarget", this.CancelAutoTarget); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("target", this.Target); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("targettype", this.TargetType); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("targetground", this.TargetGround); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("targettile", this.TargetTile); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("targettileoffset", this.TargetTileOffset); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("targettilerelative", this.TargetTileRelative); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("cleartargetqueue", this.ClearTargetQueue); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("settimer", this.SetTimer); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("removetimer", this.RemoveTimer); //TODO: This method is a stub. Remove after successful testing.
            UOScript.Interpreter.RegisterCommandHandler("createtimer", this.CreateTimer); //TODO: This method is a stub. Remove after successful testing.


            // Expressions
            UOScript.Interpreter.RegisterExpressionHandler("findalias", FindAlias);
            UOScript.Interpreter.RegisterExpressionHandler("contents", DummyExpression);
            UOScript.Interpreter.RegisterExpressionHandler("inregion", DummyExpression);
            UOScript.Interpreter.RegisterExpressionHandler("skill", DummyExpression);
            UOScript.Interpreter.RegisterExpressionHandler("findobject", DummyExpression);
            UOScript.Interpreter.RegisterExpressionHandler("useobject", this.UseObjExp);
            UOScript.Interpreter.RegisterExpressionHandler("distance", DummyExpression);
            UOScript.Interpreter.RegisterExpressionHandler("inrange", DummyExpression);
            UOScript.Interpreter.RegisterExpressionHandler("buffexists", DummyExpression);
            UOScript.Interpreter.RegisterExpressionHandler("property", Property);
            UOScript.Interpreter.RegisterExpressionHandler("findtype", FindType);
            UOScript.Interpreter.RegisterExpressionHandler("findlayer", DummyExpression);
            UOScript.Interpreter.RegisterExpressionHandler("skillstate", DummyExpression);
            UOScript.Interpreter.RegisterExpressionHandler("counttype", CountType);
            UOScript.Interpreter.RegisterExpressionHandler("counttypeground", DummyExpression);
            UOScript.Interpreter.RegisterExpressionHandler("findwand", DummyExpression);
            UOScript.Interpreter.RegisterExpressionHandler("inparty", DummyExpression);
            UOScript.Interpreter.RegisterExpressionHandler("infriendslist", DummyExpression);
            UOScript.Interpreter.RegisterExpressionHandler("war", DummyExpression);
            UOScript.Interpreter.RegisterExpressionHandler("ingump", DummyExpression);
            UOScript.Interpreter.RegisterExpressionHandler("gumpexists", GumpExists);
            UOScript.Interpreter.RegisterExpressionHandler("injournal", DummyExpression);
            UOScript.Interpreter.RegisterExpressionHandler("listexists", DummyExpression);
            UOScript.Interpreter.RegisterExpressionHandler("list", DummyExpression);
            UOScript.Interpreter.RegisterExpressionHandler("inlist", DummyExpression);
            UOScript.Interpreter.RegisterExpressionHandler("timer", DummyExpression);
            UOScript.Interpreter.RegisterExpressionHandler("timerexists", DummyExpression);

            // Player Attributes
            UOScript.Interpreter.RegisterExpressionHandler("mana", DummyExpression);
            UOScript.Interpreter.RegisterExpressionHandler("x", DummyIntExpression);
            UOScript.Interpreter.RegisterExpressionHandler("y", DummyExpression);
            UOScript.Interpreter.RegisterExpressionHandler("z", DummyExpression);
            UOScript.Interpreter.RegisterExpressionHandler("name", DummyStringExpression);

            // Object attributes
        }
        private IComparable CountType(string expression, UOScript.Argument[] args, bool quiet)
        {

            if (args.Length == 3)
            {
                int graphic = args[0].AsInt();
                int color = args[1].AsInt();
                uint container = args[2].AsSerial();
                int count = Items.ContainerCount((int)container, graphic, color);
                return count;
            }

            return false;
        }


        private IComparable UseObjExp(string expression, UOScript.Argument[] args, bool quiet)
        {

            UseObject(expression, args, quiet, false);
            return true;
        }


        IComparable FindAlias(string expression, UOScript.Argument[] args, bool quiet)
        {

            if (args.Length == 1)
            {
                string alias = args[0].AsString();
                return UOScript.Interpreter.FindAlias(alias);
            }

            return false;
        }

        private static IComparable FindType(string expression, UOScript.Argument[] args, bool quiet)
        {
            Item item = null;
            if (args.Length == 1)
            {
                int type = args[0].AsInt();
                item = Items.FindByID(type, -1, -1);
            }
            if (args.Length == 2)
            {
                int type = args[0].AsInt();
                int color = args[1].AsInt();
                item = Items.FindByID(type, color, -1);
            }
            if (args.Length == 3)
            {
                int type = args[0].AsInt();
                int color = args[1].AsInt();
                uint source = args[2].AsSerial();
                item = Items.FindByID(type, color, (int)source);
            }
            if (args.Length == 4)
            {
                int type = args[0].AsInt();
                int color = args[1].AsInt();
                uint source = args[2].AsSerial();
                int amount = args[3].AsInt();
                item = Items.FindByID(type, color, (int)source);
                if (item != null)
                {
                    if (amount != -1 && item.Amount != amount)
                    {
                        item = null;
                    }
                }
            }
            if (args.Length == 5)
            {
                int type = args[0].AsInt();
                int color = args[1].AsInt();
                uint source = args[2].AsSerial();
                int amount = args[3].AsInt();
                int range = args[4].AsInt();
                item = Items.FindByID(type, color, (int)source, range);
                if (item != null)
                {
                    if (item.Amount != amount)
                    {
                        item = null;
                    }
                }
            }

            if (item == null)
            {
                UOScript.Interpreter.UnSetAlias("found");
                return false;
            }

            UOScript.Interpreter.SetAlias("found", (uint)item.Serial);
            return true;

        }

        private static IComparable Property(string expression, UOScript.Argument[] args, bool quiet)
        {
            if (args.Length == 2)
            {
                string findProp = args[0].AsString();
                uint serial = args[1].AsSerial();
                Item item = Items.FindBySerial((int)serial);
                if (item != null)
                {
                    List<String> props = Items.GetPropStringList((int)serial);
                    foreach (String prop in props)
                    {
                        if (0 == String.Compare(findProp, prop, true))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private static IComparable GumpExists(string expression, UOScript.Argument[] args, bool quiet)
        {
            if (args.Length == 0)
            {
                return Gumps.HasGump();
            }

            if (args.Length == 1)
            {
                int gumpid = args[0].AsInt();
                return (gumpid - Gumps.CurrentGump());
            }
            return -1;
        }


        private static IComparable DummyExpression(string expression, UOScript.Argument[] args, bool quiet)
        {
            Console.WriteLine("Executing expression {0} {1}", expression, args);

            return 0;
        }

        private static int DummyIntExpression(string expression, UOScript.Argument[] args, bool quiet)
        {
            Console.WriteLine("Executing expression {0} {1}", expression, args);

            return 3;
        }

        private static string DummyStringExpression(string expression, UOScript.Argument[] args, bool quiet)
        {
            Console.WriteLine("Executing expression {0} {1}", expression, args);

            return "test";
        }

        private bool DummyCommand(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            Console.WriteLine("UOS: DummyCommand {0} {1}", command, args);
            return true;
        }

        private bool NotImplemented(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            Console.WriteLine("UOS: NotImplemented {0} {1}", command, args);
            return true;
        }


        // Commands: Stable

        private bool LandCommand(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            Player.Fly(false);
            return true;
        }

        private bool FlyCommand(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            Player.Fly(true);
            return true;
        }

        private bool Pause(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            int delay = args[0].AsInt();
            Misc.Pause(delay);
            return true;
        }

        private bool Info(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            Assistant.Targeting.OneTimeTarget(true, new Assistant.Targeting.TargetResponseCallback(Assistant.Commands.GetInfoTarget_Callback));
            return true;
        }

        private bool SetAbility(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 2)
            {
                Misc.SendMessage("set ability not proper syntax");
                return true;
            }
            string ability = args[0].AsString().ToLower();
            bool on = args[1].AsBool();

            switch (ability)
            {
                case "primary":
                    if (on)
                    {
                        Player.WeaponPrimarySA();
                    }
                    else
                    {
                        // I dunno how to turn off
                        Player.WeaponPrimarySA();
                    }
                    break;
                case "secondary":
                    if (on)
                    {
                        Player.WeaponSecondarySA();
                    }
                    else
                    {
                        // I dunno how to turn off
                        Player.WeaponSecondarySA();
                    }
                    break;
                case "stun":
                    if (on)
                    {
                        Player.WeaponStunSA();
                    }
                    else
                    {
                        // I dunno how to turn off
                        Player.WeaponStunSA();
                    }
                    break;
                case "disarm":
                    if (on)
                    {
                        Player.WeaponDisarmSA();
                    }
                    else
                    {
                        // I dunno how to turn off
                        Player.WeaponDisarmSA();
                    }
                    break;
                default:
                    return true;
            }

            return true;
        }
        private bool Attack(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 1)
            {
                Player.SetWarMode(true);
            }
            else
            {
                uint serial = args[0].AsSerial();
                Mobile mobile = Mobiles.FindBySerial((int)serial);
                if (mobile != null)
                    Player.Attack(mobile);
            }

            return true;
        }

        private bool Walk(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0)
                Player.Walk(Player.Direction);

            if (args.Length == 1)
            {
                string direction = args[0].AsString();
                Player.Walk(direction);
            }

            return true;
        }

        private bool Turn(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 1)
            {
                string direction = args[0].AsString();
                if (Player.Direction != direction)
                    Player.Walk(direction);
            }

            return true;
        }


        private bool ClearHands(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0 || args[0].AsString().ToLower() == "both")
            {
                Player.UnEquipItemByLayer("RightHand", false);
                Player.UnEquipItemByLayer("LeftHand", false);
            }
            if (args.Length == 1)
            {
                if (args[0].AsString().ToLower() == "right")
                    Player.UnEquipItemByLayer("RightHand", false);
                if (args[0].AsString().ToLower() == "left")
                    Player.UnEquipItemByLayer("LeftHand", false);
            }


            return true;
        }
        private bool ClickObject(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 1)
            {
                int serial = (int)args[0].AsSerial();
                Items.SingleClick(serial);
            }

            return true;
        }

        private bool BandageSelf(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            BandageHeal.Heal(Assistant.World.Player);
            return true;
        }

        private bool UseType(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0)
            {
                Misc.SendMessage("Insufficient parameters");
                return true;
            }
            int itemID = args[0].AsInt();
            int color = -1;
            int container = -1;
            if (args.Length > 1)
            {
                color = args[1].AsInt();
            }
            if (args.Length > 2)
            {
                container = args[2].AsInt();
            }

            Item item = Items.FindByID(itemID, color, container);
            if (item != null)
            {
                Items.UseItem(item.Serial);
            }

            return true;
        }

        private bool UseObject(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0)
            {
                Misc.SendMessage("Insufficient parameters");
                return true;
            }
            int serial = (int)args[0].AsSerial();
            Items.UseItem(serial);

            return true;
        }

        private bool UseOnce(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            // This is a bit problematic
            // UOSteam highlights the selected item red in your backpack, and it searches recursively to find the item id
            // Current logic for us, only searches 1 deep .. maybe thats enough for now
            if (args.Length == 0)
            {
                Misc.SendMessage("Insufficient parameters");
                return true;
            }

            int itemID = args[0].AsInt();
            int color = -1;
            if (args.Length > 1)
            {
                color = args[1].AsInt();
            }
            List<Item> items = Player.Backpack.Contains;
            Item selectedItem = null;

            foreach (Item item in items)
            {
                if (item.ItemID == itemID && (!m_serialUseOnceIgnoreList.Contains(item.Serial)))
                    selectedItem = item;
            }

            if (selectedItem != null)
            {
                m_serialUseOnceIgnoreList.Add(selectedItem.Serial);
                Items.UseItem(selectedItem.Serial);
            }

            return true;
        }

        private bool CleanUseQueue(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            m_serialUseOnceIgnoreList.Clear();
            return true;
        }


        private bool MoveItem(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 2)
            {
                Misc.SendMessage("Insufficient parameters");
                return true;
            }
            int source = (int)args[0].AsSerial();
            int dest = (int)args[1].AsSerial();
            int x = -1;
            int y = -1;
            int z = -1;
            int amount = -1;

            if (args.Length == 3)
            {
                amount = args[2].AsInt();
            }

            if (args.Length > 5)
            {
                x = args[2].AsInt();
                y = args[3].AsInt();
                z = args[4].AsInt();
                amount = args[5].AsInt();
            }
            Items.Move(source, dest, amount, x, y);

            return true;
        }
        private bool UseSkill(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            if (args.Length != 1)
            {
                Misc.SendMessage("Insufficient parameters");
                return true;
            }
            string skill = args[0].AsString();

            Player.UseSkill(skill);

            return true;
        }

        // Feed doesn't support food groups etc unless someone adds it
        private bool Feed(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            if (args.Length < 2)
            {
                Misc.SendMessage("Insufficient parameters");
                return false;
            }
            int target = (int)args[0].AsSerial();
            int graphic = args[1].AsInt();
            int color = -1;
            int amount = 1;

            if (args.Length > 2)
            {
                color = args[2].AsInt();
            }
            if (args.Length > 3)
            {
                amount = args[1].AsInt();
            }
            Item food = Items.FindByID(graphic, color, Player.Backpack.Serial);
            if (food != null)
            {
                if (target == Player.Serial)
                    Items.UseItem(food.Serial);
                else
                    Items.Move(food, target, amount);
            }
            return true;
        }

        private bool RenamePet(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            if (args.Length != 2)
            {
                Misc.SendMessage("Incorrect parameters");
                return true;
            }
            int serial = (int)args[0].AsSerial();
            string newName = args[1].AsString();

            Misc.PetRename(serial, newName);

            return true;
        }

        private bool ToggleHands(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 1)
            {
                string hand = args[0].AsString().ToLower();
                if (hand == "left")
                {
                    Item left = Player.GetItemOnLayer("LeftHand");
                    if (left == null)
                    {
                        if (m_toggle_LeftSave != 0)
                        {
                            Player.EquipItem(m_toggle_LeftSave);
                        }
                    }
                    else
                    {
                        m_toggle_LeftSave = left.Serial;
                        Player.UnEquipItemByLayer("LeftHand", false);
                    }
                }
                if (hand == "right")
                {
                    Item right = Player.GetItemOnLayer("RightHand");
                    if (right == null)
                    {
                        if (m_toggle_RightSave != 0)
                        {
                            Player.EquipItem(m_toggle_RightSave);
                        }
                    }
                    else
                    {
                        m_toggle_RightSave = right.Serial;
                        Player.UnEquipItemByLayer("RightHand", false);
                    }
                }
            }
            return true;
        }

        private bool UnSetAlias(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 1)
            {
                string alias = args[0].AsString();
                UOScript.Interpreter.UnSetAlias(alias);
            }

            return true;
        }

        private bool SetAlias(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 1)
            {
                return PromptAlias(command, args, quiet, force);
            }
            if (args.Length == 2)
            {
                string alias = args[0].AsString();
                uint value = args[1].AsSerial();
                UOScript.Interpreter.SetAlias(alias, value);
            }

            return true;
        }


        private bool PromptAlias(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 1)
            {
                string alias = args[0].AsString();
                RazorEnhanced.Target target = new RazorEnhanced.Target();
                int value = target.PromptTarget("Target Alias");
                UOScript.Interpreter.SetAlias(alias, (uint)value);
            }
            return true;
        }


        private bool HeadMsg(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            string msg = args[0].AsString();
            int color = 0;
            int mobile = Player.Serial;
            if (args.Length == 2)
            {
                int value = (int)args[1].AsSerial();
                if (value < 10240)
                    color = value;
                else
                    mobile = value;
            }
            if (args.Length == 3)
            {
                color = args[1].AsInt();
                mobile = (int)args[2].AsSerial();
            }

            Mobiles.Message(mobile, color, msg);

            return true;
        }

        //TODO: Not implemented properly .. I dunno how to do a party only msg
        private bool PartyMsg(string command, UOScript.Argument[] args, bool quiet, bool force)
        {

            if (args.Length == 1)
            {
                string msg = args[0].AsString();
                Player.ChatParty(msg);
            }

            if (args.Length >= 2)
            {
                string msg = args[0].AsString();
                // 2nd parameter of ChatParty is Serial, to send private messages, not color, what0's
                int serial = args[1].AsInt();
                Player.ChatParty(msg, serial);
            }

            return true;
        }

        private bool MsgCommand(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            string msg = args[0].AsString();
            if (args.Length == 1)
            {
                Misc.SendMessage(msg);
            }
            if (args.Length == 2)
            {
                int color = args[1].AsInt();
                Misc.SendMessage(msg, color);
            }

            return true;
        }

        private bool CreateList(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            if (args.Length >= 1)
            {
                Console.WriteLine("Creating list {0}", args[0].AsString());
                UOScript.Interpreter.CreateList(args[0].AsString());
            }
            return true;
        }

        private bool PushList(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            if (args.Length >= 2)
            {
                Console.WriteLine("Pushing {0} to list {1}", args[1].AsString(), args[0].AsString());
                UOScript.Interpreter.PushList(args[0].AsString(), args[1], true, false);
            }
            return true;
        }


        //Dalamar: Create all stubs for methods
        private bool MoveItemOffset(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            uint serial = args[0].AsSerial();
            // string ground = args[1].AsString();
            if (args.Length == 2)
            {
                Items.DropItemGroundSelf((int)serial);
            }

            if (args.Length == 3)
            {
                int amount = args[2].AsInt();
                Items.DropItemGroundSelf((int)serial, amount);
            }
            if (args.Length == 5)
            {
                int x = args[2].AsInt();
                int y = args[3].AsInt();
                int z = args[4].AsInt();
                Items.MoveOnGround((int)serial, -1, x, y, z);
            }

            if (args.Length == 6)
            {
                int x = args[2].AsInt();
                int y = args[3].AsInt();
                int z = args[4].AsInt();
                int amount = args[5].AsInt();
                Items.MoveOnGround((int)serial, amount, x, y, z);
            }

            return true;
        }

        private bool MoveType(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 3)
            {
                int id = args[0].AsInt();
                uint src = args[1].AsSerial();
                uint dest = args[2].AsSerial();
                Item item = Items.FindByID(id, -1, (int)src);
                if (item != null)
                {
                    Items.Move(item.Serial, (int)dest, item.Amount);
                }
            }
            if (args.Length == 4)
            {
                int id = args[0].AsInt();
                uint src = args[1].AsSerial();
                uint dest = args[2].AsSerial();
                int color = args[3].AsInt();
                Item item = Items.FindByID(id, color, (int)src);
                if (item != null)
                {
                    Items.Move(item.Serial, (int)dest, item.Amount);
                }
            }
            if (args.Length == 5)
            {
                int id = args[0].AsInt();
                uint src = args[1].AsSerial();
                uint dest = args[2].AsSerial();
                int color = args[3].AsInt();
                int amount = args[4].AsInt();
                Item item = Items.FindByID(id, color, (int)src);
                if (item != null)
                {
                    Items.Move(item.Serial, (int)dest, amount);
                }
            }
            if (args.Length == 6)
            {
                int id = args[0].AsInt();
                uint src = args[1].AsSerial();
                uint dest = args[2].AsSerial();
                int x = args[3].AsInt();
                int y = args[4].AsInt();
                int z = args[5].AsInt();
                Item item = Items.FindByID(id, -1, (int)src);
                if (item != null)
                {
                    Items.Move(item.Serial, (int)dest, 0, x, y);
                }
            }
            if (args.Length == 7)
            {
                int id = args[0].AsInt();
                uint src = args[1].AsSerial();
                uint dest = args[2].AsSerial();
                int x = args[3].AsInt();
                int y = args[4].AsInt();
                int z = args[5].AsInt();
                int color = args[6].AsInt();
                Item item = Items.FindByID(id, color, (int)src);
                if (item != null)
                {
                    Items.Move(item.Serial, (int)dest, -1, x, y);
                }
            }
            if (args.Length == 8)
            {
                int id = args[0].AsInt();
                uint src = args[1].AsSerial();
                uint dest = args[2].AsSerial();
                int x = args[3].AsInt();
                int y = args[4].AsInt();
                int z = args[5].AsInt();
                int color = args[6].AsInt();
                int amount = args[7].AsInt();
                Item item = Items.FindByID(id, color, (int)src);
                if (item != null)
                {
                    Items.Move(item.Serial, (int)dest, amount, x, y);
                }
            }

            return true;
        }

        private bool MoveTypeOffset(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 2 || args.Length == 3)
            {
                int id = args[0].AsInt();
                uint src = args[1].AsSerial();
                Item item = Items.FindByID(id, -1, (int)src);
                if (item != null)
                {
                    Items.DropItemGroundSelf(item.Serial);
                }
            }
            if (args.Length == 4)
            {
                int id = args[0].AsInt();
                uint src = args[1].AsSerial();
                //uint dest = args[2].AsSerial();
                int color = args[3].AsInt();
                Item item = Items.FindByID(id, color, (int)src);
                if (item != null)
                {
                    Items.MoveOnGround(item.Serial, 0, Player.Position.X, Player.Position.Y, Player.Position.Z);
                }
            }
            if (args.Length == 5)
            {
                int id = args[0].AsInt();
                uint src = args[1].AsSerial();
                // uint dest = args[2].AsSerial();
                int color = args[3].AsInt();
                int amount = args[4].AsInt();
                Item item = Items.FindByID(id, color, (int)src);
                if (item != null)
                {
                    Items.MoveOnGround(item.Serial, amount, Player.Position.X, Player.Position.Y, Player.Position.Z);
                }
            }
            if (args.Length == 6)
            {
                int id = args[0].AsInt();
                uint src = args[1].AsSerial();
                //uint dest = args[2].AsSerial();
                int x = args[3].AsInt();
                int y = args[4].AsInt();
                int z = args[5].AsInt();
                Item item = Items.FindByID(id, -1, (int)src);
                if (item != null)
                {
                    Items.MoveOnGround(item.Serial, 0, x, y, z);
                }
            }
            if (args.Length == 7)
            {
                int id = args[0].AsInt();
                uint src = args[1].AsSerial();
                //uint dest = args[2].AsSerial();
                int x = args[3].AsInt();
                int y = args[4].AsInt();
                int z = args[5].AsInt();
                int color = args[6].AsInt();
                Item item = Items.FindByID(id, color, (int)src);
                if (item != null)
                {
                    Items.MoveOnGround(item.Serial, 0, x, y, z);
                }
            }
            if (args.Length == 8)
            {
                int id = args[0].AsInt();
                uint src = args[1].AsSerial();
                //uint dest = args[2].AsSerial();
                int x = args[3].AsInt();
                int y = args[4].AsInt();
                int z = args[5].AsInt();
                int color = args[6].AsInt();
                int amount = args[7].AsInt();
                Item item = Items.FindByID(id, color, (int)src);
                if (item != null)
                {
                    Items.MoveOnGround(item.Serial, amount, x, y, z);
                }
            }

            return true;
        }

        private bool EquipItem(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 1 || args.Length == 2)
            {
                uint serial = args[0].AsSerial();
                //int layer = args[1].AsInt();
                Player.EquipItem((int)serial);
            }
            return true;
        }

        private bool ToggleMounted(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            // uosteam has a crappy implementation
            // I am gonna change how it works a bit
            if (null != Player.Mount)
            {
                m_lastMount = Player.Mount.Serial;
                UOScript.Interpreter.SetAlias("mount", (uint)m_lastMount);
                Mobiles.UseMobile(Player.Serial);
            }
            else
            {
                Mobile mount = null;
                if (m_lastMount == 0)
                {
                    m_lastMount = (int)UOScript.Interpreter.GetAlias("mount");
                    if (m_lastMount == 0)
                    {
                        RazorEnhanced.Target target = new RazorEnhanced.Target();
                        m_lastMount = target.PromptTarget("Select a new mount");
                    }
                }
                mount = Mobiles.FindBySerial(m_lastMount);
                if (mount != null)
                {
                    Items.UseItem(mount.Serial);
                }

            }
            return true;
        }

        private bool EquipWand(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool Buy(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool Sell(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool ClearBuy(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool ClearSell(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool Organizer(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool AutoTargetObject(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 1)
            {
                uint serial = args[0].AsSerial();
                Assistant.Targeting.SetAutoTarget(serial);
            }
            return true;
        }

        private bool Autoloot(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool Dress(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool Undress(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool DressConfig(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool ToggleAutoloot(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool ToggleScavenger(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool Counter(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool UnsetAlias(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 1)
            {
                UOScript.Interpreter.UnSetAlias(args[0].AsString());
                return true;
            }
            return true;
        }

        private bool WaitForGump(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 2)
            {
                uint gumpid = args[0].AsUInt();
                int delay = args[1].AsInt();
                Gumps.WaitForGump(gumpid, delay);
            }
            return true;
        }

        private bool ReplyGump(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 2)
            {
                uint gumpid = args[0].AsUInt();
                int buttonid = args[1].AsInt();
                Gumps.SendAction(gumpid, buttonid);
            }
            return true;
        }

        private bool CloseGump(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool ClearJournal(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool WaitForJournal(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool PopList(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool RemoveList(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool ClearList(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool Ping(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            Assistant.Commands.Ping(null);
            return true;
        }

        private bool PlayMacro(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 1)
            {
                string script = args[0].AsString();
                Misc.ScriptRun(script);
            }
            return true;
        }

        private bool PlaySound(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool Resync(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            Misc.Resync();
            return true;
        }

        private bool Snapshot(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool Hotkeys(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool Where(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool MessageBox(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool MapUO(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool ClickScreen(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool Paperdoll(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool HelpButton(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool GuildButton(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool QuestsButton(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool LogoutButton(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool Virtue(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool GuildMsg(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool AllyMsg(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool WhisperMsg(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool YellMsg(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool SysMsg(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 1)
            {
                Misc.SendMessage(args[0].AsString());
            }
            if (args.Length == 2)
            {
                Misc.SendMessage(args[0].AsString(), args[1].AsInt(), false);
            }

            return true;
        }

        private bool ChatMsg(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool EmoteMsg(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool PromptMsg(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool TimerMsg(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool WaitForPrompt(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool CancelPrompt(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool AddFriend(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool RemoveFriend(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool ContextMenu(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool WaitForContext(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool IgnoreObject(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool ClearIgnoreList(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool SetSkill(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool WaitForProperties(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 2)
            {
                uint serial = args[0].AsSerial();
                int timeout = args[1].AsInt();
                Item item = Items.FindBySerial((int)serial);
                if (item != null)
                {
                    Items.WaitForProps(item, timeout);
                }
            }
            return true;
        }

        private bool AutoColorPick(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool WaitForContents(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 2)
            {
                uint serial = args[0].AsSerial();
                int timeout = args[0].AsInt();
                Item item = Items.FindBySerial((int)serial);
                if (item != null)
                {
                    Items.WaitForContents(item, timeout);
                }
            }
            return true;
        }

        private bool MiniHeal(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool BigHeal(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool Cast(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool ChivalryHeal(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool WaitForTarget(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool CancelAutoTarget(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool CancelTarget(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool Target(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool TargetType(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool TargetGround(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool TargetTile(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool TargetTileOffset(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool TargetTileRelative(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool ClearTargetQueue(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool SetTimer(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool RemoveTimer(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool CreateTimer(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }

        private bool ShowNames(string command, UOScript.Argument[] args, bool quiet, bool force)
        {
            return NotImplemented(command, args, quiet, force);
        }






    }


    namespace UOScript
    // This code from  https://github.com/jaedan/steam-engine.git
    {
        public class RunTimeError : Exception
        {
            public ASTNode Node;

            public RunTimeError(ASTNode node, string error) : base(error)
            {
                Node = node;
            }
        }

        internal static class TypeConverter
        {
            public static int ToInt(string token)
            {
                int val;

                if (token.StartsWith("0x"))
                {
                    if (int.TryParse(token.Substring(2), System.Globalization.NumberStyles.HexNumber, UOScript.Interpreter.Culture, out val))
                        return val;
                }
                else if (int.TryParse(token, out val))
                    return val;

                throw new RunTimeError(null, "Cannot convert argument to int");
            }

            public static uint ToUInt(string token)
            {
                uint val;

                if (token.StartsWith("0x"))
                {
                    if (uint.TryParse(token.Substring(2), System.Globalization.NumberStyles.HexNumber, UOScript.Interpreter.Culture, out val))
                        return val;
                }
                else if (uint.TryParse(token, out val))
                    return val;

                throw new RunTimeError(null, "Cannot convert " + token + " argument to uint");
            }

            public static ushort ToUShort(string token)
            {
                ushort val;

                if (token.StartsWith("0x"))
                {
                    if (ushort.TryParse(token.Substring(2), System.Globalization.NumberStyles.HexNumber, UOScript.Interpreter.Culture, out val))
                        return val;
                }
                else if (ushort.TryParse(token, out val))
                    return val;

                throw new RunTimeError(null, "Cannot convert argument to ushort");
            }

            public static double ToDouble(string token)
            {
                double val;

                if (double.TryParse(token, out val))
                    return val;

                throw new RunTimeError(null, "Cannot convert argument to double");
            }

            public static bool ToBool(string token)
            {
                bool val;

                if (bool.TryParse(token, out val))
                    return val;

                throw new RunTimeError(null, "Cannot convert argument to bool");
            }
        }

        internal class Scope
        {
            private Dictionary<string, Argument> _namespace = new Dictionary<string, Argument>();

            public readonly ASTNode StartNode;
            public readonly Scope Parent;

            public Scope(Scope parent, ASTNode start)
            {
                Parent = parent;
                StartNode = start;
            }

            public Argument GetVar(string name)
            {
                Argument arg;

                if (_namespace.TryGetValue(name, out arg))
                    return arg;

                return null;
            }

            public void SetVar(string name, Argument val)
            {
                _namespace[name] = val;
            }

            public void ClearVar(string name)
            {
                _namespace.Remove(name);
            }
        }

        public class Argument
        {
            private ASTNode _node;
            private Script _script;

            public Argument(Script script, ASTNode node)
            {
                _node = node;
                _script = script;
            }

            // Treat the argument as an integer
            public int AsInt()
            {
                if (_node.Lexeme == null)
                    throw new RunTimeError(_node, "Cannot convert argument to int");

                // Try to resolve it as a scoped variable first
                var arg = _script.Lookup(_node.Lexeme);
                if (arg != null)
                    return arg.AsInt();

                if (UOScript.Interpreter.FindAlias(_node.Lexeme))
                {
                    int value = (int)UOScript.Interpreter.GetAlias(_node.Lexeme);
                    return value;
                }

                return TypeConverter.ToInt(_node.Lexeme);
            }

            // Treat the argument as an unsigned integer
            public uint AsUInt()
            {
                if (_node.Lexeme == null)
                    throw new RunTimeError(_node, "Cannot convert argument to uint");

                // Try to resolve it as a scoped variable first
                var arg = _script.Lookup(_node.Lexeme);
                if (arg != null)
                    return arg.AsUInt();

                if (UOScript.Interpreter.FindAlias(_node.Lexeme))
                {
                    uint value = UOScript.Interpreter.GetAlias(_node.Lexeme);
                    return value;
                }

                return TypeConverter.ToUInt(_node.Lexeme);
            }

            public ushort AsUShort()
            {
                if (_node.Lexeme == null)
                    throw new RunTimeError(_node, "Cannot convert argument to ushort");

                // Try to resolve it as a scoped variable first
                var arg = _script.Lookup(_node.Lexeme);
                if (arg != null)
                    return arg.AsUShort();

                return TypeConverter.ToUShort(_node.Lexeme);
            }

            // Treat the argument as a serial or an alias. Aliases will
            // be automatically resolved to serial numbers.
            public uint AsSerial()
            {
                if (_node.Lexeme == null)
                    throw new RunTimeError(_node, "Cannot convert argument to serial");

                // Try to resolve it as a scoped variable first
                var arg = _script.Lookup(_node.Lexeme);
                if (arg != null)
                    return arg.AsSerial();

                // Resolve it as a global alias next
                if (UOScript.Interpreter.FindAlias(_node.Lexeme))
                {
                    uint serial = UOScript.Interpreter.GetAlias(_node.Lexeme);
                    return serial;
                }

                uint val;
                if (uint.TryParse(_node.Lexeme, out val))
                    return AsUInt();

                // This is a bad place to be
                return 0;
            }

            // Treat the argument as a string
            public string AsString()
            {
                if (_node.Lexeme == null)
                    throw new RunTimeError(_node, "Cannot convert argument to string");

                // Try to resolve it as a scoped variable first
                var arg = _script.Lookup(_node.Lexeme);
                if (arg != null)
                    return arg.AsString();

                return _node.Lexeme;
            }

            public bool AsBool()
            {
                if (_node.Lexeme == null)
                    throw new RunTimeError(_node, "Cannot convert argument to bool");

                return TypeConverter.ToBool(_node.Lexeme);
            }

            public override bool Equals(object obj)
            {
                if (obj == null)
                    return false;

                Argument arg = obj as Argument;

                if (arg == null)
                    return false;

                return Equals(arg);
            }

            public bool Equals(Argument other)
            {
                if (other == null)
                    return false;

                return (other._node.Lexeme == _node.Lexeme);
            }
        }

        public class Script
        {
            private ASTNode _statement;

            private Scope _scope;

            public Argument Lookup(string name)
            {
                var scope = _scope;
                Argument result = null;

                while (scope != null)
                {
                    result = scope.GetVar(name);
                    if (result != null)
                        return result;

                    scope = scope.Parent;
                }

                return result;
            }

            private void PushScope(ASTNode node)
            {
                _scope = new Scope(_scope, node);
            }

            private void PopScope()
            {
                _scope = _scope.Parent;
            }

            private Argument[] ConstructArguments(ref ASTNode node)
            {
                List<Argument> args = new List<Argument>();

                node = node.Next();

                while (node != null)
                {
                    switch (node.Type)
                    {
                        case ASTNodeType.AND:
                        case ASTNodeType.OR:
                        case ASTNodeType.EQUAL:
                        case ASTNodeType.NOT_EQUAL:
                        case ASTNodeType.LESS_THAN:
                        case ASTNodeType.LESS_THAN_OR_EQUAL:
                        case ASTNodeType.GREATER_THAN:
                        case ASTNodeType.GREATER_THAN_OR_EQUAL:
                            return args.ToArray();
                    }

                    args.Add(new Argument(this, node));

                    node = node.Next();
                }

                return args.ToArray();
            }

            // For now, the scripts execute directly from the
            // abstract syntax tree. This is relatively simple.
            // A more robust approach would be to "compile" the
            // scripts to a bytecode. That would allow more errors
            // to be caught with better error messages, as well as
            // make the scripts execute more quickly.
            public Script(ASTNode root)
            {
                // Set current to the first statement
                _statement = root.FirstChild();

                // Create a default scope
                _scope = new Scope(null, _statement);
            }

            public bool ExecuteNext()
            {
                if (_statement == null)
                    return false;

                if (_statement.Type != ASTNodeType.STATEMENT)
                    throw new RunTimeError(_statement, "Invalid script");

                var node = _statement.FirstChild();

                if (node == null)
                    throw new RunTimeError(_statement, "Invalid statement");

                int depth = 0;

                switch (node.Type)
                {
                    case ASTNodeType.IF:
                        {
                            PushScope(node);

                            var expr = node.FirstChild();
                            var result = EvaluateExpression(ref expr);

                            // Advance to next statement
                            Advance();

                            // Evaluated true. Jump right into execution.
                            if (result)
                                break;

                            // The expression evaluated false, so keep advancing until
                            // we hit an elseif, else, or endif statement that matches
                            // and try again.
                            depth = 0;

                            while (_statement != null)
                            {
                                node = _statement.FirstChild();

                                if (node.Type == ASTNodeType.IF)
                                {
                                    depth++;
                                }
                                else if (node.Type == ASTNodeType.ELSEIF)
                                {
                                    if (depth == 0)
                                    {
                                        expr = node.FirstChild();
                                        result = EvaluateExpression(ref expr);

                                        // Evaluated true. Jump right into execution
                                        if (result)
                                        {
                                            Advance();
                                            break;
                                        }
                                    }
                                }
                                else if (node.Type == ASTNodeType.ELSE)
                                {
                                    if (depth == 0)
                                    {
                                        // Jump into the else clause
                                        Advance();
                                        break;
                                    }
                                }
                                else if (node.Type == ASTNodeType.ENDIF)
                                {
                                    if (depth == 0)
                                        break;

                                    depth--;
                                }

                                Advance();
                            }

                            if (_statement == null)
                                throw new RunTimeError(node, "If with no matching endif");

                            break;
                        }
                    case ASTNodeType.ELSEIF:
                        // If we hit the elseif statement during normal advancing, skip over it. The only way
                        // to execute an elseif clause is to jump directly in from an if statement.
                        depth = 0;

                        while (_statement != null)
                        {
                            node = _statement.FirstChild();

                            if (node.Type == ASTNodeType.IF)
                            {
                                depth++;
                            }
                            else if (node.Type == ASTNodeType.ENDIF)
                            {
                                if (depth == 0)
                                    break;

                                depth--;
                            }

                            Advance();
                        }

                        if (_statement == null)
                            throw new RunTimeError(node, "If with no matching endif");

                        break;
                    case ASTNodeType.ENDIF:
                        PopScope();
                        Advance();
                        break;
                    case ASTNodeType.ELSE:
                        // If we hit the else statement during normal advancing, skip over it. The only way
                        // to execute an else clause is to jump directly in from an if statement.
                        depth = 0;

                        while (_statement != null)
                        {
                            node = _statement.FirstChild();

                            if (node.Type == ASTNodeType.IF)
                            {
                                depth++;
                            }
                            else if (node.Type == ASTNodeType.ENDIF)
                            {
                                if (depth == 0)
                                    break;

                                depth--;
                            }

                            Advance();
                        }

                        if (_statement == null)
                            throw new RunTimeError(node, "If with no matching endif");

                        break;
                    case ASTNodeType.WHILE:
                        {
                            // When we first enter the loop, push a new scope
                            if (_scope.StartNode != node)
                            {
                                PushScope(node);
                            }

                            var expr = node.FirstChild();
                            var result = EvaluateExpression(ref expr);

                            // Advance to next statement
                            Advance();

                            // The expression evaluated false, so keep advancing until
                            // we hit an endwhile statement.
                            if (!result)
                            {
                                depth = 0;

                                while (_statement != null)
                                {
                                    node = _statement.FirstChild();

                                    if (node.Type == ASTNodeType.WHILE)
                                    {
                                        depth++;
                                    }
                                    else if (node.Type == ASTNodeType.ENDWHILE)
                                    {
                                        if (depth == 0)
                                        {
                                            PopScope();
                                            // Go one past the endwhile so the loop doesn't repeat
                                            Advance();
                                            break;
                                        }

                                        depth--;
                                    }

                                    Advance();
                                }
                            }
                            break;
                        }
                    case ASTNodeType.ENDWHILE:
                        // Walk backward to the while statement
                        _statement = _statement.Prev();

                        depth = 0;

                        while (_statement != null)
                        {
                            node = _statement.FirstChild();

                            if (node.Type == ASTNodeType.ENDWHILE)
                            {
                                depth++;
                            }
                            else if (node.Type == ASTNodeType.WHILE)
                            {
                                if (depth == 0)
                                    break;

                                depth--;
                            }

                            _statement = _statement.Prev();
                        }

                        if (_statement == null)
                            throw new RunTimeError(node, "Unexpected endwhile");

                        break;
                    case ASTNodeType.FOR:
                        {
                            // The iterator variable's name is the hash code of the for loop's ASTNode.
                            var iterName = node.GetHashCode().ToString();

                            // When we first enter the loop, push a new scope
                            if (_scope.StartNode != node)
                            {
                                PushScope(node);

                                // Grab the arguments
                                var max = node.FirstChild();

                                if (max.Type != ASTNodeType.INTEGER)
                                    throw new RunTimeError(max, "Invalid for loop syntax");

                                // Create a dummy argument that acts as our loop variable
                                var iter = new ASTNode(ASTNodeType.INTEGER, "0", node, 0);

                                _scope.SetVar(iterName, new Argument(this, iter));
                            }
                            else
                            {
                                // Increment the iterator argument
                                var arg = _scope.GetVar(iterName);

                                var iter = new ASTNode(ASTNodeType.INTEGER, (arg.AsUInt() + 1).ToString(), node, 0);

                                _scope.SetVar(iterName, new Argument(this, iter));
                            }

                            // Check loop condition
                            var i = _scope.GetVar(iterName);

                            // Grab the max value to iterate to
                            node = node.FirstChild();
                            var end = new Argument(this, node);

                            if (i.AsUInt() < end.AsUInt())
                            {
                                // enter the loop
                                Advance();
                            }
                            else
                            {
                                // Walk until the end of the loop
                                Advance();

                                depth = 0;

                                while (_statement != null)
                                {
                                    node = _statement.FirstChild();

                                    if (node.Type == ASTNodeType.FOR ||
                                        node.Type == ASTNodeType.FOREACH)
                                    {
                                        depth++;
                                    }
                                    else if (node.Type == ASTNodeType.ENDFOR)
                                    {
                                        if (depth == 0)
                                        {
                                            PopScope();
                                            // Go one past the end so the loop doesn't repeat
                                            Advance();
                                            break;
                                        }

                                        depth--;
                                    }

                                    Advance();
                                }
                            }
                        }
                        break;
                    case ASTNodeType.FOREACH:
                        {
                            // foreach VAR in LIST
                            // The iterator's name is the hash code of the for loop's ASTNode.
                            var varName = node.FirstChild().Lexeme;
                            //var listName = node.FirstChild().Next().Lexeme; this was getting invalid list name
                            var listName = node.FirstChild().Lexeme;
                            var iterName = node.GetHashCode().ToString();

                            // When we first enter the loop, push a new scope
                            if (_scope.StartNode != node)
                            {
                                PushScope(node);

                                // Create a dummy argument that acts as our iterator object
                                var iter = new ASTNode(ASTNodeType.INTEGER, "0", node, 0);
                                _scope.SetVar(iterName, new Argument(this, iter));

                                // Make the user-chosen variable have the value for the front of the list
                                var arg = UOScript.Interpreter.GetListValue(listName, 0);

                                if (arg != null)
                                    _scope.SetVar(varName + "[]", arg);
                                else
                                    _scope.ClearVar(varName + "[]");
                            }
                            else
                            {
                                // Increment the iterator argument
                                var idx = _scope.GetVar(iterName).AsInt() + 1;
                                var iter = new ASTNode(ASTNodeType.INTEGER, idx.ToString(), node, 0);
                                _scope.SetVar(iterName, new Argument(this, iter));

                                // Update the user-chosen variable
                                var arg = UOScript.Interpreter.GetListValue(listName, idx);

                                if (arg != null)
                                    _scope.SetVar(varName + "[]", arg);
                                else
                                    _scope.ClearVar(varName + "[]");
                            }

                            // Check loop condition
                            var i = _scope.GetVar(varName + "[]");

                            if (i != null)
                            {
                                // enter the loop
                                Advance();
                            }
                            else
                            {
                                // Walk until the end of the loop
                                Advance();

                                depth = 0;

                                while (_statement != null)
                                {
                                    node = _statement.FirstChild();

                                    if (node.Type == ASTNodeType.FOR ||
                                        node.Type == ASTNodeType.FOREACH)
                                    {
                                        depth++;
                                    }
                                    else if (node.Type == ASTNodeType.ENDFOR)
                                    {
                                        if (depth == 0)
                                        {
                                            PopScope();
                                            // Go one past the end so the loop doesn't repeat
                                            Advance();
                                            break;
                                        }

                                        depth--;
                                    }

                                    Advance();
                                }
                            }
                            break;
                        }
                    case ASTNodeType.ENDFOR:
                        // Walk backward to the for statement
                        _statement = _statement.Prev();

                        while (_statement != null)
                        {
                            node = _statement.FirstChild();

                            if (node.Type == ASTNodeType.FOR ||
                                node.Type == ASTNodeType.FOREACH)
                            {
                                break;
                            }

                            _statement = _statement.Prev();
                        }

                        if (_statement == null)
                            throw new RunTimeError(node, "Unexpected endfor");

                        break;
                    case ASTNodeType.BREAK:
                        // Walk until the end of the loop
                        Advance();

                        depth = 0;

                        while (_statement != null)
                        {
                            node = _statement.FirstChild();

                            if (node.Type == ASTNodeType.WHILE ||
                                node.Type == ASTNodeType.FOR ||
                                node.Type == ASTNodeType.FOREACH)
                            {
                                depth++;
                            }
                            else if (node.Type == ASTNodeType.ENDWHILE ||
                                node.Type == ASTNodeType.ENDFOR)
                            {
                                if (depth == 0)
                                {
                                    PopScope();

                                    // Go one past the end so the loop doesn't repeat
                                    Advance();
                                    break;
                                }

                                depth--;
                            }

                            Advance();
                        }

                        PopScope();
                        break;
                    case ASTNodeType.CONTINUE:
                        // Walk backward to the loop statement
                        _statement = _statement.Prev();

                        depth = 0;

                        while (_statement != null)
                        {
                            node = _statement.FirstChild();

                            if (node.Type == ASTNodeType.ENDWHILE ||
                                node.Type == ASTNodeType.ENDFOR)
                            {
                                depth++;
                            }
                            else if (node.Type == ASTNodeType.WHILE ||
                                     node.Type == ASTNodeType.FOR ||
                                     node.Type == ASTNodeType.FOREACH)
                            {
                                if (depth == 0)
                                    break;

                                depth--;
                            }

                            _statement = _statement.Prev();
                        }

                        if (_statement == null)
                            throw new RunTimeError(node, "Unexpected continue");
                        break;
                    case ASTNodeType.STOP:
                        _statement = null;
                        break;
                    case ASTNodeType.REPLAY:
                        _statement = _statement.Parent.FirstChild();
                        break;
                    case ASTNodeType.QUIET:
                    case ASTNodeType.FORCE:
                    case ASTNodeType.COMMAND:
                        if (ExecuteCommand(node))
                            Advance();

                        break;
                }

                return (_statement != null) ? true : false;
            }

            public void Advance()
            {
                UOScript.Interpreter.ClearTimeout();
                _statement = _statement.Next();
            }

            private ASTNode EvaluateModifiers(ASTNode node, out bool quiet, out bool force, out bool not)
            {
                quiet = false;
                force = false;
                not = false;

                while (true)
                {
                    switch (node.Type)
                    {
                        case ASTNodeType.QUIET:
                            quiet = true;
                            break;
                        case ASTNodeType.FORCE:
                            force = true;
                            break;
                        case ASTNodeType.NOT:
                            not = true;
                            break;
                        default:
                            return node;
                    }

                    node = node.Next();
                }
            }

            private bool ExecuteCommand(ASTNode node)
            {
                node = EvaluateModifiers(node, out bool quiet, out bool force, out _);

                var handler = UOScript.Interpreter.GetCommandHandler(node.Lexeme);

                if (handler == null)
                    throw new RunTimeError(node, "Unknown command");

                var cont = handler(node.Lexeme, ConstructArguments(ref node), quiet, force);

                if (node != null)
                    throw new RunTimeError(node, "Command did not consume all available arguments");

                return cont;
            }

            private bool EvaluateExpression(ref ASTNode expr)
            {
                if (expr == null || (expr.Type != ASTNodeType.UNARY_EXPRESSION && expr.Type != ASTNodeType.BINARY_EXPRESSION && expr.Type != ASTNodeType.LOGICAL_EXPRESSION))
                    throw new RunTimeError(expr, "No expression following control statement");

                var node = expr.FirstChild();

                if (node == null)
                    throw new RunTimeError(expr, "Empty expression following control statement");

                switch (expr.Type)
                {
                    case ASTNodeType.UNARY_EXPRESSION:
                        return EvaluateUnaryExpression(ref node);
                    case ASTNodeType.BINARY_EXPRESSION:
                        return EvaluateBinaryExpression(ref node);
                }

                bool lhs = EvaluateExpression(ref node);

                node = node.Next();

                while (node != null)
                {
                    // Capture the operator
                    var op = node.Type;
                    node = node.Next();

                    if (node == null)
                        throw new RunTimeError(node, "Invalid logical expression");

                    bool rhs;

                    var e = node.FirstChild();

                    switch (node.Type)
                    {
                        case ASTNodeType.UNARY_EXPRESSION:
                            rhs = EvaluateUnaryExpression(ref e);
                            break;
                        case ASTNodeType.BINARY_EXPRESSION:
                            rhs = EvaluateBinaryExpression(ref e);
                            break;
                        default:
                            throw new RunTimeError(node, "Nested logical expressions are not possible");
                    }

                    switch (op)
                    {
                        case ASTNodeType.AND:
                            lhs = lhs && rhs;
                            break;
                        case ASTNodeType.OR:
                            lhs = lhs || rhs;
                            break;
                        default:
                            throw new RunTimeError(node, "Invalid logical operator");
                    }

                    node = node.Next();
                }

                return lhs;
            }

            private bool CompareOperands(ASTNodeType op, IComparable lhs, IComparable rhs)
            {
                if (lhs.GetType() != rhs.GetType())
                {
                    // Different types. Try to convert one to match the other.

                    if (rhs is double)
                    {
                        // Special case for rhs doubles because we don't want to lose precision.
                        lhs = (double)lhs;
                    }
                    else if (rhs is bool)
                    {
                        // Special case for rhs bools because we want to down-convert the lhs.
                        var tmp = Convert.ChangeType(lhs, typeof(bool));
                        lhs = (IComparable)tmp;
                    }
                    else
                    {
                        var tmp = Convert.ChangeType(rhs, lhs.GetType());
                        rhs = (IComparable)tmp;
                    }
                }

                try
                {
                    // Evaluate the whole expression
                    switch (op)
                    {
                        case ASTNodeType.EQUAL:
                            return lhs.CompareTo(rhs) == 0;
                        case ASTNodeType.NOT_EQUAL:
                            return lhs.CompareTo(rhs) != 0;
                        case ASTNodeType.LESS_THAN:
                            return lhs.CompareTo(rhs) < 0;
                        case ASTNodeType.LESS_THAN_OR_EQUAL:
                            return lhs.CompareTo(rhs) <= 0;
                        case ASTNodeType.GREATER_THAN:
                            return lhs.CompareTo(rhs) > 0;
                        case ASTNodeType.GREATER_THAN_OR_EQUAL:
                            return lhs.CompareTo(rhs) >= 0;
                    }
                }
                catch (ArgumentException e)
                {
                    throw new RunTimeError(null, e.Message);
                }

                throw new RunTimeError(null, "Unknown operator in expression");

            }

            private bool EvaluateUnaryExpression(ref ASTNode node)
            {
                node = EvaluateModifiers(node, out bool quiet, out _, out bool ifnot);

                var handler = UOScript.Interpreter.GetExpressionHandler(node.Lexeme);

                if (handler == null)
                    throw new RunTimeError(node, "Unknown expression");

                var result = handler(node.Lexeme, ConstructArguments(ref node), quiet);

                if (ifnot)
                    return CompareOperands(ASTNodeType.EQUAL, result, false);
                else
                    return CompareOperands(ASTNodeType.EQUAL, result, true);
            }

            private bool EvaluateBinaryExpression(ref ASTNode node)
            {
                // Evaluate the left hand side
                var lhs = EvaluateBinaryOperand(ref node);

                // Capture the operator
                var op = node.Type;
                node = node.Next();

                // Evaluate the right hand side
                var rhs = EvaluateBinaryOperand(ref node);

                return CompareOperands(op, lhs, rhs);
            }

            private IComparable EvaluateBinaryOperand(ref ASTNode node)
            {
                IComparable val;

                node = EvaluateModifiers(node, out bool quiet, out _, out _);
                switch (node.Type)
                {
                    case ASTNodeType.INTEGER:
                        val = TypeConverter.ToInt(node.Lexeme);
                        break;
                    case ASTNodeType.SERIAL:
                        val = TypeConverter.ToUInt(node.Lexeme);
                        break;
                    case ASTNodeType.STRING:
                        val = node.Lexeme;
                        break;
                    case ASTNodeType.DOUBLE:
                        val = TypeConverter.ToDouble(node.Lexeme);
                        break;
                    case ASTNodeType.OPERAND:
                        {
                            // This might be a registered keyword, so do a lookup
                            var handler = UOScript.Interpreter.GetExpressionHandler(node.Lexeme);

                            if (handler == null)
                            {
                                // It's just a string
                                val = node.Lexeme;
                            }
                            else
                            {
                                val = handler(node.Lexeme, ConstructArguments(ref node), quiet);
                            }
                            break;
                        }
                    default:
                        throw new RunTimeError(node, "Invalid type found in expression");
                }

                return val;
            }
        }

        public static class Interpreter
        {
            // Aliases only hold serial numbers
            private static Dictionary<string, uint> _aliases = new Dictionary<string, uint>();

            // Lists
            private static Dictionary<string, List<Argument>> _lists = new Dictionary<string, List<Argument>>();

            // Timers
            private static Dictionary<string, DateTime> _timers = new Dictionary<string, DateTime>();

            // Expressions
            public delegate IComparable ExpressionHandler(string expression, Argument[] args, bool quiet);
            public delegate T ExpressionHandler<T>(string expression, Argument[] args, bool quiet) where T : IComparable;

            private static Dictionary<string, ExpressionHandler> _exprHandlers = new Dictionary<string, ExpressionHandler>();

            public delegate bool CommandHandler(string command, Argument[] args, bool quiet, bool force);

            private static Dictionary<string, CommandHandler> _commandHandlers = new Dictionary<string, CommandHandler>();

            public delegate uint AliasHandler(string alias);

            private static Dictionary<string, AliasHandler> _aliasHandlers = new Dictionary<string, AliasHandler>();

            private static Script _activeScript = null;

            private enum ExecutionState
            {
                RUNNING,
                PAUSED,
                TIMING_OUT
            };

            public delegate bool TimeoutCallback();

            private static ExecutionState _executionState = ExecutionState.RUNNING;
            private static long _pauseTimeout = long.MaxValue;
            private static TimeoutCallback _timeoutCallback = null;

            public static System.Globalization.CultureInfo Culture;

            static Interpreter()
            {
                Culture = new System.Globalization.CultureInfo(System.Globalization.CultureInfo.CurrentCulture.LCID, false);
                Culture.NumberFormat.NumberDecimalSeparator = ".";
                Culture.NumberFormat.NumberGroupSeparator = ",";
            }

            public static void RegisterExpressionHandler<T>(string keyword, ExpressionHandler<T> handler) where T : IComparable
            {
                _exprHandlers[keyword] = (expression, args, quiet) => handler(expression, args, quiet);
            }

            public static ExpressionHandler GetExpressionHandler(string keyword)
            {
                _exprHandlers.TryGetValue(keyword, out var expression);

                return expression;
            }

            public static void RegisterCommandHandler(string keyword, CommandHandler handler)
            {
                _commandHandlers[keyword] = handler;
            }

            public static CommandHandler GetCommandHandler(string keyword)
            {
                _commandHandlers.TryGetValue(keyword, out CommandHandler handler);

                return handler;
            }

            public static void RegisterAliasHandler(string keyword, AliasHandler handler)
            {
                _aliasHandlers[keyword] = handler;
            }

            public static void UnregisterAliasHandler(string keyword)
            {
                _aliasHandlers.Remove(keyword);
            }

            public static uint GetAlias(string alias)
            {
                // If a handler is explicitly registered, call that.
                if (_aliasHandlers.TryGetValue(alias, out AliasHandler handler))
                    return handler(alias);

                uint value;
                if (_aliases.TryGetValue(alias, out value))
                    return value;

                return uint.MaxValue;
            }
            public static bool FindAlias(string alias)
            {
                // If a handler is explicitly registered, call that.
                if (_aliasHandlers.TryGetValue(alias, out AliasHandler handler))
                    return true;

                return _aliases.ContainsKey(alias);
            }

            public static void UnSetAlias(string alias)
            {
                _aliases.Remove(alias);
            }
            public static void SetAlias(string alias, uint serial)
            {
                _aliases[alias] = serial;
            }

            public static void CreateList(string name)
            {
                if (_lists.ContainsKey(name))
                    return;

                _lists[name] = new List<Argument>();
            }

            public static void DestroyList(string name)
            {
                _lists.Remove(name);
            }

            public static void ClearList(string name)
            {
                if (!_lists.ContainsKey(name))
                    return;

                _lists[name].Clear();
            }

            public static bool ListExists(string name)
            {
                return _lists.ContainsKey(name);
            }

            public static bool ListContains(string name, Argument arg)
            {
                if (!_lists.ContainsKey(name))
                    throw new RunTimeError(null, "List does not exist");

                return _lists[name].Contains(arg);
            }

            public static int ListLength(string name)
            {
                if (!_lists.ContainsKey(name))
                    throw new RunTimeError(null, "List does not exist");

                return _lists[name].Count;
            }

            public static void PushList(string name, Argument arg, bool front, bool unique)
            {
                if (!_lists.ContainsKey(name))
                    throw new RunTimeError(null, "List does not exist");

                if (unique && _lists[name].Contains(arg))
                    return;

                if (front)
                    _lists[name].Insert(0, arg);
                else
                    _lists[name].Add(arg);
            }

            public static bool PopList(string name, Argument arg)
            {
                if (!_lists.ContainsKey(name))
                    throw new RunTimeError(null, "List does not exist");

                return _lists[name].Remove(arg);
            }

            public static bool PopList(string name, bool front)
            {
                if (!_lists.ContainsKey(name))
                    throw new RunTimeError(null, "List does not exist");

                var idx = front ? 0 : _lists[name].Count - 1;

                _lists[name].RemoveAt(idx);

                return _lists[name].Count > 0;
            }

            public static Argument GetListValue(string name, int idx)
            {
                if (!_lists.ContainsKey(name))
                    throw new RunTimeError(null, "List does not exist");

                var list = _lists[name];

                if (idx < list.Count)
                    return list[idx];

                return null;
            }

            public static void CreateTimer(string name)
            {
                _timers[name] = DateTime.UtcNow;
            }

            public static TimeSpan GetTimer(string name)
            {
                if (!_timers.TryGetValue(name, out DateTime timestamp))
                    throw new RunTimeError(null, "Timer does not exist");

                TimeSpan elapsed = DateTime.UtcNow - timestamp;

                return elapsed;
            }

            public static void SetTimer(string name, int elapsed)
            {
                // Setting a timer to start at a given value is equivalent to
                // starting the timer that number of milliseconds in the past.
                _timers[name] = DateTime.UtcNow.AddMilliseconds(-elapsed);
            }

            public static void RemoveTimer(string name)
            {
                _timers.Remove(name);
            }

            public static bool TimerExists(string name)
            {
                return _timers.ContainsKey(name);
            }

            public static bool StartScript(Script script)
            {

                if (_activeScript != null)
                    return false;

                _activeScript = script;
                _executionState = ExecutionState.RUNNING;

                ExecuteScript();

                return true;
            }

            public static void StopScript()
            {
                _activeScript = null;
                _executionState = ExecutionState.RUNNING;
            }

            public static bool ExecuteScript()
            {
                if (_activeScript == null)
                    return false;

                if (_executionState == ExecutionState.PAUSED)
                {
                    if (_pauseTimeout < DateTime.UtcNow.Ticks)
                        _executionState = ExecutionState.RUNNING;
                    else
                        return true;
                }
                else if (_executionState == ExecutionState.TIMING_OUT)
                {
                    if (_pauseTimeout < DateTime.UtcNow.Ticks)
                    {
                        if (_timeoutCallback != null)
                        {
                            if (_timeoutCallback())
                            {
                                _activeScript.Advance();
                                ClearTimeout();
                            }

                            _timeoutCallback = null;
                        }

                        /* If the callback changed the state to running, continue
                         * on. Otherwise, exit.
                         */
                        if (_executionState != ExecutionState.RUNNING)
                        {
                            _activeScript = null;
                            return false;
                        }
                    }
                }

                if (!_activeScript.ExecuteNext())
                {
                    _activeScript = null;
                    return false;
                }

                return true;
            }

            // Pause execution for the given number of milliseconds
            public static void Pause(long duration)
            {
                // Already paused or timing out
                if (_executionState != ExecutionState.RUNNING)
                    return;

                _pauseTimeout = DateTime.UtcNow.Ticks + (duration * 10000);
                _executionState = ExecutionState.PAUSED;
            }

            // Unpause execution
            public static void Unpause()
            {
                if (_executionState != ExecutionState.PAUSED)
                    return;

                _pauseTimeout = 0;
                _executionState = ExecutionState.RUNNING;
            }

            // If forward progress on the script isn't made within this
            // amount of time (milliseconds), bail
            public static void Timeout(long duration, TimeoutCallback callback)
            {
                // Don't change an existing timeout
                if (_executionState != ExecutionState.RUNNING)
                    return;

                _pauseTimeout = DateTime.UtcNow.Ticks + (duration * 10000);
                _executionState = ExecutionState.TIMING_OUT;
                _timeoutCallback = callback;
            }

            // Clears any previously set timeout. Automatically
            // called any time the script advances a statement.
            public static void ClearTimeout()
            {
                if (_executionState != ExecutionState.TIMING_OUT)
                    return;

                _pauseTimeout = 0;
                _executionState = ExecutionState.RUNNING;
            }
        }
    }

    public class SyntaxError : Exception
    {
        public ASTNode Node;
        public string Line;
        public int LineNumber;

        public SyntaxError(ASTNode node, string error) : base(error)
        {
            Node = node;
            Line = null;
            LineNumber = 0;
        }

        public SyntaxError(string line, int lineNumber, ASTNode node, string error) : base(error)
        {
            Line = line;
            LineNumber = lineNumber;
            Node = node;
        }
    }

    public enum ASTNodeType
    {
        // Keywords
        IF,
        ELSEIF,
        ELSE,
        ENDIF,
        WHILE,
        ENDWHILE,
        FOR,
        FOREACH,
        ENDFOR,
        BREAK,
        CONTINUE,
        STOP,
        REPLAY,

        // Operators
        EQUAL,
        NOT_EQUAL,
        LESS_THAN,
        LESS_THAN_OR_EQUAL,
        GREATER_THAN,
        GREATER_THAN_OR_EQUAL,

        // Logical Operators
        NOT,
        AND,
        OR,

        // Value types
        STRING,
        SERIAL,
        INTEGER,
        DOUBLE,
        LIST,

        // Modifiers
        QUIET, // @ symbol
        FORCE, // ! symbol

        // Everything else
        SCRIPT,
        STATEMENT,
        COMMAND,
        OPERAND,
        LOGICAL_EXPRESSION,
        UNARY_EXPRESSION,
        BINARY_EXPRESSION,
    }

    // Abstract Syntax Tree Node
    public class ASTNode
    {
        public readonly ASTNodeType Type;
        public readonly string Lexeme;
        public readonly ASTNode Parent;
        public readonly int LineNumber;

        internal LinkedListNode<ASTNode> _node;
        private LinkedList<ASTNode> _children;

        public ASTNode(ASTNodeType type, string lexeme, ASTNode parent, int lineNumber)
        {
            Type = type;
            if (lexeme != null)
                Lexeme = lexeme;
            else
                Lexeme = "";
            Parent = parent;
            LineNumber = lineNumber;
        }

        public ASTNode Push(ASTNodeType type, string lexeme, int lineNumber)
        {
            var node = new ASTNode(type, lexeme, this, lineNumber);

            if (_children == null)
                _children = new LinkedList<ASTNode>();

            node._node = _children.AddLast(node);

            return node;
        }

        public ASTNode FirstChild()
        {
            if (_children == null || _children.First == null)
                return null;

            return _children.First.Value;
        }

        public ASTNode Next()
        {
            if (_node == null || _node.Next == null)
                return null;

            return _node.Next.Value;
        }

        public ASTNode Prev()
        {
            if (_node == null || _node.Previous == null)
                return null;

            return _node.Previous.Value;
        }
    }

    public static class Lexer
    {
        private static int _curLine = 0;

        public static T[] Slice<T>(this T[] src, int start, int end)
        {
            if (end < start)
                return new T[0];

            int len = end - start + 1;

            T[] slice = new T[len];
            for (int i = 0; i < len; i++)
            {
                slice[i] = src[i + start];
            }

            return slice;
        }

        public static ASTNode Lex(string[] lines)
        {
            ASTNode node = new ASTNode(ASTNodeType.SCRIPT, null, null, 0);

            try
            {
                for (_curLine = 0; _curLine < lines.Length; _curLine++)
                {
                    foreach (var l in lines[_curLine].Split(';'))
                    {
                        ParseLine(node, l);
                    }
                }
            }
            catch (SyntaxError e)
            {
                throw new SyntaxError(lines[_curLine], _curLine, e.Node, e.Message);
            }
            catch (Exception e)
            {
                throw new SyntaxError(lines[_curLine], _curLine, null, e.Message);
            }

            return node;
        }

        public static ASTNode Lex(string fname)
        {
            ASTNode node = new ASTNode(ASTNodeType.SCRIPT, null, null, 0);

            using (var file = new System.IO.StreamReader(fname))
            {
                _curLine = 0;
                string line = null;

                try
                {
                    while (true)
                    {
                        // Each line in the file is a statement. Statements starting
                        // with a control flow keyword contain an expression.

                        line = file.ReadLine();

                        // End of file
                        if (line == null)
                            break;

                        foreach (var l in line.Split(';'))
                            ParseLine(node, line);

                        _curLine++;
                    }
                }
                catch (SyntaxError e)
                {
                    throw new SyntaxError(line, _curLine, e.Node, e.Message);
                }
                catch (Exception e)
                {
                    throw new SyntaxError(line, _curLine, null, e.Message);
                }
            }

            return node;
        }

        private static TextParser _tfp = new TextParser("", new char[] { ' ' }, new char[] { }, new char[] { '\'', '\'', '"', '"' });
        private static void ParseLine(ASTNode node, string line)
        {
            line = line.Trim();

            if (line.StartsWith("//") || line.StartsWith("#"))
                return;

            // Split the line by spaces (unless the space is in quotes)
            var lexemes = _tfp.GetTokens(line, false);

            if (lexemes.Length == 0)
                return;

            ParseStatement(node, lexemes);
        }

        private static void ParseValue(ASTNode node, string lexeme, ASTNodeType typeDefault)
        {
            if (lexeme.StartsWith("0x"))
                node.Push(ASTNodeType.SERIAL, lexeme, _curLine);
            else if (int.TryParse(lexeme, out _))
                node.Push(ASTNodeType.INTEGER, lexeme, _curLine);
            else if (double.TryParse(lexeme, out _))
                node.Push(ASTNodeType.DOUBLE, lexeme, _curLine);
            else
                node.Push(typeDefault, lexeme, _curLine);
        }

        private static void ParseCommand(ASTNode node, string lexeme)
        {
            // A command may start with an '@' symbol. Pick that
            // off.
            if (lexeme[0] == '@')
            {
                node.Push(ASTNodeType.QUIET, null, _curLine);
                lexeme = lexeme.Substring(1, lexeme.Length - 1);
            }

            // A command may end with a '!' symbol. Pick that
            // off.
            if (lexeme.EndsWith("!"))
            {
                node.Push(ASTNodeType.FORCE, null, _curLine);
                lexeme = lexeme.Substring(0, lexeme.Length - 1);
            }

            node.Push(ASTNodeType.COMMAND, lexeme, _curLine);
        }

        private static void ParseOperand(ASTNode node, string lexeme)
        {
            bool modifier = false;

            // An operand may start with an '@' symbol. Pick that
            // off.
            if (lexeme[0] == '@')
            {
                node.Push(ASTNodeType.QUIET, null, _curLine);
                lexeme = lexeme.Substring(1, lexeme.Length - 1);
                modifier = true;
            }

            // An operand may end with a '!' symbol. Pick that
            // off.
            if (lexeme.EndsWith("!"))
            {
                node.Push(ASTNodeType.FORCE, null, _curLine);
                lexeme = lexeme.Substring(0, lexeme.Length - 1);
                modifier = true;
            }

            if (!modifier)
                ParseValue(node, lexeme, ASTNodeType.OPERAND);
            else
                node.Push(ASTNodeType.OPERAND, lexeme, _curLine);
        }

        private static void ParseOperator(ASTNode node, string lexeme)
        {
            switch (lexeme)
            {
                case "==":
                case "=":
                    node.Push(ASTNodeType.EQUAL, null, _curLine);
                    break;
                case "!=":
                    node.Push(ASTNodeType.NOT_EQUAL, null, _curLine);
                    break;
                case "<":
                    node.Push(ASTNodeType.LESS_THAN, null, _curLine);
                    break;
                case "<=":
                    node.Push(ASTNodeType.LESS_THAN_OR_EQUAL, null, _curLine);
                    break;
                case ">":
                    node.Push(ASTNodeType.GREATER_THAN, null, _curLine);
                    break;
                case ">=":
                    node.Push(ASTNodeType.GREATER_THAN_OR_EQUAL, null, _curLine);
                    break;
                default:
                    throw new SyntaxError(node, "Invalid operator in binary expression");
            }
        }

        private static void ParseStatement(ASTNode node, string[] lexemes)
        {
            var statement = node.Push(ASTNodeType.STATEMENT, null, _curLine);

            // Examine the first word on the line
            switch (lexemes[0])
            {
                // Ignore comments
                case "#":
                case "//":
                    return;

                // Control flow statements are special
                case "if":
                    {
                        if (lexemes.Length <= 1)
                            throw new SyntaxError(node, "Script compilation error");

                        var t = statement.Push(ASTNodeType.IF, null, _curLine);
                        ParseLogicalExpression(t, lexemes.Slice(1, lexemes.Length - 1));
                        break;
                    }
                case "elseif":
                    {
                        if (lexemes.Length <= 1)
                            throw new SyntaxError(node, "Script compilation error");

                        var t = statement.Push(ASTNodeType.ELSEIF, null, _curLine);
                        ParseLogicalExpression(t, lexemes.Slice(1, lexemes.Length - 1));
                        break;
                    }
                case "else":
                    if (lexemes.Length > 1)
                        throw new SyntaxError(node, "Script compilation error");

                    statement.Push(ASTNodeType.ELSE, null, _curLine);
                    break;
                case "endif":
                    if (lexemes.Length > 1)
                        throw new SyntaxError(node, "Script compilation error");

                    statement.Push(ASTNodeType.ENDIF, null, _curLine);
                    break;
                case "while":
                    {
                        if (lexemes.Length <= 1)
                            throw new SyntaxError(node, "Script compilation error");

                        var t = statement.Push(ASTNodeType.WHILE, null, _curLine);
                        ParseLogicalExpression(t, lexemes.Slice(1, lexemes.Length - 1));
                        break;
                    }
                case "endwhile":
                    if (lexemes.Length > 1)
                        throw new SyntaxError(node, "Script compilation error");

                    statement.Push(ASTNodeType.ENDWHILE, null, _curLine);
                    break;
                case "for":
                    {
                        if (lexemes.Length <= 1)
                            throw new SyntaxError(node, "Script compilation error");

                        ParseForLoop(statement, lexemes.Slice(1, lexemes.Length - 1));
                        break;
                    }
                case "foreach":
                    {
                        if (lexemes.Length != 4)
                            throw new SyntaxError(node, "Script compilation error");

                        ParseForEachLoop(statement, lexemes.Slice(1, lexemes.Length - 1));
                        break;
                    }
                case "endfor":
                    if (lexemes.Length > 1)
                        throw new SyntaxError(node, "Script compilation error");

                    statement.Push(ASTNodeType.ENDFOR, null, _curLine);
                    break;
                case "break":
                    if (lexemes.Length > 1)
                        throw new SyntaxError(node, "Script compilation error");

                    statement.Push(ASTNodeType.BREAK, null, _curLine);
                    break;
                case "continue":
                    if (lexemes.Length > 1)
                        throw new SyntaxError(node, "Script compilation error");

                    statement.Push(ASTNodeType.CONTINUE, null, _curLine);
                    break;
                case "stop":
                    if (lexemes.Length > 1)
                        throw new SyntaxError(node, "Script compilation error");

                    statement.Push(ASTNodeType.STOP, null, _curLine);
                    break;
                case "replay":
                case "loop":
                    if (lexemes.Length > 1)
                        throw new SyntaxError(node, "Script compilation error");

                    statement.Push(ASTNodeType.REPLAY, null, _curLine);
                    break;
                default:
                    // It's a regular statement.
                    ParseCommand(statement, lexemes[0]);

                    foreach (var lexeme in lexemes.Slice(1, lexemes.Length - 1))
                    {
                        ParseValue(statement, lexeme, ASTNodeType.STRING);
                    }
                    break;
            }

        }

        private static bool IsOperator(string lexeme)
        {
            switch (lexeme)
            {
                case "==":
                case "=":
                case "!=":
                case "<":
                case "<=":
                case ">":
                case ">=":
                    return true;
            }

            return false;
        }

        private static void ParseLogicalExpression(ASTNode node, string[] lexemes)
        {
            // The steam language supports logical operators 'and' and 'or'.
            // Catch those and split the expression into pieces first.
            // Fortunately, it does not support parenthesis.
            var expr = node;
            bool logical = false;
            int start = 0;

            for (int i = start; i < lexemes.Length; i++)
            {
                if (lexemes[i] == "and" || lexemes[i] == "or")
                {
                    if (!logical)
                    {
                        expr = node.Push(ASTNodeType.LOGICAL_EXPRESSION, null, _curLine);
                        logical = true;
                    }

                    ParseExpression(expr, lexemes.Slice(start, i - 1));
                    start = i + 1;
                    expr.Push(lexemes[i] == "and" ? ASTNodeType.AND : ASTNodeType.OR, null, _curLine);

                }
            }

            ParseExpression(expr, lexemes.Slice(start, lexemes.Length - 1));
        }

        private static void ParseExpression(ASTNode node, string[] lexemes)
        {

            // The steam language supports both unary and
            // binary expressions. First determine what type
            // we have here.

            bool unary = false;
            bool binary = false;

            foreach (var lexeme in lexemes)
            {
                if (lexeme == "not")
                {
                    // The not lexeme only appears in unary expressions.
                    // Binary expressions would use "!=".
                    unary = true;
                }
                else if (IsOperator(lexeme))
                {
                    // Operators mean it is a binary expression.
                    binary = true;
                }
            }

            // If no operators appeared, it's a unary expression
            if (!unary && !binary)
                unary = true;

            if (unary && binary)
                throw new SyntaxError(node, "Invalid expression");

            if (unary)
                ParseUnaryExpression(node, lexemes);
            else
                ParseBinaryExpression(node, lexemes);
        }

        private static void ParseUnaryExpression(ASTNode node, string[] lexemes)
        {
            var expr = node.Push(ASTNodeType.UNARY_EXPRESSION, null, _curLine);

            int i = 0;

            if (lexemes[i] == "not")
            {
                expr.Push(ASTNodeType.NOT, null, _curLine);
                i++;
            }

            ParseOperand(expr, lexemes[i++]);

            for (; i < lexemes.Length; i++)
            {
                ParseValue(expr, lexemes[i], ASTNodeType.STRING);
            }
        }

        private static void ParseBinaryExpression(ASTNode node, string[] lexemes)
        {
            var expr = node.Push(ASTNodeType.BINARY_EXPRESSION, null, _curLine);

            int i = 0;

            // The expressions on either side of the operator can be values
            // or operands that need to be evaluated.
            ParseOperand(expr, lexemes[i++]);

            for (; i < lexemes.Length; i++)
            {
                if (IsOperator(lexemes[i]))
                    break;

                ParseValue(expr, lexemes[i], ASTNodeType.STRING);
            }

            ParseOperator(expr, lexemes[i++]);

            ParseOperand(expr, lexemes[i++]);

            for (; i < lexemes.Length; i++)
            {
                if (IsOperator(lexemes[i]))
                    break;

                ParseValue(expr, lexemes[i], ASTNodeType.STRING);
            }
        }

        private static void ParseForLoop(ASTNode statement, string[] lexemes)
        {
            // There are 4 variants of for loops in steam. The simplest two just
            // iterate a fixed number of times. The other two iterate
            // parts of lists. We call those second two FOREACH.

            // We're intentionally deprecating two of the variants here.
            // The for X to Y variant, where both X and Y are integers,
            // is useless. It can be just written as for X.
            // The for X to Y in LIST variant may have some niche uses, but
            // is annoying to implement.

            // The for X loop remains supported as is, while the
            // for X in LIST form is actually transformed into a foreach
            // statement.

            if (lexemes.Length == 1)
            {
                // for X
                var loop = statement.Push(ASTNodeType.FOR, null, _curLine);

                ParseValue(loop, lexemes[0], ASTNodeType.STRING);

            }
            else if (lexemes.Length == 3 && lexemes[1] == "to")
            {
                // for X to LIST
                var loop = statement.Push(ASTNodeType.FOREACH, null, _curLine);

                loop.Push(ASTNodeType.STRING, lexemes[2], _curLine);
                loop.Push(ASTNodeType.LIST, lexemes[2].Substring(0, lexemes[2].Length - 2), _curLine);
            }
            else
            {
                throw new SyntaxError(statement, "Invalid for loop");
            }
        }

        private static void ParseForEachLoop(ASTNode statement, string[] lexemes)
        {
            // foreach X in LIST
            var loop = statement.Push(ASTNodeType.FOREACH, null, _curLine);

            if (lexemes[1] != "in")
                throw new SyntaxError(statement, "Invalid foreach loop");

            // This is the iterator name
            ParseValue(loop, lexemes[0], ASTNodeType.STRING);
            loop.Push(ASTNodeType.LIST, lexemes[2], _curLine);
        }
    }

    internal class TextParser
    {
        private readonly char[] _delimiters, _comments, _quotes;
        private int _eol;
        private int _pos;
        private int _Size;
        private string _string;
        private bool _trim;

        public TextParser(string str, char[] delimiters, char[] comments, char[] quotes)
        {
            _delimiters = delimiters;
            _comments = comments;
            _quotes = quotes;
            _Size = str.Length;
            _string = str;
        }

        internal bool IsDelimiter()
        {
            bool result = false;

            for (int i = 0; i < _delimiters.Length && !result; i++)
                result = _string[_pos] == _delimiters[i];

            return result;
        }

        private void SkipToData()
        {
            while (_pos < _eol && IsDelimiter())
                _pos++;
        }

        private bool IsComment()
        {
            bool result = _string[_pos] == '\n';

            for (int i = 0; i < _comments.Length && !result; i++)
            {
                result = _string[_pos] == _comments[i];

                if (result && i + 1 < _comments.Length && _comments[i] == _comments[i + 1] && _pos + 1 < _eol)
                {
                    result = _string[_pos] == _string[_pos + 1];
                    i++;
                }
            }

            return result;
        }

        private string ObtainData()
        {
            StringBuilder result = new StringBuilder();

            while (_pos < _Size && _string[_pos] != '\n')
            {
                if (IsDelimiter())
                    break;

                if (IsComment())
                {
                    _pos = _eol;

                    break;
                }

                if (_string[_pos] != '\r' && (!_trim || _string[_pos] != ' ' && _string[_pos] != '\t'))
                    result.Append(_string[_pos]);

                _pos++;
            }

            return result.ToString();
        }

        private string ObtainQuotedData()
        {
            bool exit = false;
            string result = "";

            for (int i = 0; i < _quotes.Length; i += 2)
            {
                if (_string[_pos] == _quotes[i])
                {
                    char endQuote = _quotes[i + 1];
                    exit = true;

                    int pos = _pos + 1;
                    int start = pos;

                    while (pos < _eol && _string[pos] != '\n' && _string[pos] != endQuote)
                    {
                        if (_string[pos] == _quotes[i]) // another {
                        {
                            _pos = pos;
                            ObtainQuotedData(); // skip
                            pos = _pos;
                        }

                        pos++;
                    }

                    _pos++;
                    int size = pos - start;

                    if (size > 0)
                    {
                        result = _string.Substring(start, size).TrimEnd('\r', '\n');
                        _pos = pos;

                        if (_pos < _eol && _string[_pos] == endQuote)
                            _pos++;
                    }

                    break;
                }
            }

            if (!exit)
                result = ObtainData();

            return result;
        }

        internal string[] GetTokens(string str, bool trim = true)
        {
            _trim = trim;
            List<string> result = new List<string>();

            _pos = 0;
            _string = str;
            _Size = str.Length;
            _eol = _Size - 1;

            while (_pos < _eol)
            {
                SkipToData();

                if (IsComment())
                    break;

                string buf = ObtainQuotedData();

                if (buf.Length > 0)
                    result.Add(buf);
            }

            return result.ToArray();
        }
    }
}
