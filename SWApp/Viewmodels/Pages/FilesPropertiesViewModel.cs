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

namespace SWApp.Viewmodels.Pages
{
    public partial class FilesPropertiesViewModel : ObservableObject
    {
        private SWObject _swObject;
        private IContentDialogService _contentDialogService;
        private HelpService _helpService;

        private ObservableCollection<SWFileProperties> _properties;
        public ObservableCollection<SWFileProperties> Properties
        {
            get => _properties;
            set => SetProperty(ref _properties, value);
        }

        public ObservableCollection<string> EngineersList { get; set; }

        public FilesPropertiesViewModel()
        {
            EngineersList = new ObservableCollection<string> { "Błaz", "Ktoś", "ktoś2s" };

            _swObject = new SWObject();
            _helpService = new HelpService();

            _contentDialogService = HelpService.GetRequiredService<IContentDialogService>();
            _swObject.SupressedElementsDetected += OnSuprresedElementsDetected;
            _swObject.ErrorOccurred += OnErrorOccured;
        }

        private void OnErrorOccured(string title, string message, ControlAppearance appearance, SymbolIcon icon)
        {
            _helpService.SnackbarService.Show(title, message, appearance, icon, TimeSpan.FromSeconds(3));
        }

        private async void OnSuprresedElementsDetected(object sender, bool e)
        {
            await ShowContentDialogAsync();
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
                _swObject.SetProperties(doneParts, customProperties, optionsStr, options);
            }
            catch (NullReferenceException)
            {
                OnErrorOccured("Uwaga!", "Włącz poprawny plik SolidWorks", ControlAppearance.Danger, new SymbolIcon(SymbolRegular.Important24));
            }
            

        }
    }
}