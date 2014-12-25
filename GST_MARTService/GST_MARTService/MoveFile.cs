using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace GST_MARTService
{
   public  class MoveFile
    {

       public static string move(string filepath,string fileName)
       { 
           try
           {
               string sourceFile = @filepath;
               string destinationFile = @ConfigurationManager.AppSettings["FilesImported"] + fileName + DateTime.Now.ToString("ddMMyyy");

                    // To move a file or folder to a new location:
               if (File.Exists(destinationFile))
                       {
                           File.Delete(destinationFile);
                       } 
                 File.Move(sourceFile, destinationFile);

                    // To move an entire directory. To programmatically modify or combine
                    // path strings, use the System.IO.Path class.
                  //  System.IO.Directory.Move(@filepath, @destinationFile);
                    return "Success";
           }
           catch(Exception ex)
           {
               
               return ex.Message.ToString();

           }
    }
       
      
    }
}
