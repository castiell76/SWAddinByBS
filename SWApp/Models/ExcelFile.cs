using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using System.Data;
using System.Windows;
using SolidWorks.Interop.sldworks;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.FileIO;
using Wpf.Ui.Controls;

namespace SWApp.Models
{

    public class ExcelFile
    {
        private Dictionary<string, string> ColumnsNames = new Dictionary<string, string>
        {
            { "type", "typ"},
            { "filepath", "widok"},
            { "description", "opis"},
            { "material", "materiał"},
            { "index", "indeks"},
            { "thickness", "grubość"},
            { "mass", "masa [kg]"},
            { "area", "powierzchnia [dm2]"},
            { "paintQty", "ilość farby [kg]"},
            { "drawingNum", "nr rysunku"},
            { "Qty", "sztuk na kpl"},
            { "configuration", "konfiguracja"},
            { "comments", "uwagi"},

        };

        public event Action<string, string, ControlAppearance, SymbolIcon> ErrorOccurred;
        public void CreateWorkBook(DataTable dt, string indexName, string filepath, string assemblyFilepath, string assemblyConfig)
        {
            SWObject swObject = new SWObject();
            string modelFilepath;
            string configName;
            byte[] imageBytes;

            int pictureIndex;
            XSSFCreationHelper helper;
            XSSFDrawing drawing;
            XSSFClientAnchor anchor;
            XSSFPicture picture;

            //creating new workbook
            IWorkbook workbook = new XSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet = workbook.CreateSheet("BOM");
            int k = 0;

            //Create styles for IndexHeader
            IFont fontIndex = workbook.CreateFont();
            fontIndex.IsBold = true;
            fontIndex.IsItalic = false;
            fontIndex.FontHeightInPoints = 9;
            ICellStyle headerIndexStyle = workbook.CreateCellStyle();
            headerIndexStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            headerIndexStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            headerIndexStyle.BorderBottom = BorderStyle.Thick;
            headerIndexStyle.BorderLeft = BorderStyle.Thick;
            headerIndexStyle.BorderRight = BorderStyle.Thick;
            headerIndexStyle.BorderTop = BorderStyle.Thick;
            headerIndexStyle.SetFont(fontIndex);

            //creating Index Header
            IRow rowMainIndex = sheet.CreateRow(0);
            ICell cellMainIndex = rowMainIndex.CreateCell(0);
            cellMainIndex.SetCellValue("Nr indeksu");
            cellMainIndex.CellStyle = headerIndexStyle;
            cellMainIndex = rowMainIndex.CreateCell(1);
            cellMainIndex.SetCellValue(indexName);
            cellMainIndex.CellStyle = headerIndexStyle;

            //Create styles for headers
            IFont fontHeader = workbook.CreateFont();
            fontHeader.IsBold = true;
            fontHeader.IsItalic = false;
            fontIndex.FontHeightInPoints = 9;
            ICellStyle headerStyle = workbook.CreateCellStyle();
            headerStyle.BorderBottom = BorderStyle.Thick;
            headerStyle.BorderLeft = BorderStyle.Thick;
            headerStyle.BorderRight = BorderStyle.Thick;
            headerStyle.BorderTop = BorderStyle.Thick;
            headerStyle.SetFont(fontHeader);

            //create styles for data
            IFont dataFont = workbook.CreateFont();
            fontIndex.FontHeightInPoints = 9;
            ICellStyle dataStyle = workbook.CreateCellStyle();
            dataStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            dataStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            dataStyle.BorderBottom = BorderStyle.Medium;
            dataStyle.BorderLeft = BorderStyle.Thin;
            dataStyle.BorderRight = BorderStyle.Thin;
            dataStyle.BorderTop = BorderStyle.Thin;
            dataStyle.SetFont(dataFont);
            IRow rowMain = sheet.CreateRow(1);


            foreach (DataColumn column in dt.Columns)
            {
                if (column.ColumnName == "status" || column.ColumnName == "createdBy" || column.ColumnName == "checkedBy" || column.ColumnName == "dxfExist"||
                    column.ColumnName == "assemblyFilePath" || column.ColumnName == "stepExist"  || column.ColumnName == "assemblyConfig")
                {

                }
                else
                {
                    ICell cellMain = rowMain.CreateCell(k);
                    cellMain.SetCellValue(ColumnsNames[column.ColumnName]);
                    cellMain.CellStyle = headerStyle;
                    k++;
                }
            }

            //creating new rows with data
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 2);
                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    DataColumn dc = dt.Columns[j];
                    row.Height = 2500;
                    if (dc.ColumnName == "filepath")
                    {
                        ICell cell = row.CreateCell(j);
                        sheet.SetColumnWidth(j, 10000);

                        configName = dt.Rows[i].Field<string>("configuration");
                        modelFilepath = dt.Rows[i].Field<string>(j);
                        imageBytes = swObject.GetBitMap(modelFilepath, configName);
                        try
                        {
                            //pictureIndex = workbook.AddPicture(imageBytes, (PictureType)XSSFWorkbook.PICTURE_TYPE_BMP);
                            //helper = workbook.GetCreationHelper() as XSSFCreationHelper;
                            //drawing = sheet.CreateDrawingPatriarch() as XSSFDrawing;
                            //anchor = helper.CreateClientAnchor() as XSSFClientAnchor;
                            //anchor.Dx1 = 200000;
                            //anchor.Dy1 = 50000;
                            //anchor.Col1 = j;
                            //anchor.Row1 = i + 2;
                            //picture = drawing.CreatePicture(anchor, pictureIndex) as XSSFPicture;
                            //cell.CellStyle = dataStyle;
                            //picture.Resize(1, 1);
                        }
                        catch (FileNotFoundException)
                        {

                        }


                    }
                    else if(dc.ColumnName == "type")
                    {
                        ICell cell = row.CreateCell(j);
                        var type = dt.Rows[i].Field<string>(j);
                        switch (type)
                        {
                            case "part":
                                cell.SetCellValue("część");
                                break;
                            case "assembly":
                                cell.SetCellValue("złożenie");
                                break;
                            case "sheet":
                                cell.SetCellValue("wcięte");
                                break;
                        }
                    }

