using Microsoft.Office.Interop.Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace GST_BLL.Factory
{
   public class ExcelConvertor
    {
       public bool ConvertTOXLSX(string filepath)
       {

           try
           {

               
               //Application app = new Application();
               //Workbook wb = app.Workbooks.Open(filepath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
               //wb.SaveAs(@"C:\testcsv.xlsx", XlFileFormat.xlOpenXMLWorkbook, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
               //wb.Close();
               //app.Quit();
           }
           catch (Exception ex)
           {
               throw ex;
          
           }

           return true;
       }
    }
}
