using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SWApp.Models
{
    public static class NaturalSorting
    {
        public static class SafeNativeMethods
        {
            [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
            public static extern int StrCmpLogicalW(string psz1, string psz2);
        }

        public sealed class NaturalStringComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                // Upewniamy się, że porównujemy tylko stringi
                string s1 = x as string;
                string s2 = y as string;

                if (s1 == null || s2 == null)
                {
                    throw new ArgumentException("Objects must be strings to compare.");
                }

                return SafeNativeMethods.StrCmpLogicalW(s1, s2);
            }
        }
    }
}
