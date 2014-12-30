
using Microsoft.SqlServer.Dts.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Configuration;

namespace GST_MARTService
{
    public partial class ImpoterService : ServiceBase
    {

       // private EventLog eventLog1;
        System.Threading.Thread processThread;
       private Boolean Cancel;
        System.Timers.Timer timer = new System.Timers.Timer();
        public ImpoterService()
        {
            InitializeComponent();

            //this.AutoLog = false;
            //if (!System.Diagnostics.EventLog.SourceExists("DoDyLogSourse"))

            //    System.Diagnostics.EventLog.CreateEventSource("DoDyLogSourse",
            //                                                          "DoDyLog");

            //eventLog1.Source = "DoDyLogSourse";
            //// the event log source by which 

            ////the application is registered on the computer

            //eventLog1.Log = "DoDyLog";
            OnStart(new string[20]);
        }

        protected override void OnStart(string[] args)
        {
            // Set up a timer to trigger every minute.

            timer.Interval = (20 * 60 * 1000); // 20 minutes
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();
            Cancel = false;


            processThread = new System.Threading.Thread(new ThreadStart(DoWork));
            
            processThread.Start();
        }

        protected override void OnStop()
        {
            if (processThread.ThreadState == System.Threading.ThreadState.Running)
            {
                Cancel = true;
                // Give thread a chance to stop
                processThread.Join(500);
                processThread.Abort();
            }
        }
        public void Start()
        {
            OnStart(new string[20]);
        }
        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            processThread = new System.Threading.Thread(new ThreadStart(DoWork));
            processThread.Start();

        }

        protected override void OnContinue()
        {
            timer.Start();
        }

        public bool Importer(string moduleName, string finalPath,string cycleID,string companyID)
        {

            #region To Import Files
            try
            {


                Application app = new Application();
                Package package = null;
                Variables SSISvar;





                if (finalPath.Contains(".csv"))
                {


                    if (moduleName.Contains("company"))
                    {
                        package = app.LoadPackage(@ConfigurationManager.AppSettings["CSV_company_Path"], null);
                    }
                    else if (moduleName.Contains("ledger"))
                    {

                        package = app.LoadPackage(@ConfigurationManager.AppSettings["CSV_ledger_Path"], null);
                    }
                    else if (moduleName.Contains("supply"))
                    {

                        package = app.LoadPackage(@ConfigurationManager.AppSettings["CSV_supply_Path"], null);

                    }
                    else if (moduleName.Contains("purchase"))
                    {

                        package = app.LoadPackage(@ConfigurationManager.AppSettings["CSV_purchase_Path"], null);


                    }
                    else if (moduleName.Contains("footer"))
                    {
                        package = app.LoadPackage(@ConfigurationManager.AppSettings["CSV_purchase_Path"], null);
                        return false;

                    }
                }
                #region XLSX file
                else if (finalPath.Contains(".xlsx"))
                {

                    if (moduleName.Contains("company"))
                    {
                        package = app.LoadPackage(@ConfigurationManager.AppSettings["Excel_company_Path"], null);
                    }
                    else if (moduleName.Contains("ledger"))
                    {

                        package = app.LoadPackage(@ConfigurationManager.AppSettings["Excel_ledger_Path"], null);
                    }
                    else if (moduleName.Contains("supply"))
                    {
                        package = app.LoadPackage(@ConfigurationManager.AppSettings["Excel_supply_Path"], null);

                    }
                    else if (moduleName.Contains("purchase"))
                    {
                        package = app.LoadPackage(@ConfigurationManager.AppSettings["Excel_purchase_Path"], null);


                    }
                    else if (moduleName.Contains("footer"))
                    {
                        package = app.LoadPackage(@ConfigurationManager.AppSettings["Excel_footter_Path"], null);

                    }

                }
                #endregion


                string connString = "";
                if (finalPath.Contains(".csv"))
                {
                    var splitPath = finalPath.Replace("/", "\\");
                    connString = @splitPath;
                    
                    package.Connections["SourceConnectionFlatFile"].ConnectionString = connString;
                }
                else if (finalPath.Contains(".xlsx"))
                {
                    connString = "provider=Microsoft.ACE.OLEDB.12.0;data source=" + finalPath + ";Extended Properties=Excel 12.0 Xml;HDR=Yes;IMEX=1;";
                    package.Connections["SourceConnectionExcel"].ConnectionString = connString;
                }

                SSISvar = package.Variables;
                SSISvar["CycleID"].Value = cycleID;
               SSISvar["CompanyID"].Value = companyID;


                //Excute Package
               Microsoft.SqlServer.Dts.Runtime.DTSExecResult results = package.Execute(null, SSISvar, null, null, null);

                if (results == Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure)
                {
                    foreach (Microsoft.SqlServer.Dts.Runtime.DtsError local_DtsError in package.Errors)
                    {

                        Console.WriteLine("Package Execution results: {0}", local_DtsError.Description.ToString());
                       // Logger.Write(local_DtsError.Description.ToString() + "------");
                        return false;

                    }
                }
                else
                {
                   // Logger.Write("success");
                    return true;
                }

            }
            catch (DtsException ex)
            {


                return false;
            }

            return false;

            #endregion
        }

