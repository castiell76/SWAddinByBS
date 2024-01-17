using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WF_Host
{
    [ComVisible(true)]
    [Guid("0CCD5F4F-FCF8-4C9B-8990-AB950F2F03E5")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IServer
    {
        /// <summary>
        /// Compute the value of the constant Pi.
        /// </summary>
        /// 

    }
}
