using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GST_BLL.Factory
{
//    public class Importer 
//    {
//      public bool RunImporter()
//      {

//          //Start the SSIS Here

//          try
//          {
//             Application app = new Application();

//              Package package = null;
//              package = app.LoadPackage(@"D:\GST-mart\GST-mart\Package1.dtsx", null);
//              string fileName = Server.MapPath(System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName.ToString()));
//              FileUpload1.PostedFile.SaveAs(fileName);
//              string connString = "provider=Microsoft.ACE.OLEDB.12.0;data source=" + fileName + ";Extended Properties=Excel 12.0 Xml;HDR=Yes;IMEX=1;";
//              package.Connections["SourceConnectionExcel"].ConnectionString = connString;
//              // package.Connections["OLEDB"].ConnectionString =  "Data Source=VUYISWA;Initial Catalog=oDirectv3;Persist Security Info=True;User ID=sa;Password=abacus" providerName="System.Data.SqlClient”
//              //Excute Package
//              Microsoft.SqlServer.Dts.Runtime.DTSExecResult results = package.Execute();

//              if (results == Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure)
//              {
//                  foreach (Microsoft.SqlServer.Dts.Runtime.DtsError local_DtsError in package.Errors)
//                  {

//                      Console.WriteLine("Package Execution results: {0}", local_DtsError.Description.ToString());
//                      Console.WriteLine();
//                  }
//              }

//          }
//          catch (DtsException ex)
//          {
//              //throw  ex.Message;
//          }
      
//      }
//    }

//  class MyEventListener : DefaultEvents
//  {
//      public override bool OnError(DtsObject source, int errorCode, string subComponent,
//        string description, string helpFile, int helpContext, string idofInterfaceWithError)
//      {
//          // Add application-specific diagnostics here.
//          Console.WriteLine("Error in {0}/{1} : {2}", source, subComponent, description);
//          return false;
//      }
//  }
}
