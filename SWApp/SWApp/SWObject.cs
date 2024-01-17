using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Text.RegularExpressions;

namespace SWApp
{
   public class SWObject
    {

        List<string> swTreeFilenames = new List<string>();
        public SldWorks swApp = new SldWorks();
        public ModelDoc2 swModel;
        PartDoc swPart;
        SheetMetalFeatureData swSheetMetalData;
        Feature swFeature;
        ModelDocExtension swModelExt;
        DrawingDoc drawingDoc;

        public List<string> GetDirectory()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = "Złożenie";
            saveFileDialog1.DefaultExt = ".SLDASM";
            saveFileDialog1.Filter = "Pliki złożenie Solidworks (*.SLDASM) |*.SLDASM";

            saveFileDialog1.ShowDialog();
            string filename = saveFileDialog1.FileName;
            string assemblyName = System.IO.Path.GetFileNameWithoutExtension(filename);
            string dirFolder = System.IO.Path.GetDirectoryName(filename);
            List<string> filePathName = new List<string>();

            filePathName.Add(filename);
            filePathName.Add(assemblyName);
            filePathName.Add(dirFolder);

            return filePathName;
        }
        public void CreateAssembly(string filepath)
        {
            SldWorks swApp = new SldWorks();
            ModelDoc2 swAssembly;
            
            swAssembly = swApp.NewDocument("C:\\EBA\\SZABLONY\\Solidworks\\EBA_Złożenie.asmdot", 0, 0, 0);
            swAssembly.SaveAs3(filepath, 0, 8);

        }

        public void AddToAssembly(string filepath,string assemblyName)
        {

            SldWorks swApp = new SldWorks();
            ModelDoc2 swModel;
            AssemblyDoc swAssemblyDoc;

            swModel = swApp.ActivateDoc3(assemblyName,false,2,0);
            swAssemblyDoc = (AssemblyDoc)swModel;

            swAssemblyDoc.AddComponent5(filepath, 0, "", false, "", 0, 0, 0);
            swModel.Save3(4, 0, 0);
            
        }

        public string GetDirDXF()
        {
            try
            {
                var dialog = new CommonOpenFileDialog();
                dialog.IsFolderPicker = true;
                CommonFileDialogResult result = dialog.ShowDialog();
                string filepath = dialog.FileName;
                return filepath;
            }

            catch (System.InvalidOperationException)
            {
                return null;
            }
        }
        public void saveDXFFromDrawing(string filedir, bool isDefaultDir)
        {
            swModel = swApp.ActiveDoc;
            swModelExt = swModel.Extension;
            string assemblyTitle = swModel.GetTitle();
            string assemblyFilepath = swModel.GetPathName();
            string assemblyDrawingFilepath = assemblyFilepath.Remove(assemblyFilepath.Length - 6,6) + "SLDDRW";

            if (filedir == "" || isDefaultDir == true)
            {
                int signs = assemblyTitle.Count() + 7;
                filedir = swModel.GetPathName();
                filedir = filedir.Remove(filedir.Length - signs, signs);
            }
            //else
            //{
            //    filedir = filedir.Replace("/", "//");
            //}

            swModel = swApp.OpenDoc6(assemblyDrawingFilepath, 3, 2, "", 0, 0);
            drawingDoc = (DrawingDoc)swModel;
            swModel = swApp.ActiveDoc;
            swModelExt = swModel.Extension;
            string[] sheetNames = drawingDoc.GetSheetNames();


            drawingDoc.ActivateSheet(sheetNames[0]);

            for(int i = 0;i< sheetNames.Length; i++)
            {
                string sheetName = sheetNames[i];
                string modelName = System.IO.Path.GetFileNameWithoutExtension(swModel.GetPathName());

                if (sheetName.Contains("dxf".ToUpper()) == true)
                {
                    swModelExt.SaveAs3($"{filedir}\\{modelName}_{i}.DXF", 0, 2, null, null, 16, 1);
                }
                drawingDoc.SheetNext();
            }
            swApp.CloseDoc(assemblyDrawingFilepath);
        }
        public void SaveDXF(string modelFilepath, string filename, string filedir, List<string> totalParts, int options)
        {
            try
            {
                swModel = swApp.OpenDoc6(modelFilepath, 1, 2, "", 0, 0);
                swApp.ActivateDoc3(filename, true, 2, 0);
                swModel = swApp.ActiveDoc;
                swPart = (PartDoc)swModel;
                swModelExt = swModel.Extension;
                swFeature = swModelExt.GetTemplateSheetMetal();
                swSheetMetalData = swFeature.GetDefinition();
                if (swSheetMetalData != null)
                {
                    var thickness = 1000 * swSheetMetalData.Thickness; //getting thickness in m, not mm
                    var quantity = totalParts.Where(x => x == filename).Count();
                    var material = swPart.MaterialIdName.Split('|')[1];
                    swPart.ExportToDWG2($"{filedir}\\{thickness}{material}_{filename}_{quantity}-szt.DXF", modelFilepath, 1, true, null, true, true, 73, null);
                }
            }
            catch (System.NullReferenceException)
            {

            }
            finally
            {
                swTreeFilenames.Add(filename);
                swApp.CloseDoc(filename);
            }
        }
        public void GenerateDXF(Component2 swComp,string filedir, bool isDefaultDir, int options, List<string> totalParts)
        {
            SldWorks swApp = new SldWorks();
            ModelDoc2 swModel;
            Component2 swChildComp;
            object[] swChildComps;
            string modelFilepath;
            string filename;
            string assemblyTitle;

            swModel = swApp.ActiveDoc;
            assemblyTitle = swModel.GetTitle();

            if (filedir == "" || isDefaultDir == true)
            {
                int signs = assemblyTitle.Count() + 7;
                filedir = swModel.GetPathName();
                filedir = filedir.Remove(filedir.Length - signs , signs);
            }
            swChildComps = swComp.GetChildren();

            for (int i = 0; i < swChildComps.Length; i++)
            {
                swChildComp = (Component2)swChildComps[i];
                swModel = swChildComp.GetModelDoc2();
                modelFilepath = swModel.GetPathName();
                filename = swModel.GetTitle();
                
                if(swModel.GetType() == 1)
                {
                    if (swTreeFilenames.FirstOrDefault(x => x.Contains(filename)) == null)
                    {
                        switch (options)
                        {
                            case 1:
                                if (filename.Substring(0, 2) == "PB".ToUpper())
                                {
                                    SaveDXF(modelFilepath, filename, filedir, totalParts, options);
                                }
                                break;
                            case 2:
                                if (filename.Substring(0, 2) == "PT".ToUpper())
                                {
                                    SaveDXF(modelFilepath, filename, filedir, totalParts, options);
                                }
                                break;
                            case 3:
                                if (filename.Substring(0, 2) == "PT".ToUpper() || filename.Substring(0, 2) == "PB".ToUpper())
                                {
                                    SaveDXF(modelFilepath, filename, filedir, totalParts, options);
                                }
                                break;
                            case 0:
                                break;
                            case 8:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                                SaveDXF(modelFilepath, filename, filedir, totalParts, options);
                                break;
                        }
                    }
                }
                GenerateDXF(swChildComp,filedir, isDefaultDir, options,totalParts);
            }
        }

