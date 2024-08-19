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

namespace SWApp.Models
{

    public class ExcelFile
    {
        SWObject swObject = new SWObject();
        string modelFilepath;
        object imageObj;
        string configName;
        string imgFilepath;
        string assemblyImageFilepath;
        byte[] data;
        int pictureIndex;
        XSSFCreationHelper helper;
        XSSFDrawing drawing;
        XSSFClientAnchor anchor;
        XSSFPicture picture;


        public void CreateWorkBook(DataTable dt, string indexName, string filepath, string assemblyFilepath, string assemblyConfig)
        {

            //creating new workbook
            IWorkbook workbook = new XSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet = workbook.CreateSheet("BOM");
            int k = 0;

            //Create styles for IndexHeader
            IFont fontIndex = workbook.CreateFont();
            fontIndex.IsBold = true;
            fontIndex.IsItalic = true;
            fontIndex.FontHeightInPoints = 14;
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
            fontHeader.IsItalic = true;
            fontIndex.FontHeightInPoints = 12;
            ICellStyle headerStyle = workbook.CreateCellStyle();
            headerStyle.BorderBottom = BorderStyle.Medium;
            headerStyle.BorderLeft = BorderStyle.Medium;
            headerStyle.BorderRight = BorderStyle.Medium;
            headerStyle.BorderTop = BorderStyle.Medium;
            headerStyle.SetFont(fontHeader);

            //create styles for data
            IFont dataFont = workbook.CreateFont();
            fontIndex.FontHeightInPoints = 11;
            ICellStyle dataStyle = workbook.CreateCellStyle();
            dataStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            dataStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            dataStyle.BorderBottom = BorderStyle.Thin;
            dataStyle.BorderLeft = BorderStyle.Thin;
            dataStyle.BorderRight = BorderStyle.Thin;
            dataStyle.BorderTop = BorderStyle.Thin;
            dataStyle.SetFont(dataFont);
            IRow rowMain = sheet.CreateRow(1);


            foreach (DataColumn column in dt.Columns)
            {
                if (column.ColumnName == "status" || column.ColumnName == "createdBy" || column.ColumnName == "checkedBy" || column.ColumnName == "dxfExist" || column.ColumnName == "stepExist")
                {

                }
                else
                {
                    ICell cellMain = rowMain.CreateCell(k);
                    cellMain.SetCellValue(column.ColumnName);
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
                    row.Height = 2000;
                    if (dc.ColumnName == "filepath")
                    {
                        ICell cell = row.CreateCell(j);
                        sheet.SetColumnWidth(j, 7000);

                        configName = dt.Rows[i].Field<string>(10);
                        modelFilepath = dt.Rows[i].Field<string>(j);
                        imgFilepath = swObject.GetBitMap(modelFilepath, configName);
                        try
                        {
                            data = File.ReadAllBytes(imgFilepath);
                            pictureIndex = workbook.AddPicture(data, (PictureType)XSSFWorkbook.PICTURE_TYPE_PNG);
                            helper = workbook.GetCreationHelper() as XSSFCreationHelper;
                            drawing = sheet.CreateDrawingPatriarch() as XSSFDrawing;
                            anchor = helper.CreateClientAnchor() as XSSFClientAnchor;
                            anchor.Dx1 = 200000;
                            anchor.Dy1 = 50000;
                            anchor.Col1 = j;
                            anchor.Row1 = i + 2;
                            picture = drawing.CreatePicture(anchor, pictureIndex) as XSSFPicture;
                            cell.CellStyle = dataStyle;
                            picture.Resize(1, 1);
                            FileSystem.DeleteFile(imgFilepath, UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently);
                        }
                        catch (FileNotFoundException)
                        {

                        }


                    }
                    else if (dc.ColumnName == "status" || dc.ColumnName == "createdBy" || dc.ColumnName == "checkedBy" || dc.ColumnName == "dxfExist" || dc.ColumnName == "stepExist")
                    {


                    }
                    else
                    {

                        if (dc.ColumnName == "comments")
                        {
                            ICell cell = row.CreateCell(j - 5);
                            cell.SetCellValue(dt.Rows[i].Field<string>(j));
                            sheet.AutoSizeColumn(j);
                            cell.CellStyle = dataStyle;
                        }
                        else
                        {
                            ICell cell = row.CreateCell(j);
                            cell.SetCellValue(dt.Rows[i].Field<string>(j));
                            sheet.AutoSizeColumn(j);
                            cell.CellStyle = dataStyle;
                        }

                    }

                }

            }

            //cell and its where need to add assembly bitmap
            ICell cellAssemblyPic = rowMainIndex.CreateCell(2);
            headerIndexStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            headerIndexStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            rowMainIndex.Height = 2500;
            sheet.SetColumnWidth(2, 10000);

            //adding image of assembly
            assemblyImageFilepath = swObject.GetBitMap(assemblyFilepath, assemblyConfig);
            data = File.ReadAllBytes(assemblyImageFilepath);
            pictureIndex = workbook.AddPicture(data, (PictureType)XSSFWorkbook.PICTURE_TYPE_BMP);
            helper = workbook.GetCreationHelper() as XSSFCreationHelper;
            drawing = sheet.CreateDrawingPatriarch() as XSSFDrawing;
            anchor = helper.CreateClientAnchor() as XSSFClientAnchor;
            anchor.Dx1 = 100000;
            anchor.Dy1 = 50000;
            anchor.Col1 = 2;
            anchor.Row1 = 0;
            picture = drawing.CreatePicture(anchor, pictureIndex) as XSSFPicture;
            picture.Resize(1, 1);
            FileSystem.DeleteFile(assemblyImageFilepath, UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently);


            //Selecting column Q, cler all and select to default
            cellAssemblyPic.CellStyle = headerIndexStyle;
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
                MessageBox.Show("Plik tylko do odczytu");
            }

        }

    }
}
