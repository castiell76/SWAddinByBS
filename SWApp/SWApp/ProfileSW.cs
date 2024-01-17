using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWApp
{
    public class ProfileSW
    {
        public string Name;
        public int X;
        public int Y;
        public double Thickness;
        public double Length;
        public int DraftCount;

        public ProfileSW(string name, int x, int y, double thickness, double length, int draftCount)
        {
            Name = name;
            X = x;
            Y = y;
            Thickness = thickness;
            Length = length;
            DraftCount = draftCount;
        }
    }
}
