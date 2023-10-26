using Assistant;
using Assistant.UI;
using System;
using System.Collections.Generic;
using System.Media;
using System.Threading;
using System.Collections.Concurrent;
using System.Linq;
using System.Drawing;
using static RazorEnhanced.HotKey;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using Accord.Imaging.Filters;
using IronPython.Runtime;
using System.Globalization;
using System.Web.UI.WebControls;
using Accord.Collections;
using Accord.Math;

namespace RazorEnhanced
{
    /// <summary>
    /// The CUO_Functions class contains invocation of CUO code using reflection
    /// DANGER !!
    /// </summary>
    public class CUO
    {
        /// <summary>
        /// Invokes the LoadMarkers function inside the CUO code
        /// Map must be open for this to work
        /// </summary>
        public static void LoadMarkers()
        {
            if (!Client.IsOSI)
            {
                // WorldMapGump worldMap = UIManager.GetGump<WorldMapGump>();
                var getAllGumps = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.Managers.UIManager")?.GetProperty("Gumps", BindingFlags.Public | BindingFlags.Static);
                if (getAllGumps != null)
                {
                    var listOfGumps = getAllGumps.GetValue(null);
                    if (listOfGumps != null)
                    {
                        IEnumerable<Object> temp = listOfGumps as IEnumerable<Object>;
                        foreach (var gump in temp)
                        {
                            if (gump != null)
                            {
                                var GumpType = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.UI.Gumps.Gump")?.GetProperty("GumpType", BindingFlags.Public | BindingFlags.Instance);
                                if (GumpType != null)
                                {
                                    int GumpTypeEnum = (int)GumpType.GetValue(gump);
                                    if (GumpTypeEnum == 18)
                                    {
                                        var WorldMapGump = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.UI.Gumps.WorldMapGump");
                                        if (WorldMapGump != null)
                                        {
                                            var LoadMarkers = WorldMapGump?.GetMethod("LoadMarkers", BindingFlags.Instance | BindingFlags.NonPublic);
                                            if (LoadMarkers != null)
                                            {
                                                LoadMarkers.Invoke(gump, null);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //PropertyInfo ProfileClass = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Configuration.Profile")?.GetProperty("AutoOpenDoors", BindingFlags.Public | BindingFlags.Instance);
                        //if (ProfileClass != null)
                        //{
                        //    ProfileClass.SetValue(profile, true, null);
                        //}
                    }
                }
            }
        }

        /// <summary>
        /// Invokes the GoToMarker function inside the CUO code
        /// Map must be open for this to work
        /// </summary>
        public static void GoToMarker(int x, int y)
        {
            if (!Client.IsOSI)
            {
                // WorldMapGump worldMap = UIManager.GetGump<WorldMapGump>();
                var getAllGumps = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.Managers.UIManager")?.GetProperty("Gumps", BindingFlags.Public | BindingFlags.Static);
                if (getAllGumps != null)
                {
                    var listOfGumps = getAllGumps.GetValue(null);
                    if (listOfGumps != null)
                    {
                        IEnumerable<Object> temp = listOfGumps as IEnumerable<Object>;
                        foreach (var gump in temp)
                        {
                            if (gump != null)
                            {
                                var GumpType = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.UI.Gumps.Gump")?.GetProperty("GumpType", BindingFlags.Public | BindingFlags.Instance);
                                if (GumpType != null)
                                {
                                    int GumpTypeEnum = (int)GumpType.GetValue(gump);
                                    if (GumpTypeEnum == 18)
                                    {
                                        var WorldMapGump = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.UI.Gumps.WorldMapGump");
                                        if (WorldMapGump != null)
                                        {
                                            var flags = BindingFlags.Default; //BindingFlags.Instance | BindingFlags.Public;
                                            foreach (var method in WorldMapGump.GetMethods())
                                            {
                                                if (method.Name == "GoToMarker")
                                                {
                                                    if (method.IsPublic)
                                                        flags |= BindingFlags.Public;
                                                    else
                                                        flags |= BindingFlags.NonPublic;
                                                    if (method.IsStatic)
                                                        flags |= BindingFlags.Static;
                                                    else
                                                        flags |= BindingFlags.Instance;
                                                }

                                            }
                                            var GoToMarker = WorldMapGump?.GetMethod("GoToMarker", BindingFlags.Instance | BindingFlags.Public);
                                            if (GoToMarker != null)
                                            {
                                                var parameters = new object[3] { x, y, true };
                                                GoToMarker.Invoke(gump, parameters);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Invokes the FreeView function inside the CUO code
        /// First value is retrieved, and then only set if its not correct
        /// </summary>
        public static void FreeView(bool free)
        {
            if (!Client.IsOSI)
            {
                // WorldMapGump worldMap = UIManager.GetGump<WorldMapGump>();
                var getAllGumps = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.Managers.UIManager")?.GetProperty("Gumps", BindingFlags.Public | BindingFlags.Static);
                if (getAllGumps != null)
                {
                    var listOfGumps = getAllGumps.GetValue(null);
                    if (listOfGumps != null)
                    {
                        IEnumerable<Object> temp = listOfGumps as IEnumerable<Object>;
                        foreach (var gump in temp)
                        {
                            if (gump != null)
                            {
                                var GumpType = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.UI.Gumps.Gump")?.GetProperty("GumpType", BindingFlags.Public | BindingFlags.Instance);
                                if (GumpType != null)
                                {
                                    int GumpTypeEnum = (int)GumpType.GetValue(gump);
                                    if (GumpTypeEnum == 18)
                                    {
                                        var WorldMapGump = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.UI.Gumps.WorldMapGump");
                                        if (WorldMapGump != null)
                                        {
                                            System.Reflection.PropertyInfo property = null;
                                            foreach (var propSearch in WorldMapGump.GetProperties())
                                            {
                                                if (propSearch.Name == "FreeView")
                                                {
                                                    property = propSearch;
                                                    break;
                                                }
                                            }
                                            if (property != null)
                                            {
                                                bool curr = (bool)property.GetValue(gump);
                                                if (curr != free)
                                                    property.SetValue(gump, free, null);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //PropertyInfo ProfileClass = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Configuration.Profile")?.GetProperty("AutoOpenDoors", BindingFlags.Public | BindingFlags.Instance);
                        //if (ProfileClass != null)
                        //{
                        //    ProfileClass.SetValue(profile, true, null);
                        //}
                    }
                }
            }
        }

        /// <summary>
        /// Invokes the CloseWithRightClick function inside the CUO code
        /// First T-Map is retrieved, and then only closed if it is a map
        /// Returns True if a map was closed, else False
        /// </summary>
        public static bool CloseTMap()
        {
            bool result = false;
            if (!Client.IsOSI)
            {
                // WorldMapGump worldMap = UIManager.GetGump<WorldMapGump>();
                var getAllGumps = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.Managers.UIManager")?.GetProperty("Gumps", BindingFlags.Public | BindingFlags.Static);
                if (getAllGumps != null)
                {
                    var listOfGumps = getAllGumps.GetValue(null);
                    if (listOfGumps != null)
                    {
                        IEnumerable<Object> temp = listOfGumps as IEnumerable<Object>;
                        foreach (var gump in temp)
                        {
                            if (gump != null)
                            {
                                var gumpClass = gump.GetType();
                                if (gumpClass.FullName == "ClassicUO.Game.UI.Gumps.MapGump")
                                {
                                    var MapGump = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.UI.Gumps.Gump");
                                    if (MapGump != null)
                                    {
                                        System.Reflection.MethodInfo method = null;
                                        var allMethods = MapGump.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
                                        foreach (var methSearch in allMethods)
                                        {
                                            if (methSearch.Name == "CloseWithRightClick")
                                            {
                                                method = methSearch;
                                                break;
                                            }
                                        }
                                        if (method != null)
                                        {
                                            var parameters = new object[0] { };
                                            method.Invoke(gump, parameters);
                                            result = true;
                                            Thread.Sleep(50);
                                            return result;
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Set a bool Config property in CUO by name
        /// </summary>
        public static void ProfilePropertySet(string propertyName, bool enable)
        {
            if (!Client.IsOSI)
            {
                var currentProfileProperty = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Configuration.ProfileManager")?.GetProperty("CurrentProfile", BindingFlags.Public | BindingFlags.Static);
                if (currentProfileProperty != null)
                {
                    var profile = currentProfileProperty.GetValue(null);
                    if (profile != null)
                    {
                        var profileClass = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Configuration.Profile");
                        System.Reflection.PropertyInfo property = null;
                        foreach (var propSearch in profileClass.GetProperties())
                        {
                            if (propSearch.Name == propertyName)
                            {
                                property = propSearch;
                                break;
                            }
                        }
                        if (property != null)
                        {
                            property.SetValue(profile, enable);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Set a int Config property in CUO by name
        /// </summary>
        public static void ProfilePropertySet(string propertyName, int value)
        {
            if (!Client.IsOSI)
            {
                var currentProfileProperty = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Configuration.ProfileManager")?.GetProperty("CurrentProfile", BindingFlags.Public | BindingFlags.Static);
                if (currentProfileProperty != null)
                {
                    var profile = currentProfileProperty.GetValue(null);
                    if (profile != null)
                    {
                        var profileClass = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Configuration.Profile");
                        System.Reflection.PropertyInfo property = null;
                        foreach (var propSearch in profileClass.GetProperties())
                        {
                            if (propSearch.Name == propertyName)
                            {
                                property = propSearch;
                                break;
                            }
                        }
                        if (property != null)
                        {
                            property.SetValue(profile, value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Set a string Config property in CUO by name
        /// </summary>
        public static void ProfilePropertySet(string propertyName, string value)
        {
            if (!Client.IsOSI)
            {
                var currentProfileProperty = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Configuration.ProfileManager")?.GetProperty("CurrentProfile", BindingFlags.Public | BindingFlags.Static);
                if (currentProfileProperty != null)
                {
                    var profile = currentProfileProperty.GetValue(null);
                    if (profile != null)
                    {
                        var profileClass = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Configuration.Profile");
                        System.Reflection.PropertyInfo property = null;
                        foreach (var propSearch in profileClass.GetProperties())
                        {
                            if (propSearch.Name == propertyName)
                            {
                                property = propSearch;
                                break;
                            }
                        }
                        if (property != null)
                        {
                            property.SetValue(profile, value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Set a location that CUO will open the container at
        /// </summary>
        public static void OpenContainerAt(Item bag, int x, int y)
        {
            OpenContainerAt((uint)bag.Serial, x, y);
        }

        /// <summary>
        /// Set a location that CUO will open the container at
        /// </summary>
        public static void OpenContainerAt(uint bag, int x, int y)
        {
            if (!Client.IsOSI)
            {
                SetGumpOpenLocation(bag, x, y);
                Items.UseItem((int)bag);
            }
        }

        /// <summary>
        /// Set a location that CUO will open the next gump or container at
        /// </summary>
        public static void SetGumpOpenLocation(uint gumpserial, int x, int y)
        {
            if (!Client.IsOSI)
            {

                System.Reflection.Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "FNA");
                if (assembly != null)
                {
                    Type Point = assembly.GetType("Microsoft.Xna.Framework.Point");
                    System.Reflection.ConstructorInfo ctor = Point.GetConstructor(new[] { typeof(int), typeof(int) });
                    var pos = ctor.Invoke(new object[] { x, y });
                    var SavePosition = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.Managers.UIManager")?.GetMethod("SavePosition", BindingFlags.Public | BindingFlags.Static);
                    if (SavePosition != null)
                    {
                        SavePosition.Invoke(assembly, new object[] { (uint)gumpserial, pos });
                    }
                }
            }
        }

        /// <summary>
        /// Invokes the Method move a gump or container if open.
        /// </summary>
        public static void MoveGump(uint serial, int x, int y)
        {
            if (!Client.IsOSI)
            {
                System.Reflection.Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "FNA");
                if (assembly != null)
                {
                    var getAllGumps = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.Managers.UIManager")?.GetProperty("Gumps", BindingFlags.Public | BindingFlags.Static);
                    if (getAllGumps != null)
                    {
                        var listOfGumps = getAllGumps.GetValue(null);
                        if (listOfGumps != null)
                        {
                            IEnumerable<Object> temp = listOfGumps as IEnumerable<Object>;
                            foreach (var gump in temp)
                            {
                                if (gump != null)
                                {
                                    var GumpType = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.UI.Gumps.Gump")?.GetProperty("GumpType", BindingFlags.Public | BindingFlags.Instance);
                                    if (GumpType != null)
                                    {
                                        int GumpTypeEnum = (int)GumpType.GetValue(gump);
                                      
                                        if (GumpTypeEnum == 0)
                                        {
                                            var Gump = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.UI.Gumps.Gump");
                                            if (Gump != null)
                                            {
                                                var prop = Gump.GetProperty("ServerSerial", BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
                                                if (prop != null)
                                                {
                                                    var gumpserial = prop.GetValue(gump);
                                                    if ((uint)gumpserial == serial)
                                                    {
                                                        var locprop = Gump.GetProperty("Location");

                                                        Type Point = assembly.GetType("Microsoft.Xna.Framework.Point");
                                                        System.Reflection.ConstructorInfo ctor = Point.GetConstructor(new[] { typeof(int), typeof(int) });
                                                        var pos = ctor.Invoke(new object[] { x, y });

                                                        if (locprop != null)
                                                        {
                                                            locprop.SetValue(gump, pos, null);
                                                        }
                                                    }
                                                }  
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Invokes the Method to close your status bar gump inside the CUO code
        /// </summary>
        public static void CloseMyStatusBar()
        {
            if (!Client.IsOSI)
            {
                System.Reflection.Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "FNA");
                if (assembly != null)
                {
                    var StatusGumpBase = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.UI.Gumps.StatusGumpBase");
                    if (StatusGumpBase != null)
                    {
                        var status = StatusGumpBase?.GetMethod("GetStatusGump", BindingFlags.Public | BindingFlags.Static);
                        if (status != null)
                        {
                            var gump = status.Invoke(assembly, new object[] { });

                            if(gump!= null)
                            {
                                var Gumps = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.UI.Gumps.Gump");
                                if (Gumps != null)
                                {
                                    MethodInfo Dispose = Gumps.GetMethod("Dispose");
                                    if (Dispose != null)
                                    {
                                        Dispose.Invoke(gump, new object[] { });
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Invokes the Method to open your status bar gump inside the CUO code
        /// </summary>
        public static void OpenMyStatusBar(int x, int y)
        {
            if (!Client.IsOSI)
            {
                CloseMyStatusBar();
                System.Reflection.Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "FNA");
                if (assembly != null)
                {
                    var StatusGumpBase = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.UI.Gumps.StatusGumpBase");
                    if (StatusGumpBase != null)
                    {
                        var status = StatusGumpBase?.GetMethod("AddStatusGump", BindingFlags.Public | BindingFlags.Static);
                        if (status != null)
                        {
                            var parameters = new object[2] { x, y };
                            var gump = status.Invoke(assembly, parameters);
                            
                            var uimanager = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.Managers.UIManager");
                            if (uimanager != null)
                            {
                                var add = uimanager?.GetMethod("Add", BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
                                 if(add != null)
                                {
                                    add.Invoke(assembly, new object[] { gump, true });
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Open a mobiles health bar at a specified location on the screen
        /// </summary>
        public static void OpenMobileHealthBar(int mobileserial, int x, int y, bool custom)
        {
            OpenMobileHealthBar((uint)mobileserial, x, y, custom);
        }

        /// <summary>
        /// Invokes the Method to open your status bar gump inside the CUO code
        /// </summary>
        public static void OpenMobileHealthBar(uint mobileserial, int x, int y, bool custom)
        {
            if (!Client.IsOSI)
            {
                System.Reflection.Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "FNA");
                if (assembly != null)
                {
                    if (custom)
                    {
                        var StatusGumpBase = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.UI.Gumps.HealthBarGumpCustom");
                        if (StatusGumpBase != null)
                        {
                            Type[] types = new Type[1];
                            types[0] = typeof(uint);

                            var status = StatusGumpBase?.GetConstructor(types);

                            if (status != null)
                            {
                                var parameters = new object[1] { (uint)mobileserial };
                                var gump = status.Invoke(parameters);

                                var uimanager = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.Managers.UIManager");
                                if (uimanager != null)
                                {
                                    var add = uimanager?.GetMethod("Add", BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
                                    if (add != null)
                                    {
                                        add.Invoke(assembly, new object[] { gump, true });

                                        var prop = StatusGumpBase.GetProperty("Location");

                                        Type Point = assembly.GetType("Microsoft.Xna.Framework.Point");
                                        System.Reflection.ConstructorInfo ctor = Point.GetConstructor(new[] { typeof(int), typeof(int) });
                                        var pos = ctor.Invoke(new object[] { x, y });

                                        if (prop != null)
                                        {
                                            prop.SetValue(gump, pos, null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var StatusGumpBase = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.UI.Gumps.HealthBarGump");
                        if (StatusGumpBase != null)
                        {
                            Type[] types = new Type[1];
                            types[0] = typeof(uint);

                            var status = StatusGumpBase?.GetConstructor(types);

                            if (status != null)
                            {
                                var parameters = new object[1] { (uint)mobileserial };
                                var gump = status.Invoke(parameters);

                                var uimanager = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.Managers.UIManager");
                                if (uimanager != null)
                                {
                                    var add = uimanager?.GetMethod("Add", BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
                                    if (add != null)
                                    {
                                        add.Invoke(assembly, new object[] { gump, true });

                                        var prop = StatusGumpBase.GetProperty("Location");

                                        Type Point = assembly.GetType("Microsoft.Xna.Framework.Point");
                                        System.Reflection.ConstructorInfo ctor = Point.GetConstructor(new[] { typeof(int), typeof(int) });
                                        var pos = ctor.Invoke(new object[] { x, y });

                                        if (prop != null)
                                        {
                                            prop.SetValue(gump, pos, null);
                                        }




                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Closes a Mobile Status Gump of an Entity
        /// </summary>
        public static void CloseMobileHealthBar(int mobileserial)
        {
            CloseMobileHealthBar((uint)mobileserial);
        }

        /// <summary>
        /// Closes a Mobile Status Gump of an Entity
        /// </summary>
        public static void CloseMobileHealthBar(uint mobileserial)
        {
            if (!Client.IsOSI)
            {
                var getAllGumps = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.Managers.UIManager")?.GetProperty("Gumps", BindingFlags.Public | BindingFlags.Static);
                if (getAllGumps != null)
                {
                    var listOfGumps = getAllGumps.GetValue(null);
                    if (listOfGumps != null)
                    {
                        IEnumerable<Object> temp = listOfGumps as IEnumerable<Object>;
                        foreach (var gump in temp)
                        {
                            if (gump != null)
                            {
                                var GumpType = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.UI.Gumps.Gump")?.GetProperty("GumpType", BindingFlags.Public | BindingFlags.Instance);
                                if (GumpType != null)
                                {
                                    int GumpTypeEnum = (int)GumpType.GetValue(gump);
                                    if (GumpTypeEnum == 4)
                                    {
                                        var HealthBarGump = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.UI.Gumps.BaseHealthBarGump");
                                        if (HealthBarGump != null)
                                        {
                                            var prop = HealthBarGump.GetProperty("LocalSerial", BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
                                            if (prop != null)
                                            {
                                                var gumpserial = prop.GetValue(gump);

                                                if ((uint)gumpserial == mobileserial)
                                                {
                                                    MethodInfo Dispose = HealthBarGump.GetMethod("Dispose");
                                                    if (Dispose != null)
                                                    {
                                                        Dispose.Invoke(gump, new object[] { });
                                                    }

                                                }
                                                
                                            }
                                        }
                                    }
                                   
                                }
                            }
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Invokes the Method close a gump
        /// </summary>
        public static void CloseGump(uint serial)
        {
            if (!Client.IsOSI)
            {
                var UIManager = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.Managers.UIManager");
                if (UIManager != null)
                {
                    MethodInfo GetGump = UIManager.GetMethods().Where(x => x.Name == "GetGump" && !x.IsGenericMethod).First();
                    if (GetGump != null)
                    {
                        var gump = GetGump.Invoke(ClassicUOClient.CUOAssembly, new object[] { serial });
                        if (gump != null && gump.GetType().FullName == "ClassicUO.Game.UI.Gumps.ContainerGump")
                        {
                            var Gumps = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.UI.Gumps.Gump");
                            if (Gumps != null)
                            {
                                MethodInfo Dispose = Gumps.GetMethod("Dispose");
                                if (Dispose != null)
                                {
                                    Dispose.Invoke(gump, new object[] { });
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Retrieve Current CUO Setting
        /// </summary>
        public static string GetSetting(string settingName)
        {
            if (!Client.IsOSI)
            {
                var currentSettingProperty = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Configuration.Settings")?.GetField("GlobalSettings", BindingFlags.Public | BindingFlags.Static);

                if (currentSettingProperty != null)
                {
                    var settings = currentSettingProperty.GetValue(null);
                    if (settings != null)
                    {
                        var SettingsClass = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Configuration.Settings");
                        PropertyInfo settingProperty = null;
                        foreach (var settingSearch in SettingsClass.GetProperties())
                        {
                            if (settingSearch.Name == settingName)
                            {
                                settingProperty = settingSearch;
                                break;
                            }
                        }
                        if (settingProperty != null)
                        {
                            var xxx = settingProperty.GetValue(settings);
                            return xxx.ToString();
                        }
                    }
                }

            }
            return "";

        }
        
        /// <summary>
        /// Play a CUO macro by name
        /// Warning, limited testing !! 
        /// </summary>
        public static void PlayMacro(string macroName)
        {
            if (!Client.IsOSI)
            {
                var Client = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Client");
                PropertyInfo Game = null;
                foreach (var g in Client.GetProperties())
                {
                    if (g.Name == "Game")
                    {
                        Game = g;
                        break;
                    }
                }
                if (Game != null)
                {
                    var game = Game.GetValue(Client, null);
                    if (game != null)
                    {
                        PropertyInfo Scene = null;
                        foreach (var s in game.GetType().GetProperties())
                        {
                            if (s.Name == "Scene")
                            {
                                Scene = s;
                                break;
                            }
                        }
                        if (Scene != null)
                        {
                            var scene = Scene.GetValue(game);
                            if (scene != null)
                            {
                                PropertyInfo Macros = null;
                                foreach (var m in scene.GetType().GetProperties())
                                {
                                    if (m.Name == "Macros")
                                    {
                                        Macros = m;
                                        break;
                                    }
                                }
                                if (Macros != null)
                                {
                                    var macros = Macros.GetValue(scene);
                                    if (macros != null)
                                    {
                                        MethodInfo FindMacro = null;
                                        MethodInfo SetMacroToExecute = null;
                                        MethodInfo WaitForTargetTimer = null;
                                        MethodInfo Update = null;
                                        foreach (var macroFn in macros.GetType().GetMethods())
                                        {
                                            if (macroFn.Name == "FindMacro" && macroFn.GetParameters().Length == 1) 
                                            {
                                                FindMacro = macroFn;
                                            }
                                            if (macroFn.Name == "SetMacroToExecute")
                                            {
                                                SetMacroToExecute = macroFn;
                                            }
                                            if (macroFn.Name == "set_WaitForTargetTimer")
                                            {
                                                WaitForTargetTimer = macroFn;
                                            }
                                            if (macroFn.Name == "Update")
                                            {
                                                Update = macroFn;
                                            }
                                        }
                                        if (FindMacro != null && SetMacroToExecute != null 
                                            && WaitForTargetTimer != null && Update != null)
                                        {
                                            var theMacro = FindMacro.Invoke(macros, new object[] { macroName });
                                            var fields = theMacro.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
                                            foreach (var field in fields)
                                            {
                                                if (field.Name == "Items")
                                                {
                                                    var items = field.GetValue(theMacro);
                                                    SetMacroToExecute.Invoke(macros, new object[] { items });
                                                    WaitForTargetTimer.Invoke(macros, new object[] { 0 });
                                                    //Update.Invoke(macros, new object[] { });
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
                
                var currentClient = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.Scenes.GameScene");
                //{ ClassicUO.Game.Managers.MacroManager}
                // int macroid = Client.Game.GetScene<GameScene>().Macros.GetAllMacros().IndexOf(_macro);



            }
            return;
        }

    }
}

