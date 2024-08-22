
using NPOI.SS.UserModel;
using SkiaSharp;
using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ZXing;
using ZXing.Common;
using ZXing.Windows.Compatibility;


namespace SWApp.Helpers
{
    public class TypeToBarcodeConverter 
    {
        public static byte[] GenerateBarcode(string text)
        {
            BarcodeWriter writer = new BarcodeWriter()
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions
                {
                    Height = 400,
                    Width = 800,
                    PureBarcode = false,
                    Margin = 10,
                },
            };

            var bitmap = writer.Write(text);

            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                return stream.ToArray();
            }
        }
    }

}
    

