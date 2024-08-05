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
using System.Data;
using System.Timers;
using System.Windows.Media.Media3D;
using System.Collections;
using System.Runtime.InteropServices;
using NPOI.OpenXmlFormats.Dml.Picture;
using System.Globalization;
using System.ComponentModel;
using PdfSharp.Pdf;
using Path = System.IO.Path;
using PdfSharp.Pdf.IO;
using NPOI.SS.Formula.PTG;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using SWApp.Models;
using SWApp.Viewmodels;
using SWApp.Services;
using Wpf.Ui.Controls;
using System.Linq.Expressions;
using Aspose.Pdf.Operators;

namespace SWApp
{

    //[RunInstaller(true)]
    public class SWObject /*: System.Configuration.Install.Installer*/
    {

        private SldWorks _swApp;
        string installDir;
        ModelDoc2 swModel;
        PartDoc swPart;
        SheetMetalFeatureData swSheetMetalData;
        Feature swFeature;
        ModelDocExtension swModelExt;
        DrawingDoc drawingDoc;
        AssemblyDoc swAss;
        SketchManager skManager;
        ModelDocExtension swExt;
        SweepFeatureData swSweep;
        FeatureManager swFeatMgr;
        Component2 swComp;
        CustomPropertyManager swCustomPropMgr;
        Configuration swConfig;
        Configuration swAssConfig;
        MassProperty2 swMassProp;
        private string _templateAssemblyPath;
        private string _templatePartPath;
        private string _systemPath;
        private HelpService _helpService;
        //public override void Install(System.Collections.IDictionary stateSaver)
        //{
        //    base.Install(stateSaver);
        //    string installPath = this.Context.Parameters["targetdir"];

        //    installDir = installPath;
        //}

        Dictionary<string, string> dicPolEng = new Dictionary<string, string>()
        {
            {"PRZEKRÓJ","CROSS-SECTION" },
            {"SZCZEGÓŁ","DETAIL" },
            {"SKALA","SCALE" },
            {"MONTAŻ","INSTALLATION" }
        };
        private readonly string allOperationsstr = File.ReadAllText("C:\\Users\\ebabs\\source\\repos\\SWAddinByBS\\SWApp\\assets\\Operations.json");

        public SWObject()
        {
            _swApp = (SldWorks)Marshal2.GetActiveObject("SldWorks.Application");
            _systemPath = System.IO.Path.GetPathRoot(System.Environment.GetFolderPath(System.Environment.SpecialFolder.System));
            _templateAssemblyPath = ($"{_systemPath}EBA\\SZABLONY\\Solidworks\\EBA_Złożenie.asmdot");
            _templatePartPath = ($"{_systemPath}EBA\\SZABLONY\\Solidworks\\EBA_Część.prtdot");
            _helpService = new HelpService();
        }

        public event Action<string> ErrorOccurred;

        //public (string,string,string,List<SWFileProperties>) ReadData()
        //{
        //    try
        //    {
        //        SldWorks swApp = (SldWorks)Marshal2.GetActiveObject("SldWorks.Application");
        //        ModelDoc2 swModel;
        //        ConfigurationManager swConfMgr;
        //        swAss = (AssemblyDoc)swApp.ActiveDoc;


        //        //get data for the assembly filepath and configuration
        //        swModel = (ModelDoc2)swApp.ActiveDoc;
        //        swModelExt = swModel.Extension;
        //        string path = swModel.GetPathName();
        //        string assemblyConfig;
        //        swConfMgr = swModel.ConfigurationManager;
        //        swAssConfig = (Configuration)swConfMgr.ActiveConfiguration;
        //        assemblyConfig = swAssConfig.Name;
        //        swCustomPropMgr = swModelExt.get_CustomPropertyManager("");

        //        List<SWFileProperties> swFilesProperties = new List<SWFileProperties>();

        //        object[] swComps;
        //        string name;
        //        string valOutDescription;
        //        string valOutDrawingNum;
        //        string valOutQty;
        //        string resolvedValOut;
        //        string material;
        //        string resolvedMaterial;
        //        string mass;
        //        string resolvedMass;
        //        string surface;
        //        string paintQty;
        //        string comments;
        //        string thickness;
        //        string resolvedThickness;
        //        bool wasResolved;
        //        bool linkToProperty;
        //        string configName;
        //        string status;
        //        string createdBy;
        //        string checkedBy;
        //        bool dxfExist;
        //        bool stepExist;


        //        swCustomPropMgr.Get6("index xl", false, out valOutDescription, out resolvedValOut, out wasResolved, out linkToProperty);
        //        string index = valOutDescription;

        //        MessageBoxResult suppressedChoice = default(MessageBoxResult);

        //        swComps = (object[])swAss.GetComponents(false); //true for all comps in the assembly
        //        List<string> doneswComps = new List<string>();
        //        bool hasSuppresed = swAss.HasUnloadedComponents();
        //        int lightWeightCompsCount = swAss.GetLightWeightComponentCount();
        //        if (hasSuppresed || lightWeightCompsCount != 0)
        //        {
        //            suppressedChoice = MessageBox.Show("Wykryto pliki w odciążeniu i/lub wygaszone. Czy chcesz przywrócić je do pełnej pamięci i je wygenerować?", "Złożenie posiada wygaszone pliki", MessageBoxButton.YesNo);
        //        }


        //        //adding rows for each component

        //        foreach (Component2 swComp in swComps)
        //        {
        //            try
        //            {
        //                if (suppressedChoice == MessageBoxResult.Yes && swComp.IsSuppressed() == true)
        //                {
        //                    swComp.SetSuppression2((int)swComponentSuppressionState_e.swComponentFullyResolved);
        //                }
        //                swModel = (ModelDoc2)swComp.GetModelDoc2();
        //                string compFilepath = swModel.GetPathName();
        //                swModelExt = swModel.Extension;
        //                swConfMgr = swModel.ConfigurationManager;
        //                swConfig = (Configuration)swConfMgr.ActiveConfiguration;
        //                configName = swConfig.Name;
        //                swCustomPropMgr = swModelExt.get_CustomPropertyManager("");
        //                name = System.IO.Path.GetFileNameWithoutExtension(swModel.GetPathName());
        //                if (doneswComps.Contains(name) == false)
        //                {
        //                    doneswComps.Add(name);
        //                    swCustomPropMgr.Get6("opis", false, out valOutDescription, out resolvedValOut, out wasResolved, out linkToProperty);
        //                    swCustomPropMgr.Get6("material", false, out material, out resolvedMaterial, out wasResolved, out linkToProperty);
        //                    swCustomPropMgr.Get6("grubosc materialu", false, out thickness, out resolvedThickness, out wasResolved, out linkToProperty);
        //                    swCustomPropMgr.Get6("masa", false, out mass, out resolvedMass, out wasResolved, out linkToProperty);
        //                    swCustomPropMgr.Get6("powierzchnia dm2", false, out surface, out resolvedValOut, out wasResolved, out linkToProperty);
        //                    swCustomPropMgr.Get6("ilosc farby", false, out paintQty, out resolvedValOut, out wasResolved, out linkToProperty);
        //                    swCustomPropMgr.Get6("nr rysunku", false, out valOutDrawingNum, out resolvedValOut, out wasResolved, out linkToProperty);
        //                    swCustomPropMgr.Get6("szt na kpl", false, out valOutQty, out resolvedValOut, out wasResolved, out linkToProperty);
        //                    swCustomPropMgr.Get6("uwagi", false, out comments, out resolvedValOut, out wasResolved, out linkToProperty);
        //                    swCustomPropMgr.Get6("status dokumentacji", false, out status, out resolvedValOut, out wasResolved, out linkToProperty);
        //                    swCustomPropMgr.Get6("utworzyl", false, out createdBy, out resolvedValOut, out wasResolved, out linkToProperty);
        //                    swCustomPropMgr.Get6("sprawdzil", false, out checkedBy, out resolvedValOut, out wasResolved, out linkToProperty);

        //                    dxfExist = File.Exists(System.IO.Path.ChangeExtension(swModel.GetPathName(), "DXF"));

        //                    stepExist = File.Exists(System.IO.Path.ChangeExtension(swModel.GetPathName(), "STEP"));

        //                    SWFileProperties swFileProperties = new SWFileProperties();
        //                    swFileProperties.filepath = compFilepath;
        //                    swFileProperties.name = name;
        //                    swFileProperties.description = valOutDescription;
        //                    swFileProperties.material = resolvedMaterial;
        //                    swFileProperties.thickness = resolvedThickness;
        //                    swFileProperties.mass = resolvedMass;
        //                    swFileProperties.area = surface;
        //                    swFileProperties.paintQty = paintQty;
        //                    swFileProperties.drawingNum = valOutDrawingNum;
        //                    swFileProperties.Qty = valOutQty;
        //                    swFileProperties.configuration = configName;
        //                    swFileProperties.status = status;
        //                    swFileProperties.createdBy = createdBy;
        //                    swFileProperties.checkedBy = checkedBy;
        //                    swFileProperties.dxfExist = dxfExist;
        //                    swFileProperties.stepExist = stepExist;
        //                    swFileProperties.comments = comments;
        //                    swFilesProperties.Add(swFileProperties);

        //                }
        //            }

        //            catch (System.NullReferenceException)
        //            {

        //            }

        //        }
        //        return (index, path, assemblyConfig, swFilesProperties);
        //    }
        //    catch
        //    {
        //        MessageBox.Show("Otwórz plik SolidWorks");
        //        return (null,null,null,null);
        //    }

        //}

        public string CreateAssembly()
        {
            ModelDoc2 swAssembly;
            string filepath;
            string systemPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.System);
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = "Złożenie";
            saveFileDialog1.DefaultExt = ".SLDASM";
            saveFileDialog1.Filter = "Pliki złożenie Solidworks (*.SLDASM) |*.SLDASM";

            saveFileDialog1.ShowDialog();
            filepath = saveFileDialog1.FileName;

            swAssembly = (ModelDoc2)_swApp.NewDocument(_templateAssemblyPath, 0, 0, 0);
            swAssembly.SaveAs3($"{filepath}.SLDASM", 0, 8);
            return filepath;
        }

