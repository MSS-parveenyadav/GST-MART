using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using GST_BLL.DTO;
using GST_DB;
using GST_BLL.MasterAdmin;
using System.Web.Services;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.Security;
using System.Net.Mail;
using PagedList;
using PagedList.Mvc;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using GST_BLL.SendMail;
using GST_BLL.Enum;

namespace GST_Mart.Controllers
{
    public class MasterController : Controller
    {
  
        readonly string ms_username = ConfigurationManager.AppSettings["masterusername"].ToString();
        readonly string ms_password = ConfigurationManager.AppSettings["masterpwd"].ToString();
        readonly string ms_email = ConfigurationManager.AppSettings["master_email"].ToString();
        readonly string smtp_port = ConfigurationManager.AppSettings["SMTP_PORT"].ToString();
        readonly string Smtp_Password = ConfigurationManager.AppSettings["Smtp_Password"].ToString();
        readonly string Smtp_Host = ConfigurationManager.AppSettings["SMTP_HOST"].ToString();
        readonly string DbDetails = ConfigurationManager.AppSettings["DbDetails"].ToString();

        public static string DataStatus = "";
        public static string CompaneyUpdatedStatus = "";
       

        Random random = new Random();

        UserModel usermodel = new UserModel();
        CompanyModel companymodel = new CompanyModel();
        SystemModel systemmodel = new SystemModel();
        MasterAdmin masteradmin = new MasterAdmin();
        CreateCompaneyModel createcompaneymodel = new CreateCompaneyModel();
        Mail mail = new Mail();
        
        //Action Method to create master login.
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Username, string Password)
        {
            //Check whether username and password is empty or not.
            if (Username != "" && Password != "")
            {
                //Condition to check correct username and password.
                if (Username == ms_username && Password == ms_password)
                {

                    Session["MasterUserName"] = Username;

                    Session["LoginTime"] = DateTime.Now.ToString("dd MMM yyyy, HH:MM tt");

                    Session["MasterLoginStatus"] = "LoggedIn";

                    ViewData["Sucess"] = "Login Successfully";

                    return RedirectToAction("CreateCompany");
                }
                else
                {

                     string Body = "Someone is going to access your account.";

                    mail.SendMail(ms_email, ms_email, smtp_port, Smtp_Password, Smtp_Host, "GST Mart Login failed!!", Body);

                    ViewData["Error"] = "Wrong Username or Password.";

                    return View();
                }
            }
            else
            {
                if (Username == "" && Password == "")
                    ViewData["Error"] = "Enter Username and Password";
                else if (Username == "")
                    ViewData["Error"] = "Enter Username";
                else
                    ViewData["Error"] = "Enter Password";
                return View();
            }

        }