                    else if (dc.ColumnName != "status" && dc.ColumnName != "createdBy" && dc.ColumnName != "checkedBy" && dc.ColumnName != "dxfExist" &&
                             dc.ColumnName != "assemblyFilePath" && dc.ColumnName != "stepExist" && dc.ColumnName != "assemblyConfig")
                    {
                        ICell cell = row.CreateCell(j);
                        cell.SetCellValue(dt.Rows[i].Field<string>(j));
                        sheet.AutoSizeColumn(j);
                        cell.CellStyle = dataStyle;
                    }

                }

            }

            //cell and its where need to add assembly bitmap
            ICell cellAssemblyPic = rowMainIndex.CreateCell(2);
            headerIndexStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            headerIndexStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            rowMainIndex.Height = 3500;
            sheet.SetColumnWidth(2, 14000);

            //adding image of assembly
            imageBytes = swObject.GetBitMap(assemblyFilepath, assemblyConfig);
            //data = File.ReadAllBytes(assemblyImageFilepath);
            //pictureIndex = workbook.AddPicture(imageBytes, (PictureType)XSSFWorkbook.PICTURE_TYPE_BMP);
            //helper = workbook.GetCreationHelper() as XSSFCreationHelper;
            //drawing = sheet.CreateDrawingPatriarch() as XSSFDrawing;
            //anchor = helper.CreateClientAnchor() as XSSFClientAnchor;
            //anchor.Dx1 = 100000;
            //anchor.Dy1 = 50000;
            //anchor.Col1 = 2;
            //anchor.Row1 = 0;
            //picture = drawing.CreatePicture(anchor, pictureIndex) as XSSFPicture;
            //picture.Resize(1, 1);


            //Selecting column Q, cler all and select to default
            //cellAssemblyPic.CellStyle = headerIndexStyle;

            //save file
            try
            {

                using (FileStream stream = new FileStream(filepath, FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(stream);
                }
            }
            catch (IOException)
            {
                ErrorOccurred?.Invoke("Uwaga!", "Wybrano plik tylko do odczytu. Plik nie został zapisany", ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Important24));
            }

        }

    }
}