        public void SetAllProperties(TreeControlItem node, List<string> doneParts, List<string>allParts, List<FileProperty> fileProperties, string parentNum, bool copyToAllConfigs, bool setQty, bool setThickness, bool clearNums, 
                bool setNums, bool addEngineer, bool AddCheckingEngineer, bool setMaterial, string material, string engineer, string checkingEngineer)
        {

            ModelDoc2 swModel;

            int drawingNum = 1;
            string evaluatedParentNum;
            string parentNumOld;
            string finalDrawingNum = drawingNum.ToString();
            string paintQty;

            int nodeType;
            TreeControlItem childNode;

            while (node != null)
            {
                nodeType = node.ObjectType;
                if (nodeType == (int)swTreeControlItemType_e.swFeatureManagerItem_Component)
                {
                    swComp = (Component2)node.Object;
                    swModel = (ModelDoc2)swComp.GetModelDoc2();
                    swModelExt = swModel.Extension;
                    string path = swModel.GetPathName();
                    swCustomPropMgr = swModelExt.get_CustomPropertyManager("");
                    swMassProp = (MassProperty2)swModelExt.CreateMassProperty2();
                    paintQty = Math.Round((swMassProp.SurfaceArea * 100 * 0.0015),3).ToString();

                    if (doneParts.Contains(swModel.GetPathName()) == false)
                    {
                        doneParts.Add(swModel.GetPathName());

                        if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
                        {
                            parentNumOld = parentNum;
                            parentNum = $"{parentNum}.{drawingNum}";
                            

                            foreach (FileProperty fileProperty in fileProperties)
                            {
                                SetCustomProperty(swModel, fileProperty.name, fileProperty.value, "");
                                
                            }

                            if (setQty)
                            {
                                int qty = allParts.Count(x=> x== swModel.GetPathName());
                                SetCustomProperty(swModel, "szt na kpl", qty.ToString(), "");
                            }
                            if (setThickness)
                            {
                                swFeature = (Feature)swModelExt.GetTemplateSheetMetal();
                                if(swFeature != null)
                                {
                                    SheetMetalFeatureData swMetalData = (SheetMetalFeatureData)swFeature.GetDefinition();
                                    double thickness;
                                    
                                    thickness = swMetalData.Thickness;
                                    SetCustomProperty(swModel, "grubosc materialu", thickness.ToString(), "");
                                }
                            }
                            if (clearNums)
                            {
                                SetCustomProperty(swModel, "nr rysunku", " ", "");
                            }
                            if (setNums)
                            {
                                SetCustomProperty(swModel, "nr rysunku", parentNum.Remove(0, 1), "");
                            }
                            if (addEngineer)
                            {
                                SetCustomProperty(swModel, "utworzyl", engineer , "");
                            }
                            if (AddCheckingEngineer)
                            {
                                SetCustomProperty(swModel, "sprawdzil", checkingEngineer , "");
                            }

                          
                            if (copyToAllConfigs)
                            {
                                SetPropForAllConfigs(swModel);
                            }
                            //Set paint quantity - ALWAYS
                            SetCustomProperty(swModel, "ilosc farby", paintQty, "");
                       
                            childNode = node.GetFirstChild();
                            SetAllProperties(childNode, doneParts,allParts,fileProperties, parentNum, copyToAllConfigs,setQty,setThickness,clearNums,setNums,addEngineer,AddCheckingEngineer,setMaterial,material,engineer,checkingEngineer);
                            parentNum = parentNumOld;

                        }
                        else
                        {
                            foreach (FileProperty fileProperty in fileProperties)
                            {
                                SetCustomProperty(swModel, fileProperty.name, fileProperty.value, "");
                            }

                            
                            if (setQty)
                            {
                                int qty = allParts.Count(x => x == swModel.GetPathName());
                                SetCustomProperty(swModel, "szt na kpl", qty.ToString(), "");
                            }
                            if (setThickness)
                            {
                                swFeature = (Feature)swModelExt.GetTemplateSheetMetal();
                                if (swFeature != null)
                                {
                                    SheetMetalFeatureData swMetalData = (SheetMetalFeatureData)swFeature.GetDefinition();
                                    double thickness;

                                    thickness = swMetalData.Thickness;
                                    SetCustomProperty(swModel, "grubosc materialu", String.Format("{0:.0}",(thickness*1000),1).ToString(), "");
                                }
                            }
                            if (clearNums)
                            {
                                SetCustomProperty(swModel, "nr rysunku", "", "");
                            }
                            if (setNums)
                            {
                                evaluatedParentNum = $"{parentNum}.{drawingNum}";
                                SetCustomProperty(swModel, "nr rysunku", evaluatedParentNum.Remove(0, 1), "");
                            }
                            if (addEngineer)
                            {
                                SetCustomProperty(swModel, "utworzyl", engineer, "");
                            }
                            if (AddCheckingEngineer)
                            {
                                SetCustomProperty(swModel, "sprawdzil", checkingEngineer, "");
                            }
                            if (setMaterial)
                            {
                                swPart = (PartDoc)swModel;
                                swPart.SetMaterialPropertyName2("", "c:\\eba\\szablony\\solidworks\\eba materiały.sldmat", material);
                            }
                            if (copyToAllConfigs)
                            {
                                SetPropForAllConfigs(swModel);
                            }

                            //Set paint quantity - ALWAYS
                            SetCustomProperty(swModel, "ilosc farby", paintQty, "");
                        }
                        drawingNum++;
                    }
                }
                node = node.GetNext();
            }

        }
        public void SetCustomProperty(ModelDoc2 swModel, string name, string value, string configName)
        {
            swModelExt = swModel.Extension;
            swCustomPropMgr = swModelExt.get_CustomPropertyManager(configName);
            swCustomPropMgr.Add3(name, (int)swCustomInfoType_e.swCustomInfoText, value, (int)swCustomPropertyAddOption_e.swCustomPropertyReplaceValue);
        }
        public void SetPropForAllConfigs(ModelDoc2 swModel)
        {
            swModelExt = swModel.Extension;
            string[] configNames = (string[])swModel.GetConfigurationNames();
            object vpropNames = null;
            string[] propNames;
            object vpropTypes = null;
            object vpropValues = null;
            object[] propValues;
            object resolved = null;
            swCustomPropMgr = swModelExt.get_CustomPropertyManager("");
            swCustomPropMgr.GetAll3(ref vpropNames,ref vpropTypes, ref vpropValues, ref resolved, 0);
            propValues = (object[])vpropValues;
            propNames = (string[])vpropNames;

            foreach (string configName in configNames)
            {
                int counter = 0;

                foreach (string property in propNames)
                {
                    SetCustomProperty(swModel, propNames[counter], (string)propValues[counter], configName);
                    counter++;
                }
            }
        }
        public void TraverseFeatureFeatures_Test(Feature swFeat)
        {
            Feature swSubFeat;
            Feature swSubSubFeat;
            Feature swSubSubSubFeat;
            string sPadStr = " ";

            while ((swFeat != null))
            {
                Debug.Print(swFeat.GetTypeName2() + swFeat.Name);
                swSubFeat = (Feature)swFeat.GetFirstSubFeature();

                while ((swSubFeat != null))
                {
                    Debug.Print(swSubFeat.GetTypeName2() + swSubFeat.Name);
                    swSubSubFeat = (Feature)swSubFeat.GetFirstSubFeature();

                    while ((swSubSubFeat != null))
                    {
                        Debug.Print(swSubSubFeat.GetTypeName2() + swSubSubFeat.Name);
                        swSubSubSubFeat = (Feature)swSubSubFeat.GetFirstSubFeature();

                        while ((swSubSubSubFeat != null))
                        {
                            Debug.Print(swSubSubSubFeat.GetTypeName2() + swSubSubSubFeat.Name);
                            swSubSubSubFeat = (Feature)swSubSubSubFeat.GetNextSubFeature();

                        }

                        swSubSubFeat = (Feature)swSubSubFeat.GetNextSubFeature();

                    }

                    swSubFeat = (Feature)swSubFeat.GetNextSubFeature();

                }

                swFeat = (Feature)swFeat.GetNextFeature();

            }

        }
        