        public void CloseSWDoc(string Name)
        {
            SldWorks swApp = new SldWorks();
            swApp.CloseDoc(Name);
        }
        public List<string> CountParts()
        {
            SldWorks swApp = new SldWorks();
            AssemblyDoc swAss;
            ModelDoc2 swModel;
            swAss = swApp.ActiveDoc;

            var obj = swAss.GetComponents(false); //false for take all parts from main assembly and subassemblies
            List<string> totalParts = new List<string>();

            foreach (Component2 comp in obj)
            {
                comp.SetSuppression2(3);
                swModel = comp.GetModelDoc2();
                totalParts.Add(swModel.GetTitle());
            }
            return totalParts;

        }
        
        public void CreateRectangleProfile(ProfileSW profileSW, string filepath)
        {
            SldWorks swApp = new SldWorks();
            ModelDoc2 swModel;
            SketchManager skManager;
            bool boolstatus;
            ModelDocExtension swExt;
            SweepFeatureData swSweep;
            FeatureManager swFeatMgr;

            double x = Convert.ToDouble(profileSW.X);
            double y = Convert.ToDouble(profileSW.Y);
            string partName = Convert.ToString(profileSW.Name);
            double thickness = Convert.ToDouble(profileSW.Thickness);
            double length = Convert.ToDouble(profileSW.Length);
            int draftCount = Convert.ToInt32(profileSW.DraftCount);

            swModel = swApp.NewDocument("C:\\EBA\\SZABLONY\\Solidworks\\EBA_Część.prtdot", 0, 0, 0);
            swModel.SaveAs3($"{filepath}{partName}.SLDPRT", 0, 8);
            
            swExt = swModel.Extension;
            skManager = swModel.SketchManager;

            //Suppress the dimension dialog box
            swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, false);

            //creats a line of the length of the pipe
            boolstatus = swExt.SelectByID2("Płaszczyzna przednia", "PLANE", 0, 0, 0, false, 0, null, 0); //choosing depends on a name in first ""
            skManager.InsertSketch(true);
            skManager.CreateLine(-length/2000, 0, 0, length/2000, 0, 0); //the unit is m NOT mm
            swModel.IAddDiameterDimension2(length/2000, 0, 0); //TODO on x dim it should be l/2 of the line

