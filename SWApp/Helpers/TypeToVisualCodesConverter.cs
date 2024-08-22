
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
    public class TypeToVisualCodesConverter 
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
                    Margin = 5,
                },
            };

            var bitmap = writer.Write(text);

            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                return stream.ToArray();
            }
        }

        public static byte[] GenerateQR(string text)
        {
            BarcodeWriter writer = new BarcodeWriter()
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = 400,
                    Width = 400,
                    Margin = 2,
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
    

