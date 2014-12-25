using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GST_BLL.DTO
{
   public class ConfigurationModel
    {
       public int id
       {
           get;
           set;

       }
       public string comapnylogo
       {
           get;
           set;
       }
       public string Dateformat
       {
           get;
           set;
       }
       public string Timeformat
       {
           get;
           set;
       }
       public string databaseaddress
       {
           get;
           set;
       }

       public string databasename
       {
           get;
           set;
       }
       public string databaseuserid
       {
           get;
           set;
       }
       public string databasepassword
       {
           get;
           set;
       }
       public string directorypath
       {
           get;
           set;

       }

    }
}
