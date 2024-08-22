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
using NPOI.XWPF.UserModel;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.Util;
using SWApp.Helpers;
using Aspose.Pdf;

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
            { "thickness", "grubość [mm]"},
            { "mass", "masa [kg]"},
            { "area", "powierzchnia [dm2]"},
            { "paintQty", "ilość farby [kg]"},
            { "drawingNum", "nr rysunku"},
            { "Qty", "sztuk na kpl"},
            { "configuration", "konfiguracja"},
            { "comments", "uwagi"},
            { "name", "nazwa" },

        };

        public event Action<string, string, ControlAppearance, SymbolIcon> ErrorOccurred;
        public void CreateWorkBook(DataTable dt, string indexName, string filepath, string assemblyFilepath, string assemblyConfig, string projectName,  string size, string assemblyMass)
        {
            SWObject swObject = new SWObject();
            string modelFilepath;
            string configName;
            byte[] imageBytes;
            byte[] barcodeBytes;

            int pictureIndex;
            int barcodeIndex;
            int k = 0;
            int i = 0;
            XSSFCreationHelper helper;
            XSSFDrawing drawing;
            XSSFClientAnchor anchor;
            XSSFPicture picture;

            //creating new workbook
            XSSFWorkbook workbook = new XSSFWorkbook();
            XSSFSheet sheet = workbook.CreateSheet("BOM") as XSSFSheet;
            

            //Create styles for IndexHeader descriptions
            XSSFFont fontIndex = workbook.CreateFont() as XSSFFont;
            fontIndex.IsBold = true;
            fontIndex.IsItalic = false;
            fontIndex.FontHeightInPoints = 12;
            XSSFCellStyle headerIndexStyle = workbook.CreateCellStyle() as XSSFCellStyle;
            headerIndexStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            headerIndexStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            headerIndexStyle.BorderBottom = BorderStyle.Medium;
            headerIndexStyle.BorderLeft = BorderStyle.Medium;
            headerIndexStyle.BorderRight = BorderStyle.Thin;
            headerIndexStyle.BorderTop = BorderStyle.Medium;
            headerIndexStyle.SetFont(fontIndex);

            //Create styles for IndexHeader Values
            XSSFFont fontValuesIndex = workbook.CreateFont() as XSSFFont;
            fontValuesIndex.IsBold = false;
            fontValuesIndex.IsItalic = false;
            fontValuesIndex.FontHeightInPoints = 10;
            XSSFCellStyle headerIndexValuesStyle = workbook.CreateCellStyle() as XSSFCellStyle;
            headerIndexValuesStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            headerIndexValuesStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            headerIndexValuesStyle.BorderBottom = BorderStyle.Medium;
            headerIndexValuesStyle.BorderLeft = BorderStyle.Thin;
            headerIndexValuesStyle.BorderRight = BorderStyle.Medium;
            headerIndexValuesStyle.BorderTop = BorderStyle.Medium;
            headerIndexValuesStyle.SetFont(fontValuesIndex);

            //creating Index Header
            XSSFRow rowMainIndex0 = sheet.CreateRow(0) as XSSFRow;
            XSSFRow rowMainIndex1 = sheet.CreateRow(1) as XSSFRow;
            XSSFRow rowMainIndex2 = sheet.CreateRow(2) as XSSFRow;

            //merged for assembly pic
            XSSFCell cellMainIndex = rowMainIndex0.CreateCell(0) as XSSFCell;
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 2, 0, 3));

            //index header data
            #region row 0
            cellMainIndex = rowMainIndex0.CreateCell(4) as XSSFCell;
            cellMainIndex.SetCellValue("nazwa wyrobu");
            cellMainIndex.CellStyle = headerIndexStyle;

            cellMainIndex = rowMainIndex0.CreateCell(5) as XSSFCell;
            cellMainIndex.SetCellValue(projectName);
            cellMainIndex.CellStyle = headerIndexValuesStyle;

            cellMainIndex = rowMainIndex0.CreateCell(6) as XSSFCell;
            cellMainIndex.SetCellValue("nr indeksu");
            cellMainIndex.CellStyle = headerIndexStyle;

            cellMainIndex = rowMainIndex0.CreateCell(7) as XSSFCell;
            cellMainIndex.SetCellValue(indexName);
            cellMainIndex.CellStyle = headerIndexValuesStyle;
            #endregion
            #region row 1
            cellMainIndex = rowMainIndex1.CreateCell(4) as XSSFCell;
            cellMainIndex.SetCellValue("wymiary gabarytowe");
            cellMainIndex.CellStyle = headerIndexStyle;

            cellMainIndex = rowMainIndex1.CreateCell(5) as XSSFCell;
            cellMainIndex.SetCellValue(assemblyMass);
            cellMainIndex.CellStyle = headerIndexValuesStyle;

            cellMainIndex = rowMainIndex1.CreateCell(6) as XSSFCell;
            cellMainIndex.SetCellValue("nr zlecenia");
            cellMainIndex.CellStyle = headerIndexStyle;

            cellMainIndex = rowMainIndex1.CreateCell(7) as XSSFCell;
            cellMainIndex.SetCellValue("");
            cellMainIndex.CellStyle = headerIndexValuesStyle;
            #endregion
            #region row 2
            cellMainIndex = rowMainIndex2.CreateCell(4) as XSSFCell;
            cellMainIndex.SetCellValue("waga [kg]");
            cellMainIndex.CellStyle = headerIndexStyle;

            cellMainIndex = rowMainIndex2.CreateCell(5) as XSSFCell;
            cellMainIndex.SetCellValue(size);
            cellMainIndex.CellStyle = headerIndexValuesStyle;

            cellMainIndex = rowMainIndex2.CreateCell(6) as XSSFCell;
            cellMainIndex.SetCellValue("ilość sztuk wyrobu");
            cellMainIndex.CellStyle = headerIndexStyle;

            cellMainIndex = rowMainIndex2.CreateCell(7) as XSSFCell;
            cellMainIndex.SetCellValue("");
            cellMainIndex.CellStyle = headerIndexValuesStyle;
            #endregion

            //Create styles for table headers
            XSSFFont fontHeader = workbook.CreateFont() as XSSFFont;
            fontHeader.IsBold = true;
            fontHeader.IsItalic = false;
            fontHeader.FontHeightInPoints = 11;
            XSSFCellStyle headerStyle = workbook.CreateCellStyle() as XSSFCellStyle;
            headerStyle.BorderBottom = BorderStyle.Medium;
            headerStyle.BorderLeft = BorderStyle.Medium;
            headerStyle.BorderRight = BorderStyle.Medium;
            headerStyle.BorderTop = BorderStyle.Medium;
            headerStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            headerStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            headerStyle.SetFont(fontHeader);

            //create styles for data
            XSSFFont dataFont = workbook.CreateFont() as XSSFFont;
            dataFont.FontHeightInPoints = 10;
            XSSFCellStyle dataStyle = workbook.CreateCellStyle() as XSSFCellStyle;
            dataStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            dataStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            dataStyle.BorderBottom = BorderStyle.Medium;
            dataStyle.BorderLeft = BorderStyle.Thin;
            dataStyle.BorderRight = BorderStyle.Thin;
            dataStyle.BorderTop = BorderStyle.Thin;
            dataStyle.SetFont(dataFont);
            dataStyle.WrapText = true;
            XSSFRow rowMain = sheet.CreateRow(3) as XSSFRow;


            ////styling headers
            foreach (DataColumn column in dt.Columns)
            {
                if (column.ColumnName == "status" || column.ColumnName == "createdBy" || column.ColumnName == "checkedBy" || column.ColumnName == "dxfExist" ||
                    column.ColumnName == "assemblyFilePath" || column.ColumnName == "stepExist" || column.ColumnName == "assemblyConfig")
                {

                }
                else
                {
                    XSSFCell cellMain = rowMain.CreateCell(k) as XSSFCell;
                    cellMain.SetCellValue(ColumnsNames[column.ColumnName]);
                    cellMain.CellStyle = headerStyle;
                    k++;
                }
            }

            ////creating new rows with data
            for (i = 0; i < dt.Rows.Count; i++)
            {
                XSSFRow row = sheet.CreateRow(i + 4) as XSSFRow;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    DataColumn dc = dt.Columns[j];
                    row.Height = 2500;
                    if (dc.ColumnName == "filepath")
                    {
                        XSSFCell cell = row.CreateCell(j) as XSSFCell;
                        sheet.SetColumnWidth(j, 10000);

                        configName = dt.Rows[i].Field<string>("configuration");
                        modelFilepath = dt.Rows[i].Field<string>(j);
                        imageBytes = swObject.GetBitMap(modelFilepath, configName);
                        try
                        {
                            pictureIndex = workbook.AddPicture(imageBytes, (NPOI.SS.UserModel.PictureType)XSSFWorkbook.PICTURE_TYPE_BMP);
                            helper = workbook.GetCreationHelper() as XSSFCreationHelper;
                            drawing = sheet.CreateDrawingPatriarch() as XSSFDrawing;
                            anchor = helper.CreateClientAnchor() as XSSFClientAnchor;
                            anchor.Dx1 = 25000;
                            anchor.Dy1 = 25000;
                            anchor.Col1 = j;
                            anchor.Row1 = i + 4;
                            picture = drawing.CreatePicture(anchor, pictureIndex) as XSSFPicture;
                            cell.CellStyle = dataStyle;
                            picture.Resize(1, 1);



                        }
                        catch (FileNotFoundException)
                        {

                        }


                    }
                    else if (dc.ColumnName == "type")
                    {
                        XSSFCell cell = row.CreateCell(j) as XSSFCell;
                        var type = dt.Rows[i].Field<string>(j);
                        switch (type)
                        {
                            case "part":
                                cell.SetCellValue("część");
                                cell.CellStyle = dataStyle;
                                break;
                            case "assembly":
                                cell.SetCellValue("złożenie");
                                cell.CellStyle = dataStyle;
                                break;
                            case "sheet":
                                cell.SetCellValue("wcięte");
                                cell.CellStyle = dataStyle;
                                break;
                        }
                    }

                    else if (dc.ColumnName != "status" && dc.ColumnName != "createdBy" && dc.ColumnName != "checkedBy" && dc.ColumnName != "dxfExist" &&
                             dc.ColumnName != "assemblyFilePath" && dc.ColumnName != "stepExist" && dc.ColumnName != "assemblyConfig")
                    {
                        XSSFCell cell = row.CreateCell(j) as XSSFCell;
                        cell.SetCellValue(dt.Rows[i].Field<string>(j));
                        
                        sheet.AutoSizeColumn(j);
                        cell.CellStyle = dataStyle;
                    }

                }
                row.Height = 2500;
            }

            //cell and its where need to add assembly bitmap
            XSSFCell cellAssemblyPic = rowMainIndex0.CreateCell(0) as XSSFCell;
            headerIndexStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            headerIndexStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            rowMainIndex0.Height = 1666;
            rowMainIndex1.Height = 1666;
            rowMainIndex2.Height = 1666;
            sheet.SetColumnWidth(0, 10000);

            //adding image of assembly
            imageBytes = swObject.GetBitMap(assemblyFilepath, assemblyConfig);
            pictureIndex = workbook.AddPicture(imageBytes, (NPOI.SS.UserModel.PictureType)XSSFWorkbook.PICTURE_TYPE_BMP);
            helper = workbook.GetCreationHelper() as XSSFCreationHelper;
            drawing = sheet.CreateDrawingPatriarch() as XSSFDrawing;
            anchor = helper.CreateClientAnchor() as XSSFClientAnchor;
            anchor.Dx1 = 50000;
            anchor.Dy1 = 25000;
            anchor.Col1 = 0;
            anchor.Row1 = 0;
            picture = drawing.CreatePicture(anchor, pictureIndex) as XSSFPicture;
            picture.Resize(2, 3);

            //adding index barcode
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 8, 9));
            barcodeBytes = TypeToBarcodeConverter.GenerateBarcode(indexName);
            barcodeIndex = workbook.AddPicture(barcodeBytes, (NPOI.SS.UserModel.PictureType)XSSFWorkbook.PICTURE_TYPE_BMP);
            helper = workbook.GetCreationHelper() as XSSFCreationHelper;
            drawing = sheet.CreateDrawingPatriarch() as XSSFDrawing;
            anchor = helper.CreateClientAnchor() as XSSFClientAnchor;
            anchor.Dx1 = 100000;
            anchor.Dy1 = 50000;
            anchor.Col1 = 8;
            anchor.Row1 = 1;
            picture = drawing.CreatePicture(anchor, barcodeIndex) as XSSFPicture;
            var barcodeCell = rowMainIndex1.CreateCell(8);
            //barcodeCell.CellStyle = dataStyle;
            picture.Resize(2, 1);

            //resize type columns
            sheet.AutoSizeColumn(3);
            sheet.SetColumnWidth(2, 10000);
            sheet.SetColumnWidth(13, 10000);

            ////format as table
            XSSFTable table = sheet.CreateTable();
            CT_Table ctTable = table.GetCTTable();
            AreaReference myDataRange = new AreaReference(new CellReference(3, 0), new CellReference(i + 4, 13));
            ctTable.@ref = myDataRange.FormatAsString();
            ctTable.id = 1;
            ctTable.name = "Table1";
            ctTable.displayName = "Table1";
            ctTable.tableStyleInfo = new CT_TableStyleInfo();
            ctTable.tableStyleInfo.name = "TableStyleMedium2"; // TableStyleMedium2 is one of XSSFBuiltinTableStyle
            ctTable.tableStyleInfo.showRowStripes = true;
            ctTable.autoFilter = new CT_AutoFilter();
            ctTable.autoFilter.filterColumn = new List<CT_FilterColumn>();

            for (int l = 1; l < 15; l++)
            {
                ctTable.autoFilter.filterColumn.Add(new CT_FilterColumn()
                { colId = Convert.ToUInt32(l), showButton = true });
            }


            ctTable.tableColumns = new CT_TableColumns();
            ctTable.tableColumns.tableColumn = new List<CT_TableColumn>();
            ctTable.tableColumns.tableColumn.Add(new CT_TableColumn() { id = 1, name = "widok" });
            ctTable.tableColumns.tableColumn.Add(new CT_TableColumn() { id = 2, name = "nazwa" });
            ctTable.tableColumns.tableColumn.Add(new CT_TableColumn() { id = 3, name = "opis" });
            ctTable.tableColumns.tableColumn.Add(new CT_TableColumn() { id = 4, name = "typ" });
            ctTable.tableColumns.tableColumn.Add(new CT_TableColumn() { id = 5, name = "materiał" });
            ctTable.tableColumns.tableColumn.Add(new CT_TableColumn() { id = 6, name = "indeks" });
            ctTable.tableColumns.tableColumn.Add(new CT_TableColumn() { id = 7, name = "grubość [mm]" });
            ctTable.tableColumns.tableColumn.Add(new CT_TableColumn() { id = 8, name = "masa [kg]" });
            ctTable.tableColumns.tableColumn.Add(new CT_TableColumn() { id = 9, name = "powierzchnia [dm2]" });
            ctTable.tableColumns.tableColumn.Add(new CT_TableColumn() { id = 10, name = "ilość farby [kg]" });
            ctTable.tableColumns.tableColumn.Add(new CT_TableColumn() { id = 11, name = "nr rysunku" });
            ctTable.tableColumns.tableColumn.Add(new CT_TableColumn() { id = 12, name = "sztuk na kpl" });
            ctTable.tableColumns.tableColumn.Add(new CT_TableColumn() { id = 13, name = "konfiguracja" });
            ctTable.tableColumns.tableColumn.Add(new CT_TableColumn() { id = 14, name = "uwagi" });
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