            //relation between middle and datum point (pipe)
            boolstatus = swExt.SelectByID2("Line1", "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            boolstatus = swExt.SelectByID2("", "EXTSKETCHPOINT", 0, 0, 0, true, 0, null, 0);
            swModel.SketchAddConstraints("sgATMIDDLE");
            skManager.InsertSketch(true); //closes a sketch

            //creats a cross-section of the pipe
            boolstatus = swExt.SelectByID2("Płaszczyzna prawa", "PLANE", 0, 0, 0, false, 0, null, 0); //choosing depends on a name in first ""
            skManager.InsertSketch(true);
            skManager.CreateCenterRectangle(0, 0, 0, x/2000, y/2000, 0); //the unit is m NOT mm
            swModel.ClearSelection2(true);

            //sometimes center of axis "loses" relation with the middle of the rectangle --> need to add it again
            boolstatus = swExt.SelectByID2("Line5", "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            boolstatus = swExt.SelectByID2("", "EXTSKETCHPOINT", 0, 0, 0, true, 0, null, 0);
            swModel.SketchAddConstraints("sgATMIDDLE");

            //adds dimensions to one side
            boolstatus = swExt.SelectByID2("Line1", "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            swModel.IAddDiameterDimension2(length/200, 0.01, 0); //TODO on x dim it should be l/2 of the line 
            swModel.ClearSelection2(true);

            //adds dimensions to other side
            boolstatus = swExt.SelectByID2("Line2", "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            swModel.IAddDiameterDimension2(length/200, 0.1, 0); //TODO on x dim it should be l/2 of the line
            swModel.ClearSelection2(true);

            //fillets corners
            boolstatus = swExt.SelectByID2("Point2", "SKETCHPOINT", 0, 0, 0, false, 0, null, 0);
            boolstatus = swExt.SelectByID2("Point3", "SKETCHPOINT", 0, 0, 0, true, 0, null, 0);
            boolstatus = swExt.SelectByID2("Point4", "SKETCHPOINT", 0, 0, 0, true, 0, null, 0);
            boolstatus = swExt.SelectByID2("Point5", "SKETCHPOINT", 0, 0, 0, true, 0, null, 0);
            skManager.CreateFillet((thickness/1000)+(0.5/1000), 1);
            swModel.ClearSelection2(true);

            //chain offset
            boolstatus = swExt.SelectByID2("Line1", "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            skManager.SketchOffset2(-thickness/1000, false, true, 0, 0, true);

            //closes a sketch
            skManager.InsertSketch(true);

            //Creating extrude from profile and path (sweep)
            swExt.SelectByID2("Szkic1", "SKETCH", 0, 0, 0, false, 0, null, 0);
            swExt.SelectByID2("Szkic2", "SKETCH", 0, 0, 0, true, 0, null, 0);
            swFeatMgr = swModel.FeatureManager;
            swSweep = (SweepFeatureData)swFeatMgr.CreateDefinition((int)swFeatureNameID_e.swFmSweep); //from enum :swFmSweep
            swSweep.Direction = 1;
            swFeatMgr.CreateFeature(swSweep);

            //Create draft at the ends of the pipe if needed
            swModel.ClearSelection2(true);

            if (draftCount == 1)
            {
                swExt.SelectByID2("", "FACE", length / 2000, (y / 2000) - (thickness / 2000), 0, false, 2, null, 0); //side face
                swModel.ViewRotateplusx(); // don't know why need to slighty rotate view in SW to select another face now
                swExt.SelectByID2("", "FACE", 0, y / 2000, 0, true, 1, null, 0); //neutral face
                swFeatMgr.InsertMultiFaceDraft(0.785, true, false, 0, false, false);
            }
            else if(draftCount == 2)
            {
                swExt.SelectByID2("", "FACE", length/2000, (y/2000)-(thickness/2000), 0, false, 2, null, 0); //side face
                swModel.ViewRotateplusx(); // don't know why need to slighty rotate view in SW to select another face now
                swExt.SelectByID2("", "FACE", -length/2000, (y / 2000) - (thickness / 2000), 0, true, 2, null, 0); //side face
                swExt.SelectByID2("", "FACE", 0, y/2000, 0, true, 1, null, 0); //neutral face

                swFeatMgr.InsertMultiFaceDraft(Math.PI *0.25, true, false, 0, false, false);
            }
            else if(draftCount == 0)
            {
                swModel.ViewRotateplusx(); // don't know why need to slighty rotate view in SW to select another face now
            }
            else
            {
                MessageBox.Show("Wprowadź wartość od 0 do 2");
            }

            swModel.Save3(4, 0, 0);

            //Unsuppress the dimension dialog box
            swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, true);
            //swApp.ExitApp();

        }
    }
}
