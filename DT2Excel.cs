using System;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Application = Microsoft.Office.Interop.Excel.Application;
using DataTable = System.Data.DataTable;

/// <summary>
///     DataTable 导出为Excel文件
/// </summary>
public static class DT2Excel
{
    ///把数据表的内容导出到Excel文件中
    ///Copy from http://blog.csdn.net/conan8126/article/details/20547057
    public static void OutDataToExcel2(DataTable srcDataTable, string excelFilePath)
    {
        var xlApp = new Application();
        object missing = Missing.Value;

        //导出到execl   
        try
        {
            if (xlApp == null)
                return;

            var xlBooks = xlApp.Workbooks;
            var xlBook = xlBooks.Add(XlWBATemplate.xlWBATWorksheet);
            var xlSheet = (Worksheet) xlBook.Worksheets[1];

            //让后台执行设置为不可见，为true的话会看到打开一个Excel，然后数据在往里写  
            xlApp.Visible = false;
            //生成Excel中列头名称  
            for (var i = 0; i < srcDataTable.Columns.Count; i++)
                xlSheet.Cells[1, i + 1] = srcDataTable.Columns[i].ColumnName; //输出DataGridView列头名  

            //把DataGridView当前页的数据保存在Excel中   
            if (srcDataTable.Rows.Count > 0)
                for (var i = 0; i < srcDataTable.Rows.Count; i++) //控制Excel中行  
                for (var j = 0; j < srcDataTable.Columns.Count; j++) //控制Excel中列  
                    xlSheet.Cells[i + 2, j + 1] =
                        srcDataTable.Rows[i][j]; //i控制行，从Excel中第2行开始输出第一行数据，j控制列，从Excel中第1列输出第1列数据  

            //设置禁止弹出保存和覆盖的询问提示框  
            xlApp.DisplayAlerts = false;
            xlApp.AlertBeforeOverwriting = false;

            if (xlSheet != null)
            {
                xlSheet.SaveAs(excelFilePath, missing, missing, missing, missing, missing, missing, missing, missing,
                    missing);
                xlApp.Quit();
            }
        }
        catch (Exception ex)
        {
            xlApp.Quit();
            //throw ex;
            MessageBox.Show(ex.Message); // 保存报错提示
        }
    }
}