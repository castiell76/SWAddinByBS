using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private HelpService _helpService = new HelpService();
        private ISnackbarService _snackbarService;

        public CrossSectionsViewmodel(ISnackbarService snackbarService)
        {
            _snackbarService= snackbarService;
            CrossSectionsList = new ObservableCollection<ProfileSW>();
        }

        public void OnOpenSnackbar(string title, string message, ControlAppearance appearance)
        {
            _snackbarService = _helpService.SnackbarService;
            _snackbarService.Show(
                title,
                message,
                appearance,
                new SymbolIcon(SymbolRegular.Fluent24),
                TimeSpan.FromSeconds(3)
            );
        }

        public void GenerateCrossSections()
        {
            string assemblyFilepath;
            string filePathDir;
            SWObject sWObject = new SWObject();
            try
            {
                
                if (CrossSectionsList.Any(x => x.Type == ("wprowadź dane profila")) || CrossSectionsList.Any(x => x.Type == ("")) || CrossSectionsList.Count == 0 || CrossSectionsList.Any(x=> x.Type == string.Empty))
                {
                    OnOpenSnackbar("Uwaga!","Wprowadź poprawne dane profila", ControlAppearance.Caution);
                }
                else
                {
                    assemblyFilepath = sWObject.CreateAssembly();
                    filePathDir = System.IO.Path.GetDirectoryName(assemblyFilepath);
                    sWObject.CloseDoc(assemblyFilepath);


                    foreach (ProfileSW profile in CrossSectionsList)
                    {
                        switch (profile.Type)
                        {
                            case "pręt okrągły":
                                sWObject.CreateCircularRod(profile, $"{filePathDir}\\");
                                sWObject.AddToAssembly($"{filePathDir}\\{profile.Name}.SLDPRT", assemblyFilepath);
                                sWObject.CloseDoc($"{profile.Name}.SLDPRT");
                                break;
                            case "pręt prostokątny":
                                sWObject.CreateRectangleRod(profile, $"{filePathDir}\\");
                                sWObject.AddToAssembly($"{filePathDir}\\{profile.Name}.SLDPRT", assemblyFilepath);
                                sWObject.CloseDoc($"{profile.Name}.SLDPRT");
                                break;
                            case "rura prostokątna":
                                sWObject.CreateRectangleProfile(profile, $"{filePathDir}\\");
                                sWObject.AddToAssembly($"{filePathDir}\\{profile.Name}.SLDPRT", assemblyFilepath);
                                sWObject.CloseDoc($"{profile.Name}.SLDPRT");
                                break;
                            case "rura okrągła":
                                sWObject.CreateCircularProfile(profile, $"{filePathDir}\\");
                                sWObject.AddToAssembly($"{filePathDir}\\{profile.Name}.SLDPRT", assemblyFilepath);
                                sWObject.CloseDoc($"{profile.Name}.SLDPRT");
                                break;
                        }

                    }
                    OnOpenSnackbar("Sukces!", "Generowanie zakończone", ControlAppearance.Success);
                }
            }
            catch (System.InvalidCastException)
            {
                OnOpenSnackbar("Uwaga!", "Wprowadź poprawne dane profila", ControlAppearance.Caution);
            }
            catch (System.NullReferenceException)
            {
                OnOpenSnackbar("Uwaga!REF", "Wprowadź poprawne dane profila", ControlAppearance.Caution);
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