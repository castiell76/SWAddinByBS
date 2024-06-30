using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using SWApp.Models;

namespace SWApp.Viewmodels.Pages
{
    public partial class CrossSectionsViewmodel : ObservableObject, INotifyPropertyChanged
    {
        [ObservableProperty]
        private ObservableCollection<ProfileSW> _crossSectionsList;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public CrossSectionsViewmodel()
        {
            CrossSectionsList = new ObservableCollection<ProfileSW>();
        }

        public void GenerateCrossSections()
        {
            SWObject sWObject = new SWObject();
            try
            {
                if (CrossSectionsList.Any(x => x.Type == ("wprowadź dane profila")) || CrossSectionsList.Any(x => x.Type == ("")))
                {
                    MessageBox.Show("Wprowadź poprawne wartości liczbowe");
                }
                else
                {
                    List<string> filepathWithName = sWObject.CreateAssembly();

                    string assemblyName = filepathWithName[1];
                    string filepathAsm = filepathWithName[0];
                    string filepathDir = filepathWithName[2];

                    foreach (ProfileSW profile in CrossSectionsList)
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
   

        public void Add()
        {
            CrossSectionsList.Add(new ProfileSW { Name = "", X = 0, Y = 0, Thickness = 0, Length = 0, DraftCount = 0, Type = "" });
        }

        public void Delete(DataGrid dgprofiles)
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

        public List<ProfileSW> Copy(DataGrid dgprofiles)
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

        public void Paste(DataGrid dgprofiles)
        {
            List<ProfileSW> toPaste = Copy(dgprofiles);
            foreach (ProfileSW profile in toPaste)
            {
                CrossSectionsList.Add(profile);
            }
        }

    }
}