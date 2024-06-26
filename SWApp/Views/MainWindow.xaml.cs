using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using Microsoft.Win32;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using static SWApp.Models.NaturalSorting;
//using MachinelearningwithFullPhotos;
using Aspose.Pdf.Operators;
using System.Windows.Shapes;
using System.Windows.Media;
using SWApp.Models;
using SWApp.VM;
using Wpf.Ui.Appearance;

namespace SWApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [ComVisible(true)]
    public partial class MainWindow : UserControl
    {
        
        private readonly ProfilesVM _viewModel = new ProfilesVM(); 
        SWObject sWObject = new SWObject();
        ModelDoc2 swModel;
        FeatureManager swFeatMgr;
        Component2 swComp;
        Configuration swConfig;
        ConfigurationManager swConfMgr;
        List<FileProperty> fileProperties = new List<FileProperty>();
        List<string> engineers = new List<string>();
        ExcelFile excelFile = new ExcelFile();
        ObservableCollection<ProfileSW> profilesSW = new ObservableCollection<ProfileSW>();

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new ProfilesVM();
            //generate rows for datagrid profiles
            dataGridProfile.ItemsSource = _viewModel.ProfilesSWVM;

 

            comboDevelopedBy.ItemsSource = engineers;
            comboCheckedBy.ItemsSource = engineers;

            //generate rows for datagrid properties
            dgProperties.ItemsSource = fileProperties;
            
            fileProperties.Add(new FileProperty() { name = "index xl", value = ""});
            fileProperties.Add(new FileProperty() { name = "Nazwa wyrobu", value = ""});
            fileProperties.Add(new FileProperty() { name = "status dokumentacji", value = null});
            fileProperties.Add(new FileProperty() { name = "Akronim klienta", value = ""});

            engineers.Add("B.Szulc");
            engineers.Add("K.Królikowski");
            engineers.Add("M.Szczawiński");
            engineers.Add("P.Węgrzyn");
            engineers.Add("J.Kielar");
            engineers.Add("M.Gładysz");
            engineers.Add("J.Gaździk");
            engineers.Add("D.Jakubik");
            engineers.Add("M.Zdybel");
            engineers.Add("G.Kosterski");
            engineers.Sort();

        }



        private void BtnGenDXF_Click(object sender, RoutedEventArgs e)
        {
            SldWorks swApp = (SldWorks)Marshal2.GetActiveObject("SldWorks.Application");
            
            UserProgressBar pb; 
            swApp.GetUserProgressBar(out pb);
            int quantity = 0;
            List<ExportStatus> exportStatuses = new List<ExportStatus>();
            try
            {
                if (cbCreateDXF.IsChecked == false && cbCreateSTEP.IsChecked == false && cbDXFFromDrawing.IsChecked == false)
                {
                    MessageBox.Show("Wskaż wymagane opcje");

                }
             
                else
                {
                   if (cbCreateDXFForSigma.IsChecked == true)
                    {
                        QuantityForSigma sigmaWindow = new QuantityForSigma();
                        sigmaWindow.ShowDialog();
                        quantity = sigmaWindow.Quantity;
                    }
                    ConfigurationManager swConfigurationManager;
                    Configuration swConf;
                    AssemblyDoc swAss;
                    swAss = (AssemblyDoc)swApp.ActiveDoc;
                    object[] swComps;
                    int options = 1;
                    int swCompsCount;
                    
                    List<string> totalParts = sWObject.CountParts(swAss);
                    List<string> swTreeFilenames = new List<string>();
                    int pbPosition = 0;
                  
                    //options for dxf parts
                    if (cbSketchInclude.IsChecked == true)
                    {
                        options = options + 8;
                    }
                    if (cbFormingToolsInclude.IsChecked == true)
                    {
                        options = options + 64;
                    }


                    if (cbDXFFromDrawing.IsChecked == true)
                    {
                        sWObject.saveDXFFromDrawing(txtFolderDir.Text, (bool)cbDXFDir.IsChecked);
                    }
                   
                    swComps = (object[])swAss.GetComponents(false);
                    swCompsCount = swAss.GetComponentCount(false);
                    pb.Start(0, 1000,"In Progress");

                    if (txtFolderDir.Text == "")
                    {
                        swModel = (ModelDoc2)swAss;
                        txtFolderDir.Text = System.IO.Path.GetDirectoryName(swModel.GetPathName());
                    }

                    foreach (Component2 swComp in swComps)
                    {

                        if (cbAllDXF.IsChecked == true)
                        {
                            exportStatuses.Add(sWObject.ExportFile(swComp, txtFolderDir.Text, (bool)cbDXFDir.IsChecked, options, totalParts, cbCreateDXF.IsChecked == true, cbCreateSTEP.IsChecked == true, swTreeFilenames, quantity));
                           
                        }
                       else if (cbPBSheet.IsChecked == true && cbPTSheet.IsChecked == true)
                        {
                            if (swComp.Name2.Contains("PB") == true || swComp.Name.Contains("PT") == true)
                            {
                                exportStatuses.Add(sWObject.ExportFile(swComp, txtFolderDir.Text, (bool)cbDXFDir.IsChecked, options, totalParts, cbCreateDXF.IsChecked == true, cbCreateSTEP.IsChecked == true, swTreeFilenames, quantity));
                               
                            }
                        }
                        else if (cbPBSheet.IsChecked == true)
                        {
                            if (swComp.Name2.Contains("PB") == true)
                            {
                                exportStatuses.Add(sWObject.ExportFile(swComp, txtFolderDir.Text, (bool)cbDXFDir.IsChecked, options, totalParts, cbCreateDXF.IsChecked == true, cbCreateSTEP.IsChecked == true, swTreeFilenames, quantity));
                                
                            }
                        }
                        else if (cbPTSheet.IsChecked == true)
                        {
                            if (swComp.Name2.Contains("PT") == true)
                            {
                                exportStatuses.Add(sWObject.ExportFile(swComp, txtFolderDir.Text, (bool)cbDXFDir.IsChecked, options, totalParts, cbCreateDXF.IsChecked == true, cbCreateSTEP.IsChecked == true, swTreeFilenames, quantity));
                               
                            }
                        }

                        else
                        {
                            exportStatuses.Add(sWObject.ExportFile(swComp, txtFolderDir.Text, (bool)cbDXFDir.IsChecked, options, totalParts, cbCreateDXF.IsChecked == true, cbCreateSTEP.IsChecked == true, swTreeFilenames, quantity));
                            

                        }
                        pbPosition = pbPosition + (1000 / swCompsCount);
                        pb.UpdateProgress(pbPosition);

                    }
                    var exportGrouped = exportStatuses.GroupBy(x => new { x.name}).Select(x=>x.First()).ToList();
                    dgExport.ItemsSource = exportGrouped;

                    if(cbCreateDXFForSigma.IsChecked == true)
                    {
                       
                        string[] filepaths = new string[exportStatuses.GroupBy(x => x.dxfFilepath).Distinct().Count() - 1];
                        string[] sigmaNotes = new string[exportStatuses.GroupBy(x => x.dxfFilepath).Distinct().Count() -1];
                        int i = 0;
                        foreach(ExportStatus exportStatus in exportStatuses)
                        {
                            if(filepaths.Contains(exportStatus.dxfFilepath) == false && exportStatus.dxfFilepath != null && exportStatus.dxfFilepath != "" &&exportStatus.dxfCreated == true)
                            {
                                filepaths[i] = exportStatus.dxfFilepath;
                                sigmaNotes[i] = exportStatus.sigmaNote;
                                i++;
                            }
                        }
                       
                        sWObject.CreateSigmaTemplate(filepaths, sigmaNotes);
                    }
                    pb.End();
                }
        }
            catch (System.ObjectDisposedException)
            {

            }
            catch (System.NullReferenceException)
            {
                MessageBox.Show("Włącz złożenie, z którego chcesz wygenerować pliki");
            }
            catch (System.InvalidCastException)
            {
                MessageBox.Show("Wybierz plik typu złożenie");
            }
            finally
            {
                pb.End();
            }

        }



        private void BtnFolderDir_Click(object sender, RoutedEventArgs e)
        {
            txtFolderDir.Text = sWObject.GetDirDXF();
            cbDXFDir.IsChecked = false;
        }


        private void CbAllDXF_Checked(object sender, RoutedEventArgs e)
        {
            cbPBSheet.IsEnabled = false;
            cbPTSheet.IsEnabled = false;
            cbPBSheet.IsChecked = true;
            cbPTSheet.IsChecked = true;
        }

        private void CbAllDXF_Unchecked(object sender, RoutedEventArgs e)
        {
            cbPBSheet.IsEnabled = true;
            cbPTSheet.IsEnabled = true;
            cbPBSheet.IsChecked = false;
            cbPTSheet.IsChecked = false;
        }

        private void BtnSort_Click(object sender, RoutedEventArgs e)
        {
            SldWorks swApp = (SldWorks)Marshal2.GetActiveObject("SldWorks.Application");
            swModel = (ModelDoc2)swApp.ActiveDoc;
            sWObject.SortTree_All(cboxSort.IsChecked == true, swModel);
        }

        private void BtnProperties_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SldWorks swApp = (SldWorks)Marshal2.GetActiveObject("SldWorks.Application");
               
                swModel = (ModelDoc2)swApp.ActiveDoc;
                int modelType = swModel.GetType();

                string material;
                if(lbMaterials.SelectedItem == null)
                {
                    material = "";
                }
                else
                {
                    material = lbMaterials.SelectedItem.ToString();
                }

             
                List<FileProperty> fileProperties = new List<FileProperty>();

                if (modelType == (int)swDocumentTypes_e.swDocASSEMBLY)
                {
                    AssemblyDoc swAss = (AssemblyDoc)swApp.ActiveDoc;
                    if (swAss.HasUnloadedComponents() == true || swAss.GetLightWeightComponentCount() != 0)
                    {
                        MessageBox.Show("Przed konwersją należy przywrócić wygaszone/odciążone pliki w złożeniu");
                    }

                    swFeatMgr = swModel.FeatureManager;

                    if (cbSetNums.IsChecked == true)
                    {
                        ModelDocExtension swModelExt = swModel.Extension;
                        CustomPropertyManager swCustomPropMgr = swModelExt.get_CustomPropertyManager("");
                        swCustomPropMgr.Add3("nr rysunku", (int)swCustomInfoType_e.swCustomInfoText, "0", (int)swCustomPropertyAddOption_e.swCustomPropertyReplaceValue);
                    }

                    swConfMgr = (ConfigurationManager)swModel.ConfigurationManager;
                    TreeControlItem node = swFeatMgr.GetFeatureTreeRootItem2((int)swFeatMgrPane_e.swFeatMgrPaneBottom);
                    node = node.GetFirstChild();
                    List<string> doneParts = new List<string>();
                    List<string> allParts = sWObject.CountParts(swAss);
                        

                    foreach (FileProperty fileProperty in dgProperties.Items)
                    {
                        fileProperties.Add(fileProperty);
                    }
                    bool cos = (bool)cbClearNums.IsChecked;
                    sWObject.SetAllProperties(node, doneParts, allParts, fileProperties, "", cbCopyPropsToAllConfigs.IsChecked == true, cbSetQuantity.IsChecked == true, (bool)cbSetThickness.IsChecked, (bool)cbClearNums.IsChecked, (bool)cbSetNums.IsChecked,
                        cbDevelopedBy.IsChecked == true, cbCheckedBy.IsChecked == true, cbSetMaterial.IsChecked == true, material, comboDevelopedBy.Text, comboCheckedBy.Text);
                    if (cbGenerateDrawings.IsChecked == true)
                    {
                        if (rbDrawingsTogether.IsChecked == true)
                        {
                            sWObject.GenerateDrawingsTogether();
                        }
                        else if (rbDrawingsSplitted.IsChecked == true)
                        {
                            List<string> filepaths = new List<string>();
                            sWObject.GenerateDrawingsSplitted(swAss, filepaths);
                        }
                    }
                }
                else if (modelType == (int)swDocumentTypes_e.swDocPART)
                {
                    foreach (FileProperty fileProperty in dgProperties.Items)
                    {
                        fileProperties.Add(fileProperty);
                    }
                    swFeatMgr = swModel.FeatureManager;
                    swConfMgr = (ConfigurationManager)swModel.ConfigurationManager;
                    TreeControlItem node = swFeatMgr.GetFeatureTreeRootItem2((int)swFeatMgrPane_e.swFeatMgrPaneBottom);
                    node = node.GetFirstChild();

                    sWObject.SetAllProperties(node, null, null, fileProperties, "", cbCopyPropsToAllConfigs.IsChecked == true, cbSetQuantity.IsChecked == true, cbSetThickness.IsChecked == true, cbClearNums.IsChecked == true, false,
                        cbDevelopedBy.IsChecked == true, cbCheckedBy.IsChecked == true, cbSetMaterial.IsChecked == true, material, comboDevelopedBy.Text, comboCheckedBy.Text);
                    if (cbGenerateDrawings.IsChecked == true)
                    {
                        sWObject.GenerateDrawingsTogether();
                    }
                }
                TakeProperties();
            }
            catch (InvalidCastException)
            {
                MessageBox.Show("Otwórz plik typu złożenie");
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Otwórz plik SolidWorks");
            }


        }
        private void TakeProperties()
        {
            var (index, assemblypath, assemblyconfig, swFilesProperties) = sWObject.ReadData();
            string cellValue;
            tbIndex.Text = index;
            dgAllProperties.ItemsSource = swFilesProperties;

            tbassFilepath.Text = assemblypath;
            tbassConfig.Text = assemblyconfig;
        }
        private void btnTakeProperties_Click_1(object sender, RoutedEventArgs e)
        {
            TakeProperties();
        }

        private void BtnSaveToExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();

                string cellValue;
                string cellValueToSort;
                int cellValueInt;
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.FileName = tbIndex.Text;
                saveFileDialog1.DefaultExt = ".xlsx";
                saveFileDialog1.Filter = "Plik Excel (*.xlsx) |*.xlsx";

                saveFileDialog1.ShowDialog();
                string filepath = saveFileDialog1.FileName;
                string index = tbIndex.Text;
                string assemblyFilepath = tbassFilepath.Text;
                string assemblyConfig = tbassConfig.Text;

                var listFromDG = new List<SWFileProperties>(dgAllProperties.ItemsSource as IEnumerable<SWFileProperties>);

                //to avoid sorting empty string as 1st 
                foreach(var item in listFromDG)
                {
                    if(item.drawingNum == "")
                    {
                        item.drawingNum = "a";
                    }
                }

                var sortedlistFromDG = listFromDG.OrderByDescending(x=>x.drawingNum.Length).OrderBy(x => x.drawingNum, new NaturalStringComparer()).ToList();

                dt = ToDataTable(sortedlistFromDG);


                //removing char a from cells which should be empty
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cellValue = (dt.Rows[i]["drawingNum"].ToString());
                    if(cellValue == "a")
                    {
                        cellValue = "";
                        dt.Rows[i]["drawingNum"]= cellValue;
                    }

                }

                dt = dt.DefaultView.ToTable();

                excelFile.CreateWorkBook(dt, index, filepath, assemblyFilepath,assemblyConfig);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Pobierz dane do tabelki");
            }

            catch (System.ArgumentNullException)
            {
                MessageBox.Show("Otwórz plik SolidWorks");
            }
        }
     

        public  DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        private void SetCompsNums()
        {
            try
            {
                SldWorks swApp = (SldWorks)Marshal2.GetActiveObject("SldWorks.Application");
                swModel = (ModelDoc2)swApp.ActiveDoc;
                swFeatMgr = swModel.FeatureManager;
                swConfMgr = (ConfigurationManager)swModel.ConfigurationManager;
                swConfig = (Configuration)swConfMgr.ActiveConfiguration;
                swComp = (Component2)swConfig.GetRootComponent3(true);
                TreeControlItem node = swFeatMgr.GetFeatureTreeRootItem2((int)swFeatMgrPane_e.swFeatMgrPaneBottom);
                swModel = (ModelDoc2)swComp.GetModelDoc2();
                sWObject.SetCustomProperty(swModel, "nr rysunku", "0", "");
                node = node.GetFirstChild();
                List<string> doneParts = new List<string>();
                sWObject.SetCompsNums(node, "", doneParts);
            }
            catch (System.NullReferenceException)
            {
                MessageBox.Show("Otwórz plik typu złożenie SolidWorks.");
            }
        }

        private void ComboDevelopedBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbDevelopedBy.IsChecked = true;
        }

        private void ComboCheckedBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbCheckedBy.IsChecked = true;
        }

        private void miAdd_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Add();
        }

        private void miDelete_Click(object sender, RoutedEventArgs e)
        {

            _viewModel.Delete(dataGridProfile);
            
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_viewModel.ProfilesSWVM.Any(x => x.Type == ("wprowadź dane profila")) || _viewModel.ProfilesSWVM.Any(x => x.Type == ("")))
                {
                    MessageBox.Show("Wprowadź poprawne wartości liczbowe");
                }
                else
                {
                    List<string> filepathWithName = sWObject.CreateAssembly();

                    string assemblyName = filepathWithName[1];
                    string filepathAsm = filepathWithName[0];
                    string filepathDir = filepathWithName[2];

                    foreach (ProfileSW profile in _viewModel.ProfilesSWVM)
                    {
                        switch (profile.Type)
                        {
                            case "pręt okrągły":
                                sWObject.CreateCircularRod(profile, $"{filepathDir}\\");
                                sWObject.AddToAssembly($"{filepathDir}\\{profile.Name}.SLDPRT", assemblyName);
                                sWObject.CloseDoc($"{profile.Name}.SLDPRT");
                                break;
                            case "pręt prostokątny":
                                sWObject.CreateRectangleRod(profile, $"{filepathDir}\\");
                                sWObject.AddToAssembly($"{filepathDir}\\{profile.Name}.SLDPRT", assemblyName);
                                sWObject.CloseDoc($"{profile.Name}.SLDPRT");
                                break;
                            case "rura prostokątna":
                                sWObject.CreateRectangleProfile(profile, $"{filepathDir}\\");
                                sWObject.AddToAssembly($"{filepathDir}\\{profile.Name}.SLDPRT", assemblyName);
                                sWObject.CloseDoc($"{profile.Name}.SLDPRT");
                                break;
                            case "rura okrągła":
                                sWObject.CreateCircularProfile(profile, $"{filepathDir}\\");
                                sWObject.AddToAssembly($"{filepathDir}\\{profile.Name}.SLDPRT", assemblyName);
                                sWObject.CloseDoc($"{profile.Name}.SLDPRT");
                                break;
                        }

                    }
                    MessageBox.Show($"Wykonane");
                }
            }
            catch (System.InvalidCastException)
            {
                MessageBox.Show("Wprowadź poprawne wartości liczbowe");
            }
        }


        private void MiCopy_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Copy(dataGridProfile);
        }

        private void MiPaste_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Paste(dataGridProfile);
        }

        private void BtnConvertToSheet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<object> comps = new List<object>(profilesSW);
                List<string> compsNames = new List<string>();
                SldWorks swApp = (SldWorks)Marshal2.GetActiveObject("SldWorks.Application");
                AssemblyDoc swAss = (AssemblyDoc)swApp.ActiveDoc;
                if (swAss.HasUnloadedComponents() == true || swAss.GetLightWeightComponentCount() != 0)
                {
                    MessageBox.Show("Przed konwersją należy przywrócić wygaszone/odciążone pliki w złożeniu");
                }
                else
                {
                    ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;
                    swModel.Save3(4, 0, 0);
                    swFeatMgr = swModel.FeatureManager;
                    TreeControlItem rootNode = swFeatMgr.GetFeatureTreeRootItem2(0);
                    rootNode = rootNode.GetFirstChild();
                    List<object> compsToConvert = sWObject.TakeSheetsToConvert(rootNode, comps, compsNames);
                    List<ConvertStatus> convertedParts = sWObject.ConvertToSheets(swAss, compsToConvert);
                    var convertedPartsGrouped = convertedParts.GroupBy(x => new { x.name }).Select(x => x.First()).ToList();
                    dgConvert.ItemsSource = convertedPartsGrouped;
                    miConvertOpen.IsEnabled = true;
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Włącz plik typu złożenie");
            }
           

        }

        private void BtnSplitPartsToAssembly_Click(object sender, RoutedEventArgs e)
        {
            SldWorks swApp = (SldWorks)Marshal2.GetActiveObject("SldWorks.Application");
            swModel = (ModelDoc2)swApp.ActiveDoc;
            string partFilepath = swModel.GetPathName();
            List<string> filepathWithName = sWObject.CreateAssembly();

            string assemblyName = filepathWithName[1];
            string filepathAsm = filepathWithName[0];
            string filepathDir = filepathWithName[2];

            sWObject.SplitPart(filepathAsm, assemblyName, partFilepath);


        }

        private void MiConvertOpen_Click(object sender, RoutedEventArgs e)
        {
            
            SldWorks swApp = (SldWorks)Marshal2.GetActiveObject("SldWorks.Application");
            ConvertStatus convertToOpen = (ConvertStatus)dgConvert.SelectedItem;
            string filepath = convertToOpen.filepath;
            swApp.OpenDoc6(filepath, (int)swDocumentTypes_e.swDocPART, 0, "", 0, 0);
            swApp.ActivateDoc3(System.IO.Path.GetFileName(filepath), false, 0, 0);
        }

        private void miExportOpen_Click(object sender, RoutedEventArgs e)
        {
            SldWorks swApp = (SldWorks)Marshal2.GetActiveObject("SldWorks.Application");
            ExportStatus exportToOpen = (ExportStatus)dgExport.SelectedItem;
            string filepath = exportToOpen.filepath;
            swApp.OpenDoc6(filepath, (int)swDocumentTypes_e.swDocPART, 0, "", 0, 0);
            swApp.ActivateDoc3(System.IO.Path.GetFileName(filepath), false, 0, 0);
        }

        private void DataGridProfile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                _viewModel.Delete(dataGridProfile);
            }
            if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                _viewModel.Copy(dataGridProfile);
            }
            if (e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                _viewModel.Paste(dataGridProfile);
            }
            if (e.Key == Key.N && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                _viewModel.Add();
            }
        }


        private void CbGenerateDrawings_Checked(object sender, RoutedEventArgs e)
        {
            rbDrawingsSplitted.IsEnabled = true;
            rbDrawingsTogether.IsEnabled = true;
            rbDrawingsTogether.IsChecked = true;
        }

        private void CbGenerateDrawings_Unchecked(object sender, RoutedEventArgs e)
        {
            rbDrawingsSplitted.IsEnabled = false;
            rbDrawingsTogether.IsEnabled = false;
            rbDrawingsSplitted.IsChecked = false;
            rbDrawingsTogether.IsChecked = false;
        }

        private void BtnRevision_Click(object sender, RoutedEventArgs e)
        {
            char revision;
            revision = sWObject.SetRevision();
            tbRevision.Text = $"Obecna rewizja: \n {revision}";
        }

        private void CbSetMaterial_Checked(object sender, RoutedEventArgs e)
        {
            string materialPath = "c:\\eba\\szablony\\solidworks\\eba materiały.sldmat";
            List<string> materials = new List<string>();
            XmlDocument XMLD;
            XmlNodeList nodelist;
            XMLD = new XmlDocument();
            // Add Namespace
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(XMLD.NameTable);
            nsmgr.AddNamespace("mstns", "http://www.solidworks.com/sldmaterials");
            // Load document
            XMLD.Load(materialPath);
            nodelist = XMLD.SelectNodes("/mstns:materials/classification/material", nsmgr);
            foreach (XmlNode node in nodelist)
            {
                string material = node.Attributes.GetNamedItem("name").Value;
                if (material.Contains("BLACHA") == false && material.Contains("KSZTAŁTOWNIK") == false && material.Contains("RURA") == false && material.Contains("DRUT") == false && material.Contains("PRĘT") == false)
                {
                    materials.Add(material);
                }
            }

            lbMaterials.IsEnabled = true;
            lbMaterials.ItemsSource = materials;
            lbMaterials.SelectedItem = lbMaterials.Items[1];
        }

        private void CbSetMaterial_Unchecked(object sender, RoutedEventArgs e)
        {
            lbMaterials.IsEnabled = false;
        }


        private void CbCreateDXFForSigma_Checked(object sender, RoutedEventArgs e)
        {
            cbCreateDXF.IsChecked = true;
        }

        private void comboDevelopedBy_SelectionChanged_1(object sender, RoutedEventArgs e)
        {
            if(comboDevelopedBy.Text != "")
            {
                cbDevelopedBy.IsChecked = true;
            }
            
        }

        private void comboCheckedBy_SelectionChanged_1(object sender, RoutedEventArgs e)
        {
            if(comboCheckedBy.Text != "")
            {
                cbCheckedBy.IsChecked = true;
            }
            
        }

        private void BtnSaveSplittedPDFs_Click(object sender, RoutedEventArgs e)
        {
            sWObject.ExportSplittedPDFs();
        }

        //private void BtnTrackParts_Click(object sender, RoutedEventArgs e)
        //{
        //    //Load sample data
        //    var imageBytes = File.ReadAllBytes("C:\\Users\\BIP\\Desktop\\guwnit.PNG");

        //    MLModel1FromBitMaps.ModelInput sampleData = new MLModel1FromBitMaps.ModelInput()
        //    {
        //        ImageSource = imageBytes,
        //    };

        //    //Load model and predict output
        //    var result = MLModel1FromBitMaps.Predict(sampleData);

        //    MessageBox.Show(result.PredictedLabel.ToString());

        //}


        private void TabItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CalculationModule calculationModule = new CalculationModule();
            calculationModule.ShowDialog();
        }


    }
}
