using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GST_BLL
{
    class CsvConvertor
    {
        //public static void WriteToCSV(List<Person> personList)
        //{
        //    string attachment = "attachment; filename=PersonList.csv";
        //    HttpContext.Current.Response.Clear();
        //    HttpContext.Current.Response.ClearHeaders();
        //    HttpContext.Current.Response.ClearContent();
        //    HttpContext.Current.Response.AddHeader("content-disposition", attachment);
        //    HttpContext.Current.Response.ContentType = "text/csv";
        //    HttpContext.Current.Response.AddHeader("Pragma", "public");
        //    WriteColumnName();
        //    foreach (Person person in personList)
        //    {
        //        WriteUserInfo(person);
        //    }
        //    HttpContext.Current.Response.End();
        //}

        //private static void WriteUserInfo(Person person)
        //{
        //    StringBuilder stringBuilder = new StringBuilder();
        //    AddComma(person.Name, stringBuilder);
        //    AddComma(person.Family, stringBuilder);
        //    AddComma(person.Age.ToString(), stringBuilder);
        //    AddComma(string.Format("{0:C2}", person.Salary), stringBuilder);
        //    HttpContext.Current.Response.Write(stringBuilder.ToString());
        //    HttpContext.Current.Response.Write(Environment.NewLine);
        //}

        //private static void AddComma(string value, StringBuilder stringBuilder)
        //{
        //    stringBuilder.Append(value.Replace(',', ' '));
        //    stringBuilder.Append(", ");
        //}

        //private static void WriteColumnName()
        //{
        //    string columnNames = "Name, Family, Age, Salary";
        //    HttpContext.Current.Response.Write(columnNames);
        //    HttpContext.Current.Response.Write(Environment.NewLine);
        //}
    }
}
