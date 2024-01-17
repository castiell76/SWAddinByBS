using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SWApp
{
    public class ProfileSW : INotifyPropertyChanged
    {
        private string type;
        private string name;
        private int x;
        private int y;
        private double length;
        private double thickness;
        private int draftCount;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                UpdateType();
                OnPropertyChanged();
            }
        }
        public int X
        {
            get { return x; }
            set
            {
                x = value;
                UpdateType();
                OnPropertyChanged();
            }
        }
        public int Y
        {

            get { return y; }
            set
            {
                y = value;
                UpdateType();
                OnPropertyChanged();
            }
        }
        public double Length
        {
            get { return length; }
            set
            {
                length = value;
                UpdateType();
                OnPropertyChanged();
            }
        }
        public int DraftCount
        {
            get { return draftCount; }
            set
            {
                draftCount = value;
                if (value > 2)
                {
                    draftCount = 2;
                }
                UpdateType();
                OnPropertyChanged();
            }
        }
        public string Type
        {
            get { return type;  }
            set { type = value; OnPropertyChanged(); }


        }
        public double Thickness
        {
            get { return thickness; }
            set
            {
                thickness = value;
                UpdateType();
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private void UpdateType()
        {
            if (thickness == 0 && y == 0 && x!= 0 && length !=0)
            {
                Type = "pręt okrągły";
            }
            else if (thickness == 0 && y != 0 && x != 0 && length != 0)
            {
                Type = "pręt prostokątny";
            }
            else if (thickness != 0 && y == 0 && length != 0)
            {
                Type = "rura okrągła";
            }
            else if (y !=0 && thickness != 0 && x!=0 && length != 0)
            {
                Type = "rura prostokątna";
            }
            else
            {
                Type = "wprowadź dane profila";
            }
            
        }
    }
}
