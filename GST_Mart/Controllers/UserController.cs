using GST_BLL.AdminUser;
using GST_BLL.AuthorizedUser;
using GST_BLL.DTO;
using GST_BLL.Enum;
using GST_BLL.Factory;
using Microsoft.SqlServer.Dts.Runtime;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services;
using PagedList.Mvc;
using PagedList;
using GST_BLL.MasterAdmin;
using System.Collections.ObjectModel;
using GST_BLL.SendMail;
using System.Web.UI.WebControls;
using System.Web.UI;


namespace GST_Mart.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /Account/

        string originalConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["GSTMARTEntities"].ConnectionString;
        AuthorizedUser user = new AuthorizedUser();
        AdminUser adminuser = new AdminUser();

        readonly string ms_email = ConfigurationManager.AppSettings["master_email"].ToString();
        readonly string smtp_port = ConfigurationManager.AppSettings["SMTP_PORT"].ToString();
        readonly string Smtp_Password = ConfigurationManager.AppSettings["Smtp_Password"].ToString();
        readonly string Smtp_Host = ConfigurationManager.AppSettings["SMTP_HOST"].ToString();
        readonly string URL = ConfigurationManager.AppSettings["URL"].ToString();

        readonly string DataSource = ConfigurationManager.AppSettings["DataSource"].ToString();
        readonly string Catalog = ConfigurationManager.AppSettings["Catalog"].ToString();
        readonly string Password = ConfigurationManager.AppSettings["Password"].ToString();
        readonly string Username = ConfigurationManager.AppSettings["Username"].ToString();
        readonly string DestCatalog = ConfigurationManager.AppSettings["DestCatalog"].ToString();



        List<Companylist> ob = new List<Companylist>();
        AuditLogModel AuditLog = new AuditLogModel();
        string ErrorMessage = "";
        Random random = new Random();
        UserModel usermodel = new UserModel();
        CompanyModel companymodel = new CompanyModel();
        SystemModel systemmodel = new SystemModel();
        MasterAdmin masteradmin = new MasterAdmin();
        CreateCompaneyModel createcompaneymodel = new CreateCompaneyModel();
        Mail mail = new Mail();
        CycleModel cyclemodel = new CycleModel();
        Mail MailMessage = new Mail();
        public ActionResult Login()
        {
            IPAddress IP = adminuser.getipaddress();
            TempData["IP"] = IP;

            ob = adminuser.GetCompanyList();
            ViewBag.Company = ob;
            return View();
        }
        [HttpPost]

        public ActionResult Login(AdminUserModel model, string companylist)
        {
            IPAddress IP = adminuser.getipaddress();
            try
            {
                Session["dbname"] = "Gst" + companylist.Replace(" ", string.Empty);
                Session["CompanyName"] = companylist.ToString();


                TempData["IP"] = IP;
                string IpStatus = adminuser.CkeckIpAddress(IP);
                if (IpStatus == "IpNotFound")
                {
                    string ReturnValue = adminuser.auditLog(DateTime.Now, IP.ToString(), "Ip Not Recognised", model.UserId, "", "Fail");
                    TempData["Loginerror"] = "Your Ip is Not Recognised";
                    return RedirectToAction("Login");
                }
                string UserId = "";
                string UserName = "";
                string Message = "";
                var result = user.CheckLogin(model, companylist, Session["dbname"].ToString(), out UserId, out UserName, out Message);
                if (result == true)
                {
                    Session["UserId"] = UserId;

                    //User Settings permission
                    var Userpermissions = user.GetPermission_ToUser(UserId);
                    Session["Settings"] = Userpermissions.isAccessSetting;

                    Session["UserLoginStatus"] = "LoggedIn";
                    Session["AuthorisedUserName"] = UserName;
                    Session["LoginTime"] = DateTime.Now.ToString("dd MMM yyyy,  HH:MM tt");
                    string ReturnValue = adminuser.auditLog(DateTime.Now, IP.ToString(), Message, UserId, UserName, "Success");
                    // FormsAuthentication.SetAuthCookie(model.UserId, false);
                    return RedirectToAction("ValidateImport");
                }
                else
                {
                    string ReturnValue = adminuser.auditLog(DateTime.Now, IP.ToString(), Message, model.UserId, UserName, "Fail");
                    string emailto = user.GetemailbyUserId(model.UserId, originalConnectionString, Session["dbname"].ToString());
                    if (!string.IsNullOrEmpty(emailto))
                    {
                        MailMessage mail = new MailMessage();
                        mail.To.Add(emailto);
                        mail.From = new MailAddress(ms_email);
                        mail.Subject = "Login Failure Mail";
                        string Body = "Someone is going to access your account.";
                        mail.Body = Body;
                        mail.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = Smtp_Host;
                        smtp.Port = Convert.ToInt32(smtp_port);
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential
                        (ms_email, Smtp_Password);
                        smtp.EnableSsl = true;
                        try
                        {
                            smtp.Send(mail);
                        }
                        catch (Exception ex)
                        {
                            // throw ex;
                            TempData["Loginerror"] = ex.Message;


                        }
                    }
                    TempData["Loginerror"] = "You have entered wrong credential.";

                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("The underlying provider failed on Open"))
                {
                    TempData["Loginerror"] = "Your Company database is not found";
                }
                else
                {
                    TempData["Loginerror"] = ex.Message;
                }
                string ReturnValue = adminuser.auditLog(DateTime.Now, IP.ToString(), ex.Message, model.UserId, "", "Fail");
                return RedirectToAction("Login");
            }
            return View();
        }


        // [Authorize(Roles = "Normal User")]
        public ActionResult CreateCycle()
        {
            if (Session["UserLoginStatus"] == "LoggedIn")
            {

                string UserId = Session["UserId"].ToString();
                var Userpermissions = user.GetPermission_ToUser(UserId);
                if (Userpermissions.isCreatecycle != "Deny")
                {
                    string companyid = Session["CompanyName"].ToString();
                    ViewBag.SystemList = user.GetSystemID(companyid);
                    return View();
                }
                else
                {
                    return RedirectToAction("ValidateImport");
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public ActionResult CreateCycle(CycleModel model, List<HttpPostedFileBase> fileUpload, string systemlist)
        {
            if (Session["UserLoginStatus"] == "LoggedIn")
            {

                try
                {
                    string companyid = Session["CompanyName"].ToString();
                    ViewBag.SystemList = user.GetSystemID(companyid);

                    // Handling Attachments -

                    int count = 0;
                    if (


                        (fileUpload[0].FileName.Contains(".xlsx") || fileUpload[0].FileName.Contains(".txt") || fileUpload[0].FileName.Contains(".csv"))
                        &&
                        (fileUpload[1].FileName.Contains(".xlsx") || fileUpload[1].FileName.Contains(".txt") || fileUpload[1].FileName.Contains(".csv"))
                        &&
                        (fileUpload[2].FileName.Contains(".xlsx") || fileUpload[2].FileName.Contains(".txt") || fileUpload[2].FileName.Contains(".csv"))
                        &&
                        (fileUpload[3].FileName.Contains(".xlsx") || fileUpload[3].FileName.Contains(".txt") || fileUpload[3].FileName.Contains(".csv"))
                        &&
                        (fileUpload[4].FileName.Contains(".xlsx") || fileUpload[4].FileName.Contains(".txt") || fileUpload[4].FileName.Contains(".csv"))
                       )
                    {

                        model.SystemId = systemlist;
                        var countCycle = user.GetLstCycleNumber(originalConnectionString, Session["dbname"].ToString());

                        countCycle = countCycle + 1;


                        model.CycleID = countCycle.ToString();

                        string AdminPath = adminuser.getAdminPath();
                        if (string.IsNullOrEmpty(AdminPath))
                        {
                            TempData["ErrorMessage"] = "Please contact your Administrator to Set path";
                            return View();
                        }
                        var companyPath = AdminPath + @"\Import\FilestoImport\" + Session["dbname"].ToString() + "/" + model.CycleID;



                        foreach (HttpPostedFileBase item in fileUpload)
                        {

                            string path = "";

                            count = count + 1;
                            if (count == 1)
                            {
                                if (item.FileName.Contains(".csv"))
                                {
                                    model.CPath = EnumClass.ImportFamily.Company + "-" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
                                }
                                else if (item.FileName.Contains(".xlsx"))
                                {
                                    model.CPath = EnumClass.ImportFamily.Company + "-" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
                                }
                                else if (item.FileName.Contains(".txt"))
                                {
                                    model.CPath = EnumClass.ImportFamily.Company + "-" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                                }
                                if (item.FileName.Contains(".sql"))
                                {
                                    model.CPath = EnumClass.ImportFamily.Company + "-" + DateTime.Now.ToString("yyyyMMdd") + ".sql";
                                }

                                path = model.CPath;

                                if (!Directory.Exists(companyPath))
                                {
                                    Directory.CreateDirectory(companyPath);
                                }
                                var finalPath = companyPath + "/" + path;


                                var length = item.ContentLength;
                                item.SaveAs(finalPath);

                                bool Status = Importer(1, finalPath, Session["CompanyName"].ToString(), model.CycleID);
                                if (Status == false)
                                {
                                    TempData["ErrorMessage"] = ErrorMessage;
                                    return RedirectToAction("CreateCycle");
                                }


                            }
                            else if (count == 2)
                            {


                                if (item.FileName.Contains(".csv"))
                                {
                                    model.Lpath = EnumClass.ImportFamily.Ledger + "-" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
                                }
                                else if (item.FileName.Contains(".xlsx"))
                                {
                                    model.Lpath = EnumClass.ImportFamily.Ledger + "-" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
                                }
                                else if (item.FileName.Contains(".txt"))
                                {
                                    model.Lpath = EnumClass.ImportFamily.Ledger + "-" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                                }
                                if (item.FileName.Contains(".sql"))
                                {
                                    model.Lpath = EnumClass.ImportFamily.Ledger + "-" + DateTime.Now.ToString("yyyyMMdd") + ".sql";
                                }



                                path = model.Lpath;

                                if (!Directory.Exists(companyPath))
                                {
                                    Directory.CreateDirectory(companyPath);
                                }
                                var finalPath = companyPath + "/" + path;



                                item.SaveAs(finalPath);


                                bool Status = Importer(2, finalPath, Session["CompanyName"].ToString(), model.CycleID);
                                if (Status == false)
                                {
                                    TempData["ErrorMessage"] = ErrorMessage;
                                    return RedirectToAction("CreateCycle");
                                }


                            }
                            else if (count == 3)
                            {


                                if (item.FileName.Contains(".csv"))
                                {
                                    model.Spath = EnumClass.ImportFamily.Supply + "-" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
                                }
                                else if (item.FileName.Contains(".xlsx"))
                                {
                                    model.Spath = EnumClass.ImportFamily.Supply + "-" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
                                }
                                else if (item.FileName.Contains(".txt"))
                                {
                                    model.Spath = EnumClass.ImportFamily.Supply + "-" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                                }
                                else if (item.FileName.Contains(".sql"))
                                {
                                    model.Spath = EnumClass.ImportFamily.Supply + "-" + DateTime.Now.ToString("yyyyMMdd") + ".sql";
                                }

                                path = model.Spath;

                                if (!Directory.Exists(companyPath))
                                {
                                    Directory.CreateDirectory(companyPath);
                                }
                                var finalPath = companyPath + "/" + path;



                                item.SaveAs(finalPath);

                                bool Status = Importer(3, finalPath, Session["CompanyName"].ToString(), model.CycleID);
                                if (Status == false)
                                {
                                    TempData["ErrorMessage"] = ErrorMessage;
                                    return RedirectToAction("CreateCycle");
                                }


                            }
                            else if (count == 4)
                            {

                                if (item.FileName.Contains(".csv"))
                                {
                                    model.Ppath = EnumClass.ImportFamily.Purchase + "-" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
                                }
                                else if (item.FileName.Contains(".xlsx"))
                                {
                                    model.Ppath = EnumClass.ImportFamily.Purchase + "-" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
                                }
                                else if (item.FileName.Contains(".txt"))
                                {
                                    model.Ppath = EnumClass.ImportFamily.Purchase + "-" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                                }
                                else if (item.FileName.Contains(".sql"))
                                {
                                    model.Ppath = EnumClass.ImportFamily.Purchase + "-" + DateTime.Now.ToString("yyyyMMdd") + ".sql";
                                }

                                path = model.Ppath;

                                if (!Directory.Exists(companyPath))
                                {
                                    Directory.CreateDirectory(companyPath);
                                }
                                var finalPath = companyPath + "/" + path;



                                item.SaveAs(finalPath);

                                bool Status = Importer(4, finalPath, Session["CompanyName"].ToString(), model.CycleID);
                                if (Status == false)
                                {
                                    TempData["ErrorMessage"] = ErrorMessage;
                                    return RedirectToAction("CreateCycle");
                                }

                            }
                            else if (count == 5)
                            {


                                if (item.FileName.Contains(".csv"))
                                {
                                    model.FPath = EnumClass.ImportFamily.Footer + "-" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
                                }
                                else if (item.FileName.Contains(".xlsx"))
                                {
                                    model.FPath = EnumClass.ImportFamily.Footer + "-" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
                                }
                                else if (item.FileName.Contains(".txt"))
                                {
                                    model.FPath = EnumClass.ImportFamily.Footer + "-" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                                }
                                else if (item.FileName.Contains(".sql"))
                                {
                                    model.FPath = EnumClass.ImportFamily.Footer + "-" + DateTime.Now.ToString("yyyyMMdd") + ".sql";
                                }
                                    path = model.FPath;
                                    if (!Directory.Exists(companyPath))
                                    {
                                        Directory.CreateDirectory(companyPath);
                                    }
                                    var finalPath = companyPath + "/" + path;

                                    item.SaveAs(finalPath);


                                    bool Status = Importer(5, finalPath, Session["CompanyName"].ToString(), model.CycleID);
                                    if (Status == false)
                                    {
                                        TempData["ErrorMessage"] = ErrorMessage;
                                        return RedirectToAction("CreateCycle");
                                    }


                            }
                        }

                        var result = user.AddCycle(model, originalConnectionString, Session["dbname"].ToString(), Session["UserId"].ToString());

                        if (result)
                        {
                            TempData["SuccessMessage"] = "Cycle Created Successfully";
                            model.Description = "";

                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Error Occured";
                        }





                    }
                    else
                    {

                        TempData["ErrorMessage"] = "it only Supports .txt,.sql and .csv";
                        return View();
                    }
                }

                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                }

                return RedirectToAction("CreateCycle");

                //return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult RecoverPassword()
        {

            return View();
        }
        [HttpPost]
        public ActionResult RecoverPassword(FormCollection Form)
        {
            string uniq = Guid.NewGuid().ToString();
            string Email = Form["Email"];
            string ReturnMessage = user.FindUser(Email);
            if (ReturnMessage != "")
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(ReturnMessage);
                mail.From = new MailAddress(ms_email);
                mail.Subject = "Reset Password Mail";
                string Link = user.GenerateLink(URL, ReturnMessage, uniq);
                string Body = "<br/><br/>Welcome : '" + ReturnMessage + "'<br/><br/>'" + Link + "'";
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = Smtp_Host;
                smtp.Port = Convert.ToInt32(smtp_port);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential
                (ms_email, Smtp_Password);
                smtp.EnableSsl = true;
                try
                {
                    smtp.Send(mail);
                }
                catch (Exception ex)
                {
                    ReturnMessage = "";
                }
            }
            if (ReturnMessage == "")
            {
                TempData["Loginerror"] = "Incorrect Email Id";
            }
            else
            {
                TempData["Loginsuccess"] = "Activation Link is send to your mail.";
            }
            return View();
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {

            string Email = Request.QueryString["Email"];
            string Guid = Request.QueryString["Guid"];
            string message = "";
            UserModel UserData = user.FindUserByEmail(Email, Guid, out message);
            if (message == EnumClass.MessageFamily.Error.ToString())
            {
                return RedirectToAction("INvalidLInk", "Admin");
            }
            return View(UserData);
        }


        [HttpPost]
        public ActionResult ChangePassword(UserModel Data)
        {
            if (Data.Password == null || Data.ConfirmPwd == null)
            {
                TempData["ErrorMessage"] = "Fields Required";
                return View();
            }
            else if (Data.Password != Data.ConfirmPwd)
            {
                TempData["ErrorMessage"] = "Password and Confirm Password does not match";
                return View();
            }
            string Email = "";
            string Username = "";
            Int32 Returnvalue = user.UpdatePassword(Data, out Email, out Username);
            if (Returnvalue == 2)
            {
                TempData["ErrorMessage"] = "User Does not exist";
                return View();
            }
            else
            {
                if (Email != "")
                {
                    string body = "Dear : " + Username + "<br/><br/>Your Updated Password is : " + Data.Password + "";
                    MailMessage.SendMail(Email, ms_email, smtp_port, Smtp_Password, Smtp_Host, "Your New Password is :", body);
                }
            }
            return RedirectToAction("Login");
        }



        public bool Importer(int fileIndex, string finalPath, string CompanyID, string CycleID)
        {

            #region To Import Files
            try
            {

                Application app = new Application();

                Package package = null;
                Variables SSISvar;
                //app.PackagePassword = "Admin123#";





                #region .xlsx File
                string excelFile = "";
                if (finalPath.Contains(".xlsx"))
                {
                    switch (fileIndex)
                    {

                        case 1:
                            {
                                excelFile = ConfigurationManager.AppSettings["xlsx_company"].ToString();
                                package = app.LoadPackage(ConfigurationManager.AppSettings["Excel_company_Path"], null);
                                break;
                            }
                        case 2:
                            {
                               
                                excelFile = ConfigurationManager.AppSettings["xlsx_ledger"].ToString();
                                package = app.LoadPackage(ConfigurationManager.AppSettings["Excel_ledger_Path"], null);
                                break;
                            }
                        case 3:
                            {
                                excelFile = ConfigurationManager.AppSettings["xlsx_supply"].ToString();
                                package = app.LoadPackage(ConfigurationManager.AppSettings["Excel_supply_Path"], null);
                                break;
                              
                            }
                        case 4:
                            {
                                excelFile = ConfigurationManager.AppSettings["xlsx_purchase"].ToString();
                                package = app.LoadPackage(ConfigurationManager.AppSettings["Excel_purchase_Path"], null);
                                break;
                              
                            }
                        case 5:
                            {
                                excelFile = ConfigurationManager.AppSettings["xlsx_footer"].ToString();
                                package = app.LoadPackage(ConfigurationManager.AppSettings["Excel_footter_Path"], null);
                                break;

                            }



                    }
                }
                #endregion

                #region .csv File and txt files
                
                    string csvFile = "";
                if (finalPath.Contains(".csv") || finalPath.Contains(".txt"))
                {

                    switch (fileIndex)
                    {

                        case 1:
                            {

                                csvFile = ConfigurationManager.AppSettings["csv_company"].ToString();
                                package = app.LoadPackage(ConfigurationManager.AppSettings["CSV_company_Path"], null);
                                break;


                            }
                        case 2:
                            {
                                csvFile = ConfigurationManager.AppSettings["csv_ledger"].ToString();
                                package = app.LoadPackage(ConfigurationManager.AppSettings["CSV_ledger_Path"], null);
                                break;

                            }
                        case 3:
                            {
                                csvFile = ConfigurationManager.AppSettings["csv_supply"].ToString();
                                package = app.LoadPackage(ConfigurationManager.AppSettings["CSV_supply_Path"], null);
                                break;
                            }
                        case 4:
                            {
                                csvFile = ConfigurationManager.AppSettings["csv_purchase"].ToString();
                                package = app.LoadPackage(ConfigurationManager.AppSettings["CSV_purchase_Path"], null);
                                break;
                            }
                        case 5:
                            {
                                csvFile = ConfigurationManager.AppSettings["csv_footer"].ToString();
                                package = app.LoadPackage(ConfigurationManager.AppSettings["CSV_footter_Path"], null);
                                break;


                            }



                    }
                }
                #endregion

                #region Package level variable
                SSISvar = package.Variables;
                SSISvar["CycleID"].Value = CycleID;
                SSISvar["CompanyID"].Value = CompanyID;
                SSISvar["Catalog"].Value = Catalog;
                SSISvar["Datasource"].Value = DataSource;
                SSISvar["Password"].Value = Password;
                SSISvar["Username"].Value = Username;
              
                #endregion

                #region Execute Package
                string connString = "";
                if (finalPath.Contains(".xlsx"))
                {
                    SSISvar["ExcelFilePath"].Value = excelFile; 
                    connString = "provider=Microsoft.ACE.OLEDB.12.0;data source=" + finalPath + ";Extended Properties=Excel 12.0 Xml;HDR=Yes;IMEX=1;";
                    package.Connections["SourceConnectionExcel"].ConnectionString = connString;
                }
                else if (finalPath.Contains(".csv"))
                {
                    SSISvar["CsvFile"].Value = csvFile; 
                    var splitPath = finalPath.Replace("/", "\\");
                    connString = @splitPath;
                    package.Connections["SourceConnectionFlatFile"].ConnectionString = connString;
                }
                else if (finalPath.Contains(".txt"))
                {

                }
                //Excute Package
                Microsoft.SqlServer.Dts.Runtime.DTSExecResult results = package.Execute(null, SSISvar, null, null, null);

                //Check for Package Error
                if (results == Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure)
                {
                    foreach (Microsoft.SqlServer.Dts.Runtime.DtsError local_DtsError in package.Errors)
                    {
                        Console.WriteLine("Package Execution results: {0}", local_DtsError.Description.ToString());
                        ErrorMessage = local_DtsError.Description.ToString();
                        return false;
                    }
                }


                #endregion
            }
            catch (DtsException ex)
            {

                ErrorMessage = ex.Message.ToString();
                return false;
            }

            return true;

            #endregion
        }

        [WebMethod]
        public void Logout()
        {
            Session["UserId"] = "";
            Session["UserLoginStatus"] = "LoggedOut";
        }


        public string Alphanumrical(int length, Random random)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var result = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return result;
        }


        public ActionResult DownloadUserData()
        {
            if (Session["UserLoginStatus"] == "LoggedIn")
            {
            var userid = Session["UserId"].ToString();
            UserModel model = user.FindUserbyUserid(userid);

            string securitycode = Alphanumrical(6, random);
            Session["Securitycode"] = securitycode;
            mail.Sendsecuritycode(model.Email, securitycode, model.Name, ms_email, smtp_port, Smtp_Password, Smtp_Host, "Security Code for Download");
            return View("Securitycodeaccess");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult DownloadUserData(string SecurityCode)
        {
            UserModel model = new UserModel();

            String securitycode = Session["Securitycode"].ToString();

            if (securitycode == SecurityCode)
            {
                var userid = Session["UserId"].ToString();
                var companyid = Session["CompanyName"].ToString();
                string error = "";
                var result = user.GetImportProcess(originalConnectionString, Session["dbname"].ToString(), Session["CompanyName"].ToString(), out error);

                
                foreach (var item in result)
                {
                    cyclemodel.SystemId = item.SystemId;
                    cyclemodel.CycleErrors = item.CycleErrors;
                }
              

                var products = new System.Data.DataTable("teste");
                products.Columns.Add("Id", typeof(int));
                products.Columns.Add("System id", typeof(string));
                products.Columns.Add("Error", typeof(string));
                products.Columns.Add("Duplicates ", typeof(string));
              


                products.Rows.Add(1, cyclemodel.SystemId, cyclemodel.CycleErrors, "45");



                var grid = new GridView();
                grid.DataSource = products;
                grid.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + userid + ".xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

            }
            return View();
        }


        [HttpGet]
        public ActionResult ValidateImport(int? page, int pagesize = 0, string DDlPagingsize = "5")
        {
            if (Session["UserLoginStatus"] == "LoggedIn")
            {
                string UserId = Session["UserId"].ToString();
                var Userpermissions = user.GetPermission_ToUser(UserId);
                ViewBag.Userpermissions = Userpermissions;

                if (pagesize == 0)
                {
                    pagesize = Convert.ToInt32(DDlPagingsize);
                }
                string error = "";
                var result = user.GetImportProcess(originalConnectionString, Session["dbname"].ToString(), Session["CompanyName"].ToString(), out error);


                if (error.Contains("error"))
                {
                    TempData["Loginerror"] = "Your Database is not Found in GST System";

                    return RedirectToAction("Login");
                }



                if (result != null)
                {

                    int pageNumber = (page ?? 1);

                    TempData["pagesize"] = pagesize;
                    return View(result.ToPagedList(pageNumber, pagesize));
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();

        }


        [HttpPost]
        public ActionResult ValidateImport(string SecurityCode, string ValidateSecurityCode)
        {
            String securitycode = Session["Securitycode"].ToString();

            if (securitycode == SecurityCode)
            {

                // SecurityCode.
                var userid = Session["UserId"].ToString();
                var companyid = Session["CompanyName"].ToString();
                string error = "";
                var result = user.GetImportProcess(originalConnectionString, Session["dbname"].ToString(), Session["CompanyName"].ToString(), out error);


                foreach (var item in result)
                {
                    cyclemodel.SystemId = item.SystemId;

                    cyclemodel.CycleErrors = item.CycleErrors;



                }


                #region Print Errors Reports
                var products = new System.Data.DataTable("teste");
                products.Columns.Add("Id", typeof(int));

                products.Columns.Add("System id", typeof(string));
                products.Columns.Add("Error", typeof(string));
                products.Columns.Add("Duplicates ", typeof(string));

                products.Rows.Add(1, cyclemodel.SystemId, cyclemodel.CycleErrors, "45");

                var grid = new GridView();
                grid.DataSource = products;
                grid.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + userid + ".xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
                #endregion
            }
            else
            {
                TempData["CodeError"] = "Invalid Code";
                return View("Securitycodeaccess");
            }
            return RedirectToAction("ValidateImport", companymodel);
        }

        public ActionResult DeleteProcess(int id, string CycleId)
        {

            if (Session["UserLoginStatus"] == "LoggedIn")
            {


                #region Run Delete SSIS package
                Application app = new Application();

                Package package = null;
                Variables SSISvar;

                package = app.LoadPackage(ConfigurationManager.AppSettings["Delete_Errors_Path"], null);


                package.Connections["SourceConnectionFlatFile"].ConnectionString = ConfigurationManager.AppSettings["LedgerStaticFile_Path"];

                SSISvar = package.Variables;
                SSISvar["CycleID"].Value = CycleId;
                SSISvar["CompanyID"].Value = Session["CompanyName"].ToString();
                SSISvar["Catalog"].Value = Catalog;
                SSISvar["Datasource"].Value = DataSource;
                SSISvar["Password"].Value = Password;
                SSISvar["Username"].Value = Username;
                SSISvar["CsvFile"].Value = ConfigurationManager.AppSettings["LedgerStaticFile_Path"];
                

                //Excute Package
                Microsoft.SqlServer.Dts.Runtime.DTSExecResult results = package.Execute(null, SSISvar, null, null, null);

                if (results == Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure)
                {
                    foreach (Microsoft.SqlServer.Dts.Runtime.DtsError local_DtsError in package.Errors)
                    {
                        Console.WriteLine("Package Execution results: {0}", local_DtsError.Description.ToString());
                        ErrorMessage = local_DtsError.Description.ToString();
                        TempData["ErrorMessage"] = ErrorMessage;
                        return RedirectToAction("ValidateImport");
                    }
                }

                #endregion
                var result = user.TempDeleteCycle(originalConnectionString, Session["dbname"].ToString(), id, "Deleted");
                if (result == true)
                {



                    return RedirectToAction("ValidateImport");

                }
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();

        }

        public ActionResult ImportTODb(int id, string CycleId)
        {
            if (Session["UserLoginStatus"] == "LoggedIn")
            {

                Application app = new Application();

                Package package = null;

                Variables SSISvar;



                for (var i = 1; i <= 5; i++)
                {

                    if (i == 1)
                    {
                        package = app.LoadPackage(ConfigurationManager.AppSettings["Importer_ledger_Path"], null);
                    }
                    else if (i == 2)
                    {
                        package = app.LoadPackage(ConfigurationManager.AppSettings["Importer_supply_Path"], null);
                    }
                    else if (i == 3)
                    {
                        package = app.LoadPackage(ConfigurationManager.AppSettings["Importer_purchase_Path"], null);
                    }
                    else if (i == 4)
                    {
                        package = app.LoadPackage(ConfigurationManager.AppSettings["Importer_company_Path"], null);
                    }
                    else if (i == 5)
                    {
                        package = app.LoadPackage(ConfigurationManager.AppSettings["Importer_footter_Path"], null);
                    }



                    #region Package level variable
                    SSISvar = package.Variables;
                    SSISvar["CycleID"].Value = CycleId;
                    SSISvar["CycleIDDelete"].Value = CycleId;
                    SSISvar["Catalog"].Value = Catalog;
                    SSISvar["Datasource"].Value = DataSource;
                    SSISvar["Password"].Value = Password;
                    SSISvar["Username"].Value = Username;
                    SSISvar["DestCatalog"].Value = DestCatalog;



                    #endregion
                    //Excute Package
                    Microsoft.SqlServer.Dts.Runtime.DTSExecResult results = package.Execute(null, SSISvar, null, null, null);

                    if (results == Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure)
                    {
                        foreach (Microsoft.SqlServer.Dts.Runtime.DtsError local_DtsError in package.Errors)
                        {
                            Console.WriteLine("Package Execution results: {0}", local_DtsError.Description.ToString());
                            ErrorMessage = local_DtsError.Description.ToString();
                            TempData["ErrorMessage"] = ErrorMessage;
                            return RedirectToAction("ValidateImport");
                        }
                    }

                }
                var result = user.TempDeleteCycle(originalConnectionString, Session["dbname"].ToString(), id, "Imported");
                TempData["SuccessMessage"] = "Data Importing Completed";

                return RedirectToAction("ValidateImport");

            }
            else
            {
                return RedirectToAction("Login");
            }

        }

        [HttpGet]
        public ActionResult CycleErrors(string CycleId)
        {
            if (Session["UserLoginStatus"] == "LoggedIn")
            {

            //To get Data Type Conversion Errors
            var data = user.ReadRrrors(CycleId, Session["CompanyName"].ToString());
            ViewBag.purchaseError = user.ReadPurchaseRrrors(CycleId, Session["CompanyName"].ToString());
            ViewBag.supplyError = user.ReadSupplyRrrors(CycleId, Session["CompanyName"].ToString());
            ViewBag.companyError = user.ReadCompanyRrrors(CycleId, Session["CompanyName"].ToString());
            ViewBag.footerError = user.ReadFooterRrrors(CycleId, Session["CompanyName"].ToString());
            //To get duplicate records
            ViewBag.purchase = user.ReadPurchaseDuplicateErrors(CycleId, Session["CompanyName"].ToString());
            ViewBag.supply = user.ReadSupplyDuplicateErrors(CycleId, Session["CompanyName"].ToString());
            ViewBag.ledger = user.ReadDuplicateLedgerErrors(CycleId, Session["CompanyName"].ToString());
            ViewBag.comopany = user.ReadCompanyDuplicateErrors(CycleId, Session["CompanyName"].ToString());
            ViewBag.footer = user.ReadFooterDuplicateErrors(CycleId, Session["CompanyName"].ToString());
            //To get missing(Required Filed Validation Failed) records
            ViewBag.MissingPurchase = user.ReadPurchaseMissingErrors(CycleId, Session["CompanyName"].ToString());
            ViewBag.MissingLedger = user.ReadMissingLedgerErrors(CycleId, Session["CompanyName"].ToString());
            ViewBag.MissingSupply = user.ReadSupplyMissingErrors(CycleId, Session["CompanyName"].ToString());
            ViewBag.MissingCompany = user.ReadCompanyMissingErrors(CycleId, Session["CompanyName"].ToString());
            ViewBag.MissingFooter = user.ReadFooterMissingErrors(CycleId, Session["CompanyName"].ToString());


            return View(data);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

    }
    class MyEventListener : DefaultEvents
    {
        public override bool OnError(DtsObject source, int errorCode, string subComponent,
          string description, string helpFile, int helpContext, string idofInterfaceWithError)
        {
            // Add application-specific diagnostics here.
            Console.WriteLine("Error in {0}/{1} : {2}", source, subComponent, description);
            return false;
        }
    }
}
