using stdole;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWApp.Helpers
{
    public static class PictureDispConverter
    {
        public static MemoryStream ConvertToStream(IPictureDisp pictureDisp)
        {
            var bitmap = System.Drawing.Image.FromHbitmap(new IntPtr(pictureDisp.Handle));
            var ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }
    }
}