        // [Authorize(Roles = "Admin")]    
        public ActionResult CreateCompany(int? page, int pagesize = 0, string DDlPageingsize = "5")
        {
            if (Session["MasterLoginStatus"] == "LoggedIn")
            {
                //ForUserHeading & Todays Date
                TempData["UserName"] = Session["MasterUserName"].ToString();
                TempData["TodaysDate"] = Session["LoginTime"].ToString();

                if (pagesize == 0)
                {
                    pagesize = Convert.ToInt32(DDlPageingsize);
                }
                if (DataStatus != "")
                {
                    if (DataStatus == "Company Deleted Successfully")
                    {
                        TempData["DeleteSucess"] = DataStatus;
                    }
                    else if (!DataStatus.Contains("Duplicate"))
                    {
                        TempData["SuccessStatus"] = DataStatus;
                    }
                    DataStatus = "";
                }

                var list = masteradmin.getcompanydetails();
                ViewBag.company = list;
                int pageNumber = (page ?? 1);
                TempData["pagesize"] = pagesize;
                return View(list.ToPagedList(pageNumber, pagesize));
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


   
        public ActionResult DownloadData(string id)
        {
          
       // var ob = masteradmin.GetCompanyByID(id);
        //var list = masteradmin.getcompanydetails();
           var model = masteradmin.Findcompany(id);

            var date = model.Company.CreatedDate;
       
            string systems = "";

            string industrys = "";

            foreach (var system in model.Company.systemModel)
                                {
                                    systems = systems + system.SystemId + ",";
                                }

              foreach (var industry in model.Company.industrymodel)
                                {
                                     industrys = industrys + industry.Industrycode + ",";
                                }




              systems = systems.TrimEnd(',');
              industrys = industrys.TrimEnd(','); 
            
            var products = new System.Data.DataTable("teste");
            products.Columns.Add("Id", typeof(int));
     
            products.Columns.Add("Company id", typeof(string));
            products.Columns.Add("System id", typeof(string));

            products.Columns.Add("Created Date", typeof(string));
            products.Columns.Add("Industry  id", typeof(string));
            products.Columns.Add("Admin  id", typeof(string));
            products.Rows.Add(1, model.Company.CompanyId,systems, date,industrys, model.Company.userModel.Name);
            var grid = new GridView();
            grid.DataSource = products;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + model.Company.CompanyId+ ".xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();


            return View();
        }

        
        public ActionResult DeleteCompany(int Companyid, int? page)
        {

            if (Session["MasterLoginStatus"] == "LoggedIn")
            {
                var result = masteradmin.DeleteCompany(Companyid);
                if (result == true)
                {
                    DataStatus = "Company Deleted Successfully";
                }
                return RedirectToAction("CreateCompany", new { page });
            }
            else
            {
                return RedirectToAction("Login");
            }


        }


        public ActionResult EditCompany(int Companyid)
        {
            if (Session["MasterLoginStatus"] == "LoggedIn")
            {
                var ob = masteradmin.GetCompanyByID(Companyid);
                foreach (var item in ob)
                {
                    companymodel.CompanyName = item.CompanyName;
                    companymodel.Description = item.Description;
                    companymodel.Remarks = item.Remarks;
                }
                List<User> userdetail = new List<User>();
                userdetail = masteradmin.GetUsers();
                ViewBag.userlist = userdetail;
                var list = masteradmin.getcompanydetails();
                ViewBag.company = list;
                return View("CreateCompany", companymodel);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [WebMethod]
        public JsonResult CreateCompaney(string CompaneyId, string CompneyName, string Description, string Remarks)
        {
            if (Session["MasterLoginStatus"] == "LoggedIn")
            {
                int result = 0;
                string compid = CompaneyUpdatedStatus;
                CompaneyUpdatedStatus = "";
                if (compid != "")
                {
                    result = masteradmin.UpdateCompany(CompaneyId, CompneyName, compid, Description, Remarks);
                    DataStatus = "Company details updated successfully";
                }
                else
                {
                    if (masteradmin.Check_duplicateCompanyID(CompaneyId) == false)
                    {
                        DataStatus = "Duplicate Company ID is not Allowed";
                        return Json(DataStatus, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        string dbResult = CreateDBScript("Gst"+CompaneyId.Replace(" ", String.Empty), DbDetails);
                        if (dbResult != "Success")
                        {
                            return Json("Error:" + dbResult, JsonRequestBehavior.AllowGet);

                        }

                        if (ModelState.IsValid)
                        {
                            {
                                string Exception = "";
                                result = masteradmin.CreateCompany(CompaneyId, CompneyName, Description, Remarks, out Exception);
                                if (result == 0)
                                {
                                    return Json(Exception, JsonRequestBehavior.AllowGet);
                                }
                                DataStatus = "Company details inserted successfully";
                            }
                        }
                        else
                        {
                            return Json("Model State Is Not Valid", JsonRequestBehavior.AllowGet);
                        }
                    }

                    var list = masteradmin.getcompanydetails();
                    ViewBag.company = list;

                    Session["sessionString"] = result;
                }
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json("Please Login First", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult chkDuptSysid(string systemid)
        {

            if (masteradmin.Check_duplicateSystemID(systemid) == false)
            {

                DataStatus = "Duplicate System ID is not Allowed";
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Pass", JsonRequestBehavior.AllowGet);
            }


        }
        public JsonResult chkDuptindustryid(string systemid)
        {

            if (masteradmin.Check_duplicateIndustryid(systemid) == false)
            {

                DataStatus = "Duplicate System ID is not Allowed";
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Pass", JsonRequestBehavior.AllowGet);
            }


        }

        public JsonResult DeleteSystems(string CompanyId)
        
        {
            string result = masteradmin.DeleteSystems(CompanyId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteIndustry(string CompanyId)
        {
            string result = masteradmin.DeleteIndustry(CompanyId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [WebMethod]
        public JsonResult SaveSystem(string ID, string SystemId, string SystemName)
        {
            int Returnintvalue = 0;



            if (ModelState.IsValid)
            {


                Returnintvalue = masteradmin.CreateSystem(ID, SystemId, SystemName);
                var list = masteradmin.getcompanydetails();
                ViewBag.company = list;
                Session["sessionString"] = Returnintvalue;
            }
           
            return Json(Returnintvalue, JsonRequestBehavior.AllowGet);
        }

        [WebMethod]
        public JsonResult SaveIndustryCode(string ID, string Industrycode)
        {
            int Returnintvalue = 0;
           
            if (ModelState.IsValid && masteradmin.Check_duplicateIndustryCode(Industrycode) == true)
                {


                    Returnintvalue = masteradmin.CreateIndustry(ID, Industrycode);
                    var list = masteradmin.getcompanydetails();
                    ViewBag.company = list;
                    Session["sessionString"] = Returnintvalue;
                }
          

            return Json(Returnintvalue, JsonRequestBehavior.AllowGet);
        }


        [WebMethod]
        public JsonResult EditCompaney(string CompanyId)
        {
            if (Session["MasterLoginStatus"] == "LoggedIn")
            {
                var model = masteradmin.Findcompany(CompanyId);              
                CompaneyUpdatedStatus = CompanyId;
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Please Login First", JsonRequestBehavior.AllowGet);
            }
        }
        public string CreateDBScript(string DbName, string DbDetails)
        {
            try
            {
            
                SqlConnection con = new SqlConnection(DbDetails);
                string strCreatecmd = "create database " + DbName + "";
                SqlCommand cmd = new SqlCommand(strCreatecmd, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                var file = new System.IO.FileInfo(System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["ScriptLocation"]));
                string strscript = file.OpenText().ReadToEnd();
                string strupdatescript = strscript.Replace("[GSTMART]", DbName);
                var server = new Microsoft.SqlServer.Management.Smo.Server(new Microsoft.SqlServer.Management.Common.ServerConnection(con));
                server.ConnectionContext.ExecuteNonQuery(strupdatescript);
                con.Close();
                return EnumClass.MessageFamily.Success.ToString();


            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return error;
            }

        }
        [WebMethod]
        public void Logout()
        {
            Session["MasterUserName"] = "";
            Session["MasterLoginStatus"] = "LoggedOut";
        }

        [WebMethod]
        public JsonResult CheckLoginStatus()
        {
            string logstatus = Session["MasterLoginStatus"].ToString();
            return Json(logstatus, JsonRequestBehavior.AllowGet);
        }

        [WebMethod]
        public JsonResult CreateAdmin(string AdminID, string AdminName, string Password, string Email, string Mobile)
        {
            if (Session["MasterLoginStatus"] == "LoggedIn")
            {
                var gstmart = new GSTMARTEntities();
                string result = "";
                var query = (from p in gstmart.Users where p.Usertype == "Admin" select p).SingleOrDefault();
                if (query != null)
                {
                    result = masteradmin.UpdateAdmin(AdminID, AdminName, Password, Email, Mobile, query.Id);
                    DataStatus = result;
                    if(DataStatus=="Admin details updated successfully")
                    {
                        Mail Mailmessage = new Mail();
                        string body = "Dear : " + AdminName + "<br/><br/>Your Updated Password is : " + Password + "";
                        Mailmessage.SendMail(Email,ms_email, smtp_port, Smtp_Password, Smtp_Host, "Password Change Mail",body);
                    }
                }

                else
                {
                    if (masteradmin.Check_duplicateAdminID(AdminID) == false)
                    {
                        DataStatus = "Duplicate Admin ID is not Allowed";
                        return Json(DataStatus, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {

                        if (ModelState.IsValid)
                        {
                            {
                                result = masteradmin.CreateAdmin(AdminID, AdminName, Password, Email, Mobile);
                                DataStatus = result;
                                if (DataStatus == "Admin details Inserted successfully")
                                {
                                    Mail Mailmessage = new Mail();
                                    string body = "Dear : " + AdminName + "<br/><br/>Your Updated Password is : " + Password + "";
                                    Mailmessage.SendMail(Email,ms_email, smtp_port, Smtp_Password, Smtp_Host, "Your Password is",body);
                                }
                            }
                        }
                    }
                    var list = masteradmin.getcompanydetails();
                    ViewBag.company = list;

                    //Session["sessionString"] = result;
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Please Login First", JsonRequestBehavior.AllowGet);
            }
        }

        //For DublicateEmail
        public JsonResult SearchDublicateEmail(string Email)
        {
            var result = masteradmin.SearchDublicateEmail(Email);
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        //public  string Alphanumrical(int length, Random random)
        //{
        //    var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        //    var result = new string(
        //        Enumerable.Repeat(chars, length)
        //                  .Select(s => s[random.Next(s.Length)])
        //                  .ToArray());

        //    return result;
        //}


    


    }
}
