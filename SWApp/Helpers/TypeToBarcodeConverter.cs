using BarcodeStandard;
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


namespace SWApp.Helpers
{
    public class TypeToBarcodeConverter 
    {
        public static byte[] GenerateBarcode(string text)
        {
            Barcode barcode = new Barcode();
            var barcodeImage = barcode.Encode(BarcodeStandard.Type.Code128, text, SKColors.Black, SKColors.White, 300, 100);
            using (var ms = new MemoryStream())
            {
                barcodeImage.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }
    }

}
    

