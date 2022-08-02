using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Common.Utils
{
    public class ExcelHelper
    {
        #region 读取Excel文件
        /// <summary>
        /// 根据多个sheet获取DataSet
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(IFormFile file)
        {
            var fs = file.OpenReadStream();
            IWorkbook workbook;
            DataSet ds = new DataSet();
            if (file.FileName.IndexOf(".xlsx") > -1)
            {
                workbook = new XSSFWorkbook(fs);
            }
            else
            {
                workbook = new HSSFWorkbook(fs);
            }
            //根据
            if (workbook != null)
            {
                workbook.GetEnumerator().ToList().ForEach(t =>
                {
                    ds.Tables.Add(GetTableFromSheet(t));
                });
            }
            return ds;
        }

        /// <summary>
        /// 根据
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(IFormFile file)
        {
            var fs = file.OpenReadStream();
            IWorkbook workbook;
            ISheet sheet;
            DataTable dt = new DataTable();
            if (file.FileName.IndexOf(".xlsx") > -1)
            {
                workbook = new XSSFWorkbook(fs);
            }
            else
            {
                workbook = new HSSFWorkbook(fs);
            }
            //根据
            if (workbook != null)
            {
                sheet = workbook.GetSheetAt(0);
                dt = GetTableFromSheet(sheet);
            }
            return dt;
        }

        /// <summary>
        /// 根据一个sheet获取DataTable
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public static DataTable GetTableFromSheet(ISheet sheet)
        {
            DataTable dt = new DataTable("domain");
            List<string> columnNames = new List<string>();
            if (sheet != null)
            {
                IRow row;
                ICell cell;
                int rowNum = sheet.PhysicalNumberOfRows;
                if (rowNum > 0)
                {
                    //表头
                    row = sheet.GetRow(0);
                    for (int i = row.FirstCellNum; i < row.LastCellNum; i++)
                    {
                        cell = row.GetCell(i);
                        if (cell != null && cell.StringCellValue != null)
                        {
                            columnNames.Add(cell.StringCellValue);
                            dt.Columns.Add(new DataColumn(cell.StringCellValue));
                        }
                    }

                    //表头没有问题，则填充table，从第二行开始
                    for (int i = 1; i < rowNum; i++)
                    {
                        DataRow dr = dt.NewRow();
                        row = sheet.GetRow(i);
                        for (int j = row.FirstCellNum; j < dt.Columns.Count; j++)
                        {
                            cell = row.GetCell(j);
                            if (cell == null)
                            {
                                dr[j] = "";
                            }
                            else
                            {
                                switch (cell.CellType)
                                {
                                    case CellType.Blank:
                                        dr[j] = "";
                                        break;
                                    case CellType.Numeric:
                                        short format = cell.CellStyle.DataFormat;
                                        if (format == 14 || format == 31 || format == 57 || format == 58)
                                            dr[j] = cell.DateCellValue;
                                        else
                                            dr[j] = cell.NumericCellValue;
                                        break;
                                    case CellType.String:
                                        dr[j] = cell.StringCellValue;
                                        break;
                                    default:
                                        try
                                        {
                                            dr[j] = cell.StringCellValue;
                                        }
                                        catch (Exception ex)
                                        {
                                            dr[j] = "";
                                        }
                                        break;
                                }
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt;
        }
        #endregion

        #region 生成Excel文件
        /// <summary>
        /// 数据导出
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sheetName"></param>
        public static bool ExportToExcel(DataTable data, string filePath, string sheetName = "Sheet1")
        {
            try
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet(sheetName);
                IRow rowHead = sheet.CreateRow(0);

                //填写表头
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    rowHead.CreateCell(i, CellType.String).SetCellValue(data.Columns[i].ColumnName.ToString());
                }
                //填写内容
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    IRow row = sheet.CreateRow(i + 1);
                    for (int j = 0; j < data.Columns.Count; j++)
                    {
                        row.CreateCell(j, CellType.String).SetCellValue(data.Rows[i][j].ToString());
                    }
                }

                for (int i = 0; i < data.Columns.Count; i++)
                {
                    sheet.AutoSizeColumn(i);
                }
                //创建文件路径
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }
                using (FileStream stream = File.OpenWrite(filePath))
                {
                    workbook.Write(stream);
                    stream.Close();
                }
                GC.Collect();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}
