using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using System;
using System.Runtime.InteropServices;


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
        private TaskpaneView mTaskPaneView;
        private HostUserControl userControl;
        public bool ConnectToSW(object ThisSW, int Cookie)
        {
            swApp = (SldWorks)ThisSW;
            swApp.SetAddinCallbackInfo2(0, this, Cookie);


            LoadUI();

            return true;
        }

        private void LoadUI()
        {
            string[] bitmap = new string[6];
            bitmap[0] = "C:\\Users\\ebabs\\Downloads\\20x20.bmp";
            bitmap[1] = "C:\\Users\\ebabs\\Downloads\\32x32.bmp";
            bitmap[2] = "C:\\Users\\ebabs\\Downloads\\40x40.bmp";
            bitmap[3] = "C:\\Users\\ebabs\\Downloads\\64x64.bmp";
            bitmap[4] = "C:\\Users\\ebabs\\Downloads\\96x96.bmp";
            bitmap[5] = "C:\\Users\\ebabs\\Downloads\\128x128.bmp";
            mTaskPaneView = swApp.CreateTaskpaneView3(bitmap, "SWAddin By BS");
            userControl = new HostUserControl();
            mTaskPaneView.DisplayWindowFromHandlex64(userControl.Handle.ToInt64());
            userControl = (HostUserControl)mTaskPaneView.AddControl(SWTASKPANE_PROGID, string.Empty);
        }

        public bool DisconnectFromSW()
        {
            userControl = null;
            mTaskPaneView.DeleteView();
            Marshal.ReleaseComObject(mTaskPaneView);
            Marshal.ReleaseComObject(userControl);
            mTaskPaneView = null;
            GC.Collect();
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
            addinkey.SetValue("Description", "Użytek tylko dla wybranych");

            keyname = "Software\\SOLIDWORKS\\AddInsStartup\\{" + t.GUID.ToString() + "}";
            addinkey = hkcu.CreateSubKey(keyname);
            addinkey.SetValue(null, 1, Microsoft.Win32.RegistryValueKind.DWord);
            string cos = "%~dp0\\WF-Host.dll";

            System.Diagnostics.Process.Start("CMD.exe", "%windir%\\Microsoft.NET\\Framework64\\v4.0.30319\\regasm / tlb / codebase " + cos);
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

        public double ComputePi()
        {
            throw new NotImplementedException();
        }
    }
}
