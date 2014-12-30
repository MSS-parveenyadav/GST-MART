using GST_BLL.AdminUser;
using GST_BLL.SendMail;
using GST_BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.Security;
using System.Net.Mail;
using System.Configuration;
using GST_DB;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using PagedList;
using PagedList.Mvc;
using GST_BLL.Factory;
using GST_BLL.Enum;
using GST_BLL.LdapAuthentication;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using GST_BLL.AuthorizedUser;

namespace GST_Mart.Controllers
{

    public class AdminController : Controller
    {
        string originalConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["GSTMARTEntities"].ConnectionString;
        AdminUser adminuser = new AdminUser();
        AuthorizedUser user = new AuthorizedUser();
        AdminUserModel adminusermodel = new AdminUserModel();
        Mail MailMessage = new Mail();
        string ms_email = ConfigurationManager.AppSettings["master_email"].ToString();
        string smtp_port = ConfigurationManager.AppSettings["SMTP_PORT"].ToString();
        string Smtp_Password = ConfigurationManager.AppSettings["Smtp_Password"].ToString();
        string Smtp_Host = ConfigurationManager.AppSettings["SMTP_HOST"].ToString();
        string URL = ConfigurationManager.AppSettings["URL"].ToString();
        Random random = new Random();


        DataSchedular schedularObj = new DataSchedular();
        public ActionResult Login()
        {
            List<Companylist> ob = new List<Companylist>();
            IPAddress IP = adminuser.getipaddress();
            TempData["IP"] = IP;
            ob = adminuser.GetCompanyList();
            ViewBag.Company = ob;

            return View();
        }

        [HttpPost]
        public ActionResult Login(AdminUserModel model, string companylist)
        {
            if(companylist=="")
            {
                TempData["Loginerror"] = "Please Select a Company.";
                return View();
            }
            List<Companylist> ob = new List<Companylist>();
            ob = adminuser.GetCompanyList();
            ViewBag.Company = ob;
            IPAddress IP = adminuser.getipaddress();
            TempData["IP"] = IP;
       
            Session["CompanyDB"] ="Gst"+ companylist.Replace(" ", string.Empty);
            Session["Companey"] = companylist;


            

            var testldapuser = adminuser.CheckLDAPLogin(companylist, model.AdminID, model.Password);
            if (testldapuser == true)
            {
                var list = adminuser.GetLDAPUSERBYUserID(model.AdminID).ToList();

                foreach (var item in list)
                {                       
                    try
                    {
                        //public static string adPath = "LDAP://SLINFO.COM/OU=ADVANCE_APP,DC=SLINFO,DC=COM"; 


                       string adPath = "LDAP://" + item.Domainname + "/OU=" + item.ComapanyId + ",DC=SLINFO,DC=COM"; 
                       LdapAuthentication adAuth = new LdapAuthentication(adPath);


                        if (true == adAuth.IsAuthenticated(item.Domainname,
                                                          item.UserId,
                                                          item.password))
                        {
                            // Retrieve the user's groups
                            string groups = adAuth.GetGroups();
                            // Create the authetication ticket
                            FormsAuthenticationTicket authTicket =
                                new FormsAuthenticationTicket(1,  // version
                                                               item.UserId,
                                                              DateTime.Now,
                                                              DateTime.Now.AddMinutes(60),
                                                              false, groups);
                            // Now encrypt the ticket.
                            string encryptedTicket =
                              FormsAuthentication.Encrypt(authTicket);
                            // Create a cookie and add the encrypted ticket to the
                            // cookie as data.
                            HttpCookie authCookie =
                                         new HttpCookie(FormsAuthentication.FormsCookieName,
                                                        encryptedTicket);
                            // Add the cookie to the outgoing cookies collection.
                            Response.Cookies.Add(authCookie);

                            //// Redirect the user to the originally requested page
                            //Response.Redirect(
                            //          FormsAuthentication.GetRedirectUrl(item.UserId,
                            //                                             false));
                            string Username = adminuser.GetAdminName(item.UserId);
                            Session["AdminUserName"] = Username;
                            Session["LoginTime"] = DateTime.Now.ToString("dd MMM yyyy,  HH:MM tt");
                            return RedirectToAction("CreateUser");
                        }
                        else
                        {
                            TempData["Loginerror"] = "You have entered wrong credential.";
                            //  lblError.Text = "Authentication failed, check username and password.";
                            return RedirectToAction("Login");
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["Loginerror"] = "You have entered wrong credential.";
                        // lblError.Text = "Error authenticating. " + ex.Message;
                        return RedirectToAction("Login");
                    }
                }
                return RedirectToAction("CreateUser");
            }
            else
            {
                var result = adminuser.CheckLogin(model);
                if (result == true)
                {
                    string Username = adminuser.GetAdminName(model.AdminID);
                    Session["AdminUserName"] = Username;
                    Session["LoginTime"] = DateTime.Now.ToString("dd MMM yyyy,  HH:MM tt");
                    //FormsAuthentication.SetAuthCookie(model.AdminID, false);
                    Session["AdminLoginStatus"] = "LoggedIn";
                    return RedirectToAction("CreateUser");
                }
                else
                {
                    string emailto = adminuser.GetemailbyUserId(model.AdminID);
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
                            TempData["Loginerror"] = ex.Message;
                            return RedirectToAction("Login");
                        }
                    }
                    TempData["Loginerror"] = "You have entered wrong credential.";
                    return RedirectToAction("Login");
                }
            }
        }

        public ActionResult RecoverPassword()
        {
           return View();
           
        }

        public ActionResult INvalidLInk()
        {
            return View();

        }
        [HttpPost]
        public ActionResult RecoverPassword(FormCollection Form)
       {



           string  uniq = Guid.NewGuid().ToString();
           
          
            string Email = Form["Email"];
            string ReturnMessage = adminuser.FindAdminUser(Email);
            if (ReturnMessage != "")
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(ReturnMessage);
                mail.From = new MailAddress(ms_email);
                mail.Subject = "Reset Password Mail";
                string Link = adminuser.GenerateLink(URL, ReturnMessage,uniq);
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
            AdminUserModel UserData = adminuser.FindUserByEmail(Email, Guid,out message);
            if (message == EnumClass.MessageFamily.Error.ToString())
            {
              return  RedirectToAction("INvalidLInk");
            }
            return View(UserData);
        }

