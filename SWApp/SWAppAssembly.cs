using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SWApp
{
    public class SWAppAssembly
    {
        public static Assembly Assembly => Assembly.GetExecutingAssembly();
    }
}