        //Function to Run Third
        public bool GetFile(string path,string DBName,string CompanyID)
        {

            DirectoryInfo d = new DirectoryInfo(@path);//Assuming Test is your Folder
            //Getting Text files
            FileInfo[] txtFiles = d.GetFiles("*.txt");
            //Getting csv files
            FileInfo[] csvFiles = d.GetFiles("*.csv");
            //Getting xlsx files
            FileInfo[] xlsxFiles = d.GetFiles("*.xlsx");


            #region Find CSV File
            if (csvFiles.Length > 0)
            {

                foreach (FileInfo file in csvFiles)
                {
                    var name = file.Name.ToString();
                    var finalPath = path + "/" + name;
                    if (finalPath.Contains("company") || finalPath.Contains("footer") || finalPath.Contains("purchase") || finalPath.Contains("ledger") || finalPath.Contains("supply"))
                    {
                        string CycleId = MaxCycleId(DBName);
                        var result = Importer(name, finalPath, CycleId, CompanyID);
                        string SystemID = "";
                        if (result == true)
                        {
                            string Supply="", Purchase="", Ledger="", Footer="", Company = "";
                            if (finalPath.Contains("company"))
                            {
                                SystemID = "Company Data";
                                 Supply = "";
                                 Purchase = "";
                                 Ledger = "";
                                 Footer = "";
                                 Company = finalPath;
                            }
                            if (finalPath.Contains("footer"))
                            {
                                SystemID = "Footer Data";
                                Supply = "";
                                 Purchase = "";
                                 Ledger = "";
                                 Footer = finalPath;
                                 Company = "";
                            }
                            if (finalPath.Contains("purchase"))
                            {
                                SystemID = "Purchase Data";
                                 Supply = "";
                                 Purchase = finalPath;
                                 Ledger = "";
                                 Footer = "";
                                 Company = "";
                            }
                            if (finalPath.Contains("ledger"))
                            {
                                SystemID = "Ledger Data";
                                 Supply = "";
                                 Purchase = "";
                                 Ledger = finalPath;
                                 Footer = "";
                                 Company = "";
                            }
                            if (finalPath.Contains("supply"))
                            {
                                SystemID = "Supply Data";
                                 Supply = finalPath;
                                 Purchase = "";
                                 Ledger = "";
                                 Footer = "";
                                 Company = "";
                            }
                          
                            var resultOfCycle = SaveCycle(Supply, Purchase, Ledger, Footer, Company, CycleId, DBName,SystemID);

                            if (resultOfCycle)
                            {
                                //ConfigurationManager.AppSettings["con_str"]
                                Console.WriteLine("Cycle Is Create");

                                var fileMoved = MoveFile.move(finalPath.Replace("/", "\\"), name);
                                if(fileMoved=="!Success")
                                {
                                    Console.WriteLine(DateTime.Now + "::" + "Error Occured while moving file");
                                }
                               
                            }
                            else
                            {
                                Console.WriteLine(DateTime.Now+"::"+"Cycle Creation Failed");
                            }


                        }
                    }
                }

                return true;
            }
            #endregion

            #region Find Excel File
            else if (xlsxFiles.Length > 0)
            {
                foreach (FileInfo file in xlsxFiles)
                {
                    var name = file.Name.ToString();
                    var finalPath = path + "/" + name;
                    if (finalPath.Contains("company") || finalPath.Contains("footer") || finalPath.Contains("purchase") || finalPath.Contains("ledger") || finalPath.Contains("supply"))
                    {
                      //  var result = Importer(name, finalPath);
                      ////  if (result == true)
                      //  {
                      //      SqlCommand cmd = new SqlCommand();
                      //      SqlConnection con = new SqlConnection();
                      //      con.ConnectionString =
                      //      MoveFile.move(finalPath.Replace("/", "\\"), name);
                      //  }
                    }
                }

                return true;
            }
            #endregion

            else
            {
                return false;
            }




        }