        [HttpPost]
        public ActionResult ChangePassword(AdminUserModel Data)
        {
            if (Data.Password == null || Data.ConfirmPassword == null)
            {
                TempData["ErrorMessage"] = "Fields Required";
                return View();
            }
            else if (Data.Password != Data.ConfirmPassword)
            {
                TempData["ErrorMessage"] = "Password and Confirm Password does not match";
                return View();
            }
            string Email = "";
            string Username = "";
            Int32 Returnvalue = adminuser.UpdatePassword(Data,out Email,out Username);
            if (Returnvalue == 2)
            {
                TempData["ErrorMessage"] = "User Does not exist now";
                return View();
            }
            if (Returnvalue == 5)
            {
                TempData["ErrorMessage"] = "Invalid Link ";
                return View();
            }
            else
            {
                if(Email!="")
                {
                    string body="Dear : " + Username + "<br/><br/>Your Updated Password is : "+Data.Password+"";

                    MailMessage.SendMail(Email, ms_email, smtp_port, Smtp_Password, Smtp_Host, "Your New Password is :",body);
                }
            }
            return RedirectToAction("Login");
        }

        public ActionResult CreateUser(int? page, int pagesize=0, string DDlPageingsize = "5")
        {
            if (Session["AdminLoginStatus"] == "LoggedIn")
            {
                var gstmart = new GSTMARTEntities();

                List<SelectListItem> CompaniesList = new List<SelectListItem>();

                foreach (Company company in gstmart.Companies)
                {
                    SelectListItem selectList = new SelectListItem()
                    {
                        Text = company.CompanyID,
                        Value = company.CompanyID,
                        Selected = false
                    };
                    CompaniesList.Add(selectList);
                }

                AdminUserModel User = new AdminUserModel()
                {
                    listcompaney = CompaniesList
                };

                if (pagesize == 0)
                {
                    pagesize = Convert.ToInt32(DDlPageingsize);
                }
                var companyid = Session["Companey"].ToString();
                var list = adminuser.GetUserPermissionList(originalConnectionString, Session["CompanyDB"].ToString(),companyid);
                var pageNumber = page ?? 1;
                TempData["pagesize"] = pagesize;
                var List = list.ToPagedList(pageNumber, pagesize);
                ViewBag.Userpermission = List;

                return View(User);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        [HttpPost]
        public ActionResult CreateUser(AdminUserModel model, FormCollection Form, string Createcycle, string Accesssetting, string Downloaddata, string Printreport, IEnumerable<string> Companies, int? page)
        {
            if (Session["AdminLoginStatus"] == "LoggedIn")
            {
                if (Companies == null)
                {
                    TempData["ErrorMessage"] = "Please Select Companies.";
                    return RedirectToAction("CreateUser");
                }

                int id = model.Id;
                if (id != null && id != 0)
                {
                    adminusermodel.UserId = model.UserId;
                    adminusermodel.Usename = model.Usename;
                    adminusermodel.Password = model.Password;
                    adminusermodel.Email = model.Email;
                    adminusermodel.MobileNumber = model.MobileNumber;
                    adminuser.UpdateAdminUser(adminusermodel, Createcycle, Accesssetting, Downloaddata, Printreport, id, Companies, originalConnectionString, Session["CompanyDB"].ToString());
                    TempData["SuccessMessage"] = "User Successfully Updated";
                    string body = "Dear : " + model.Usename + "<br/><br/>Your Updated Password is : " + model.Password + "";
                    string Result= MailMessage.SendMail(model.Email,ms_email, smtp_port, Smtp_Password, Smtp_Host,"Update Password Mail",body);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   

                }

                else
                {
                    adminusermodel.UserId = model.UserId;
                    adminusermodel.Usename = model.Usename;
                    adminusermodel.Password = model.Password;
                    adminusermodel.Email = model.Email;
                    adminusermodel.MobileNumber = model.MobileNumber;
                    if (adminuser.Check_duplicateUserId(adminusermodel.UserId) == true)
                    {
                        adminuser.InsertAdminUser(adminusermodel, Createcycle, Accesssetting, Downloaddata, Printreport, Companies);
                        TempData["SuccessMessage"] = "User Successfully Inserted";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "User Already Exist";
                    }
                }
                var companyid = Session["Companey"].ToString();
                var list = adminuser.GetUserPermissionList(originalConnectionString, Session["CompanyDB"].ToString(),companyid);
                var pageNumber = page ?? 1;
                var List = list.ToPagedList(pageNumber, 1);
                ViewBag.Userpermission = List;
                return RedirectToAction("CreateUser", pageNumber);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult DeleteUser(int id)
        {
            if (Session["AdminLoginStatus"] == "LoggedIn")
            {
                var result = adminuser.DeleteUser(id);
                if (result == true)
                {
                    return RedirectToAction("CreateUser");
                }
                return RedirectToAction("CreateUser");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult SuspendUser(int id)
        {
            var res = adminuser.UpdateUser(id);
            if(res==true)
            {
                return RedirectToAction("Createuser");
                        
            }
            else
            {
                return RedirectToAction("Createuser");
            }
           
        }

        public ActionResult EditUser(int id, string pagesize, int? page)
        {
            if(pagesize==null)
            {
                pagesize = "5";
            }
            if (Session["AdminLoginStatus"] == "LoggedIn")
            {
                var ob = adminuser.GetUserById(id);
                if (ob != null)
                {
                    adminusermodel.UserId = ob.UserId;
                    adminusermodel.Usename = ob.Usename;
                    adminusermodel.Email = ob.Email;
                    adminusermodel.ConfirmEmail = ob.ConfirmEmail;
                    adminusermodel.Password = ob.Password;
                    adminusermodel.ConfirmPassword = ob.ConfirmPassword;
                    adminusermodel.MobileNumber = ob.ConfirmMobileNumber;
                    adminusermodel.ConfirmMobileNumber = ob.ConfirmMobileNumber;
                    adminusermodel.CompanyId = ob.CompanyId;
                    adminusermodel.listcompaney = ob.listcompaney;
                    adminusermodel.Createcycle = ob.Createcycle;
                    adminusermodel.AccessSetting = ob.AccessSetting;
                    adminusermodel.DownloadData = ob.DownloadData;
                    adminusermodel.PrintReport = ob.PrintReport;
                    ViewBag.Create = ob.Createcycle;
                }
                var companyid = Session["Companey"].ToString();
                var list = adminuser.GetUserPermissionList(originalConnectionString, Session["CompanyDB"].ToString(),companyid);
                var pageNumber = page ?? 1;
                TempData["pagesize"] = pagesize;
                int size = Convert.ToInt32(pagesize);
                var List = list.ToPagedList(pageNumber, size);
                ViewBag.Userpermission = List;
                return View("CreateUser", adminusermodel);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult IpAddress(int ID = 0)
        {
            if (Session["AdminLoginStatus"] == "LoggedIn")
            {
                if (ID == 0)
                {
                    var List = BindIpAddressList();
                    ViewBag.IpAddressList = List;
                    return View();
                }
                else
                {
                    IpAddressModel model = new IpAddressModel();
                    model = adminuser.FindIpAddressById(ID);
                    var List = BindIpAddressList();
                    ViewBag.IpAddressList = List;
                    return View(model);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        private List<GST_BLL.DTO.IpAddressModel> BindIpAddressList()
        {
            IPAddress IP = adminuser.getipaddress();
            TempData["Ipaddress"] = IP;
            var List = adminuser.GetIpAddressList();
            return List;
        }
        [HttpPost]
        public ActionResult IpAddress(IpAddressModel Data)
        {
            if (Session["AdminLoginStatus"] == "LoggedIn")
            {
                if (Data.IP == null)
                {
                    TempData["Error"] = "Ip Address Required";
                    return RedirectToAction("IpAddress");
                }
                if (Data.Id != 0)
                {
                    int Returnvalue =adminuser.EditIpAddress(Data);
                    TempData["Status"] = "Data Updated";
                }
                else
                {
                    string IpaddressExist = adminuser.FindIpAddressByIp(Data);
                    if (IpaddressExist == "Exist")
                    {
                        TempData["Error"] = "This Ip Address already exist in database";
                        return RedirectToAction("IpAddress");
                    }
                    Int32 returnintvalue = adminuser.SaveIpAddress(Data);
                    TempData["Status"] = "Data Saved";
                }
                return RedirectToAction("IpAddress");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult DeleteIpAddress(int ID)
        {
            if (Session["AdminLoginStatus"] == "LoggedIn")
            {
                Int32 returnintvalue = adminuser.DeleteIp(ID);
                return RedirectToAction("IpAddress");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [WebMethod]
        public void Logout()
        {
            Session["AdminUserName"]="";
            Session["LoginTime"] = "";
            Session["AdminLoginStatus"] = "LoggedOut";
        }

        public ActionResult AdminPath()
        {
            if (Session["AdminLoginStatus"] == "LoggedIn")
            {
                var adminpath = adminuser.GetAdminPath();
                TempData["AdminPath"] = adminpath.AdminPath;
                return View(adminpath);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult AdminPath(AdminPathModel Data)
        {
            if (Session["AdminLoginStatus"] == "LoggedIn")
            {
                if (Data.AdminPath == null)
                {
                    TempData["Error"] = "Path Required";
                    return RedirectToAction("AdminPath");
                }
                Int32 returnintvalue = adminuser.UpdateAdminPath(Data);
                if (returnintvalue == 1)
                {
                    TempData["Status"] = "Path Updated";
                }
                else
                {
                    TempData["Error"] = "Some Error Occured Try Again";
                }
                return RedirectToAction("AdminPath");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public List<SelectListItem> GetCompanyList()
        {
            var gstmart=new GSTMARTEntities();
            List<SelectListItem> List = new List<SelectListItem>();
            var compneylist = (from p in gstmart.Companies select p).ToList();
            foreach (var item in compneylist)
            {
                List.Add(new SelectListItem { Value = item.CompanyID, Text = item.CompanyID });
            }

            return List;
        }

        #region Schedular

        [HttpGet]
        public ActionResult SaveSchedualr(int id=0)
        {

            GetDDlist();
            DataSchedular DataObj = new DataSchedular();
            var List = DataObj.GetSchedular(id, Session["Companey"].ToString());
            ViewBag.SchedularList = List;
            if (id == 0)
            {
                TempData["id"] = 0;
                TempData["Companey"] =0;
                TempData["Ledger"] = 0;
                TempData["Supply"] = 0;
                TempData["Footer"] = 0;
                TempData["Purchase"] =0;
                return View();
            }
            else
            {
                TempData["id"] = id;
                SchedularModel result = DataObj.FindSchedularById(id, Session["Companey"].ToString());
                TempData["id"] = id;
                TempData["Companey"] = result.Companyid;
                TempData["Ledger"] = result.Ledgerid;
                TempData["Supply"] = result.Supplyid;
                TempData["Footer"] = result.Footerid;
                TempData["Purchase"] = result.Purchaseid;
                return View(result);
            }
        
        }

        public ActionResult EditSchedularStatus(int id = 0)
        {
            if(id!=0)
            {
                DataSchedular DataObj = new DataSchedular();
                int result = DataObj.EditSchedularStatus(id);
            }
            return RedirectToAction("SaveSchedualr");
        }

        [HttpPost]
        public ActionResult SaveSchedualr(SchedularModel model)
        {
            model.CmpnyID = Session["Companey"].ToString();
            if (model.Companyid != 0 && model.Footerid != 0)
            {
              
                schedularObj.UpdateSchedular(model);
                TempData["MessageCompany"] = "Company Schedular Updated";
                return RedirectToAction("SaveSchedualr", new { id=0});
            }

            var checkExistingCompany = schedularObj.CheckExtingSchedular(GST_BLL.Enum.EnumClass.ImportFamily.Company.ToString(), model.CmpnyID);

            if (checkExistingCompany == null)
           {

               var responseCompany = schedularObj.CreateCompnaySchedular(model);
               if (responseCompany != EnumClass.MessageFamily.Success.ToString())
               {
                   TempData["MessageCompany"] = responseCompany;
               }
                else
               {
                   TempData["MessageCompany"] = "Company Schedular Created";

               }
            }
            else
            {

           
               TempData["MessagePurchase"] = "Company Schedulars is allready Added";
           
           
            }

            var checkExistingLedger = schedularObj.CheckExtingSchedular(GST_BLL.Enum.EnumClass.ImportFamily.Ledger.ToString(), model.CmpnyID);
            if (checkExistingLedger == null)
            {

                var responseLedger = schedularObj.CreateLedgerSchedular(model);
                if (responseLedger != EnumClass.MessageFamily.Success.ToString())
                {
                    TempData["MessageLedger"] = responseLedger;
                }
                else
                {
                    TempData["MessageLedger"] = "Ledger Schedulars created";
                }
            }
            else
            {
                TempData["MessageLedger"] = "Ledger Schedulars is allready Added";
            }



            var checkExistingSupply = schedularObj.CheckExtingSchedular(GST_BLL.Enum.EnumClass.ImportFamily.Supply.ToString(), model.CmpnyID);
             if (checkExistingSupply == null)
             {
                 var responseSupply = schedularObj.CreateSupplySchedular(model);
                 if (responseSupply != EnumClass.MessageFamily.Success.ToString())
                 {
                     TempData["MessageSupply"] = responseSupply;
                 }
                 else
                 {
                     TempData["MessageSupply"] = "Supply Schedulars created";
                 }
             }
            else
             {
                 TempData["MessageSupply"] = "Supply Schedulars is allready Added";
             }


             var checkExistingFooter = schedularObj.CheckExtingSchedular(GST_BLL.Enum.EnumClass.ImportFamily.Footer.ToString(), model.CmpnyID);
             if (checkExistingFooter == null)
             {
                 var responseFooter = schedularObj.CreateFooterSchedular(model);
                 if (responseFooter != EnumClass.MessageFamily.Success.ToString())
                 {
                     TempData["MessageFooter"] = responseFooter;
                 }
                 else
                 {
                     TempData["MessageFooter"] = "Footer Schedular Created";
                 }
             }
            else
             {

                 TempData["MessageSupply"] = "Footer Schedulars is allready Added";
             }

             var checkExistingPurchase = schedularObj.CheckExtingSchedular(GST_BLL.Enum.EnumClass.ImportFamily.Purchase.ToString(), model.CmpnyID);
             if (checkExistingFooter == null)
             {
                 var responsePurchase = schedularObj.CreatePurchaseSchedular(model);
                 if (responsePurchase != EnumClass.MessageFamily.Success.ToString())
                 {
                     TempData["MessagePurchase"] = responsePurchase;
                 }
                 else
                 {
                     TempData["MessagePurchase"] = "Purchase Schedular Created";

                 }
             }
             else
             {

                 TempData["MessagePurchase"] = "Purchase Schedulars is allready Added";
             }
      
          
               GetDDlist();

               return RedirectToAction("SaveSchedualr", new {id=0});

        }

        #region Dynamic Dropdown bynding for Schedular
        public void GetDDlist()
        {
            var CompanyList = new List<SchedularModel>();
            var LedgerList = new List<SchedularModel>();
            var FooterList = new List<SchedularModel>();
            var SupplyList = new List<SchedularModel>();
            var PurchaseList = new List<SchedularModel>();


            var CompanyUnitList = new List<SchedularModel>();
            var LedgerUnitList = new List<SchedularModel>();
            var FooterUnitList = new List<SchedularModel>();
            var SupplyUnitList = new List<SchedularModel>();
            var PurchaseUnitList = new List<SchedularModel>();

            var CompanyTimeList = new List<SchedularModel>();
            var LedgerTimeList = new List<SchedularModel>();
            var FooterTimeList = new List<SchedularModel>();
            var SupplyTimeList = new List<SchedularModel>();
            var PurchaseTimeList = new List<SchedularModel>();

            for (int i = 1; i <= 12; i++)
            {
                var model = new SchedularModel();
                model.CompanyFrequency = i.ToString();
                CompanyList.Add(model);

                model.LedgerFrequency = i.ToString();
                LedgerList.Add(model);

                model.FooterFrequency = i.ToString();
                FooterList.Add(model);

                model.SupplyFrequency = i.ToString();
                SupplyList.Add(model);

                model.PurchaseFrequency = i.ToString();
                PurchaseList.Add(model);

            }

            ViewBag.CompanyListFrequency = CompanyList;
            ViewBag.LedgerListFrequency = LedgerList;
            ViewBag.FooterListFrequency = FooterList;
            ViewBag.SupplyListFrequency = SupplyList;
            ViewBag.PurchaseListFrequency = PurchaseList;


            for (int i = 1; i <= 24; i++)
            {
                var model = new SchedularModel();

                model.CompanyTime = i.ToString();
                CompanyTimeList.Add(model);

                model.LedgerTime = i.ToString();
                LedgerTimeList.Add(model);


                model.PurchaseTime = i.ToString();
                PurchaseTimeList.Add(model);


                model.SupplyTime = i.ToString();
                SupplyTimeList.Add(model);

                model.FooterTime = i.ToString();
                FooterTimeList.Add(model);


            }


            ViewBag.CompanyTimeFrequency = CompanyTimeList;
            ViewBag.LedgerTimeFrequency = LedgerTimeList;
            ViewBag.FooterTimeFrequency = FooterTimeList;
            ViewBag.SupplyTimeFrequency = SupplyTimeList;
            ViewBag.PurchaseTimeFrequency = PurchaseTimeList;


            // Company Unit List
            var Unit = new SchedularModel();
            Unit.CompanyUnit = "HOURS";
            CompanyUnitList.Add(Unit);
            Unit = new SchedularModel();
            Unit.CompanyUnit = "DAYS";
            CompanyUnitList.Add(Unit);
            Unit = new SchedularModel();
            Unit.CompanyUnit = "WEEK";
            CompanyUnitList.Add(Unit);
            Unit = new SchedularModel();
            Unit.CompanyUnit = "MONTH";
            CompanyUnitList.Add(Unit);

            // Supply Unit List
            Unit = new SchedularModel();
            Unit.SupplyUnit = "HOURS";
            SupplyUnitList.Add(Unit);
            Unit = new SchedularModel();
            Unit.SupplyUnit = "DAYS";
            SupplyUnitList.Add(Unit);
            Unit = new SchedularModel();
            Unit.SupplyUnit = "WEEK";
            SupplyUnitList.Add(Unit);
            Unit = new SchedularModel();
            Unit.SupplyUnit = "MONTH";
            SupplyUnitList.Add(Unit);

            // Footer Unit List
            Unit = new SchedularModel();
            Unit.FooterUnit = "HOURS";
            FooterUnitList.Add(Unit);
            Unit = new SchedularModel();
            Unit.FooterUnit = "DAYS";
            FooterUnitList.Add(Unit);
            Unit = new SchedularModel();
            Unit.FooterUnit = "WEEK";
            FooterUnitList.Add(Unit);
            Unit.FooterUnit = "MONTH";
            FooterUnitList.Add(Unit);



            // Ledger Unit List
            Unit = new SchedularModel();
            Unit.LedgerUnit = "HOURS";
            LedgerUnitList.Add(Unit);
            Unit = new SchedularModel();
            Unit.LedgerUnit = "DAYS";
            LedgerUnitList.Add(Unit);
            Unit = new SchedularModel();
            Unit.LedgerUnit = "WEEK";
            LedgerUnitList.Add(Unit);
            Unit = new SchedularModel();
            Unit.LedgerUnit = "MONTH";
            LedgerUnitList.Add(Unit);


            // Purchase Unit List
            Unit = new SchedularModel();
            Unit.PurchaseUnit = "HOURS";
            PurchaseUnitList.Add(Unit);
            Unit = new SchedularModel();
            Unit.PurchaseUnit = "DAYS";
            PurchaseUnitList.Add(Unit);
            Unit = new SchedularModel();
            Unit.PurchaseUnit = "WEEK";
            PurchaseUnitList.Add(Unit);
            Unit = new SchedularModel();
            Unit.PurchaseUnit = "MONTH";
            PurchaseUnitList.Add(Unit);


            ViewBag.CompanyUnitListFrequency = CompanyUnitList;
            ViewBag.LedgerUnitListFrequency = LedgerUnitList;
            ViewBag.FooterUnitListFrequency = FooterUnitList;
            ViewBag.SupplyUnitListFrequency = SupplyUnitList;
            ViewBag.PurchaseUnitListFrequency = PurchaseUnitList;




        }
        #endregion

        #endregion

        #region LDAP USER
        public ActionResult CreateLDAPUser()
        {
            if (Session["AdminLoginStatus"] == "LoggedIn")
            {
                var list = adminuser.GetLDAPUserList();
                ViewBag.LDAPUSERS = list;
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public ActionResult CreateLDAPUser(LdapUserModel model)
        {
            if (Session["AdminLoginStatus"] == "LoggedIn")
            {
                LdapUserModel ldapuser = new LdapUserModel();
                int id = model.Id;
                if (id != null && id != 0)
                {

                    var res = adminuser.UpdateLDAPUser(model);
                 if (res == true)
                 {
                     TempData["Sucess"] = "User Updated Successfully";
                     return RedirectToAction("CreateLDAPUser");
                 }
                    else
                 {
                     TempData["Error"] = "Updation Fails.";
                     return RedirectToAction("CreateLDAPUser");
                 }

                }
                else
                {

                    var res = adminuser.CreateLdapUser(model);
                   if (res == true)
                   {
                       TempData["Sucess"] = "User Created Successfully";
                       return RedirectToAction("CreateLDAPUser");
                   }
                   else
                   {
                       TempData["Error"] = "Creation Fails";
                       return RedirectToAction("CreateLDAPUser");
                   }
                }
            }

            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult EditLDAPUser(int id)
        {
            if (Session["AdminLoginStatus"] == "LoggedIn")
            {
                var list = adminuser.GetLDAPUSERBYID(id);
                LdapUserModel model = new LdapUserModel();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        model.Id = item.Id;
                        model.ComapanyId = item.ComapanyId;
                        model.Domainname = item.Domainname;
                        model.password = item.password;
                        model.Portnumber = item.Portnumber;
                        model.UserId = item.UserId;
                        model.cnbn = item.cnbn;
                    }
                }
                var ldapuserlist = adminuser.GetLDAPUserList();
                ViewBag.LDAPUSERS = ldapuserlist;
                return View("CreateLDAPUser", model);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult DeleteLDAPUser(int id)
        {
            if (Session["AdminLoginStatus"] == "LoggedIn")
            {
                var result = adminuser.DeleteLDAPUser(id);
                if (result == true)
                {
                    TempData["Sucess"] = "User Deleted Successfully";
                    return RedirectToAction("CreateLDAPUser");
                }
                else
                {
                    TempData["Error"] = "Deletion Fails";
                    return RedirectToAction("CreateLDAPUser");
                }
                
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        #endregion


        #region Taxcode
        public ActionResult TAXCODE()
        {
            if (Session["AdminLoginStatus"] == "LoggedIn")
            {
                List<SelectListItem> comapnymodel = new List<SelectListItem>();
                List<SelectListItem> transtypemodel = new List<SelectListItem>();
                List<SelectListItem> customcodemodel = new List<SelectListItem>();
                List<SelectListItem> DateFormet = new List<SelectListItem>();

                DateFormet = adminuser.getdateformetlist();
                ViewBag.DateFormet = DateFormet;

                comapnymodel = adminuser.GetCompanyListForDDl();
                ViewBag.companylist = comapnymodel;

                transtypemodel = adminuser.GetTransactionlist();
                ViewBag.transactionlist = transtypemodel;

                customcodemodel = adminuser.GetCustomcodelist();
                ViewBag.customcodelist = customcodemodel;

                var txcodemodel = adminuser.gettaxmodellist();
                ViewBag.taxcodemodel = txcodemodel;

                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public ActionResult TAXCODE(TaxCodeModel model, string systemlist)
        {
            if (Session["AdminLoginStatus"] == "LoggedIn")
            {
                if (model.Id != 0)
                {
                    var res = adminuser.UpdateTaxcode(model, systemlist);

                    if (res == true)
                    {
                        TempData["Sucess"] = "Tax Code Update Successfully.";
                        return RedirectToAction("TAXCODE");
                    }
                    else
                    {
                        TempData["Error"] = "Updation Fail.";
                        return RedirectToAction("TAXCODE");
                    }
                }

                else
                {
                    var res = adminuser.CreateTaxcode(model, systemlist);
                    if (res == true)
                    {
                        TempData["Sucess"] = "Tax Code created Successfully.";
                        return RedirectToAction("TAXCODE");
                    }

                    else
                    {
                        TempData["Error"] = "Creation Fail.";
                        return RedirectToAction("TAXCODE");

                    }
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [WebMethod]
        public JsonResult Savecustomcode(string CustomCode)
        {

           var res= adminuser.CreateCustomcode(CustomCode);

           return Json(res, JsonRequestBehavior.AllowGet);
        }

        #endregion

        [WebMethod]
        public JsonResult SaveCurrencyType(string CurrencyType, string CurrencyCode)
        {
            var res = adminuser.CreateCurrencyType(CurrencyType, CurrencyCode);

           return Json(res, JsonRequestBehavior.AllowGet);
        }

        [WebMethod]
        public JsonResult SaveTransactiontype(string Transactiontype)
        {

            var res = adminuser.CreateTransactiontype(Transactiontype);
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        [WebMethod]
        public JsonResult GetStateId(string id)
        {
            var res = adminuser.Getsystembycompanyid(id);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        #region Currency
        [HttpPost]
        public ActionResult CurrencyExchange(CurrencyExchangeModel model)
        {
            if (Session["AdminLoginStatus"] == "LoggedIn")
            {
                string CompanyId = "Gst" + Session["Companey"].ToString();
                if (model.Id != 0)
                {
                    string returnvalue = adminuser.UpdateCurrencyExchange(model, CompanyId);
                    TempData["Message"] = returnvalue;
                    return RedirectToAction("CurrencyExchange", new { Id = 0 });

                }
                else
                {
                    string returnvalue = adminuser.InsertCurrencyExchange(model, CompanyId);
                    TempData["Message"] = returnvalue;
                    return RedirectToAction("CurrencyExchange", new { Id = 0 });
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        public ActionResult CurrencyExchange(int? page, int pagesize=0, string DDlPageingsize = "5", int id = 0)
        {
            if (Session["AdminLoginStatus"] == "LoggedIn")
            {
                List<SelectListItem> Time;
                List<SelectListItem> Frequency;
                DynamicBindingOfCurrencyExchange(out Time, out Frequency);
                ViewBag.FrequencyList = Frequency;
                ViewBag.TimeList = Time;

                //********Pageing*****//
                if (pagesize == 0)
                {
                    pagesize = Convert.ToInt32(DDlPageingsize);
                }
                var list = adminuser.GetCurrencyExchangeList();
                var pageNumber = page ?? 1;
                TempData["pagesize"] = pagesize;
                var List = list.ToPagedList(pageNumber, pagesize);
                ViewBag.CurrencyExchange = List;
                //********Pageing End*****//

                if (id == 0)
                {
                    return View();
                }
                else
                {
                    CurrencyExchangeModel CurrencyExchange = adminuser.FindCurrencyEzchangeByID(id);
                    return View(CurrencyExchange);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        


        private static void DynamicBindingOfCurrencyExchange(out List<SelectListItem> Time, out List<SelectListItem> Frequency)
        {
            Time = new List<SelectListItem>();
            for (int i = 1; i < 25; i++)
            {
                Time.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            Frequency = new List<SelectListItem>();
            for (int i = 1; i < 13; i++)
            {
                Frequency.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
        }



        public ActionResult DeleteCurrencyExchange(int Id)
        {
            if (Session["AdminLoginStatus"] == "LoggedIn")
            {
                string ReturnValue = adminuser.DeleteCurrencyExchange(Id);
                TempData["Message"] = ReturnValue;
                return RedirectToAction("CurrencyExchange", new { Id = 0 });
            }
            else
            {
                return RedirectToAction("Login");
            }
        }



        //**********Currency Group *******//
        public ActionResult CurrencyGroup(int? page, int pagesize = 0, string DDlPageingsize = "5", int id = 0)
        {
            if (Session["AdminLoginStatus"] == "LoggedIn")
            {
                //********Pageing*****//
                if (pagesize == 0)
                {
                    pagesize = Convert.ToInt32(DDlPageingsize);
                }
                var list = adminuser.GetCurrencyGroupList();
                var pageNumber = page ?? 1;
                TempData["pagesize"] = pagesize;
                var List = list.ToPagedList(pageNumber, pagesize);
                ViewBag.CurrencyGroup = List;
                //********Pageing End*****//


                var CurrencyList = adminuser.GetCurrencyList();
                ViewBag.Currencylist = CurrencyList;
                if (id == 0)
                {
                    return View();
                }
                else
                {
                    CurrencyGroupModel model = adminuser.FindCurrencfGroupById(id);
                    return View(model);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public ActionResult CurrencyGroup(CurrencyGroupModel model)
        {
            if (model.Id == 0)
            {
                string ReturnValue = adminuser.CreateCurrencyGroup(model);
                TempData["Message"] = ReturnValue;
                return RedirectToAction("CurrencyGroup", new { Id = 0 });
            }
            else
            {
                string ReturnValue = adminuser.UpdateCurrencyGroup(model);
                TempData["Message"] = ReturnValue;
                return RedirectToAction("CurrencyGroup", new { Id = 0 });
            }
        }

        public ActionResult DeleteCurrencyGroup(int Id)
        {
            if (Session["AdminLoginStatus"] == "LoggedIn")
            {
                string ReturnValue = adminuser.DeleteCurrencyGroup(Id);
                TempData["Message"] = ReturnValue;
                return RedirectToAction("CurrencyGroup", new { Id = 0 });
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        //**********End Currency Group *******//


        //******* Manual Currency Exchange *******//

        public ActionResult ManualCurrencyExchange(int? page, int pagesize = 0, string DDlPageingsize = "5", int id = 0)
        {
            if (Session["AdminLoginStatus"] == "LoggedIn")
            {
                var CurrencyList = adminuser.GetCurrencyCodelist();
                ViewBag.Currencylist = CurrencyList;


                //********Pageing*****//
                if (pagesize == 0)
                {
                    pagesize = Convert.ToInt32(DDlPageingsize);
                }
                var list = adminuser.GetCurrencyExchangeList();
                var pageNumber = page ?? 1;
                TempData["pagesize"] = pagesize;
                var List = list.ToPagedList(pageNumber, pagesize);
                ViewBag.CurrencyExchange = List;
                //********Pageing End*****//

                if (id == 0)
                {
                    return View();
                }
                else
                {
                    CurrencyExchangeModel CurrencyExchange = adminuser.FindCurrencyEzchangeByID(id);
                    return View(CurrencyExchange);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public ActionResult ManualCurrencyExchange(CurrencyExchangeModel model)
        {
            if (Session["AdminLoginStatus"] == "LoggedIn")
            {
                string CompanyId = "Gst" + Session["Companey"].ToString();
                if (model.Id != 0)
                {
                    string returnvalue = adminuser.UpdateManualCurrencyExchange(model, CompanyId);
                    TempData["Message"] = returnvalue;
                    return RedirectToAction("ManualCurrencyExchange", new { Id = 0 });

                }
                else
                {
                    string returnvalue = adminuser.InsertManualCurrencyExchange(model, CompanyId);
                    TempData["Message"] = returnvalue;
                    return RedirectToAction("ManualCurrencyExchange", new { Id = 0 });
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        public ActionResult DeleteManualCurrencyExchange(int Id)
        {
            if (Session["AdminLoginStatus"] == "LoggedIn")
            {
                string ReturnValue = adminuser.DeleteCurrencyExchange(Id);
                TempData["Message"] = ReturnValue;
                return RedirectToAction("ManualCurrencyExchange", new { Id = 0 });
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        [WebMethod]
        public JsonResult SaveCurrencyCode(string CurrencyType, string CurrencyCode)
        {

            var res = adminuser.CreateCurrencyCode(CurrencyType, CurrencyCode);

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        //******* End Manual Currency Exchange *******//

        #endregion
        public ActionResult DeleteTaxcode(int id)
        {
            if (Session["AdminLoginStatus"] == "LoggedIn")
            {

                var res = adminuser.DeleteTaxModel(id);
                if (res == true)
                {
                    TempData["Sucess"] = "Tax Code Delete Successfully.";
                    return RedirectToAction("TAXCODE");
                }
                else
                {
                    TempData["Error"] = "Deletion Fail.";
                    return RedirectToAction("TAXCODE");
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }




        //********Search User by userid or emailid*********//
        [WebMethod]
        public JsonResult SearchUserId(string id)
        {
            var res = adminuser.SearchUser(id);

            return Json(res, JsonRequestBehavior.AllowGet);
        }



        public ActionResult EditTAXCODE(int id)
        {
            if (Session["AdminLoginStatus"] == "LoggedIn")
            {
                var list = adminuser.GetTAXCODEBYID(id);
                TaxCodeModel model = new TaxCodeModel();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        model.Id = item.Id;
                        model.CompanyId = item.CompanyId;
                        model.SystemId = item.SystemId;
                        model.CustomCode = item.CustomCode;
                        model.OriginalCode = item.OriginalCode;
                        model.TransactionType = item.TransactionType;
                        model.TaxRate = item.TaxRate;
                        model.Description = item.Description;
                        model.RefernceNumber = item.RefernceNumber;
                        model.GST03Fields = item.GST03Fields;
                        model.Remarks = item.Remarks;
                        model.DateType = item.DateType;


                    }
                }

                List<SelectListItem> comapnymodel = new List<SelectListItem>();
                List<SelectListItem> transtypemodel = new List<SelectListItem>();
                List<SelectListItem> customcodemodel = new List<SelectListItem>();
                List<SelectListItem> DateFormet = new List<SelectListItem>();

                DateFormet = adminuser.getdateformetlist();
                ViewBag.DateFormet = DateFormet;

                comapnymodel = adminuser.GetCompanyListForDDl();
                ViewBag.companylist = comapnymodel;

                transtypemodel = adminuser.GetTransactionlist();
                ViewBag.transactionlist = transtypemodel;

                customcodemodel = adminuser.GetCustomcodelist();
                ViewBag.customcodelist = customcodemodel;

                var txcodemodel = adminuser.gettaxmodellist();
                ViewBag.taxcodemodel = txcodemodel;
                return View("Taxcode", model);

            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [WebMethod]
        public JsonResult Gettaxratebyoriginalcode(string id)
        {
            var res = adminuser.gettaxratebyoriginalcode(id);

            return Json(res, JsonRequestBehavior.AllowGet);


        }

           

        //****** Audit Log Start ********//

        public ActionResult AuditLog(int? page, int pagesize = 0, string DDlPageingsize = "5")
        {
            if (Session["AdminLoginStatus"] == "LoggedIn")
            {

                //********Pageing*****//
                if (pagesize == 0)
                {
                    pagesize = Convert.ToInt32(DDlPageingsize);
                }
                var list = adminuser.GetAuditLogList();
                var pageNumber = page ?? 1;
                TempData["pagesize"] = pagesize;
                var List = list.ToPagedList(pageNumber, pagesize);
                ViewBag.AuditLog = List;
                //********Pageing End*****//
                List<SelectListItem> CycleIdList = adminuser.CycleIDList(originalConnectionString, Session["CompanyDB"].ToString());
                ViewBag.CycleIdList = CycleIdList;

                List<Cycle> CycleList = adminuser.CycleList(originalConnectionString, Session["CompanyDB"].ToString());
                var ListofCycle = CycleList.ToPagedList(pageNumber, pagesize);
                ViewBag.CycleList = ListofCycle;

                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        public ActionResult DeleteAuditLog(int Id)
        {
            if (Session["AdminLoginStatus"] == "LoggedIn")
            {
                string ReturnValue = adminuser.DeleteAuditLog(Id);
                TempData["Message"] = ReturnValue;
                return RedirectToAction("AuditLog");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        public ActionResult ExportData(int Id)
        {
            AuditLogModel Audit = adminuser.FindAuditLogById(Id);


            var products = new System.Data.DataTable("teste");
            products.Columns.Add("S.No", typeof(int));
            products.Columns.Add("Created Date", typeof(string));
            products.Columns.Add("Message", typeof(string));
            products.Columns.Add("UserId", typeof(string));
            products.Columns.Add("Ip Address", typeof(string));
            products.Columns.Add("Name", typeof(string));


            products.Rows.Add(1, Audit.CreatedDate, Audit.Message, Audit.UserId, Audit.IpAddress, Audit.Name);


            var grid = new GridView();
            grid.DataSource = products;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=AuditLogFile.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            //for (int row = 0; row < grid.Rows.Count; row++)
            //{
            //    int colCount = dgv.Rows[row].Cells.Count;
            //    for (int col = 0; col < colCount; col++)
            //    {
            //        objWriter.WriteLine(dgv.Rows[row].Cells[col].Value.ToString());

            //    }
            //    // record seperator could be written here.
            //}

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();


            return RedirectToAction("AuditLog");


        }


        public ActionResult Configuration()
        {
            List<SelectListItem> DateFormat = new List<SelectListItem>();
            List<SelectListItem> TimeFormat = new List<SelectListItem>();

            DateFormat = adminuser.getdateformetlist();
            TimeFormat = adminuser.gettimeformetlist();
            ViewBag.DateFormat = DateFormat;
            ViewBag.TimeFormat = TimeFormat;
            return View();
        }

        [HttpPost]
        public ActionResult Configuration(ConfigurationModel model, HttpPostedFileBase fileUpload)
        {
            if (Session["AdminLoginStatus"] == "LoggedIn")
            {
                List<SelectListItem> DateFormat = new List<SelectListItem>();
                List<SelectListItem> TimeFormat = new List<SelectListItem>();

                DateFormat = adminuser.getdateformetlist();
                TimeFormat = adminuser.gettimeformetlist();
                ViewBag.DateFormat = DateFormat;
                ViewBag.TimeFormat = TimeFormat;

                var filename = fileUpload.FileName;
               
                string fileext = ".gif;.jpeg;.png";
                var allowedExtensions = new[] { ".gif", ".jpeg", ".jpg", ".png" };
                var extension = Path.GetExtension(fileUpload.FileName.ToLower());
                if (allowedExtensions.Contains(extension))
                {                
                    var filepath = Server.MapPath("/content/logo/" + Session["Companey"].ToString());
                    if (!Directory.Exists(filepath))
                    {
                        Directory.CreateDirectory(filepath);
                    }

                    fileUpload.SaveAs(filepath + "/" + filename);

                    ConfigurationModel configmodel = new ConfigurationModel();
                    configmodel.comapnylogo = filename;
                    configmodel.Dateformat = model.Dateformat;
                    configmodel.Timeformat = model.Timeformat;
                    configmodel.databaseaddress = model.databaseaddress;
                    configmodel.databasename = model.databasename;
                    configmodel.databaseuserid = model.databaseuserid;
                    configmodel.databasepassword = model.databasepassword;
                    configmodel.directorypath = model.directorypath;

                    var res = adminuser.InsertConfiguration(configmodel);
                    if (res == true)
                    {
                        ModelState.Clear();
                        TempData["Sucess"] = "Configuration Details insert Successfully.";
                        return View("Configuration");
                    }
                    else
                    {
                        TempData["Error"] = "Configuration Details insertion fails.";
                        return View("Configuration");
                    }
                }
                else
                {
                    TempData["Error"] = "Please choose logo with extensions .jpg,.png,.gif.";
                    return View("Configuration");
                }

            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        

        //Search User By UserId or Ip Address//SearchAuditLogByDate

        [WebMethod]
        public JsonResult SearchAuditLogByUserId_Ip(string id)
        {
            var res = adminuser.SearchAuditUser(id);

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [WebMethod]
        public JsonResult SearchAuditLogByDate(string StartDate, string EndDate)
        {
            var res = adminuser.SearchAuditUserByDate(StartDate, EndDate);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchDublicateEmail(string Email, int Id)
        {
            var result = adminuser.SearchDublicateEmail(Email,Id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchCycleByCycleId(string id)
        {
            var res = adminuser.SearchCycleId(id, originalConnectionString, Session["CompanyDB"].ToString());

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchCycleByDate(string StartDate, string EndDate)
        {
            var res = adminuser.SearchCycleByDate(StartDate, EndDate, originalConnectionString, Session["CompanyDB"].ToString());
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportDataofCycle(int Id)
        {
            Cycle Cycle = adminuser.FindCycleById(Id, originalConnectionString, Session["CompanyDB"].ToString());


            var products = new System.Data.DataTable("teste");
            products.Columns.Add("S.No", typeof(int));
            products.Columns.Add("CreatedDate", typeof(string));
            products.Columns.Add("CycleID", typeof(string));
            products.Columns.Add("Status", typeof(string));


            products.Rows.Add(1, Cycle.CreatedDate,Cycle.CycleID, Cycle.Status);


            var grid = new GridView();
            grid.DataSource = products;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=CycleRunFile.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();


            return RedirectToAction("AuditLog");


        }

        [HttpGet]
        public ActionResult CycleErrors(int Id)
        {

            //To get Data Type Conversion Errors
            var data = user.ReadRrrors(Id.ToString(), Session["Companey"].ToString());
            ViewBag.purchaseError = user.ReadPurchaseRrrors(Id.ToString(), Session["Companey"].ToString());
            ViewBag.supplyError = user.ReadSupplyRrrors(Id.ToString(), Session["Companey"].ToString());
            ViewBag.companyError = user.ReadCompanyRrrors(Id.ToString(), Session["Companey"].ToString());
            ViewBag.footerError = user.ReadFooterRrrors(Id.ToString(), Session["Companey"].ToString());
            //To get duplicate records
            ViewBag.purchase = user.ReadPurchaseDuplicateErrors(Id.ToString(), Session["Companey"].ToString());
            ViewBag.supply = user.ReadSupplyDuplicateErrors(Id.ToString(), Session["Companey"].ToString());
            ViewBag.ledger = user.ReadDuplicateLedgerErrors(Id.ToString(), Session["Companey"].ToString());
            ViewBag.comopany = user.ReadCompanyDuplicateErrors(Id.ToString(), Session["Companey"].ToString());
            ViewBag.footer = user.ReadFooterDuplicateErrors(Id.ToString(), Session["Companey"].ToString());
            //To get missing(Required Filed Validation Failed) records
            ViewBag.MissingPurchase = user.ReadPurchaseMissingErrors(Id.ToString(), Session["Companey"].ToString());
            ViewBag.MissingLedger = user.ReadMissingLedgerErrors(Id.ToString(), Session["Companey"].ToString());
            ViewBag.MissingSupply = user.ReadSupplyMissingErrors(Id.ToString(), Session["Companey"].ToString());
            ViewBag.MissingCompany = user.ReadCompanyMissingErrors(Id.ToString(), Session["Companey"].ToString());
            ViewBag.MissingFooter = user.ReadFooterMissingErrors(Id.ToString(), Session["Companey"].ToString());


            return View(data);

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


        public ActionResult DeleteData(int Id=0,string Page="")
        {
            Session["Id"] = Id.ToString();
            Session["Page"] = Page;
            var AdminUser = adminuser.FindAdminEmail();
            string securitycode = Alphanumrical(6, random);
            Session["Securitycode"] = securitycode;
            MailMessage.Sendsecuritycode(AdminUser.Email, securitycode, AdminUser.Name, ms_email, smtp_port, Smtp_Password, Smtp_Host, "Security Code for Delete Data");
            return View("Securitycodeaccess");
        }

        [HttpPost]
        public ActionResult DeleteData(string SecurityCode)
        {
            String securitycode = Session["Securitycode"].ToString();

            if (securitycode == SecurityCode)
            {
                string Page = Session["Page"].ToString();
                int Id = Convert.ToInt32(Session["Id"].ToString());
                return RedirectToAction(Page, new {Id=Id});
            }
            else
            {
                TempData["CodeError"] = "Invalid Code";
                return View("Securitycodeaccess");
            }
        }

        public ActionResult DeleteCycle(int Id)
        {
            if (Session["AdminLoginStatus"] == "LoggedIn")
            {
                var result = user.TempDeleteCycle(originalConnectionString, Session["CompanyDB"].ToString(), Id, "Audit Deleted");
                TempData["Message"] = "Cycle Deleted";
                return RedirectToAction("AuditLog");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

    }
}
