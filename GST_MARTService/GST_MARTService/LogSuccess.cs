using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;
using System.Configuration;
/// <summary>
/// Summary description for Logger
/// </summary>
public static class LogSuccess
{
    public static void Write(string msg)
    {
   
        System.IO.StreamWriter sw = System.IO.File.AppendText("D:\\Code\\GST_Mart\\dev\\SSIS\\GST-mart\\Logs\\Success.txt");
     
        try
        {
            string logLine = System.String.Format(
                "{0:G}: {1}", System.DateTime.Now, msg);
            sw.WriteLine(logLine);
        }
        finally
        {
            sw.Close();
        }
    }
}

        
