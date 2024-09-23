using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SWApp.Models;
using SWApp.Services;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace SWApp.Viewmodels.Pages
{
    public partial class CrossSectionsViewmodel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ProfileSW> _crossSectionsList;
        private HelpService _helpService;
        private ISnackbarService _snackbarService;
        private SWObject _sWObject;

        public CrossSectionsViewmodel(ISnackbarService snackbarService)
        {
            _snackbarService= snackbarService;
            CrossSectionsList = new ObservableCollection<ProfileSW>();
            _helpService = new HelpService();
            _sWObject = new SWObject();
            _sWObject.ErrorOccurred += OnErrorOccured;
        }

        private void OnErrorOccured(string title, string message, ControlAppearance appearance, SymbolIcon icon)
        {
            _helpService.SnackbarService.Show(title, message, appearance, new SymbolIcon(SymbolRegular.Important24), TimeSpan.FromSeconds(3));
        }

        public void GenerateCrossSections()
        {
            string assemblyFilepath;
            string filePathDir;
            
            try
            {
                
                if (CrossSectionsList.Any(x => x.Type == ("wprowadź dane profila")) || CrossSectionsList.Any(x => x.Type == ("")) || CrossSectionsList.Count == 0 || CrossSectionsList.Any(x=> x.Type == string.Empty))
                {
                    OnErrorOccured("Uwaga!","Wprowadź poprawne dane profila", ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Important24));
                }
                else
                {
                    assemblyFilepath = _sWObject.CreateAssembly();
                    filePathDir = System.IO.Path.GetDirectoryName(assemblyFilepath);
                    _sWObject.CloseDoc(assemblyFilepath);


                    foreach (ProfileSW profile in CrossSectionsList)
                    {
                        switch (profile.Type)
                        {
                            case "pręt okrągły":
                                _sWObject.CreateCircularRod(profile, $"{filePathDir}\\");
                                _sWObject.AddToAssembly($"{filePathDir}\\{profile.Name}.SLDPRT", assemblyFilepath);
                                _sWObject.CloseDoc($"{profile.Name}.SLDPRT");
                                break;
                            case "pręt prostokątny":
                                _sWObject.CreateRectangleRod(profile, $"{filePathDir}\\");
                                _sWObject.AddToAssembly($"{filePathDir}\\{profile.Name}.SLDPRT", assemblyFilepath);
                                _sWObject.CloseDoc($"{profile.Name}.SLDPRT");
                                break;
                            case "rura prostokątna":
                                _sWObject.CreateRectangleProfile(profile, $"{filePathDir}\\");
                                _sWObject.AddToAssembly($"{filePathDir}\\{profile.Name}.SLDPRT", assemblyFilepath);
                                _sWObject.CloseDoc($"{profile.Name}.SLDPRT");
                                break;
                            case "rura okrągła":
                                _sWObject.CreateCircularProfile(profile, $"{filePathDir}\\");
                                _sWObject.AddToAssembly($"{filePathDir}\\{profile.Name}.SLDPRT", assemblyFilepath);
                                _sWObject.CloseDoc($"{profile.Name}.SLDPRT");
                                break;
                        }

                    }
                    OnErrorOccured("Sukces!", "Generowanie zakończone", ControlAppearance.Success, new SymbolIcon(SymbolRegular.Checkmark24));

                }
            }
            catch (System.InvalidCastException)
            {
                OnErrorOccured("Uwaga!", "Wprowadź poprawne dane profila", ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Important24));
            }
            catch (System.NullReferenceException)
            {
                OnErrorOccured("Uwaga!", "Wprowadź poprawne dane profila", ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Important24));
            }
        }

        public void Add()
        {
            CrossSectionsList.Add(new ProfileSW { Name = "", X = 0, Y = 0, Thickness = 0, Length = 0, DraftCount = 0, Type = "" });
        }

        public void Delete(System.Windows.Controls.DataGrid dgprofiles)
        {
            try
            {
                for (int i = 0; i < dgprofiles.SelectedItems.Count; i++)
                {
                    CrossSectionsList.Remove((ProfileSW)dgprofiles.SelectedItems[i]);
                }
            }
            catch (InvalidCastException)
            {
                var toDelete = dgprofiles.SelectedItem;
                toDelete = new ProfileSW { Name = "", X = 0, Y = 0, Thickness = 0, DraftCount = 0, Type = "" };
                CrossSectionsList.Remove((ProfileSW)toDelete);
            }
        }

        public List<ProfileSW> Copy(System.Windows.Controls.DataGrid dgprofiles)
        {
            List<ProfileSW> toCopy = new List<ProfileSW>();
            for (int i = 0; i < dgprofiles.SelectedItems.Count; i++)
            {

                ProfileSW toDuplicate = (ProfileSW)dgprofiles.SelectedItems[i];
                ProfileSW toInsert = new ProfileSW
                {
                    Name = toDuplicate.Name,
                    X = toDuplicate.X,
                    Y = toDuplicate.Y,
                    Thickness = toDuplicate.Thickness,
                    Length = toDuplicate.Length,
                    Type = toDuplicate.Type,
                    DraftCount = toDuplicate.DraftCount
                };
                toCopy.Add(toInsert);
            }
            return toCopy;
        }

        public void Paste(System.Windows.Controls.DataGrid dgprofiles)
        {
            List<ProfileSW> toPaste = Copy(dgprofiles);
            foreach (ProfileSW profile in toPaste)
            {
                CrossSectionsList.Add(profile);
            }
        }

    }
}