        //Function to Run Fourth
        public bool UpdateSchedular(string NextTime, string lastRun, string id)
        {

            try
            {

                var builder = new SqlConnectionStringBuilder();
                builder.DataSource = "192.168.0.106";
                builder.InitialCatalog = "GSTMARTTEST";
                builder.UserID = "sa";
                builder.Password = "Admin123#";
                using (var conn = new SqlConnection(builder.ToString()))
                {
                    using (var cmd = new SqlCommand())
                    {
                        cmd.CommandText = "UPDATE  Schedulars set NextRun=@NextRun,LastRun=@LastRun where id=@id";
                        cmd.Parameters.AddWithValue("@NextRun", NextTime);
                        cmd.Parameters.AddWithValue("@LastRun", lastRun);
                        cmd.Parameters.AddWithValue("@id", id);

                        cmd.Connection = conn;
                        conn.Open();

                        cmd.ExecuteNonQuery();

                    }
                }
               // Logger.Write("success");
                return true;
            }
            catch (Exception ex)
            {
                //Logger.Write(ex.Message.ToString() + "------" + ex.InnerException);
                return false;
            }

        }

        //Function to Run First 
        private void DoWork()
        {

            try
            {
                while (!Cancel)
                {

                    if (Cancel) { return; }
                    // Do General Work

                RunSchedular();


                System.Threading.Thread.BeginCriticalRegion();
                {
                    // Do work that should not be aborted in here.
                }
                System.Threading.Thread.EndCriticalRegion();
                }

            }
            catch (System.Threading.ThreadAbortException tae)
            {
                // Clean up correctly to leave program in stable state.
            }
        }

        public bool SaveCycle(string Supply, string Purchase, string Ledger, string Footer, string Company, string CycleID, string DBName, string SystemId)
        {

            try
            {
                string Status = "Running";
                string Description = "WindowSchedular";
              //  string SystemId = "WindowSchedular";
                string UserId = "Not Defined";
                if (Supply == "")
                {
                    Supply = "NotDefined";
                }
                if (Purchase == "")
                {
                    Purchase = "NotDefined";
                }
                if (Ledger == "")
                {
                    Ledger = "NotDefined";
                }
                if (Footer == "")
                {
                    Footer = "NotDefined";
                }
                if (Company == "")
                {
                    Company = "NotDefined";
                }

                var builder = new SqlConnectionStringBuilder();
                builder.DataSource = "192.168.0.106";
                builder.InitialCatalog = DBName;
                builder.UserID = "sa";
                builder.Password = "Admin123#";
                using (var conn = new SqlConnection(builder.ToString()))
                {
                    using (var cmd = new SqlCommand())
                    {

                        cmd.CommandText = "INSERT INTO Cycles (SystemId,Description,CreatedDate,Supply,Purchase,Ledger,Footer,Company,CycleID,Status,UserId)VALUES (@SysId,@Desc,@CreDate,@Supply,@Purchase,@Ledger,@Footer,@Company,@CycId,@Status,@UserId)";
                        cmd.Parameters.AddWithValue("@SysId", SystemId);
                        cmd.Parameters.AddWithValue("@Desc", Description);
                        cmd.Parameters.AddWithValue("@CreDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Supply", Supply);
                        cmd.Parameters.AddWithValue("@Purchase", Purchase);
                        cmd.Parameters.AddWithValue("@Ledger", Ledger);
                        cmd.Parameters.AddWithValue("@Footer", Footer);
                        cmd.Parameters.AddWithValue("@Company", Company);
                        cmd.Parameters.AddWithValue("@CycId", CycleID);
                        cmd.Parameters.AddWithValue("@Status", Status);
                        cmd.Parameters.AddWithValue("@UserId", UserId);

                        cmd.Connection = conn;
                        conn.Open();

                        cmd.ExecuteNonQuery();

                    }
                }
               // Logger.Write("success");
                return true;
            }
            catch (Exception ex)
            {
              //  Logger.Write(ex.Message.ToString() + "------" + ex.InnerException);
                return false;
            }

        }