        public void SetCompsNums(TreeControlItem node, string parentNum, List<string> doneParts)
        {
            int nodeType;
            ModelDoc2 swModel;
            TreeControlItem childNode;
            int drawingNum = 1;
            string evaluatedParentNum;
            string parentNumOld;
            string finalDrawingNum = drawingNum.ToString();
            
            while (node != null)
            {
                nodeType = node.ObjectType;
                if (nodeType == (int)swTreeControlItemType_e.swFeatureManagerItem_Component)
                {
                    swComp = (Component2)node.Object;
                    swModel = (ModelDoc2)swComp.GetModelDoc2();
                    if(doneParts.Contains(swModel.GetPathName()) == false)
                    {
                        doneParts.Add(swModel.GetPathName());

                        if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
                        {
                            childNode = node.GetFirstChild();
                            parentNumOld = parentNum;
                            parentNum = $"{parentNum}.{drawingNum}";
                            SetCustomProperty(swModel, "nr rysunku", parentNum.Remove(0, 1), "");
                            SetCompsNums(childNode, parentNum, doneParts);
                            parentNum = parentNumOld;
                        }
                        else
                        {
                            evaluatedParentNum = $"{parentNum}.{drawingNum}";
                            SetCustomProperty(swModel, "nr rysunku", evaluatedParentNum.Remove(0, 1), "");
                        }
                        drawingNum++;
                    }
                }
                node = node.GetNext();
            }
        }
        public void TraverseComponent_Test(Feature swFeature)
        {
            ModelDoc2 swModel;
            string featureType;
            Feature swFeatureChild;
            while(swFeature != null)
            {
                featureType = swFeature.GetTypeName2().ToString();
                Debug.Print(swFeature.GetTypeName2().ToString() + " "+swFeature.Name);
                if(swFeature.GetTypeName2() == "Reference")
                {
                    swComp = (Component2)swFeature.GetSpecificFeature2();
                    swModel = (ModelDoc2)swComp.GetModelDoc2();
                    if(swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
                    {
                       
                        swFeatureChild = (Feature)swFeature.GetFirstSubFeature();
                        TraverseComponent_Test(swFeatureChild);
                    }
                    else
                    {
                    
                    }
                }
                swFeature = (Feature)swFeature.GetNextFeature();
            }
        }

        public void SortTree(bool allLevels, List<string>orderToSort, bool groupComponents)
        {
            swModel = (ModelDoc2)_swApp.ActiveDoc;
            try
            {
                if(swModel == null)
                {
                    throw new NullReferenceException();
                }
                if (allLevels)
                {
                    SortTreeAll(swModel,orderToSort, groupComponents);
                    _helpService.SnackbarService.Show("Sukces!", "Elementy zostały posortowane", Wpf.Ui.Controls.ControlAppearance.Success, new SymbolIcon(SymbolRegular.Important24), TimeSpan.FromSeconds(3));
                }
                else
                {
                    SortTreeSingle(swModel,orderToSort, groupComponents);
                    _helpService.SnackbarService.Show("Sukces!", "Elementy posortowane", Wpf.Ui.Controls.ControlAppearance.Success, new SymbolIcon(SymbolRegular.Important24), TimeSpan.FromSeconds(3));
                }
            }
            catch (NullReferenceException ex)
            {
                _helpService.SnackbarService.Show("Uwaga!", "Otwórz pliku typu złożenie", Wpf.Ui.Controls.ControlAppearance.Danger, new SymbolIcon(SymbolRegular.Important24), TimeSpan.FromSeconds(3));
            }

        }
        private void SortTreeAll(ModelDoc2 swModel, List<string> orderToSort, bool groupComponents)
        {
            ModelDoc2 modelChild;
            int modelChildType;
            swAss = (AssemblyDoc)swModel;
            List<string> doneComps = new List<string>();
            object[] childComps = (object[])swAss.GetComponents(true);

            SortTreeSingle(swModel, orderToSort, groupComponents);
            foreach (Component2 childComp in childComps)
            {
                modelChild = (ModelDoc2)childComp.GetModelDoc2();
                if(!doneComps.Contains(modelChild.GetPathName()))
                {
                    modelChildType = modelChild.GetType();
                    string filepath = modelChild.GetPathName();
                    if (modelChildType == 2)
                    {
                        _swApp.OpenDoc6(filepath, (int)swDocumentTypes_e.swDocASSEMBLY, (int)swOpenDocOptions_e.swOpenDocOptions_ViewOnly, "", 0, 0);
                        _swApp.ActivateDoc3(System.IO.Path.GetFileNameWithoutExtension(filepath), true, 0, 0);
                        SortTreeSingle(modelChild, orderToSort, groupComponents);
                        _swApp.CloseDoc(System.IO.Path.GetFileName(modelChild.GetPathName()));
                        doneComps.Add(modelChild.GetPathName());
                    }
                }
            }
        }

        private void SortTreeSingle(ModelDoc2 swModel, List<string>orderToSort, bool groupComponents)
        {
            Feature folderNormalia;
            Feature swFeat;
            string targetToMove = "Normalia";
            string compName;
            swAss = (AssemblyDoc)swModel;
            swModelExt = swModel.Extension;
            swFeatMgr = swModel.FeatureManager;

            swFeatMgr.GroupComponentInstances = groupComponents;
            
            folderNormalia = (Feature)swAss.FeatureByName("Normalia");
            object[] swComps = (object[])swAss.GetComponents(true); //true for top level componenets only
            List<Component2> CompsToSort = new List<Component2>();

            //Creating new folder Normalia if it does not exist 
            if (folderNormalia == null)
            {

                //traversing till find origin
                swFeat = swModel.FirstFeature() as Feature;
                while(swFeat.GetTypeName2() != "OriginProfileFeature")
                {
                        swFeat = swFeat.GetNextFeature() as Feature;
                }

                //getting the first part in assembly
                swFeat = swFeat.GetNextFeature() as Feature;
                compName = swFeat.Name;

                swModelExt.SelectByID2(compName, "COMPONENT", 0, 0, 0, false, 0, null, 0); // need to select component
                var cos = folderNormalia = swFeatMgr.InsertFeatureTreeFolder2((int)swFeatureTreeFolderType_e.swFeatureTreeFolder_EmptyBefore);
                folderNormalia.Name = "Normalia";
                swModelExt.ReorderFeature("Normalia", "Początek układu współrzędnych", (int)swMoveLocation_e.swMoveAfter);
            }
            //Adding to a list components which are main in the feature design tree
            for (int i = 0; i < swComps.Length; i++)
            {
                swComp = (Component2)swComps[i];
                if (swComp.IsPatternInstance() == false && swComp.IsMirrored() == false)
                {
                    if (swComp.Name2.StartsWith("PN"))
                    {
                        swModelExt.ReorderFeature(swComp.Name2, targetToMove, (int)swMoveLocation_e.swMoveToFolder);
                    }
                    else
                    {
                        CompsToSort.Add(swComp);
                    }
                }
            }
            //ordering items the way they should appear in the tree
            var sortedComps = CompsToSort.OrderBy(o => o.Name2.StartsWith(orderToSort[0])).ThenBy(o => o.Name2.StartsWith(orderToSort[1])).ThenBy(o => o.Name2.StartsWith(orderToSort[2])).ThenBy(o => o.Name2.StartsWith(orderToSort[3])).
            ThenBy(o => o.Name2.StartsWith(orderToSort[4])).ThenBy(o => o.Name2.StartsWith(orderToSort[5])).ThenBy(o => o.Name2.StartsWith(orderToSort[6])).ThenBy(o => o.Name2.StartsWith(orderToSort[7])).
            ThenBy(o => o.Name2.StartsWith(orderToSort[8])).ThenBy(o => o.Name2.StartsWith(orderToSort[9])).ThenBy(o => o.Name2.StartsWith(orderToSort[10])).ThenBy(o => o.Name2.StartsWith(orderToSort[11])).ThenBy(o=>o.Name2);
            foreach (Component2 comp in sortedComps)
            {
                swModelExt.ReorderFeature(comp.Name2, targetToMove, (int)swMoveLocation_e.swMoveAfter);
            }
            swFeatMgr.GroupComponentInstances = groupComponents;
        }

        public void AddToAssembly(string filepath,string assemblyFilepath)
        {
            ModelDoc2 swModel;
            AssemblyDoc swAssemblyDoc;
            string assemblyFilename = Path.GetFileName(assemblyFilepath);
            swModel = (ModelDoc2)_swApp.ActivateDoc3(assemblyFilename, false, 0, 0);
            swAssemblyDoc = (AssemblyDoc)swModel;

            swAssemblyDoc.AddComponent5(filepath, 0, "", false, "", 0, 0, 0);
            swModel.Save3((int)swSaveAsOptions_e.swSaveAsOptions_Silent, 0, 0);    
        }


        public ObservableCollection<ExportStatus> ExportFromAssembly(bool[] options, int quantitySigma, string filedirToSave, List<string> filters)
        {
            ObservableCollection<ExportStatus> exportStatuses = new ObservableCollection<ExportStatus>();

            try
            {
                ModelDoc2 swModel = _swApp.ActiveDoc as ModelDoc2;
                ModelDoc2 swModelChild;
                AssemblyDoc swAssemblyDoc = swModel as AssemblyDoc;
                Dictionary<string, int> partsToExport = CountParts(swAssemblyDoc, filters);

                if (partsToExport == null || partsToExport.Count == 0)
                {
                    throw new InvalidOperationException("No parts to export or CountParts returned null.");
                }

                int extensionOption = 1;

                foreach (string path in partsToExport.Keys)
                {
                    swModelChild = (ModelDoc2)_swApp.ActivateDoc3(path, false, 0, 0);
                    if (Path.GetExtension(path) == ".SLDASM")
                    {
                        extensionOption = 2;
                    }
                    else
                    {
                        extensionOption = 1;
                    }
                    if (swModelChild == null)
                    {
                        swModelChild = _swApp.OpenDoc6(Path.GetFileName(path), extensionOption, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", 0, 0);
                        swModelChild = (ModelDoc2)_swApp.ActivateDoc3(path, false, 0, 0);
                    }
                    exportStatuses.Add(ExportSingleFile(filedirToSave, partsToExport, options, quantitySigma));
                    _swApp.CloseDoc(path);
                }

                if (options[4])
                {
                    ExportToDXFFromDrawing(filedirToSave);
                }
            }
            catch (NullReferenceException)
            {
                ErrorOccurred?.Invoke("Aktywny plik nie jest złożeniem SolidWorks");
            }

            return exportStatuses;
        }
        private void ExportToDXFFromDrawing(string filedirToSave)
        {
            ModelDoc2 swModel;
            swModel = (ModelDoc2)_swApp.ActiveDoc;
            swModelExt = swModel.Extension;
            
            string assemblyFilepath = swModel.GetPathName();
            string assemblyName = Path.GetFileNameWithoutExtension(assemblyFilepath);
            string assemblyDrawingFilepath = Path.ChangeExtension(assemblyFilepath, "SLDDRW");
            if(filedirToSave == null)
            {
                filedirToSave = Path.GetDirectoryName(assemblyFilepath);
            }
            

            swModel = _swApp.OpenDoc6(assemblyDrawingFilepath, 3, 2, "", 0, 0);
            drawingDoc = (DrawingDoc)swModel;
            if(drawingDoc != null)
            {
                swModel = (ModelDoc2)_swApp.ActiveDoc;
                swModelExt = swModel.Extension;
                string[] sheetNames = (string[])drawingDoc.GetSheetNames();


                drawingDoc.ActivateSheet(sheetNames[0]);

                for (int i = 0; i < sheetNames.Length; i++)
                {
                    string sheetName = sheetNames[i];
                    string modelName = System.IO.Path.GetFileNameWithoutExtension(swModel.GetPathName());

                    if (sheetName.Contains("dxf".ToUpper()) == true)
                    {
                        swModelExt.SaveAs3($"{filedirToSave}\\{modelName}_{i}.DXF", 0, 2, null, null, 16, 1);
                    }
                    drawingDoc.SheetNext();
                }
                _swApp.CloseDoc(assemblyDrawingFilepath);
            }
            
        }
        private ExportedDXF ExportToDXF(string filepath, string filedirToSave, Dictionary<string,int> totalParts, bool[] options, int quantity)
        {
            ExportedDXF outputDXF = new ExportedDXF();
            bool boolstatus = false;
            string sigmaNote;
            string dxfFilepath;
            string filename = Path.GetFileName(filepath);
            if (filedirToSave == null)
            {
                filedirToSave = Path.GetDirectoryName(filepath);
            }
            int totalQuantity;
            int optionsInt = 0;
            optionsInt += options[7] ? 8 : (options[8] ? 64 : 0);
            decimal thickness;
            string thicknessStr;
            ModelDoc2 swModel;
            try
            {
                swModel = _swApp.OpenDoc6(filepath, 1, 2, "", 0, 0);
                _swApp.ActivateDoc3(filename, true, 2, 0);
                swModel = (ModelDoc2)_swApp.ActiveDoc;
                
                swPart = (PartDoc)swModel;
                swModelExt = swModel.Extension;
                swFeature = (Feature)swModelExt.GetTemplateSheetMetal();
                if(swFeature != null)
                {
                    swSheetMetalData = (SheetMetalFeatureData)swFeature.GetDefinition();
                     thickness = Convert.ToDecimal((1000 * swSheetMetalData.Thickness), new CultureInfo("pl-PL")); //getting thickness in m, not mm
                }
                else
                {
                     thickness = 0;
                }

                var quantityPerPiece = totalParts[filepath];
                var material = swPart.MaterialIdName;
                if(material == "" || material == null)
                {
                    material = "unknown";
                }
                else if (material.Contains("|"))
                {
                    material = material.Split('|')[1];
                }
                totalQuantity = quantity * quantityPerPiece;
                dxfFilepath = $"{filedirToSave}\\{thickness}{material}_{filename}_{quantityPerPiece}-szt.DXF";
                thicknessStr = thickness.ToString(CultureInfo.GetCultureInfo("pl-PL"));
                thicknessStr = thicknessStr.Replace(',', '.');
                boolstatus = swPart.ExportToDWG2(dxfFilepath, filepath, 1, true, null, true, true, optionsInt, null);
                sigmaNote = $"Name:{filename}\n" +
                            $"Material:{material}\n" +
                            $"Thickness:{thicknessStr}\n" +
                            $"Quantity:{totalQuantity}";

                outputDXF.Status = boolstatus;
                outputDXF.SigmaNote = sigmaNote;
                outputDXF.DXFFilepath = dxfFilepath;
            }
            catch (InvalidCastException)
            {
                boolstatus = false;
                sigmaNote = "";
                dxfFilepath = "";
            }


            return outputDXF;
        }
        private bool ExportToSTEP(string filepath, string filedirToSave, Dictionary<string,int> totalParts)
        {
            ModelDoc2 swModel;
            string filename = Path.GetFileName(filepath);
            swModel = _swApp.OpenDoc6(filepath, 1, 2, "", 0, 0);
            _swApp.ActivateDoc3(filename, true, 2, 0);
            swModel = (ModelDoc2)_swApp.ActiveDoc;

            swModelExt = swModel.Extension;
            var quantity = totalParts[filepath];

            return swModelExt.SaveAs($"{filedirToSave}\\{filename}_{quantity}-szt.STEP", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_SaveReferenced, null, 0, 0); ;
        }
        private ExportStatus ExportSingleFile(string filedirToSave, Dictionary<string,int> totalParts, bool[] options, int quantitySigma)
        {
            ModelDoc2 swModel;
            ExportStatus exportStatus = new ExportStatus();
            ExportedDXF exportedDxf = new ExportedDXF();
            string modelFilepath;
            string filename;
            string filepath;
            bool exportToDxf, exportToStep;

            swModel = (ModelDoc2)_swApp.ActiveDoc;
            filepath = swModel.GetPathName(); 
            if(filedirToSave == null)
            {
                filedirToSave = Path.GetDirectoryName(filepath);
            }
            if(swModel != null)
            {
                modelFilepath = swModel.GetPathName(); //pathname inc. filename with extension
                filename = System.IO.Path.GetFileName(filepath);
                exportStatus.stepCreated = false;
                exportStatus.dxfCreated = false;
                exportStatus.filepath = modelFilepath;
                exportToDxf = options[0];
                exportToStep = options[1];

                if (swModel.GetType() == 1)
                {
                   
                    if (exportToStep)
                    {
                        exportStatus.stepCreated = ExportToSTEP(modelFilepath, filedirToSave, totalParts);
                    }
                    if (exportToDxf)
                    {
                        exportedDxf = ExportToDXF(modelFilepath, filedirToSave, totalParts, options, quantitySigma);
                        exportStatus.dxfCreated = exportedDxf.Status;
                    }
                }
        }
            return exportStatus;
        }

        public Dictionary<string,int> CountParts(AssemblyDoc swAss, List<string> filters)
        {
           
                var obj = (object[])swAss.GetComponents(false); //false for take all parts from main assembly and subassemblies
                Dictionary<string, int> totalParts = new Dictionary<string, int>();
                string filepath;

                foreach (Component2 comp in obj)
                {
                    var suppresion = comp.GetSuppression2();
                    if (suppresion != (int)swComponentSuppressionState_e.swComponentResolved || suppresion != (int)swComponentSuppressionState_e.swComponentFullyResolved) //if components is in lightweight mode or is suppressed it has to be unsuppressed to get count 
                    {
                        comp.SetSuppression2(3);
                    }

                    ModelDoc2 swModel = (ModelDoc2)comp.GetModelDoc2();
                    filepath = swModel.GetPathName();
                    if (swModel != null)
                    {
                        if (!totalParts.ContainsKey(filepath))
                        {
                            totalParts.Add(filepath, 1);
                        }
                        else
                        {
                            totalParts[filepath]++;
                        }
                    }

                    comp.SetSuppression2(suppresion);

                }
                if (filters.Count() != 0)
                {
                    totalParts = totalParts.Where(k => filters.Any(f => k.Key.Contains(f))).ToDictionary(k => k.Key, k => k.Value);
                }

                return totalParts;
        }
 
        public void CreateRectangleProfile(ProfileSW profileSW, string filepath)
        {
            ModelDoc2 swModel;
            double x = Convert.ToDouble(profileSW.X);
            double y = Convert.ToDouble(profileSW.Y);
            string partName = Convert.ToString(profileSW.Name);
            double thickness = Convert.ToDouble(profileSW.Thickness);
            double length = Convert.ToDouble(profileSW.Length);
            int draftCount = Convert.ToInt32(profileSW.DraftCount);

            swModel = (ModelDoc2)_swApp.NewDocument(_templatePartPath, 0, 0, 0);
            swModel.SaveAs3($"{filepath}{partName}.SLDPRT", 0, 8);

            swExt = swModel.Extension;
            skManager = swModel.SketchManager;

            //Suppress the dimension dialog box
            _swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, false);

            //creats a line of the length of the pipe
            swExt.SelectByID2("Płaszczyzna przednia", "PLANE", 0, 0, 0, false, 0, null, 0); //choosing depends on a name in first ""
            skManager.InsertSketch(true);
            skManager.CreateLine(-length / 2000, 0, 0, length / 2000, 0, 0); //the unit is m NOT mm
            swModel.IAddDiameterDimension2(length / 2000, 0, 0); //TODO on x dim it should be l/2 of the line

            //relation between middle and datum point (pipe)
            swExt.SelectByID2("Line1", "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            swExt.SelectByID2("", "EXTSKETCHPOINT", 0, 0, 0, true, 0, null, 0);
            swModel.SketchAddConstraints("sgATMIDDLE");
            skManager.InsertSketch(true); //closes a sketch

            //creats a cross-section of the pipe
            swExt.SelectByID2("Płaszczyzna prawa", "PLANE", 0, 0, 0, false, 0, null, 0); //choosing depends on a name in first ""
            skManager.InsertSketch(true);
            skManager.CreateCenterRectangle(0, 0, 0, x / 2000, y / 2000, 0); //the unit is m NOT mm
            swModel.ClearSelection2(true);

            //sometimes center of axis "loses" relation with the middle of the rectangle --> need to add it again
            swExt.SelectByID2("Line5", "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            swExt.SelectByID2("", "EXTSKETCHPOINT", 0, 0, 0, true, 0, null, 0);
            swModel.SketchAddConstraints("sgATMIDDLE");

            //adds dimensions to one side
            swExt.SelectByID2("Line1", "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            swModel.IAddDiameterDimension2(length / 200, 0.01, 0); //TODO on x dim it should be l/2 of the line 
            swModel.ClearSelection2(true);

            //adds dimensions to other side
            swExt.SelectByID2("Line2", "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            swModel.IAddDiameterDimension2(length / 200, 0.1, 0); //TODO on x dim it should be l/2 of the line
            swModel.ClearSelection2(true);

            //fillets corners
            swExt.SelectByID2("Point2", "SKETCHPOINT", 0, 0, 0, false, 0, null, 0);
            swExt.SelectByID2("Point3", "SKETCHPOINT", 0, 0, 0, true, 0, null, 0);
            swExt.SelectByID2("Point4", "SKETCHPOINT", 0, 0, 0, true, 0, null, 0);
            swExt.SelectByID2("Point5", "SKETCHPOINT", 0, 0, 0, true, 0, null, 0);
            skManager.CreateFillet((thickness / 1000) + (0.5 / 1000), 1);
            swModel.ClearSelection2(true);

            //chain offset
            swExt.SelectByID2("Line1", "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            skManager.SketchOffset2(-thickness / 1000, false, true, 0, 0, true);

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
                swFeatMgr.InsertMultiFaceDraft(Math.PI * 0.25, true, false, 0, false, false);
                swModel.ViewRotateminusx();
            }
            else if (draftCount == 2)
            {
                swExt.SelectByID2("", "FACE", length / 2000, (y / 2000) - (thickness / 2000), 0, false, 2, null, 0); //side face
                swModel.ViewRotateplusx(); // don't know why need to slighty rotate view in SW to select another face now
                swExt.SelectByID2("", "FACE", -length / 2000, (y / 2000) - (thickness / 2000), 0, true, 2, null, 0); //side face
                swExt.SelectByID2("", "FACE", 0, y / 2000, 0, true, 1, null, 0); //neutral face
                swFeatMgr.InsertMultiFaceDraft(Math.PI * 0.25, true, false, 0, false, false);
                swModel.ViewRotateminusx();
            }
            else if (draftCount == 0)
            {
                swModel.ViewRotateplusx(); // don't know why need to slighty rotate view in SW to select another face now
                swModel.ViewRotateminusx();
            }

            swModel.Save3((int)swSaveAsOptions_e.swSaveAsOptions_Silent, 0, 0);

            //Unsuppress the dimension dialog box
            _swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, true);
          

        }
        public void CreateCircularProfile(ProfileSW profileSW, string filepath)
        {
            ModelDoc2 swModel;
            double x = Convert.ToDouble(profileSW.X);
            int y = 0;
            string partName = Convert.ToString(profileSW.Name);
            double thickness = Convert.ToDouble(profileSW.Thickness);
            double length = Convert.ToDouble(profileSW.Length);
            int draftCount = Convert.ToInt32(profileSW.DraftCount);

            swModel = (ModelDoc2)_swApp.NewDocument(_templatePartPath, 0, 0, 0);
            swModel.SaveAs3($"{filepath}{partName}.SLDPRT", 0, 8);

            swExt = swModel.Extension;
            skManager = swModel.SketchManager;

            //Suppress the dimension dialog box
            _swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, false);

            //creats a line of the length of the pipe
            swExt.SelectByID2("Płaszczyzna przednia", "PLANE", 0, 0, 0, false, 0, null, 0); //choosing depends on a name in first ""
            skManager.InsertSketch(true);
            skManager.CreateLine(-length / 2000, 0, 0, length / 2000, 0, 0); //the unit is m NOT mm
            swModel.IAddDiameterDimension2(length / 2000, 0, 0); //TODO on x dim it should be l/2 of the line

            //relation between middle and datum point (pipe)
            swExt.SelectByID2("Line1", "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            swExt.SelectByID2("", "EXTSKETCHPOINT", 0, 0, 0, true, 0, null, 0);
            swModel.SketchAddConstraints("sgATMIDDLE");
            skManager.InsertSketch(true); //closes a sketch

            //creat a cross-section of the pipe
            swExt.SelectByID2("Płaszczyzna prawa", "PLANE", 0, 0, 0, false, 0, null, 0); //choosing depends on a name in first ""
            skManager.InsertSketch(true);
            skManager.CreateCircle(0, 0, 0, x / 2000,0, 0); //the unit is m NOT mm
            swModel.ClearSelection2(true);

            //sometimes center of axis "loses" relation with the middle of the rectangle --> need to add it again
            swExt.SelectByID2("Line5", "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            swExt.SelectByID2("", "EXTSKETCHPOINT", 0, 0, 0, true, 0, null, 0);
            swModel.SketchAddConstraints("sgATMIDDLE");

            //adds dimensions to one side
            swExt.SelectByID2("Arc1", "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            swModel.IAddDiameterDimension2(length / 200, 0.01, 0); 
            swModel.ClearSelection2(true);

            //chain offset
            swExt.SelectByID2("Arc1", "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            skManager.SketchOffset2(-thickness / 1000, false, true, 0, 0, true);

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
                swExt.SelectByID2("Płaszczyzna górna", "PLANE", 0, 0, 0, false, 0, null, 0);
                swFeatMgr.InsertRefPlane((int)swRefPlaneReferenceConstraints_e.swRefPlaneReferenceConstraint_Distance, (x / 2000), 0, 0, 0, 0);
                swModel.ClearSelection2(false);
                swExt.SelectByID2("", "PLANE", 0, x/2000, 0, false, 1, null, 0); //neutral face
                swModel.ViewRotateplusx(); // don't know why need to slighty rotate view in SW to select another face now
                swExt.SelectByID2("", "FACE", length / 2000, (x / 2000) - (thickness / 2000), 0, true, 2, null, 0); //side face
                swFeatMgr.InsertMultiFaceDraft(Math.PI * 0.25, true, false, 0, false, false);
                swModel.ViewRotateminusx();
            }
            else if (draftCount == 2)
            {
                swExt.SelectByID2("Płaszczyzna górna", "PLANE", 0, 0, 0, false, 0, null, 0);
                swFeatMgr.InsertRefPlane((int)swRefPlaneReferenceConstraints_e.swRefPlaneReferenceConstraint_Distance, (x / 2000), 0, 0, 0, 0);
                swModel.ClearSelection2(false);
                swExt.SelectByID2("", "PLANE", 0, x / 2000, 0, false, 1, null, 0); //neutral face
                swModel.ViewRotateplusx(); // don't know why need to slighty rotate view in SW to select another face now
                swExt.SelectByID2("", "FACE", length / 2000, (x / 2000) - (thickness / 2000), 0, true, 2, null, 0); //side face
                swExt.SelectByID2("", "FACE", -length / 2000, (x / 2000) - (thickness / 2000), 0, true, 2, null, 0); //side face
                swFeatMgr.InsertMultiFaceDraft(Math.PI * 0.25, true, false, 0, false, false);
                swModel.ViewRotateminusx();
            }
            else if (draftCount == 0)
            {
                swModel.ViewRotateplusx(); // don't know why need to slighty rotate view in SW to select another face now
                swModel.ViewRotateminusx();
            }
            swModel.Save3(4, 0, 0);

            //Unsuppress the dimension dialog box
            _swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, true);
         
        }
        public void CreateRectangleRod(ProfileSW profileSW, string filepath)
        {
            ModelDoc2 swModel;
            double x = Convert.ToDouble(profileSW.X);
            double y = Convert.ToDouble(profileSW.Y);
            string partName = Convert.ToString(profileSW.Name);
            double thickness = 0;
            double length = Convert.ToDouble(profileSW.Length);
            int draftCount = Convert.ToInt32(profileSW.DraftCount);

            swModel =   (ModelDoc2)_swApp.NewDocument(_templatePartPath, 0, 0, 0);
            swModel.SaveAs3($"{filepath}{partName}.SLDPRT", 0, 8);

            swExt = swModel.Extension;
            skManager = swModel.SketchManager;

            //Suppress the dimension dialog box
            _swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, false);

            //creats a line of the length of the pipe
            swExt.SelectByID2("Płaszczyzna przednia", "PLANE", 0, 0, 0, false, 0, null, 0); //choosing depends on a name in first ""
            skManager.InsertSketch(true);
            skManager.CreateLine(-length / 2000, 0, 0, length / 2000, 0, 0); //the unit is m NOT mm
            swModel.IAddDiameterDimension2(length / 2000, 0, 0); //TODO on x dim it should be l/2 of the line

            //relation between middle and datum point (pipe)
            swExt.SelectByID2("Line1", "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            swExt.SelectByID2("", "EXTSKETCHPOINT", 0, 0, 0, true, 0, null, 0);
            swModel.SketchAddConstraints("sgATMIDDLE");
            skManager.InsertSketch(true); //closes a sketch

            //creats a cross-section of the pipe
            swExt.SelectByID2("Płaszczyzna prawa", "PLANE", 0, 0, 0, false, 0, null, 0); //choosing depends on a name in first ""
            skManager.InsertSketch(true);
            skManager.CreateCenterRectangle(0, 0, 0, x / 2000, y / 2000, 0); //the unit is m NOT mm
            swModel.ClearSelection2(true);

            //sometimes center of axis "loses" relation with the middle of the rectangle --> need to add it again
            swExt.SelectByID2("Line5", "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            swExt.SelectByID2("", "EXTSKETCHPOINT", 0, 0, 0, true, 0, null, 0);
            swModel.SketchAddConstraints("sgATMIDDLE");

            //adds dimensions to one side
            swExt.SelectByID2("Line1", "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            swModel.IAddDiameterDimension2(length / 200, 0.01, 0); //TODO on x dim it should be l/2 of the line 
            swModel.ClearSelection2(true);

            //adds dimensions to other side
            swExt.SelectByID2("Line2", "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            swModel.IAddDiameterDimension2(length / 200, 0.1, 0); //TODO on x dim it should be l/2 of the line
            swModel.ClearSelection2(true);


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
                swExt.SelectByID2("", "FACE", length / 2000, 0 , 0, false, 2, null, 0); //side face
                swModel.ViewRotateplusx(); // don't know why need to slighty rotate view in SW to select another face now
                swExt.SelectByID2("", "FACE", 0, y / 2000, 0, true, 1, null, 0); //neutral face
                swFeatMgr.InsertMultiFaceDraft(Math.PI * 0.25, true, false, 0, false, false);
                swModel.ViewRotateminusx();
            }
            else if (draftCount == 2)
            {
                swExt.SelectByID2("", "FACE", length / 2000, 0, 0, false, 2, null, 0); //side face
                swModel.ViewRotateplusx(); // don't know why need to slighty rotate view in SW to select another face now
                swExt.SelectByID2("", "FACE", -length / 2000, 0, 0, true, 2, null, 0); //side face
                swExt.SelectByID2("", "FACE", 0, y / 2000, 0, true, 1, null, 0); //neutral face
                swFeatMgr.InsertMultiFaceDraft(Math.PI * 0.25, true, false, 0, false, false);
                swModel.ViewRotateminusx();
            }
            else if (draftCount == 0)
            {
                swModel.ViewRotateplusx(); // don't know why need to slighty rotate view in SW to select another face now
                swModel.ViewRotateminusx();
            }

            swModel.Save3(4, 0, 0);

            //Unsuppress the dimension dialog box
            _swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, true);
    

        }
        public void CreateCircularRod(ProfileSW profileSW, string filepath)
        {
            ModelDoc2 swModel;
            double x = Convert.ToDouble(profileSW.X);
            int y = 0;
            string partName = Convert.ToString(profileSW.Name);
            double thickness = 0;
            double length = Convert.ToDouble(profileSW.Length);
            int draftCount = Convert.ToInt32(profileSW.DraftCount);

            swModel = (ModelDoc2)_swApp.NewDocument(_templatePartPath, 0, 0, 0);
            swModel.SaveAs3($"{filepath}{partName}.SLDPRT", 0, 8);

            swExt = swModel.Extension;
            skManager = swModel.SketchManager;

            //Suppress the dimension dialog box
            _swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, false);

            //creats a line of the length of the pipe
            swExt.SelectByID2("Płaszczyzna przednia", "PLANE", 0, 0, 0, false, 0, null, 0); //choosing depends on a name in first ""
            skManager.InsertSketch(true);
            skManager.CreateLine(-length / 2000, 0, 0, length / 2000, 0, 0); //the unit is m NOT mm
            swModel.IAddDiameterDimension2(length / 2000, 0, 0); //TODO on x dim it should be l/2 of the line

            //relation between middle and datum point (pipe)
            swExt.SelectByID2("Line1", "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            swExt.SelectByID2("", "EXTSKETCHPOINT", 0, 0, 0, true, 0, null, 0);
            swModel.SketchAddConstraints("sgATMIDDLE");
            skManager.InsertSketch(true); //closes a sketch

            //creat a cross-section of the pipe
            swExt.SelectByID2("Płaszczyzna prawa", "PLANE", 0, 0, 0, false, 0, null, 0); //choosing depends on a name in first ""
            skManager.InsertSketch(true);
            skManager.CreateCircle(0, 0, 0, x / 2000, 0, 0); //the unit is m NOT mm
            swModel.ClearSelection2(true);

            //sometimes center of axis "loses" relation with the middle of the rectangle --> need to add it again
            swExt.SelectByID2("Line5", "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            swExt.SelectByID2("", "EXTSKETCHPOINT", 0, 0, 0, true, 0, null, 0);
            swModel.SketchAddConstraints("sgATMIDDLE");

            //adds dimensions to one side
            swExt.SelectByID2("Arc1", "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            swModel.IAddDiameterDimension2(length / 200, 0.01, 0);
            swModel.ClearSelection2(true);

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
                swExt.SelectByID2("Płaszczyzna górna", "PLANE", 0, 0, 0, false, 0, null, 0);
                swFeatMgr.InsertRefPlane((int)swRefPlaneReferenceConstraints_e.swRefPlaneReferenceConstraint_Distance, (x / 2000), 0, 0, 0, 0);
                swModel.ClearSelection2(false);
                swExt.SelectByID2("", "PLANE", 0, x / 2000, 0, false, 1, null, 0); //neutral face
                swModel.ViewRotateplusx(); // don't know why need to slighty rotate view in SW to select another face now
                swExt.SelectByID2("", "FACE", length / 2000, 0, 0, true, 2, null, 0); //side face
                swFeatMgr.InsertMultiFaceDraft(Math.PI * 0.25, true, false, 0, false, false);
                swModel.ViewRotateminusx();
            }
            else if (draftCount == 2)
            {
                swExt.SelectByID2("Płaszczyzna górna", "PLANE", 0, 0, 0, false, 0, null, 0);
                swFeatMgr.InsertRefPlane((int)swRefPlaneReferenceConstraints_e.swRefPlaneReferenceConstraint_Distance, (x / 2000), 0, 0, 0, 0);
                swModel.ClearSelection2(false);
                swExt.SelectByID2("", "PLANE", 0, x / 2000, 0, false, 1, null, 0); //neutral face
                swModel.ViewRotateplusx(); // don't know why need to slighty rotate view in SW to select another face now
                swExt.SelectByID2("", "FACE", length / 2000, 0, 0, true, 2, null, 0); //side face
                swExt.SelectByID2("", "FACE", -length / 2000, 0, 0, true, 2, null, 0); //side face
                swFeatMgr.InsertMultiFaceDraft(Math.PI * 0.25, true, false, 0, false, false);
                swModel.ViewRotateminusx();
            }
            else if (draftCount == 0)
            {
                swModel.ViewRotateplusx(); // don't know why need to slighty rotate view in SW to select another face now
                swModel.ViewRotateminusx();
            }
            swModel.Save3(4, 0, 0);

            //Unsuppress the dimension dialog box
            _swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, true);
          
        }
        public void CloseDoc(string filepath)
        {
            _swApp.CloseDoc(filepath);
        }
        public ConvertStatus ConvertToSheetOld(ModelDoc2 swModel)
        {
            ConvertStatus convertStatus = new ConvertStatus();
            try
            {
                Body2 swBiggestBody = default(Body2);
                Entity entityFace;
                object[] swFaces;
                Face2 swBiggestFace = default(Face2);
                Face2 swSmallestFace = default(Face2);
                swFeatMgr = swModel.FeatureManager;
                string partName = System.IO.Path.GetFileNameWithoutExtension(swModel.GetPathName());
                
                swPart = (PartDoc)swModel;
                object[] bodies;
                swModel.Save3(4, 0, 0);
                bodies = (object[])swPart.GetBodies2((int)swBodyType_e.swAllBodies, false);
                swBiggestBody = (Body2)bodies[0];
                double[] massProps = (double[])swBiggestBody.GetMassProperties(7800);
                double bodyVolume = Convert.ToDouble(massProps[3]);
                double maxVolume = bodyVolume;
                double bodyVolumeBeforeConvert;
                double bodyVolumeAfterConvert;
                double volumeRatio;
                double biggestArea;
                double smallestArea;
                double bodyArea;
                convertStatus.name = partName;
                convertStatus.filepath = swModel.GetPathName();

                swModelExt = swModel.Extension;
                swMassProp = (MassProperty2)swModelExt.CreateMassProperty2();
                bodyVolumeBeforeConvert = swMassProp.Volume;

                foreach (Body2 body in bodies)
                {
                    massProps = (double[])body.GetMassProperties(7800);
                    bodyVolume = Convert.ToDouble(massProps[3]);
                    if (bodyVolume > maxVolume)
                    {
                        bodyVolume = maxVolume;
                        swBiggestBody = body;
                    }
                }
                swFaces = (object[])swBiggestBody.GetFaces();
                swBiggestFace = (Face2)swBiggestBody.GetFirstFace();
                swSmallestFace = swBiggestFace;
                biggestArea = swBiggestFace.GetArea();
                smallestArea = biggestArea;
                foreach (Face2 face in swFaces)
                {
                    bodyArea = face.GetArea();

                    if (bodyArea > biggestArea)
                    {
                        biggestArea = bodyArea;
                        swBiggestFace = face;
                    }
                    else if (bodyArea < smallestArea)
                    {
                        bodyArea = smallestArea;
                        swSmallestFace = face;
                    }
                }
                object[] edges = (object[])swSmallestFace.GetEdges();


                Curve curve = default(Curve);
                double smallestCurve;
                double thickness = 200;
                foreach (Edge edge in edges)
                {
                    curve = (Curve)edge.GetCurve();
                    smallestCurve = curve.GetLength3(0, 0);
                    if (thickness > smallestCurve)
                    {
                        thickness = smallestCurve;
                    }
                }

                bool isBend = true;
                if (((biggestArea * thickness) / bodyVolumeBeforeConvert) > 0.99 && ((biggestArea * thickness) / bodyVolumeBeforeConvert) < 1.01)
                {
                    isBend = false;

                }

                entityFace = (Entity)swBiggestFace;
                entityFace.Select4(false, null);

                bool status = swFeatMgr.InsertConvertToSheetMetal2(thickness, false, isBend, 0, 0.002, (int)swSheetMetalReliefTypes_e.swSheetMetalReliefNone, 0.5, 1, 0.5, false);



                swMassProp = (MassProperty2)swModelExt.CreateMassProperty2();
                bodyVolumeAfterConvert = swMassProp.Volume;

                volumeRatio = (bodyVolumeAfterConvert / bodyVolumeBeforeConvert) * 1000;


                
                convertStatus.volumeRatio = volumeRatio;
                

                if (Math.Round(volumeRatio, 3) == 1000)
                {
                    convertStatus.isDone = true;
                    swModel.Save3(4, 0, 0);
                    convertStatus.comments = "konwersja udana";
                }
                else if (volumeRatio > 1005 || volumeRatio < 995)
                {
                    swFeature = (Feature)swModel.FeatureByPositionReverse(1);
                    swFeature.Select2(false, 0);
                    swFeature = (Feature)swModel.FeatureByPositionReverse(2);
                    swFeature.Select2(true, 0);
                    swModelExt.DeleteSelection2((int)swDeleteSelectionOptions_e.swDelete_Absorbed);
                    convertStatus.isDone = false;
                    convertStatus.comments = "konwersja nieudana";
                }

                else
                {
                    convertStatus.isDone = true;
                    convertStatus.comments = "drobne różnice po konwersji";
                }
                if (swPart.FeatureByName("Konwertuj bryłę1") == null)
                {
                    convertStatus.isDone = false;
                    convertStatus.comments = "konwersja nieudana";
                }

                return convertStatus;
            }
            catch (System.NullReferenceException)
            {
                convertStatus.isDone = false;
                convertStatus.comments = "obiekt nie jest bryłą";
                convertStatus.name = System.IO.Path.GetFileNameWithoutExtension(swModel.GetPathName());
                convertStatus.filepath = swModel.GetPathName();
                convertStatus.volumeRatio = 0;
                return convertStatus;
            }

        }

        public ConvertStatus ConvertToSheet(ModelDoc2 swModel)
        {
            ConvertStatus convertStatus = new ConvertStatus();
            try
            {
                Body2 swBiggestBody = default(Body2);
                Entity entityFace;
                object[] swFaces;
                Face2 swBiggestFace = default(Face2);
                Face2[] swBiggestFaces = default(Face2[]);
                Face2 swSmallestFace = default(Face2);
                Face2[] swSmallestFaces = default(Face2[]);
                swFeatMgr = swModel.FeatureManager;
                string partName = System.IO.Path.GetFileNameWithoutExtension(swModel.GetPathName());

                swPart = (PartDoc)swModel;
                object[] bodies;
                swModel.Save3(4, 0, 0);
                bodies = (object[])swPart.GetBodies2((int)swBodyType_e.swAllBodies, false);
                swBiggestBody = (Body2)bodies[0];
                double[] massProps = (double[])swBiggestBody.GetMassProperties(7800);
                double bodyVolume = Convert.ToDouble(massProps[3]);
                double maxVolume = bodyVolume;
                double bodyVolumeBeforeConvert;
                double bodyVolumeAfterConvert;
                double volumeRatio;
                double biggestArea;
                
                double[] biggestAreas = new double[5];
                double[] smallestAreas = new double[5];
                double smallestArea;
                double bodyArea;
                convertStatus.name = partName;
                convertStatus.filepath = swModel.GetPathName();

                swModelExt = swModel.Extension;
                swMassProp = (MassProperty2)swModelExt.CreateMassProperty2();
                bodyVolumeBeforeConvert = swMassProp.Volume;

                foreach (Body2 body in bodies)
                {
                    massProps = (double[])body.GetMassProperties(7800);
                    bodyVolume = Convert.ToDouble(massProps[3]);
                    if (bodyVolume > maxVolume)
                    {
                        bodyVolume = maxVolume;
                        swBiggestBody = body;
                    }
                }


                swFaces = (object[])swBiggestBody.GetFaces();
                swBiggestFace = (Face2)swBiggestBody.GetFirstFace();
                swSmallestFace = swBiggestFace;
                biggestArea = swBiggestFace.GetArea();
                smallestArea = biggestArea;

                double[] allAreas = new double[swFaces.Count()];

                int i = 0;
                foreach (Face2 face in swFaces)
                {
                    bodyArea = face.GetArea();

                    allAreas[i] = bodyArea;

                    if (bodyArea > biggestArea)
                    {
                        biggestArea = bodyArea;
                        swBiggestFace = face;
                    }
                    else if (bodyArea < smallestArea)
                    {
                        bodyArea = smallestArea;
                        swSmallestFace = face;
                    }

                    i++;
                }

                Array.Sort(allAreas);
                for(int j = 0; j < allAreas.Length; j++)
                {
                    if (j < 5) 
                    {
                        smallestAreas[j] = allAreas[j];
                    }
                    else if (j> allAreas.Length - 5)
                    {
                        int k = 0;
                        biggestAreas[k] = allAreas[j];
                        k=k++;
                    }
                    
                }

                object[] edges = (object[])swSmallestFace.GetEdges();


                Curve curve = default(Curve);
                double smallestCurve;
                double thickness = 200;
                foreach (Edge edge in edges)
                {
                    curve = (Curve)edge.GetCurve();
                    smallestCurve = curve.GetLength3(0, 0);
                    if (thickness > smallestCurve)
                    {
                        thickness = smallestCurve;
                    }
                }

                bool isBend = true;
                if (((biggestArea * thickness) / bodyVolumeBeforeConvert) > 0.99 && ((biggestArea * thickness) / bodyVolumeBeforeConvert) < 1.01)
                {
                    isBend = false;

                }

                entityFace = (Entity)swBiggestFace;
                entityFace.Select4(false, null);

                bool status = swFeatMgr.InsertConvertToSheetMetal2(thickness, false, isBend, 0, 0.002, (int)swSheetMetalReliefTypes_e.swSheetMetalReliefNone, 0.5, 1, 0.5, false);



                swMassProp = (MassProperty2)swModelExt.CreateMassProperty2();
                bodyVolumeAfterConvert = swMassProp.Volume;

                volumeRatio = (bodyVolumeAfterConvert / bodyVolumeBeforeConvert) * 1000;



                convertStatus.volumeRatio = volumeRatio;


                if (Math.Round(volumeRatio, 3) == 1000)
                {
                    convertStatus.isDone = true;
                    swModel.Save3(4, 0, 0);
                    convertStatus.comments = "konwersja udana";
                }
                else if (volumeRatio > 1005 || volumeRatio < 995)
                {
                    swFeature = (Feature)swModel.FeatureByPositionReverse(1);
                    swFeature.Select2(false, 0);
                    swFeature = (Feature)swModel.FeatureByPositionReverse(2);
                    swFeature.Select2(true, 0);
                    swModelExt.DeleteSelection2((int)swDeleteSelectionOptions_e.swDelete_Absorbed);
                    convertStatus.isDone = false;
                    convertStatus.comments = "konwersja nieudana";
                }

                else
                {
                    convertStatus.isDone = true;
                    convertStatus.comments = "drobne różnice po konwersji";
                }
                if (swPart.FeatureByName("Konwertuj bryłę1") == null)
                {
                    convertStatus.isDone = false;
                    convertStatus.comments = "konwersja nieudana";
                }

                return convertStatus;
            }
            catch (System.NullReferenceException)
            {
                convertStatus.isDone = false;
                convertStatus.comments = "obiekt nie jest bryłą";
                convertStatus.name = System.IO.Path.GetFileNameWithoutExtension(swModel.GetPathName());
                convertStatus.filepath = swModel.GetPathName();
                convertStatus.volumeRatio = 0;
                return convertStatus;
            }

        }

        public List<ConvertStatus> ConvertToSheets(AssemblyDoc swAss, List<object> compsToConvert)
        {
            SldWorks swApp = (SldWorks)Marshal2.GetActiveObject("SldWorks.Application");
            ModelDoc2 swModel;
            ConvertStatus convertStatus;
            List<ConvertStatus> convertStatuses = new List<ConvertStatus>();
            foreach (Component2 swComp in compsToConvert)
            {
                swModel = (ModelDoc2)swComp.GetModelDoc2();
                if (swModel.GetType() == (int)swDocumentTypes_e.swDocPART || swModel.GetType() == (int)swDocumentTypes_e.swDocIMPORTED_PART)
                {
                    swApp.ActivateDoc3(swModel.GetPathName(), false, 0, 0);
                    convertStatus = ConvertToSheet(swModel);
                    convertStatuses.Add(convertStatus);
                    swApp.CloseDoc(swModel.GetPathName());
                }
            }
            return convertStatuses;
        }
        public List<object> TakeSheetsToConvert(TreeControlItem node, List<object> compsToConvert, List<string>compsToConvertNames)
        {
            int nodeType;
            ModelDoc2 swModel;
            TreeControlItem childNode;


            while (node != null)
            {
                nodeType = node.ObjectType;
                if (nodeType == (int)swTreeControlItemType_e.swFeatureManagerItem_Component)
                {
                    swComp = (Component2)node.Object;
                    swModel = (ModelDoc2)swComp.GetModelDoc2();

                    if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
                    {

                        childNode = node.GetFirstChild();
                        TakeSheetsToConvert(childNode, compsToConvert, compsToConvertNames);
                    }
                    else
                    {
                        if (compsToConvertNames.Contains((System.IO.Path.GetFileNameWithoutExtension(swModel.GetPathName()))) == false)
                        {
                            compsToConvertNames.Add((System.IO.Path.GetFileNameWithoutExtension(swModel.GetPathName())));
                            compsToConvert.Add(swComp);
                        }
                    }
                }
                node = node.GetNext();
            }
 
            return compsToConvert;
        }
        public void SplitPart(string assemblyFilepath, string assemblyFilename, string partToSplitFilepath)
        {
            SldWorks swApp = (SldWorks)Marshal2.GetActiveObject("SldWorks.Application");
            ModelDoc2 swModel = (ModelDoc2)swApp.ActivateDoc3(partToSplitFilepath,false,0,0);
            Body2 bodyToSave;
            string filedir = System.IO.Path.GetDirectoryName(swModel.GetPathName());
            string bodyName;
            swFeatMgr = swModel.FeatureManager;
            object[] swBodies;
            int errors = 0;
            int warnings = 0;
            swPart = (PartDoc)swModel;
            swModelExt = swModel.Extension;
            swBodies = (object[])swPart.GetBodies2((int)swBodyType_e.swAllBodies, false);
            List<string> filepaths = new List<string>();

            for (int i = 0; i < swBodies.Length; i++)
            {
                bodyToSave = (Body2)swBodies[i];
                bodyName = bodyToSave.Name;
                bodyToSave.Select2(false, null);
                swFeatMgr.InsertDeleteBody2(true);
                bool sin = bodyName.Contains('.');
                if (bodyName.Contains('.') == true || bodyName.Contains('/') == true || bodyName.Contains('<') == true || bodyName.Contains('>') == true || bodyName.Contains('*') == true
                    || bodyName.Contains('|') == true || bodyName.Contains('?') == true)
                {
                   bodyName = bodyName.Replace(".", "_");
                   bodyName = bodyName.Replace("/", "_");
                   bodyName = bodyName.Replace("<", "_");
                   bodyName = bodyName.Replace(">", "_");
                   bodyName = bodyName.Replace("*", "_");
                   bodyName = bodyName.Replace("|", "_");
                   bodyName = bodyName.Replace("?", "_");
                }
                string cos = $"{filedir}\\{bodyName}.SLDPRT";
                swModelExt.SaveAs3($"{filedir}\\{bodyName}.SLDPRT", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Copy, null, null, errors, warnings);
                filepaths.Add($"{filedir}\\{bodyName}.SLDPRT");
                swFeature = (Feature)swModel.FeatureByPositionReverse(0);
                swFeature.Select2(false, 0);
                swModelExt.DeleteSelection2((int)swDeleteSelectionOptions_e.swDelete_Absorbed);
            }

            foreach(string filepath in filepaths)
            {
                swApp.OpenDoc6(filepath, (int)swDocumentTypes_e.swDocPART, 0, "", 0, 0);
                swApp.ActivateDoc3(filepath, false, 0, 0);
                AddToAssembly(filepath, assemblyFilename);
                //swComp = 
                swApp.CloseDoc(filepath);
            }
            swApp.CloseDoc(partToSplitFilepath);
        }

        public void GenerateDrawingsTogether()
        {
            List<ModelDoc2> swParts = new List<ModelDoc2>();
            List<string> swPartsFilepaths = new List<string>();
            string filepath;
            string drawingNum;
            string resolvedValOut;
            bool wasResolved = true;
            bool linkToProperty = false;
            const string templatepath = "C:\\EBA\\SZABLONY\\SolidWorks\\EBA_A3_RYSUNEK.drwdot";
            int scale1 = 1;
            int scale2 = 1;
            double[] move;

            string configName;
            SldWorks swApp = (SldWorks)Marshal2.GetActiveObject("SldWorks.Application");
            swAss = (AssemblyDoc)swApp.ActiveDoc;
            ModelDoc2 swModel = (ModelDoc2)swAss;
            ConfigurationManager swConfMgr;
            Configuration swConfig;
            DrawingDoc swDrawing;
            filepath = swModel.GetPathName();

            //add main assembly as first model

            swParts.Add(swModel);

            //traversing the tree -> getting all models
            swModelExt = swModel.Extension;
            swFeatMgr = swModel.FeatureManager;
            swConfMgr = (ConfigurationManager)swModel.ConfigurationManager;
            TreeControlItem node = swFeatMgr.GetFeatureTreeRootItem2((int)swFeatMgrPane_e.swFeatMgrPaneBottom);
            node = node.GetFirstChild();
            swParts = TraverseForDrawings(node, swParts, swPartsFilepaths);

            //creating first page of drawing and save it into current folder
            swApp.INewDocument2(templatepath, (int)swDwgPaperSizes_e.swDwgPaperA3size, 0.297, 0.42);
            swModelExt.SaveAs3(System.IO.Path.ChangeExtension(filepath, "SLDDRW"), (int)swSaveAsVersion_e.swSaveAsStandardDrawing, (int)swSaveAsOptions_e.swSaveAsOptions_SaveReferenced, null, null, 0, 0);

            //setting property options for drawings
            swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swAutomaticScaling3ViewDrawings, true);

            //adjusting new sheets and views
            for (int i = 0; i < swParts.Count; i++)
            {
                //naming the sheet with the part num
                swDrawing = (DrawingDoc)swApp.ActiveDoc;
                Sheet swSheet;
                swSheet = (Sheet)swDrawing.GetCurrentSheet();
                ModelDoc2 part = (ModelDoc2)swParts[i];
                filepath = part.GetPathName();
                swModelExt = part.Extension;
                swConfMgr = part.ConfigurationManager;
                swConfig = swConfMgr.ActiveConfiguration;
                configName = swConfig.Name;
                swCustomPropMgr = swModelExt.get_CustomPropertyManager("");
                swCustomPropMgr.Get6("nr rysunku", false, out resolvedValOut, out drawingNum, out wasResolved, out linkToProperty);
                swSheet.SetName(drawingNum);

                //create views with alignmets
                View frontView = swDrawing.CreateDrawViewFromModelView3(filepath, "*Przód", 0, 0, 0);
                move = MoveDrawingView(frontView, "frontView");
                frontView.SetName2($"FrontView{i}");
                swModel = (ModelDoc2)swApp.ActiveDoc;
                string cos = part.GetPathName();
                swModelExt = swModel.Extension;
                swModelExt.SelectByID2($"FrontView{i}", "DRAWINGVIEW", 0, 0, 0, false, 0, null, 0);
                View righView = swDrawing.CreateUnfoldedViewAt3(move[0]+0.1, move[1], 0, false);
                //MoveDrawingView(righView, "rightView");
                swModelExt.SelectByID2($"FrontView{i}", "DRAWINGVIEW", 0, 0, 0, false, 0, null, 0);
                View topView = swDrawing.CreateUnfoldedViewAt3(move[0], move[1]+0.1, 0, false);
                //MoveDrawingView(topView, "topView");
                swModelExt.SelectByID2($"FrontView{i}", "DRAWINGVIEW", 0, 0, 0, false, 0, null, 0);
                View isometricView = swDrawing.CreateUnfoldedViewAt3(move[0]+0.1, move[1]+0.1, 0, false);
                //MoveDrawingView(isometricView, "isometricView");
                isometricView.SetName2($"IsometricView{i}");

                //create BOM and parts numeration

                if(part.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
                {
                    swModelExt.SelectByID2($"IsometricView{i}", "DRAWINGVIEW", 0, 0, 0, false, 0, null, 0);
                    isometricView.InsertBomTable4(true, 0.417, 0.047, (int)swBOMConfigurationAnchorType_e.swBOMConfigurationAnchor_BottomRight, (int)swBomType_e.swBomType_TopLevelOnly,
                       configName, "C:\\EBA\\SZABLONY\\SolidWorks\\tabela_material.sldbomtbt", false, 0, false);

                    swDrawing = (DrawingDoc)swApp.ActiveDoc;
                    swModelExt.SelectByID2($"IsometricView{i}", "DRAWINGVIEW", 0, 0, 0, false, 0, null, 0);
                    AutoBalloonOptions balloon =  (AutoBalloonOptions)swDrawing.CreateAutoBalloonOptions();
                    balloon.IgnoreMultiple = true;
                    balloon.InsertMagneticLine = true;
                    swDrawing.AutoBalloon5(balloon);

                }

                if (i != swParts.Count - 1)
                {
                    swDrawing.NewSheet4($"temp{i}", (int)swDwgPaperSizes_e.swDwgPaperA3size, (int)swDwgTemplates_e.swDwgTemplateCustom, scale1, scale2, true, templatepath, 0.297, 0.42, "", 0, 0, 0, 0, 0, 0);
                    frontView.GetNextView();
                }

            }
        }


        public void GenerateDrawingsSplitted(AssemblyDoc swAss, List<string>swPartsFilepaths)
        {
            SldWorks swApp = (SldWorks)Marshal2.GetActiveObject("SldWorks.Application");
            ModelDoc2 swModelAss = (ModelDoc2)swAss;
            DrawingDoc swDrawing;
            Sheet swSheet;
            int swModelType;
            ConfigurationManager swConfMgr;
            Configuration swConfig;
            List<ModelDoc2> swParts = new List<ModelDoc2>();
            swFeatMgr = swModelAss.FeatureManager;
            swConfMgr = (ConfigurationManager)swModelAss.ConfigurationManager;
            TreeControlItem node = swFeatMgr.GetFeatureTreeRootItem2((int)swFeatMgrPane_e.swFeatMgrPaneBottom);
            node = node.GetFirstChild();
            swParts = TraverseForDrawings(node, swParts, swPartsFilepaths);
            //object[] swComps = swAss.GetComponents(false);
            string filepath;
            string configName;
            string drawingNum;
            string resolvedValOut;
            bool wasResolved = true;
            bool linkToProperty = false;
            int scale1 = 1;
            int scale2 = 1;
            double[] move;
            const string templatepath = "C:\\EBA\\SZABLONY\\SolidWorks\\EBA_A3_RYSUNEK.drwdot";

            for (int i = 0; i < swParts.Count; i++)
            {
                ModelDoc2 swModel = swParts[i];
                swModelType = swModel.GetType();
                filepath = swModel.GetPathName();
                swPartsFilepaths.Add(filepath);
                swApp.ActivateDoc3(swModel.GetPathName(), false, 0, 0);
                swApp.INewDocument2(templatepath, (int)swDwgPaperSizes_e.swDwgPaperA3size, 0.297, 0.42);
                swDrawing = (DrawingDoc)swApp.ActiveDoc;
                swSheet = (Sheet)swDrawing.GetCurrentSheet();
                swModelExt = swModel.Extension;
                swConfMgr = swModel.ConfigurationManager;
                swConfig = swConfMgr.ActiveConfiguration;
                configName = swConfig.Name;
                swCustomPropMgr = swModelExt.get_CustomPropertyManager("");
                swCustomPropMgr.Get6("nr rysunku", false, out resolvedValOut, out drawingNum, out wasResolved, out linkToProperty);
                swSheet.SetName(drawingNum);

                View frontView = swDrawing.CreateDrawViewFromModelView3(filepath, "*Przód", 0, 0, 0);
                move = MoveDrawingView(frontView, "frontView");
                frontView.SetName2($"FrontView");
                swModel = (ModelDoc2)swApp.ActiveDoc;
                swModelExt = swModel.Extension;
                swModelExt.SelectByID2($"FrontView", "DRAWINGVIEW", 0, 0, 0, false, 0, null, 0);
                View righView = swDrawing.CreateUnfoldedViewAt3(move[0] + 0.1, move[1], 0, false);
                //MoveDrawingView(righView, "rightView");
                swModelExt.SelectByID2($"FrontView", "DRAWINGVIEW", 0, 0, 0, false, 0, null, 0);
                View topView = swDrawing.CreateUnfoldedViewAt3(move[0], move[1] + 0.1, 0, false);
                //MoveDrawingView(topView, "topView");
                swModelExt.SelectByID2($"FrontView", "DRAWINGVIEW", 0, 0, 0, false, 0, null, 0);
                View isometricView = swDrawing.CreateUnfoldedViewAt3(move[0] + 0.1, move[1] + 0.1, 0, false);
                //MoveDrawingView(isometricView, "isometricView");
                isometricView.SetName2($"IsometricView");

                if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
                {
                    //creating BOM and balloons
                    swModelExt.SelectByID2($"IsometricView", "DRAWINGVIEW", 0, 0, 0, false, 0, null, 0);
                    isometricView.InsertBomTable4(true, 0.417, 0.047, (int)swBOMConfigurationAnchorType_e.swBOMConfigurationAnchor_BottomRight, (int)swBomType_e.swBomType_TopLevelOnly,
                        configName, "C:\\EBA\\SZABLONY\\SolidWorks\\tabela_material.sldbomtbt", false, 0, false);

                    swDrawing = (DrawingDoc)swApp.ActiveDoc;
                    swModelExt.SelectByID2($"IsometricView", "DRAWINGVIEW", 0, 0, 0, false, 0, null, 0);
                    AutoBalloonOptions balloon = (AutoBalloonOptions)swDrawing.CreateAutoBalloonOptions();
                    balloon.IgnoreMultiple = true;
                    balloon.InsertMagneticLine = true;
                    swDrawing.AutoBalloon5(balloon);

                }
                swModel = (ModelDoc2)swApp.ActiveDoc;
                swModelExt = swModel.Extension;
                swModelExt.SaveAs3(System.IO.Path.ChangeExtension(filepath, "SLDDRW"), (int)swSaveAsVersion_e.swSaveAsStandardDrawing, (int)swSaveAsOptions_e.swSaveAsOptions_SaveReferenced, null, null, 0, 0);
                swApp.CloseDoc(filepath);
                swApp.CloseDoc(System.IO.Path.ChangeExtension(filepath, "SLDDRW")); 
            }
        }

        private double[] MoveDrawingView(View swView, string type)
        {

            double[] coords = new double[4];
            double[] move = new double[2];
            coords = (double[])swView.GetOutline();

            double xmin = 0;
            double xmax = 0;
            double ymin = 0;
            double ymax = 0;
            double xMove = 0;
            double xMoveFront = 0;
            double yMove = 0;
            double yMoveFront = 0;

            xmin = coords[0];
            ymin = coords[1];
            xmax = coords[2];
            ymax = coords[3];

            if(type == "frontView")
            {
                xMove = xmax + 0.017+(xmax/2);
                xMoveFront = xMove;
                yMove = ymax + 0.04+(ymax/2);
                yMoveFront = yMove;
            }
            else if(type == "rightView")
            {

                xMove = xMoveFront + (xmax / 2) + 0.25;
                
            }
            else if(type == "topView")
            {
                yMove = yMoveFront + (ymax / 2) + 0.25;
            }
            else if(type == "isometricView")
            {
                xMove = 2.086 - (xmax / 2);
                yMove = 1.470 - (ymax / 2);
            }
            move[0] = xMove;
            move[1] = yMove;
            swView.Position = move;

            return move;
        }

        private List<ModelDoc2> TraverseForDrawings(TreeControlItem node, List<ModelDoc2>swParts, List<string>swPartsFilepaths)
        {
            ModelDoc2 swModel;
            TreeControlItem childNode;
            int nodeType;

            while (node != null)
            {
                nodeType = node.ObjectType;
                if (nodeType == (int)swTreeControlItemType_e.swFeatureManagerItem_Component)
                {
                    swComp = (Component2)node.Object;
                    swModel = (ModelDoc2)swComp.GetModelDoc2();

                    if (swPartsFilepaths.Contains(swModel.GetPathName()) == false)
                    {
                        swPartsFilepaths.Add(swModel.GetPathName());

                        if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
                        {
                            swParts.Add(swModel);
                            childNode = node.GetFirstChild();
                            TraverseForDrawings(childNode, swParts,swPartsFilepaths);

                        }
                        else
                        {
                            swParts.Add(swModel);
                        }
                    }
                }
                node = node.GetNext();
            }
            return swParts;
        }

        public string GetBitMap(string filepath,string configName)
        {
            
            SldWorks swApp = (SldWorks)Marshal2.GetActiveObject("SldWorks.Application");
            string filename;
            string imgFilepath;
            string systemDir;
            if (System.IO.Path.GetExtension(filepath) == ".SLDASM")
            {
                filename = System.IO.Path.GetFileNameWithoutExtension(filepath)+"SLDASM";
            }
            else
            {
                filename = System.IO.Path.GetFileNameWithoutExtension(filepath)+"SLDPRT";
            }
            systemDir = System.IO.Path.GetPathRoot(System.Environment.SystemDirectory);

            imgFilepath = $"{systemDir}temp\\{filename}.BMP";

            if (Directory.Exists(System.IO.Path.GetDirectoryName(imgFilepath)) == false)
            {
                Directory.CreateDirectory($"{systemDir}temp");
            }

            //ONLY FOR TESTINGAPP --> ANOTHER OPTIOFOR GENERATE BITMAP
            //bool guwnit = swApp.GetPreviewBitmapFile(filepath, configName, imgFilepath);

            object imageObj = swApp.GetPreviewBitmap(filepath, configName);
            string fileName = System.IO.Path.GetFileNameWithoutExtension(filepath);
            System.Drawing.Image image;
            image = PictureDispConverter.Convert(imageObj);
            image.Save(imgFilepath, System.Drawing.Imaging.ImageFormat.Bmp);
            return imgFilepath;
            
        }

        public char SetRevision()
        {
            char newRevision;

            try
            {
                SldWorks swApp = (SldWorks)Marshal2.GetActiveObject("SldWorks.Application");
                ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;
                swModelExt = swModel.Extension;
                CustomPropertyManager swCustomPropMgr =  swModelExt.get_CustomPropertyManager("");
               

                swCustomPropMgr.Get6("rewizja", false, out string revision, out string RevolvedValOut, out bool WasResolved, out bool LinkToProperty);
                if( revision == "")
                {
                newRevision = 'A';
                }
                else
                {
                    newRevision = revision.ToCharArray()[0];
                    newRevision = (char)((int)newRevision + 1);
                }
               
                SetCustomProperty(swModel, "rewizja", newRevision.ToString(), "");
            }
            catch (NullReferenceException)
            {
               // MessageBox.Show("Włącz plik SolidWorks");
                newRevision = ' ';
            }

            return newRevision;
        }

        public void ChangeLanguageInSheet(string direction)
        {
            SldWorks swApp = (SldWorks)Marshal2.GetActiveObject("SldWorks.Application");
            //try
            //{
                ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;
                DrawingDoc swDrawing = (DrawingDoc)swApp.ActiveDoc;
                Sheet swSheet = (Sheet)swDrawing.GetCurrentSheet();
                View swDetailView;
                object[] swViews;
                object[] swAnnotations;
                object[] swDetails;
                object[] swDetailedViews;
                object[] swDetailAnnotations;
                int sheetCount = swDrawing.GetSheetCount();
                int annotationType;
                string scale;
                DrSection swSection;
                

                for(int i = 0; i < sheetCount; i++)
                {
                    swDrawing = (DrawingDoc)swApp.ActiveDoc;
                    swSheet = (Sheet)swDrawing.GetCurrentSheet();
                    swViews = (object[])swSheet.GetViews();
                    foreach (View view in swViews)
                    {
                        swAnnotations = (object[])view.GetAnnotations();
                        swDetailedViews = (object[])view.GetDetailCircles();
                        

                        if (swAnnotations != null)
                        {

                            foreach (Annotation annotation in swAnnotations)
                            {
                                annotationType = annotation.GetType();
                                string name = annotation.GetName();
                                if (annotationType == (int)swAnnotationType_e.swBlock)
                                {

                                }
                                else if (annotationType == (int)swAnnotationType_e.swDisplayDimension)
                                {

                                }
                                else if (annotationType == (int)swAnnotationType_e.swNote)
                                {
                                    ChangeNoteText(annotation, dicPolEng, direction,null);
                                }
                            }

                        }
                        else if (swDetailedViews != null)
                        {
                            foreach (DetailCircle detail in swDetailedViews)
                            {
                                swDetailView = detail.GetView();
                                swDetailAnnotations = (object[])swDetailView.GetAnnotations();
                                if(swDetailAnnotations != null)
                                {
                                    foreach (Annotation detailAnnotation in swDetailAnnotations)
                                    {
                                        annotationType = detailAnnotation.GetType();
                                        if (annotationType == (int)swAnnotationType_e.swNote)
                                        {
                                            scale = (string)view.ScaleRatio;
                                            ChangeNoteText(detailAnnotation, dicPolEng, direction,scale);
                                        }
                                    }
                                }
                                
                            }
                        
                        }

                        
                    }
                    swDrawing.SheetNext();
                }
            //}
            //catch (NullReferenceException)
            //{
            //    MessageBox.Show("Włącz plik rysunku SolidWorks - SLDDRW");
            //}

        }

        public void ChangeNoteText(Annotation swAnnotation, Dictionary<string,string>dicPolEng, string direction,string scale)
        {
            Note swNote = (Note)swAnnotation.GetSpecificAnnotation();
            string[] currentText = swNote.GetText().Split(' ');
            string[] processText = currentText;
            string finalText;
            for (int i = 0; i < currentText.Length; i++)
            {
                string word = (string)currentText[i].ToUpper();
                if (dicPolEng.Keys.Contains(word) && direction == "polToEng" && scale == null) 
                {
                    processText[i] = dicPolEng.FirstOrDefault(x=>x.Key == word).Value;
                }
                else if(dicPolEng.Values.Contains(word) && direction == "engToPol" &&  scale == null)
                {
                    processText[i] = dicPolEng.FirstOrDefault(x => x.Value == word).Key;
                }
                else
                {
                    processText[i] = dicPolEng.FirstOrDefault(x => x.Value == word).Key;
                }
            }
            finalText = String.Join(" ", processText);
            swNote.SetText(finalText);
        }

        public void CreateSigmaTemplate(string[] filepaths, string[] notes)
        {
   
            SldWorks swApp = (SldWorks)Marshal2.GetActiveObject("SldWorks.Application");
            DrawingDoc swDrawing;
            ModelDoc2 swModel;
            Sheet swSheet;
            ImportDxfDwgData swImportData;
            Feature swFeat;
            ModelDocExtension swModelExt;
            View swView;
            Annotation swAnnotation;
            object[] swAnnotationsToDelete;
            object[] swSheets;
            string filedir;
            string filename;

            FeatureManager swFeatMgr;
            const string templatepath = "C:\\EBA\\SZABLONY\\SolidWorks\\EBA_A3_RYSUNEK.drwdot";
            swApp.INewDocument2(templatepath, (int)swDwgPaperSizes_e.swDwgPaperA3size, 0.297, 0.42);

            swModel = (ModelDoc2)swApp.ActiveDoc;
            swFeatMgr = swModel.FeatureManager;
            swModelExt = swModel.Extension;
            swDrawing = (DrawingDoc)swApp.ActiveDoc;
            swSheet = (Sheet)swDrawing.GetCurrentSheet();
            swSheet.SetName($"Sheet0");

            swSheet.SheetFormatVisible = false;
            swSheet.SetScale(1, 1, false, false);


            swModelExt.SelectByID2("", "NOTE", 0.032, 0.045, 0, false, 0, null, 0);
            swModelExt.DeleteSelection2((int)swDeleteSelectionOptions_e.swDelete_Absorbed);


            for (int i = 0; i < filepaths.Length; i++)
            {

             if(filepaths[i] != null)
                {
                    //filepaths[i] = System.IO.Path.ChangeExtension(filepaths[i], "DXF");
                    swSheet = (Sheet)swDrawing.GetCurrentSheet();
                    swSheet.SheetFormatVisible = false;
                    swModelExt.SelectByID2($"Sheet{i}", "SHEET", 0, 0, 0, false, 0, null, 0);
                    swImportData = (ImportDxfDwgData)swApp.GetImportFileData(filepaths[i]);
                    swImportData.set_LengthUnit("", (int)swLengthUnit_e.swMM);
                    swImportData.SetPaperSize("", (int)swDwgPaperSizes_e.swDwgPaperA3size, 0, 0);
                    swImportData.set_ImportMethod("", (int)swImportDxfDwg_ImportMethod_e.swImportDxfDwg_ImportToExistingDrawing);
                    swImportData.SetSheetScale("", 1, 1);
                    swImportData.SetPosition("", (int)swDwgImportEntitiesPositioning_e.swDwgEntitiesSpecifyPosition, 0, 0);
                    swFeat = swFeatMgr.InsertDwgOrDxfFile2(filepaths[i], swImportData);
                    swDrawing.CreateText2(notes[i], 0, 0, 0, 0.003, 0);
                    filename = System.IO.Path.GetFileNameWithoutExtension(filepaths[i]);
                    filedir = System.IO.Path.GetDirectoryName(filepaths[i]);
                    swModelExt.SaveAs3($"{filedir}\\{filename}-SIGMA.DXF", 0, 2, null, null, 16, 1);
                    if (i != filepaths.Length - 1)
                    {
                        swDrawing.NewSheet4($"sheet{i + 1}", (int)swDwgPaperSizes_e.swDwgPaperA3size, (int)swDwgTemplates_e.swDwgTemplateCustom, 1, 1, true, templatepath, 0.297, 0.42, "", 0, 0, 0, 0, 0, 0);
                    }
                }       
            }

            swApp.CloseDoc("");
        }

        public void ExportSplittedPDFs()
        {
            try
            {
                SldWorks swApp = (SldWorks)Marshal2.GetActiveObject("SldWorks.Application");
                ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;
                ModelDocExtension swModelExt;
                ExportPdfData exportPdfData = (ExportPdfData)swApp.GetExportFileData((int)swExportDataFileType_e.swExportPdfData);
                exportPdfData.ViewPdfAfterSaving = false;

                string assemblyFilepath = swModel.GetPathName();
                string pdfFilepath;
                string[] sheetName = new string[1];
                string[] sheetNames;
              
                if (swModel.GetType() != (int)swDocumentTypes_e.swDocDRAWING)
                {
                    var szajs1 = swModel.GetType();
                    string szajs = (System.IO.Path.ChangeExtension(assemblyFilepath, ".SLDDRW"));
                    swApp.OpenDoc6(System.IO.Path.ChangeExtension(assemblyFilepath, ".SLDDRW"), (int)swDocumentTypes_e.swDocDRAWING, 0, "", 0, 0);
                    swApp.ActivateDoc3(System.IO.Path.ChangeExtension(assemblyFilepath, ".SLDDRW"), false, 0, 0);

                }
                swModel = (ModelDoc2)swApp.ActiveDoc;

                DrawingDoc swDrawing = (DrawingDoc)swApp.ActiveDoc;
                Sheet swSheet = (Sheet)swDrawing.GetCurrentSheet();
                sheetNames = (string[])swDrawing.GetSheetNames();
                int sheetCount = swDrawing.GetSheetCount();

                swDrawing = (DrawingDoc)swApp.ActiveDoc;
                swModel = (ModelDoc2)swDrawing;
                swModelExt = swModel.Extension;
                swSheet = (Sheet)swDrawing.GetCurrentSheet();
                pdfFilepath = System.IO.Path.ChangeExtension(assemblyFilepath, ".PDF");
                exportPdfData.SetSheets((int)swExportDataSheetsToExport_e.swExportData_ExportAllSheets, sheetName);
                swModelExt.SaveAs3(pdfFilepath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, exportPdfData, null, 0, 0);
                SplitPdf(sheetNames, pdfFilepath);
            }
            catch (NullReferenceException)
            {
               // MessageBox.Show("Nie znaleziono rysunku");
            }
            catch (System.InvalidCastException)
            {
               // MessageBox.Show("Nie znaleziono rysunku CAST");
            }

        }

        public void SplitPdf(string[] sheetNames, string filepath)
        {
            // Open the file
            PdfDocument inputDocument = PdfSharp.Pdf.IO.PdfReader.Open(filepath, PdfDocumentOpenMode.Import);

            for (int i = 0; i < sheetNames.Length; i++)
            {
                // Create new document
                PdfDocument outputDocument = new PdfDocument();
                outputDocument.Version = inputDocument.Version;
                outputDocument.Info.Creator = inputDocument.Info.Creator;

                // Add the page and save it
                outputDocument.AddPage(inputDocument.Pages[i]);
                outputDocument.Save(String.Format($"{System.IO.Path.GetDirectoryName(filepath)}\\{System.IO.Path.GetFileNameWithoutExtension(filepath)}-{sheetNames[i]}.PDF"));
            }

        }

         //public List<TrackingPart> TrackParts()
         //{
         //   SldWorks swApp = (SldWorks)Marshal2.GetActiveObject("SldWorks.Application");
         //   AssemblyDoc swAss = (AssemblyDoc)swApp.ActiveDoc;
         //   List<TrackingPart> trackingParts = new List<TrackingPart>();
         //   string part;
         //   var partsList = CountParts(swAss);
         //   foreach (var path in partsList)
         //   {
         //       part = System.IO.Path.GetFileNameWithoutExtension(path);
         //       if (trackingParts.Any(x => x.Name == part) == false)
         //       {
         //           TrackingPart trackingPart = new TrackingPart();
         //           trackingPart.Name = part;
         //           trackingPart.Quantity = partsList.Where(n => n == path).Count();
         //           trackingParts.Add(trackingPart);
         //       }
         //   }

         //   return trackingParts;
         //}

        public CalculationObject InfoAboutPart()
        {
            SldWorks swApp = (SldWorks)Marshal2.GetActiveObject("SldWorks.Application");
            ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;
            CalculationObject calculationObject = new CalculationObject();
            string name = System.IO.Path.GetFileNameWithoutExtension(swModel.GetPathName());
            var type = swModel.GetType();

            calculationObject.Name = name;
            calculationObject.IconURL = "";
            calculationObject.Comments = "";
            switch (type)
            {
                case ((int)swDocumentTypes_e.swDocASSEMBLY):
                    calculationObject.Type = "Assembly";
                    break;

                case ((int)swDocumentTypes_e.swDocPART):
                    calculationObject.Type = "Part";
                    break;

                case ((int)swDocumentTypes_e.swDocIMPORTED_ASSEMBLY):
                    calculationObject.Type = "ImportedAssembly";
                    break;

                case ((int)swDocumentTypes_e.swDocIMPORTED_PART):
                    calculationObject.Type = "ImportedPart";
                    break;

                case ((int)swDocumentTypes_e.swDocLAYOUT):
                case ((int)swDocumentTypes_e.swDocNONE):
                case ((int)swDocumentTypes_e.swDocSDM):
                    calculationObject.Type = "N/A";
                    break;
            }

            return calculationObject;
        }
        public IEnumerable<SWTreeNode> SWTreeInit()
        {
            SldWorks swApp = (SldWorks)Marshal2.GetActiveObject("SldWorks.Application");
            ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;
            FeatureManager swFeatMgr = swModel.FeatureManager;
            TreeControlItem node = swFeatMgr.GetFeatureTreeRootItem2((int)swFeatMgrPane_e.swFeatMgrPaneBottom);
            string assemblyName = System.IO.Path.GetFileNameWithoutExtension(swModel.GetPathName());

            //add main node assembly
            SWTreeNode mainAssembly = new SWTreeNode() { Name = assemblyName };

            //add other nodes
            SWTreeNode treeNode = new SWTreeNode() { Name = assemblyName };
            node = node.GetFirstChild();
            treeNode = CreateTreeFromSW(node, "", treeNode);
            mainAssembly.Items.Add(treeNode);

            return mainAssembly.Items;

        }
        public SWTreeNode CreateTreeFromSW(TreeControlItem node, string parentNum, SWTreeNode swTreeNodes)
        {
            int nodeType;
            ModelDoc2 swModel;
            TreeControlItem childNode;
            Component2 swComp;
            int drawingNum = 1;
            string evaluatedParentNum;
            string parentNumOld;
            string finalDrawingNum = drawingNum.ToString();
            string name;
            string path;


            while (node != null)
            {
                nodeType = node.ObjectType;
                if (nodeType == (int)swTreeControlItemType_e.swFeatureManagerItem_Component)
                {
                    swComp = (Component2)node.Object;
                    swModel = (ModelDoc2)swComp.GetModelDoc2();
                    name = System.IO.Path.GetFileNameWithoutExtension(swModel.GetPathName());
                    path = System.IO.Path.GetFullPath(swModel.GetPathName());
                    if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
                    {
                        childNode = node.GetFirstChild();

                        parentNumOld = parentNum;
                        parentNum = $"{parentNum}.{drawingNum}";
                        //add position in a tree level 
                        SWTreeNode newTreeLevel = new SWTreeNode() { Name = name, Path = path };

                        CreateTreeFromSW(childNode, parentNum, newTreeLevel);
                        swTreeNodes.Items.Add(newTreeLevel);
                        parentNum = parentNumOld;
                    }
                    else
                    {
                        evaluatedParentNum = $"{parentNum}.{drawingNum}";
                        //add position in a tree level
                        swTreeNodes.Items.Add(new SWTreeNode() { Name = name, Path = path });
                    }
                    drawingNum++;

                }
                node = node.GetNext();
            }

            return swTreeNodes;
        }

        public void OpenSelectedPart(string filepath)
        {
            int extensionInt = 0;
            if(Path.GetExtension(filepath) == "SLDPRT")
            {
                extensionInt = 1;
            }
            else if(Path.GetExtension(filepath) == "SLDASM")
            {
                extensionInt = 2;
            }
            _swApp.OpenDoc6(filepath, (int)swDocumentTypes_e.swDocPART, 0, "", 0, 0);
            _swApp.ActivateDoc3(System.IO.Path.GetFileName(filepath), false, 0, 0);
        }
        public void CalculateAssembly(ModelDoc2 swModel)
        {

        }

        public void CalculateNodes()
        {
            SldWorks swApp = (SldWorks)Marshal2.GetActiveObject("SldWorks.Application");
            ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;
            var type = swModel.GetType();

            if (type == (int)swDocumentTypes_e.swDocASSEMBLY)
            {
                CalculateAssembly(swModel);
            }
            else if(type == (int)swDocumentTypes_e.swDocPART)
            {

            }
            else if(type == (int)swDocumentTypes_e.swDocIMPORTED_ASSEMBLY)
            {

            }
            else if(type == (int)swDocumentTypes_e.swDocIMPORTED_PART)
            {

            }
            else
            {

            }
        }

        private double GetSheetThickness(Feature swFeat)
        {
            SheetMetalFeatureData swSheetMetal = default(SheetMetalFeatureData);
            double thickness;

            swSheetMetal = (SheetMetalFeatureData)swFeat.GetDefinition();
            thickness = swSheetMetal.Thickness;

            return (thickness * 1000);

        }


        private void SearchThroughOtherBodies(Feature swFeat)
        {
            Body2 swBody;
            Feature swSubFeat = (Feature)swFeat.GetFirstSubFeature();
            string name = swSubFeat.GetTypeName2();


            while (swSubFeat != null)
            {
                //swBody = swSubFeat.GetSpecificFeature2();
                swSubFeat = (Feature)swSubFeat.GetNextSubFeature();
                name = swSubFeat.GetTypeName2();
                Debug.Print(name);
            }

        }

        public CalculationPartVM CalculateConvertedPart(bool isPainted)
        {
            SldWorks swApp = (SldWorks)Marshal2.GetActiveObject("SldWorks.Application");
            ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;
            Feature swFeat;
            SheetMetalFeatureData swSheetMetal;
            ModelDocExtension swModelExt;
            MassProperty2 swMassProp;
            PartDoc swPart = (PartDoc)swModel;

            Dictionary<string, int> bends = new Dictionary<string, int>();
            double thickness;
            double mass;
            double massGross;
            double cutTime; //todo how to calculate it through which framework?
            double area;
            int bends_0_5;
            int bends_0_5_1;
            int bends_1_1_5;
            int bends_1_5;
            int threads;
            //int nutserts; //todo how to track it
            //int pems; //todo how to track it
            CalculationPartVM calculationPartVM = new CalculationPartVM();
            ObservableCollection<Operation> allOperations = JsonConvert.DeserializeObject<CalculationPartVM>(allOperationsstr).Operations;
            ObservableCollection<Operation> partOperations = new ObservableCollection<Operation>();

            object[] swBodies = (object[])swPart.GetBodies2(-1, false);
            string name;

            //getting base info about part
            swModelExt = swModel.Extension;
            swFeat = (Feature)swModelExt.GetTemplateSheetMetal();
            swSheetMetal = (SheetMetalFeatureData)swFeat.GetDefinition();
            thickness = swSheetMetal.Thickness * 1000;
            swMassProp = (MassProperty2)swModelExt.CreateMassProperty2();
            mass = swMassProp.Mass * 1000;
            area = swMassProp.SurfaceArea;

            //todo Calculate Mass gross and cut time
            //Nesting();

            // traversing through part's tree
            swFeat = (Feature)swModel.FirstFeature();

            while (swFeat != null)
            {
                name = swFeat.GetTypeName2();
                Debug.Print(name);

                //case for calcute bending from flatten sketch ---> used in converted sheets from import body
                if (name == "FlatPattern")
                {
                    bends = GetBendsFromSketch(swFeat);
                    bends_0_5 = bends["<0,5m"];
                    bends_0_5_1 = bends["0,5m<1m"];
                    bends_1_1_5 = bends["1m<1,5m"];
                    bends_1_5 = bends[">1,5m"];
                }
                else if (name == "CutListFolder")
                {

                }

                swFeat = (Feature)swFeat.GetNextFeature();

            }

            threads = DetectCircularBodies(swBodies);

            if (isPainted)
            {

                Operation operation = allOperations.FirstOrDefault(x => x.Name == "Malowanie");
                PaintingInfo paintingInfo = GetPaintingTime(swModel, area);
                operation.Time = paintingInfo.Time;
                //operation.PricePerItem = paintingInfo.Price;
                operation.QuantityPerItem = 1;
                partOperations.Add(operation);


            }
            calculationPartVM.Operations = partOperations;

            return calculationPartVM;
        }


        private Dictionary<string, int> GetBendsFromSketch(Feature swFeat)
        {
            Feature swSubFeat = (Feature)swFeat.GetFirstSubFeature();
            Sketch sketch;
            bool isConstruction;

            Dictionary<string, int> bends = new Dictionary<string, int>();
            bends.Add("<0,5m", 0);
            bends.Add("0,5m<1m", 0);
            bends.Add("1m<1,5m", 0);
            bends.Add(">1,5m", 0);

            string name;
            object[] lines;

            while (swSubFeat != null)
            {
                name = swSubFeat.GetTypeName2();

                if (name == "ProfileFeature")
                {
                    sketch = (Sketch)swSubFeat.GetSpecificFeature2();
                    lines = (object[])sketch.GetSketchSegments();
                    foreach (SketchSegment line in lines)
                    {
                        isConstruction = line.ConstructionGeometry;
                        if (isConstruction)
                        {
                            double length = line.GetLength() * 1000;
                            if (length < 500)
                            {
                                bends["<0,5m"] = bends["<0,5m"] + 1;
                            }
                            else if (length >= 500 || length < 1000)
                            {
                                bends["0,5m<1m"] = bends["0,5m<1m"] + 1;
                            }
                            else if (length >= 1000 || length < 1500)
                            {
                                bends["1m<1,5m"] = bends["1m<1,5m"] + 1;
                            }
                            else
                            {
                                bends[">1,5m"] = bends[">1,5m"] + 1;
                            }
                        }
                    }

                    swSubFeat = null;
                }
                else
                {
                    swSubFeat = (Feature)swSubFeat.GetNextSubFeature();
                }

                Debug.Print(name);
            }

            return bends;
        }

        private int DetectCircularBodies(object[] swBodies)
        {
            int threadsCount = 0;
            int faceCount;
            int circularFaces = 0;
            double[] massProps;
            double bodyArea;
            double bodyVolume;
            double vaRatio;
            bool isMetalSheet;
            double[] faceNormal;
            object[] faces;

            foreach (Body2 swBody in swBodies)
            {
                isMetalSheet = swBody.IsSheetMetal();


                if (isMetalSheet == false)
                {
                    string name = swBody.Name;
                    massProps = (double[])swBody.GetMassProperties(7850);
                    bodyArea = massProps[4] * 1000000;
                    bodyVolume = massProps[3] * 1000000000;
                    vaRatio = bodyVolume / bodyArea;
                    faces = (object[])swBody.GetFaces();
                    faceCount = swBody.GetFaceCount();
                    foreach (Face2 face in faces)
                    {
                        faceNormal = (double[])face.Normal;
                        if (faceNormal[0] == 0 && faceNormal[1] == 0 && faceNormal[2] == 0)
                        {
                            circularFaces++;
                        }
                    }


                    if (circularFaces >= 2 && vaRatio > 0.3 && vaRatio < 0.8)
                    {
                        threadsCount++;
                    }
                    circularFaces = 0;
                }
            }

            return threadsCount;
        }
        private PaintingInfo GetPaintingTime(ModelDoc2 swModel, double area)
        {
            PartDoc swPart = (PartDoc)swModel;
            double paintingTime = 0;
            double depth;
            double width;
            double height;
            double[] partCoords;

            partCoords = (double[])swPart.GetPartBox(false);

            depth = Math.Abs(partCoords[2]) + Math.Abs(partCoords[5]);
            height = Math.Abs(partCoords[1]) + Math.Abs(partCoords[4]);
            width = Math.Abs(partCoords[0]) + Math.Abs(partCoords[3]);

            return FindBestPaintingTime(depth, height, width, area);
        }
        private PaintingInfo FindBestPaintingTime(double depth, double height, double width, double area)
        {
            double boundX = 2400;
            double boundY = 1200;
            double boundZ = 800;
            double verticalDistance = 300;
            double horizontalDistance = 300;
            double depthDistance = 300;
            int verticalItemsPerOnce;
            int depthItemsPerOnce;
            double time;
            double price;
            double velocity = 0.8; //min/m
            double rate = 850; //zł/hr
            double ratio;
            double areaPrice;
            double areaRate = 0.25; //zł/dm2
            double[] opt1 = { width, height, depth };
            double[] opt2 = { width, depth, height };
            double[] opt3 = { depth, width, height };
            double[] opt4 = { depth, height, width };
            double[] opt5 = { height, width, depth };
            double[] opt6 = { height, depth, width };
            double[][] opts = { opt1, opt2, opt3, opt4, opt5, opt6 };

            area = area * 100; //converting to dm2

            List<PaintingInfo> paintingObjs = new List<PaintingInfo>();

            foreach (var array in opts)
            {
                rate = 850;
                if (array[0] > boundX || array[1] > boundY || array[2] > 800)
                {

                }
                else
                {
                    //adjusting horizontal distance for not hitting physically in painting machine
                    if (array[0] < 400)
                    {
                        horizontalDistance = 300;
                    }
                    else if (array[0] > 400 && array[0] < 1100)
                    {
                        horizontalDistance = 400;
                    }
                    else
                    {
                        horizontalDistance = 500;
                    }

                    verticalItemsPerOnce = Convert.ToInt32(Math.Round(boundY / (array[1] + verticalDistance), 0));
                    depthItemsPerOnce = Convert.ToInt32(Math.Round(boundZ / (array[2] + depthDistance), 0));
                    time = velocity * (array[0] + horizontalDistance) / (1000 * (verticalItemsPerOnce * depthItemsPerOnce)) * 60; //per one item in s
                    price = time * rate / 3600; //in zł

                    areaPrice = area * areaRate;
                    ratio = areaPrice / price;
                    while (ratio > 2)
                    {
                        rate = rate * 1.02;
                        price = time * rate / 3600; //in zł
                        ratio = areaPrice / price;
                    }
                    PaintingInfo paintingObj = new PaintingInfo();
                    paintingObj.Time = time;
                    paintingObj.Price = price;
                    paintingObj.Rate = rate;
                    paintingObj.Ratio = ratio;
                    paintingObjs.Add(paintingObj);
                }
            }

            return paintingObjs.OrderBy(x => x.Price).FirstOrDefault();

        }

    }
}


