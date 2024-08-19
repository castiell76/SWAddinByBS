using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SWApp.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Wpf.Ui.Controls;
using SWApp.Services;
using System.Threading;
using Wpf.Ui;
using System;
using System.Data;
using SWApp.Controls;
using System.Linq;
using static SWApp.Models.NaturalSorting;
using System.Reflection;

namespace SWApp.Viewmodels.Pages
{
    public partial class FilesPropertiesViewModel : ObservableObject
    {
        private SWObject _swObject;
        private IContentDialogService _contentDialogService;
        private HelpService _helpService;
        private ViewControl _viewControl;

        private ObservableCollection<SWFileProperties> _properties;
        public ObservableCollection<SWFileProperties> Properties
        {
            get => _properties;
            set => SetProperty(ref _properties, value);
        }

        public ObservableCollection<string> EngineersList { get; set; }
        public ObservableCollection<string> MaterialList { get; set; }

        public FilesPropertiesViewModel()
        {
            EngineersList = new ObservableCollection<string> { "Błaz", "Ktoś", "ktoś2s" };
            

            _swObject = new SWObject();
            _helpService = new HelpService();
            _viewControl = new ViewControl();

            _contentDialogService = HelpService.GetRequiredService<IContentDialogService>();
            _swObject.SupressedElementsDetected += OnSuprresedElementsDetected;
            _swObject.ErrorOccurred += OnErrorOccured;

            MaterialList = _swObject.GetMaterialList();
        }

        public void OnErrorOccured(string title, string message, ControlAppearance appearance, SymbolIcon icon)
        {
            _helpService.SnackbarService.Show(title, message, appearance, icon, TimeSpan.FromSeconds(3));
        }

        private async void OnSuprresedElementsDetected(object sender, bool e)
        {
            await ShowContentDialogAsync();
        }

        public void SaveToExcel(System.Windows.Controls.DataGrid datagrid)
        {
            try
            {
                DataTable dt = new DataTable();
                ExcelFile excelFile = new ExcelFile();
                string filepath;
                string cellValue;
                string cellValueToSort;
                int cellValueInt;
                string index = "2137";
                string assemblyFilepath = "C:\\Users\\ebabs\\OneDrive\\Pulpit\\projekty\\Ritzi\\Fine Dine naladowy\\AX0_028_357.SLDASM";
                string assemblyConfig = "Domyślna";
                filepath = _viewControl.SaveDialog(index, ".xlsx", "Plik Excel (*.xlsx) |*.xlsx");
               

                var listFromDG = new List<SWFileProperties>(datagrid.ItemsSource as IEnumerable<SWFileProperties>);

                //to avoid sorting empty string as 1st 
                foreach (var item in listFromDG)
                {
                    if (item.drawingNum == "")
                    {
                        item.drawingNum = "a";
                    }
                }

                //var sortedlistFromDG = listFromDG.OrderByDescending(x => x.drawingNum.Length).OrderBy(x => x.drawingNum, new NaturalStringComparer()).ToList();

                dt = ToDataTable(listFromDG);


                //removing char a from cells which should be empty
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cellValue = (dt.Rows[i]["drawingNum"].ToString());
                    if (cellValue == "a")
                    {
                        cellValue = "";
                        dt.Rows[i]["drawingNum"] = cellValue;
                    }

                }

                dt = dt.DefaultView.ToTable();

                excelFile.CreateWorkBook(dt, index, filepath, assemblyFilepath, assemblyConfig);
            }
            catch (NullReferenceException)
            {
                OnErrorOccured("Uwaga!", "Pobierz dane do tabelki.", ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Important24));
            }

            catch (System.ArgumentNullException)
            {
                OnErrorOccured("Uwaga!", "Otwórz plik SolidWorks.", ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Important24));
            }
        }
        private DataTable ToDataTable<T>(List<T> items)
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
        public async Task<ObservableCollection<SWFileProperties>> ReadPropertiesAsync()
        {
            bool hasSuppresedParts;
            int lightWeightCompsCount;
            object[] swComps;
            ObservableCollection<SWFileProperties> swFilesProperties = new ObservableCollection<SWFileProperties>();

            (swComps, hasSuppresedParts, lightWeightCompsCount) =  _swObject.ContainsSuppressedParts();

            if (hasSuppresedParts || lightWeightCompsCount != 0)
            {
                var userChoice = await ShowContentDialogAsync();

                if (userChoice == ContentDialogResult.Primary)
                {
                    swFilesProperties = _swObject.ReadProperties(swComps, true, lightWeightCompsCount, true);
                }
                else if (userChoice == ContentDialogResult.Secondary)
                {
                    swFilesProperties = _swObject.ReadProperties(swComps, true, lightWeightCompsCount, false);
                }
            }
            else
            {
                swFilesProperties = _swObject.ReadProperties(swComps, false, lightWeightCompsCount, false);
            }

            Properties = swFilesProperties; 
            return swFilesProperties;
        }

        private async Task<ContentDialogResult> ShowContentDialogAsync()
        {
            var userChoice = await _contentDialogService.ShowAsync(
                new ContentDialog
                {
                    Title = "Uwaga!",
                    Content = "Wykryto komponenty wygaszone i/lub w odciążeniu. Czy chcesz przywrócić je do pełnej pamięci?",
                    PrimaryButtonText = "Tak",
                    SecondaryButtonText = "Nie",
                }, CancellationToken.None);

            return userChoice;
        }
        public void SetProperties(List<CustomProperty> customProperties, string[] optionsStr, bool[] options)
        {
            try
            {
                List<string> doneParts = new List<string>();
                _swObject.SetAllProperties(doneParts, customProperties, optionsStr, options);
            }
            catch (NullReferenceException)
            {
                OnErrorOccured("Uwaga!", "Włącz poprawny plik SolidWorks", ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Important24));
            }
            

        }

        public void OpenSelectedComponent(System.Windows.Controls.DataGrid dataGrid)
        {
            SWFileProperties component = (SWFileProperties)dataGrid.SelectedItem;
            string filepath = component.filepath;
            _swObject.OpenSelectedPart(filepath);

        }

        public char SetRevision()
        {
            return _swObject.SetRevision();
        }
    }
}