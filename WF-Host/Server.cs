using NPOI.SS.UserModel;
using OpenAI;
using PdfSharp.Drawing.BarCodes;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace WF_Host
{
    //"%windir%\Microsoft.NET\Framework64\v4.0.30319\regasm" /tlb  "$(TargetPath)"
    //"%windir%\Microsoft.NET\Framework64\v4.0.30319\regasm" /codebase "$(TargetPath)"
    //HKEY_LOCAL_MACHINE\SOFTWARE\SolidWorks\AddIns\
    //"%windir%\Microsoft.NET\Framework64\v4.0.30319\regasm" /tlb /codebase "$(TargetDir)/WF-Host.dll" 



    [ComVisible(true)]
    [Guid("FA0D3E0B-2747-492A-9C23-7055773A6218")]
    [ProgId("Server")]


    public class Server : ISwAddin, IServer
    {
        public const string SWTASKPANE_PROGID = "BS.Taskpane";
        public SldWorks swApp;
        private int cookie;
        private ICommandManager cmdMgr;
        private TaskpaneView mTaskPaneView;
        private HostUserControl userControl;
        public bool ConnectToSW(object ThisSW, int Cookie)
        {
            swApp = (SldWorks)ThisSW;
            cookie = Cookie;
            swApp.SetAddinCallbackInfo2(0, this, cookie);
            cmdMgr =(CommandManager)swApp.GetCommandManager(cookie);
            AddToolBar();
            AddMenuItem();
            AddContextMenu();
            // AddMyCommandGroup();
            // AddCommandTab();
            LoadUI();

            return true;
        }

        private void LoadUI()
        {
            string[] bitmap = new string[6];
            bitmap[0] = "C:\\Users\\ebabs\\source\\repos\\SWAddinByBS\\SWApp\\assets\\20x20.bmp";
            bitmap[1] = "C:\\Users\\ebabs\\source\\repos\\SWAddinByBS\\SWApp\\assets\\32x32.bmp";
            bitmap[2] = "C:\\Users\\ebabs\\source\\repos\\SWAddinByBS\\SWApp\\assets\\40x40.bmp";
            bitmap[3] = "C:\\Users\\ebabs\\source\\repos\\SWAddinByBS\\SWApp\\assets\\64x64.bmp";
            bitmap[4] = "C:\\Users\\ebabs\\source\\repos\\SWAddinByBS\\SWApp\\assets\\96x96.bmp";
            bitmap[5] = "C:\\Users\\ebabs\\source\\repos\\SWAddinByBS\\SWApp\\assets\\128x128.bmp";
            mTaskPaneView = swApp.CreateTaskpaneView3(bitmap, "SWAddin By BS");
            userControl = new HostUserControl();
            
            mTaskPaneView.DisplayWindowFromHandlex64(userControl.Handle.ToInt64());
            userControl = (HostUserControl)mTaskPaneView.AddControl(SWTASKPANE_PROGID, string.Empty);
        }

        public bool DisconnectFromSW()
        {
            CommandTab cmdTab = cmdMgr.GetCommandTab((int)swDocumentTypes_e.swDocPART, "SWAddin By BS");
            if (cmdTab != null)
            {
                cmdMgr.RemoveCommandTab(cmdTab);
            }
            userControl = null;
            mTaskPaneView.DeleteView();
            Marshal.ReleaseComObject(mTaskPaneView);
            Marshal.ReleaseComObject(userControl);
            mTaskPaneView = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            return true;
        }
        #region SOLIDWORKS Registration
        [ComRegisterFunction]
        public static void RegisterFunction(Type t)
        {
            //Class1Attribute SWattr = null;
            //Type type = typeof(Class1);
            //foreach (System.Attribute attr in type.GetCustomAttributes(false))
            //{
            //    if (attr is Class1Attribute)
            //    {
            //        SWattr = attr as Class1Attribute;
            //        break;
            //    }
            //}
            #endregion

            Microsoft.Win32.RegistryKey hklm = Microsoft.Win32.Registry.LocalMachine;
            Microsoft.Win32.RegistryKey hkcu = Microsoft.Win32.Registry.CurrentUser;

            string keyname = "SOFTWARE\\SOLIDWORKS\\Addins\\{" + t.GUID.ToString() + "}";
            Microsoft.Win32.RegistryKey addinkey = hklm.CreateSubKey(keyname);
            addinkey.SetValue(null, 0);

            addinkey.SetValue("Title", "BS SWAddin");
            addinkey.SetValue("Description", "Opisówka");

            keyname = "Software\\SOLIDWORKS\\AddInsStartup\\{" + t.GUID.ToString() + "}";
            addinkey = hkcu.CreateSubKey(keyname);
            addinkey.SetValue(null, 1, Microsoft.Win32.RegistryValueKind.DWord);
            string cos = "%~dp0\\WF-Host.dll";
            string openAitlb = "OpenAIClient.dll";

            System.Diagnostics.Process.Start("CMD.exe", "%windir%\\Microsoft.NET\\Framework64\\v4.0.30319\\regasm / tlb / codebase " + cos);
            System.Diagnostics.Process.Start("CMD.exe", "%windir%\\Microsoft.NET\\Framework64\\v4.0.30319\\regasm / tlb  " + openAitlb);
        }

        [ComUnregisterFunction]
        public static void UnregisterFunction(Type t)
        {
            Microsoft.Win32.RegistryKey hklm = Microsoft.Win32.Registry.LocalMachine;
            Microsoft.Win32.RegistryKey hkcu = Microsoft.Win32.Registry.CurrentUser;

            string keyname = "SOFTWARE\\SOLIDWORKS\\Addins\\{" + t.GUID.ToString() + "}";
            hklm.DeleteSubKey(keyname);

            keyname = "Software\\SOLIDWORKS\\AddInsStartup\\{" + t.GUID.ToString() + "}";
            hkcu.DeleteSubKey(keyname);
        }
        public void AddMenuItem()
        {
            Assembly thisAssembly = default(Assembly);
            int menuId = 0;
            string[] images = new string[3];

            thisAssembly = System.Reflection.Assembly.GetAssembly(this.GetType());

            images[0] = "C:\\Users\\ebabs\\source\\repos\\SWAddinByBS\\SWApp\\assets\\20x20.bmp";
            images[1] = "C:\\Users\\ebabs\\source\\repos\\SWAddinByBS\\SWApp\\assets\\96x96.bmp";
            images[2] = "C:\\Users\\ebabs\\source\\repos\\SWAddinByBS\\SWApp\\assets\\128x128.bmp";

            menuId = swApp.AddMenu((int)swDocumentTypes_e.swDocPART, "MyMenu", 0);
            menuId = swApp.AddMenuItem5((int)swDocumentTypes_e.swDocPART, cookie, "MenuItem@MyMenu", 0, "MyMenuCallback", "MyMenuEnableMethod", "nowa ikonka", images);

            thisAssembly = null;

        }
        private void AddContextMenu()
        {
            int errors = default;
            // Tworzenie grupy narzędziowej
            CommandGroup cmdGroup = cmdMgr.CreateCommandGroup2(1, "Moja Grupa","Tooltip", "Opis grupy", -1, false, ref errors);

            // Rejestracja ikony i komendy w grupie
            int cmdIndex = cmdGroup.AddCommandItem("Moja Ikona", -1, "Opis ikony", "Podpowiedź", 0, "MyContextMenuFunction", "", (int)swCommandItemType_e.swMenuItem);

            cmdGroup.Activate();

            cmdMgr.AddContextMenu((int)swSelectType_e.swSelCOMPONENTS, "totylkotytuł");

        }
        public void AddCommandTab()
        {
            CommandTab cmdTab = cmdMgr.GetCommandTab((int)swDocumentTypes_e.swDocPART, "SWAddin By BS");
            if (cmdTab != null)
            {
                // Jeśli istnieje, usuń ją i dodaj nową
                cmdMgr.RemoveCommandTab(cmdTab);
            }

            // Tworzenie nowej karty
            cmdTab = cmdMgr.AddCommandTab(cookie, "SWAddin By BS");

            if (cmdTab != null)
            {
                CommandTabBox cmdTabBox = cmdTab.AddCommandTabBox();

                int[] cmdIDs = new int[] { 1 };
                int[] TextTypes = new int[] { (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow };

                // Dodaj grupę przycisków do karty
                cmdTabBox.AddCommands(cmdIDs, TextTypes);
            }
        }
        private void AddMyCommandGroup()
        {
            // Tworzenie grupy narzędziowej
            CommandGroup cmdGroup = cmdMgr.CreateCommandGroup(1, "Moja Grupa", "Opis grupy", "Podpowiedź", -1);

            cmdGroup.HasToolbar = true;
            cmdGroup.HasMenu = true;

            // Dodanie przycisków do grupy
            int cmdIndex = cmdGroup.AddCommandItem2("Nazwa Przycisku", -1, "Opis", "Podpowiedź", 0, "MojaFunkcja", "",0, (int)swCommandItemType_e.swMenuItem);

            // Aktywacja grupy
            cmdGroup.Activate();
        }
        public void MyMenuCallback()
        {
            MessageBox.Show("Callback function called.");
        }
        public void ButtonCallback()
        {
            MessageBox.Show("Button callback function called.");
        }
        public int ButtonEnableMethod()
        {
            return 1;
        }

        public void AddToolBar()
        {
            Assembly thisAssembly1;
            string[] toolbarImages = new string[3];
            bool bret = false;
            int iToolbarId = 0;

            thisAssembly1 = System.Reflection.Assembly.GetAssembly(this.GetType());

            toolbarImages[0] = "C:\\Users\\ebabs\\source\\repos\\SWAddinByBS\\SWApp\\assets\\20x20.bmp";
            toolbarImages[1] = "C:\\Users\\ebabs\\source\\repos\\SWAddinByBS\\SWApp\\assets\\96x96.bmp";
            toolbarImages[2] = "C:\\Users\\ebabs\\source\\repos\\SWAddinByBS\\SWApp\\assets\\128x128.bmp";

            iToolbarId = swApp.AddToolbar5(cookie, "Test ToolbarxD", toolbarImages, 0, (int)swDocTemplateTypes_e.swDocTemplateTypeASSEMBLY);

            bret = swApp.AddToolbarCommand2(cookie, iToolbarId, 0, "ButtonCallback", "ButtonEnableMethod", "Test toolbar ToolTip", "Hint string for test toolbar");

        }

    }
}