        public string MaxCycleId(string DBName)
        {
            string CycleId = "";
            try
            {
                var builder = new SqlConnectionStringBuilder();
                builder.DataSource = "192.168.0.106";
                builder.InitialCatalog = DBName;
                builder.UserID = "sa";
                builder.Password = "Admin123#";


                using (var conn = new SqlConnection(builder.ToString()))
                {
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "Select Top 1 CycleID from Cycles ORDER BY ID DESC";
                        conn.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                CycleId = reader["CycleID"].ToString();
                                CycleId = (Convert.ToInt32(CycleId) + 1).ToString();
                            }
                            else
                            {
                                CycleId = "1";
                            }
                        }
                    }
                }
                return CycleId;
            }
            catch
            {
                return CycleId;
            }
        }

        [Obsolete]
        public string CheckForCompanyDB()
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = "192.168.0.106";
            builder.InitialCatalog = "GSTMARTTEST";
            builder.UserID = "sa";
            builder.Password = "Admin123#";

            try
            {
                using (var conn = new SqlConnection(builder.ToString()))
                {
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "select * from Companies";
                        conn.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var companyId = reader["CompanyID"].ToString();
                                
                            }
                        }
                    }
                }
                return "Pass";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
           
        }

        //Function to Run Second 
        public void RunSchedular()
        
        {

            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = "192.168.0.106";
            builder.InitialCatalog = "GSTMARTTEST";
            builder.UserID = "sa";
            builder.Password = "Admin123#";

            #region Start DB "USING"
            using (var conn = new SqlConnection(builder.ToString()))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from Schedulars where status='Running'";
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            var module = reader["Module"].ToString();
                            var unit = reader["Unit"].ToString();
                            var frequency = Convert.ToInt32(reader["Frequency"]);
                            var StartTime = Convert.ToInt32(reader["STime"]);
                            var NextTime = Convert.ToInt32(reader["NextRun"]);
                            var lastRun = DateTime.Now.Hour;
                            var currentTime = DateTime.Now.Hour;
                            var id = Convert.ToInt32(reader["id"]);
                            var FilePath = reader["Path"].ToString();
                            var companyId = reader["CompanyID"].ToString();


                            #region Run Schedular for "HOUR"
                            if (unit.Contains("HOURS"))
                            {
                               

                                if (NextTime == currentTime)
                                {
                                    Console.WriteLine(DateTime.Now + "::" + "Schedular started for " + module + " in " + "GST" + companyId + " Company.");

                                    //File Found at designated location and Importre Process Comppleted
                                    var fileFound = GetFile(FilePath,"GST"+ companyId,companyId);

                                    if (fileFound == true)
                                    {
                                        NextTime = NextTime + frequency;

                                        if (NextTime > 24)
                                        {
                                            NextTime = StartTime;
                                        }
                                      var schedularCreated=UpdateSchedular(NextTime.ToString(), lastRun.ToString(), id.ToString());
                                        if(schedularCreated)
                                        {

                                            Console.WriteLine("Importing is Completed Succesfully for " + " in " + "GST" + companyId + " Company.");
                                        }
                                    }

                                }
                            }
                            #endregion


                            #region Run Schedular for "DAYS"
                            if (unit.Contains("DAYS"))
                            {


                                if (NextTime == currentTime)
                                {
                                    Console.WriteLine(DateTime.Now + "::" + "Schedular started for " + module + " in " + "GST" + companyId + " Company.");

                                    //File Found at designated location and Importre Process Comppleted
                                    var fileFound = GetFile(FilePath, "GST" + companyId, companyId);

                                    if (fileFound == true)
                                    {
                                        NextTime = NextTime + frequency;

                                        if (NextTime > 24)
                                        {
                                            NextTime = StartTime;
                                        }
                                        var schedularCreated = UpdateSchedular(NextTime.ToString(), lastRun.ToString(), id.ToString());
                                        if (schedularCreated)
                                        {

                                            Console.WriteLine("Importing is Completed Succesfully for " + " in " + "GST" + companyId + " Company.");
                                        }
                                    }

                                }
                            }
                            #endregion
                        }
                    }
                }
            #endregion

            }


        }
    }
}