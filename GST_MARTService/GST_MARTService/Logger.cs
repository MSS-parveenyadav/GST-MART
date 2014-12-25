using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;
using System.Configuration;
/// <summary>
/// Summary description for Logger
/// </summary>
public static class Logger
{
    public static void Write(string msg)
    {
        Write(msg, "D:\\Code\\GST_Mart\\dev\\SSIS\\GST-mart\\Logs\\Error.txt");       
    }
    public static void Write(string msg, string url)
    {
        //System.Threading.Thread.Sleep(500);
    //    System.IO.StreamWriter sw = System.IO.File.AppendText(HttpContext.Current.Server.MapPath("~"+url));
        System.IO.StreamWriter sw = System.IO.File.AppendText("D:\\Code\\GST_Mart\\dev\\SSIS\\GST-mart\\Logs\\Error.txt");
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

        
