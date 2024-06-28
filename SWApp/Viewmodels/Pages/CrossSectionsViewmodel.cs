using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using SWApp.Models;

namespace SWApp.Viewmodels.Pages
{
   public partial class CrossSectionsViewmodel : ObservableObject,INotifyPropertyChanged
    {
        private ProfileSW profileSW;
        [ObservableProperty]
        private ObservableCollection<ProfileSW> _crossSections;
        public ObservableCollection<ProfileSW> CrossSectionsList
        {
            get { return _crossSections; }
            set { _crossSections = value; }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public string Name
        {
            get { return profileSW.Name; }
            set
            {
                profileSW.Name = value;
            }
        }
        public int X
        {
            get { return profileSW.X; }
            set
            {
                profileSW.X = value;
            }
        }
        public int Y
        {
            get { return profileSW.Y; }
            set
            {
                profileSW.Y = value;
            }
        }
        public double Thickness
        {
            get { return profileSW.Thickness; }
            set
            {
                profileSW.Thickness = value;
            }
        }
        public double Length
        {
            get { return profileSW.Length; }
            set
            {
                profileSW.Length = value;
            }
        }
        public string Type
        {
            get { return profileSW.Type; }
            set { Type = value; }
        }
        public int Draftcount
        {
            get { return profileSW.DraftCount; }
            set
            {
                profileSW.DraftCount = value;

            }
        }
        public CrossSectionsViewmodel()
        {
            CrossSectionsList = new ObservableCollection<ProfileSW>();

        }
        public void Add()
        {
            CrossSectionsList.Add(new ProfileSW() { Name = "", X = 0, Y = 0, Thickness = 0, DraftCount = 0, Type = "", });
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
                toDelete = new ProfileSW() { Name = "", X = 0, Y = 0, Thickness = 0, DraftCount = 0, Type = "" };
                CrossSectionsList.Remove((ProfileSW)toDelete);
            }
        }
        public List<ProfileSW> Copy(DataGrid dgprofiles)
        {
            List<ProfileSW> toCopy = new List<ProfileSW>();

            for (int i = 0; i < dgprofiles.SelectedItems.Count; i++)
            {
                ProfileSW toDuplicate = (ProfileSW)dgprofiles.SelectedItems[i];
                ProfileSW toInsert = new ProfileSW()
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
