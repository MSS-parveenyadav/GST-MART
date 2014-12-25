using GST_BLL.DTO;
using GST_BLL.Enum;
using GST_DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web.Mvc;

namespace GST_BLL.AdminUser
{
    public class AdminUser
    {
        GSTMARTEntities gstmart = new GSTMARTEntities();

        public List<Companylist> GetCompanyList()
        {


            var Gstmart = new GSTMARTEntities();
            var query = (from p in Gstmart.Companies select new { CompanyId = p.CompanyID, CompanyName = p.CompanyName,Id=p.Id }).ToList();
            var list = new List<Companylist>();
            foreach (var item in query)
            {
                var model = new Companylist();
                model.CompanyId = item.CompanyId;
                model.CompanyName = item.CompanyName;
                model.id = item.Id;
                list.Add(model);
            }
            return list;

        }
        public List<SelectListItem> gettimeformetlist()
        {

            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Please Select", Value = "" });
            list.Add(new SelectListItem { Text = "hh:mm:ss tt", Value = "hh:mm:ss tt" });
            list.Add(new SelectListItem { Text = "HH:mm:ss tt", Value = "HH:mm:ss tt" });
            list.Add(new SelectListItem { Text = "hh:mm tt", Value = "hh:mm tt" });
            list.Add(new SelectListItem { Text = "HH:mm tt", Value = "HH:mm tt" });
            return list;

        }

        public string SearchDublicateEmail(string Email,int Id)
        {
            var result = new User();
            if(Id==0)
            {
                result = (from p in gstmart.Users where p.Email == Email && p.Usertype != "Admin" select p).FirstOrDefault();
            }
            else
            {
                result = (from p in gstmart.Users where p.Email == Email && p.Usertype != "Admin" && p.Id!=Id select p).FirstOrDefault();
            }
            
            if (result == null)
            {
                return "No";
            }
            else
            {
                return "Yes";
            }
        }

        public List<SelectListItem> GetCompanyListForDDl()
        {
            var Gstmart = new GSTMARTEntities();
            var query = (from p in Gstmart.Companies select new { CompanyId = p.CompanyID, CompanyName = p.CompanyName, Id = p.Id }).ToList();
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
            foreach (var item in query)
            {
                list.Add(new SelectListItem { Text = item.CompanyId, Value = item.CompanyId });
            }
            return list;
        }

        public bool CheckLogin(AdminUserModel model)
        {

            var Gstmart = new GSTMARTEntities();

            var query = (from u in Gstmart.Users where u.AdminID == model.AdminID && u.Password == model.Password select new { u.Name }).ToList();
            if (query.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }



        public void InsertAdminUser(AdminUserModel model, string Createcycle, string Accesssetting, string Downloaddata, string Printreport, IEnumerable<string> Companies)
        {
           
               var Gstmart = new GSTMARTEntities();


                User user = new User();
                CompanyUser userscompany = new CompanyUser();
                Permission permission = new Permission();
                user.UserId = model.UserId;
                user.Name = model.Usename;
                user.Password = model.Password;
                user.Email = model.Email;
                user.Mobilenumber = model.MobileNumber;
                user.Usertype = "Normal User";
                user.Createdate = DateTime.Now.ToString();
                user.Status = "Active";
                user.AdminID = "NA";

                foreach (var item in Companies)
                {
                    userscompany.UID = model.UserId;
                    userscompany.CID = item;
                    gstmart.CompanyUsers.Add(userscompany);
                    gstmart.SaveChanges();
                }
            
            
                permission.CreateCycle = Createcycle;
                permission.DownloadData = Downloaddata;
                permission.AccessSetting = Accesssetting;
                permission.PrintReport = Printreport;


                Gstmart.Users.Add(user);
                Gstmart.Permissions.Add(permission);
                Gstmart.SaveChanges();
       
        }


        public List<UserPermissionModel> GetUserPermissionList(string originalConnectionString, string DbName, string companyid)
        {
            var Gstmart = new GSTMARTEntities();
                var ob = (from u in Gstmart.Users 
                          join c in Gstmart.Permissions on u.Id equals c.User_Id
                          
                          select new { u.Id,u.Status,u.LastLoginDate, u.Createdate, u.UserId, u.Name, c }).ToList();

          

                var list = new List<UserPermissionModel>();
                foreach (var item in ob)
                {
                    var id = item.UserId.ToString();

                    var getUser = (from p in gstmart.CompanyUsers where p.CID == companyid && p.UID ==id  select p).Count();

                    if (getUser > 0)
                    {
                        var model = new UserPermissionModel();
                        model.Id = item.Id;
                        model.Date = item.Createdate;
                        model.Status = item.Status;
                        model.LastLoginDate = item.LastLoginDate;
                        model.Name = item.Name;
                        model.UserId = item.UserId;
                        model.Permission = item.c.CreateCycle + "," + item.c.AccessSetting + "," + item.c.DownloadData + "," + item.c.PrintReport;

                        list.Add(model);
                    }
                }

                return list.OrderByDescending(m=>m.Id).ToList();
         

        }

        public bool DeleteUser(int userid)
        {
            try
            {
            
                    var Gstmart = new GSTMARTEntities();
            

                    var query = Gstmart.Users.FirstOrDefault(x => x.Id == userid);
                    var permission = Gstmart.Permissions.FirstOrDefault(x => x.User_Id == userid);
                    if (query != null && permission != null)
                    {
                        Gstmart.Users.Remove(query);
                        Gstmart.Permissions.Remove(permission);

                        Gstmart.SaveChanges();
                        return true;

                    }
                    else
                    {
                        return false;
                    }
        
            }
            catch 
            {
                return false;
            }
        }



        public bool UpdateUser(int userid)
        {
            try
            {

                var Gstmart = new GSTMARTEntities();


                User user = Gstmart.Users.Where(x => x.Id == userid).Single<User>();
                if (user.Status == "suspend")
                {
                    user.Status = "Active";
                }
                else
                {
                    user.Status = "suspend";
                }
               

                Gstmart.Entry(user).State = System.Data.EntityState.Modified;
             
                Gstmart.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public AdminUserModel GetUserById(int userid)
         {
            try
            {
        
                   var Gstmart = new GSTMARTEntities();



                   var item = (from user in Gstmart.Users join permission in Gstmart.Permissions on user.Id equals permission.User_Id where user.Id == userid select new { user, permission }).FirstOrDefault();
                   var model = new AdminUserModel();
                    if(item!=null)
                    {
                        
                        model.Id = item.user.Id;
                        model.UserId = item.user.UserId;
                        model.Usename = item.user.Name;
                        model.Password = item.user.Password;
                        model.ConfirmPassword = item.user.Password;
                        model.Email = item.user.Email;
                        model.ConfirmEmail = item.user.Email;
                        model.MobileNumber = item.user.Mobilenumber;
                        model.ConfirmMobileNumber = item.user.Mobilenumber;
                        model.Createcycle = item.permission.CreateCycle;
                        model.DownloadData = item.permission.DownloadData;
                        model.PrintReport = item.permission.PrintReport;
                        model.AccessSetting = item.permission.AccessSetting;

                        List<SelectListItem> CompaniesList = new List<SelectListItem>();
                        foreach (Company company in gstmart.Companies)
                        {
                           var query1 = (from p in gstmart.CompanyUsers where p.UID == item.user.UserId && p.CID==company.CompanyID select p).SingleOrDefault();
                           if (query1 != null)
                           {
                               SelectListItem selectList = new SelectListItem()
                               {
                                   Text = company.CompanyID,
                                   Value = company.CompanyID,
                                   Selected = true
                               };
                               CompaniesList.Add(selectList);
                           }
                           else
                           {
                               SelectListItem selectList1 = new SelectListItem()
                               {
                                   Text = company.CompanyID,
                                   Value = company.CompanyID,
                                   Selected = false
                               };
                               CompaniesList.Add(selectList1);
                           }
                        }



                        model.listcompaney=CompaniesList;
                    }
                    return model;
            
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void UpdateAdminUser(AdminUserModel model, string Createcycle, string Accesssetting, string Downloaddata, string Printreport, int id,IEnumerable<string> Companies, string originalConnectionString, string DbName)
        {

            string status = "";
            //var ecsBuilder = new EntityConnectionStringBuilder(originalConnectionString);
            //var sqlCsBuilder = new SqlConnectionStringBuilder(ecsBuilder.ProviderConnectionString)
            //{
            //    InitialCatalog = DbName
            //};

            //var providerConnectionString = sqlCsBuilder.ToString();
            //ecsBuilder.ProviderConnectionString = providerConnectionString;

            //string contextConnectionString = ecsBuilder.ToString();
            //using (var db = new DbContext(contextConnectionString))
            //{
            //    var Gstmart = new GSTMARTEntities(contextConnectionString);
                var Gstmart = new GSTMARTEntities();

                status = (from p in gstmart.Users where p.UserId == model.UserId select p.Status).SingleOrDefault();
                if(status=="Active")
                {
                    status = "Active";
                }
                 else
                {
                    status = "suspend";
                }


                var query = (from p in gstmart.CompanyUsers where p.UID == model.UserId select p).ToList();
                foreach(var item in query)
                {
                    Gstmart.Entry(item).State = System.Data.EntityState.Deleted;
                    gstmart.SaveChanges();
                }

                CompanyUser userscompany = new CompanyUser();
                User user = Gstmart.Users.Where(x => x.Id == id).Single<User>();
                Permission permission = Gstmart.Permissions.Where(x => x.User_Id == id).Single<Permission>();
                user.UserId = model.UserId;
                user.Name = model.Usename;
                user.Password = model.Password;
                user.Email = model.Email;
                user.Mobilenumber = model.MobileNumber;
                user.Usertype = "Normal User";
                user.Createdate = DateTime.Now.ToString();
                user.Status = status;
                user.AdminID = "NA";

                foreach (var item in Companies)
                {
                    userscompany.UID = model.UserId;
                    userscompany.CID = item;
                    gstmart.CompanyUsers.Add(userscompany);
                    gstmart.SaveChanges();
                }

                permission.CreateCycle = Createcycle;
                permission.DownloadData = Downloaddata;
                permission.AccessSetting = Accesssetting;
                permission.PrintReport = Printreport;
                //Gstmart.Users.Add(user);
                //Gstmart.Permissions.Add(permission);
                Gstmart.Entry(user).State = System.Data.EntityState.Modified;
                Gstmart.Entry(permission).State = System.Data.EntityState.Modified;
                Gstmart.SaveChanges();


          //  }
        }

        public bool Check_duplicateUserId(string userid)
        {
            try
            {
             
                    var Gstmart = new GSTMARTEntities();

                    var qry = (from p in Gstmart.Users where p.UserId == userid select p).Count();
                    if (qry > 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
     

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string GetemailbyUserId(string UserId)
        {
            string email = "";
            var Gstmart = new GSTMARTEntities();
            var query = (from p in Gstmart.Users where p.AdminID==UserId  select p).ToList();
           foreach(var item in query)
           {
               email=item.Email;
           }
            return email;


        }

        public string FindAdminUser(string Email)
        {
            string email = "";
            var Gstmart = new GSTMARTEntities();
            var query = (from p in Gstmart.Users where p.Email == Email && p.Usertype=="Admin" select p).ToList();
            foreach (var item in query)
            {
                email = item.Email;
            }
            return email;
        }

        public AdminUserModel FindUserByEmail(string Email,string  Guid,out string message)
        {
            AdminUserModel User = new AdminUserModel();
            var Gstmart = new GSTMARTEntities();
            message = "";
            var query = (from p in Gstmart.Users where p.Email == Email && p.Usertype == "Admin" && p.Guid==Guid select p).FirstOrDefault();

            if (query == null)
            {
                   message = EnumClass.MessageFamily.Error.ToString();
            return User;

            }
            
            User.Id = query.Id;
            User.Usename = query.Name;
            User.Password = query.Password;
            User.ConfirmPassword = query.Password;
           
            return User;
        }


        public string GenerateLink(string URL, string email,string uniq)
        {
            var Gstmart = new GSTMARTEntities();

            var query = (from p in Gstmart.Users where p.Email == email && p.Usertype=="Admin" select p).FirstOrDefault();
            if (query!=null)
            {

                query.Guid = uniq;
                Gstmart.Entry(query).State = System.Data.EntityState.Modified;
                Gstmart.SaveChanges();
            }
          
            var url = URL + "/Admin/ChangePassword?Email=" + email + "&&Guid=" + uniq;
            string ResetLink = "<br /><br />Please click the following link to Reset your Password <a href = '" + url + "'>Click here</a>";
            return ResetLink;
        }


        public int UpdatePassword(AdminUserModel Data,out string Email,out string Username)
        {
            var Gstmart = new GSTMARTEntities();
            var User = (from p in Gstmart.Users where p.Id == Data.Id select p).FirstOrDefault();
            if (User != null)
            {
                User.Password = Data.Password;
                User.Guid = "";
                Gstmart.Entry(User).State = System.Data.EntityState.Modified;
                Gstmart.SaveChanges();

                Username = User.Name;
                Email = User.Email;
                return 1;
            }
            if(Data.Guid==null)
            {
                Username = "";
                Email = "";
                return 5;
            }
            Username = "";
            Email="";
            return 2;
        }


        public int SaveIpAddress(IpAddressModel Data)
        {
            var Gstmart = new GSTMARTEntities();
            var IpAddress = new IpAddress();
            IpAddress.IP = Data.IP;
            IpAddress.CreatedDate = DateTime.Now;
            Gstmart.IpAddresses.Add(IpAddress);
            Gstmart.SaveChanges();
            return 1;
        }

        public string FindIpAddressByIp(IpAddressModel Data)
        {
            string status = "";
            var Gstmart = new GSTMARTEntities();
            var query = (from p in Gstmart.IpAddresses where p.IP == Data.IP select p).FirstOrDefault();
            if (query != null)
            {
                status = "Exist";
                return status;
            }
            else
            {
                status = "NotExist";
                return status;
            }
        }

        public List<IpAddress> GetIpAddressList()
        {
            var query=(from p in gstmart.IpAddresses select p).OrderByDescending(ip=>ip.Id).ToList();
            return query;
        }

        public int DeleteIp( int ID)
        {
            var query = (from p in gstmart.IpAddresses where p.Id == ID select p).FirstOrDefault();
            if (query != null)
            {
                gstmart.Entry(query).State = System.Data.EntityState.Deleted;
                gstmart.SaveChanges();
            }
            return 1;
        }

        public int EditIpAddress(IpAddressModel Data)
        {
            var Ipadd = (from p in gstmart.IpAddresses where p.Id == Data.Id select p).FirstOrDefault();
            if (Ipadd != null)
            {
                Ipadd.IP = Data.IP;
                gstmart.Entry(Ipadd).State = System.Data.EntityState.Modified;
                gstmart.SaveChanges();
            }
            return 1;
        }

        public IpAddressModel FindIpAddressById(int id)
        {
            var Ipadd = (from p in gstmart.IpAddresses where p.Id == id select p).FirstOrDefault();
            var Ipaddress=new IpAddressModel();
            Ipaddress.Id = Ipadd.Id;
            Ipaddress.IP = Ipadd.IP;
            return Ipaddress;
        }


        public IPAddress getipaddress()
        {
            return Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }

        public string CkeckIpAddress(IPAddress IP)
        {
            Int32 Count = 0;
            string ip = IP.ToString();
            var queryList = (from p in gstmart.IpAddresses select p).ToList();
            foreach (var item in queryList)
            {
                Count = Count + 1;
            }
            if (Count != 0)
            {
                var query = (from p in gstmart.IpAddresses where p.IP == ip select p).FirstOrDefault();
                if (query != null)
                {
                    return "IpFound";
                }
                else
                {
                    return "IpNotFound";
                }
            }
            else
            {
                return "ListEmpty";
            }

        }

        public AdminPathModel GetAdminPath()
        {
            AdminPath adpath = new AdminPath();
            var query = (from p in gstmart.AdminPaths select p).FirstOrDefault();
            if(query==null)
            {
                adpath.adminpath1 = "";
                gstmart.AdminPaths.Add(adpath);
                gstmart.SaveChanges();
            }
            var query1= (from p in gstmart.AdminPaths select p).FirstOrDefault();
            adpath = query1;
            var adminpath = new AdminPathModel();
            adminpath.ID = query1.Id;
            adminpath.AdminPath = query1.adminpath1;
            return adminpath;
        }

        public Int32 UpdateAdminPath(AdminPathModel adminpath)
        {
            var Gstmart = new GSTMARTEntities();
            var adpath = (from p in Gstmart.AdminPaths where p.Id == adminpath.ID select p).FirstOrDefault();
            if (adpath != null)
            {
                adpath.adminpath1 = adminpath.AdminPath;
                Gstmart.Entry(adpath).State = System.Data.EntityState.Modified;
                Gstmart.SaveChanges();
                return 1;
            }
            return 2;
        }

        public string getAdminPath()
        {
            var query = (from p in gstmart.AdminPaths select p.adminpath1).FirstOrDefault();
            string adminpath = query;
            return adminpath;
        }

        public bool CreateLdapUser(LdapUserModel model)
        {
            var Gstmart = new GSTMARTEntities();
            LDAP ldap = new LDAP();
            ldap.CompanyId = model.ComapanyId;
            ldap.DomianName = model.Domainname;
            ldap.PortNumber = model.Portnumber;
            ldap.UserId = model.UserId;
            ldap.Password = model.password;
            ldap.CNBN = model.cnbn;
            gstmart.LDAPs.Add(ldap);

           int res= gstmart.SaveChanges();
            if(res==1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public List<LdapUserModel> GetLDAPUserList()
        {
            var Gstmart = new GSTMARTEntities();

            var query = (from p in Gstmart.LDAPs select new { Id = p.Id,CNBN=p.CNBN, CompanyId = p.CompanyId, DomainName = p.DomianName, PortNmber = p.PortNumber, UserId = p.UserId, Password = p.Password }).OrderByDescending(m=>m.Id).ToList();
            var list = new List<LdapUserModel>();
            foreach (var item in query)
            {
                var model = new LdapUserModel();
                model.Id = item.Id;
                model.ComapanyId = item.CompanyId;
                model.Domainname = item.DomainName;
                model.Portnumber = item.PortNmber;
                model.UserId = item.UserId;
                model.password = item.Password;
                model.cnbn = item.CNBN;
                list.Add(model);
            }
            return list;

        }

        public List<LdapUserModel> GetLDAPUSERBYID(int id)
        {
            var Gstmart = new GSTMARTEntities();

            var query = (from p in Gstmart.LDAPs where p.Id == id select new { Id = p.Id,cnbn=p.CNBN, CompanyId = p.CompanyId, DomainName = p.DomianName, PortNmber = p.PortNumber, UserId = p.UserId, Password = p.Password }).ToList();
            var list = new List<LdapUserModel>();
            foreach (var item in query)
            {
                var model = new LdapUserModel();
                model.Id = item.Id;
                model.ComapanyId = item.CompanyId;
                model.Domainname = item.DomainName;
                model.Portnumber = item.PortNmber;
                model.UserId = item.UserId;
                model.password = item.Password;
                model.cnbn = item.cnbn;
                list.Add(model);
            }
            return list;
        }
        public bool UpdateLDAPUser(LdapUserModel model)
        {
            var Gstmart = new GSTMARTEntities();
            LDAP ldap = Gstmart.LDAPs.Where(x => x.Id == model.Id).Single<LDAP>();
            ldap.UserId = model.UserId;
            ldap.CompanyId = model.ComapanyId;
            ldap.Password = model.password;
            ldap.PortNumber = model.Portnumber;
            ldap.DomianName = model.Domainname;
            ldap.CNBN = model.cnbn;


            Gstmart.Entry(ldap).State = System.Data.EntityState.Modified;
           int res= Gstmart.SaveChanges();
            if(res==1)
            {
                return true;

            }
            else
            {
                return false;
            }
        }
        public bool DeleteLDAPUser(int ID)
        {
            var query = (from p in gstmart.LDAPs where p.Id == ID select p).FirstOrDefault();
            if (query != null)
            {
                gstmart.Entry(query).State = System.Data.EntityState.Deleted;
                gstmart.SaveChanges();
                return true;
            }
            return false;
        }

        public bool CheckLDAPLogin(string companyId, string userId, string password)
        {
            var Gstmart = new GSTMARTEntities();
            var query = (from p in Gstmart.LDAPs where p.CompanyId == companyId.Trim() && p.UserId == userId && p.Password == password select p).ToList();
            if (query.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        public List<LdapUserModel> GetLDAPUSERBYUserID(string UserID)
        {
            var Gstmart = new GSTMARTEntities();

            var query = (from p in Gstmart.LDAPs where p.UserId == UserID select new { Id = p.Id, CompanyId = p.CompanyId, DomainName = p.DomianName, PortNmber = p.PortNumber, UserId = p.UserId, Password = p.Password }).ToList();
            var list = new List<LdapUserModel>();
            foreach (var item in query)
            {
                var model = new LdapUserModel();
                model.Id = item.Id;
                model.ComapanyId = item.CompanyId;
                model.Domainname = item.DomainName;
                model.Portnumber = item.PortNmber;
                model.UserId = item.UserId;
                model.password = item.Password;
                list.Add(model);
            }
            return list;
        }

        public CurrencyExchangeModel FindCurrencyEzchangeByID(int id)
        {
            var CurrencyExchange = (from p in gstmart.CurrencyExchanges where p.Id == id select p).FirstOrDefault();
            var model = new CurrencyExchangeModel();
            model.Id = CurrencyExchange.Id;
            model.CompanyId = CurrencyExchange.CompanyId;
            model.ConversionRate = CurrencyExchange.ConversionRate;
            model.CreatedDate = CurrencyExchange.CreatedDate;
            model.CurrencyCode = CurrencyExchange.CurrencyCode;
            model.Description = CurrencyExchange.Discription;
           
            return model;
        }
        public string InsertCurrencyExchange(CurrencyExchangeModel model, string CompanyId)
        {
            try
            {
                var currencyExchange = new CurrencyExchange();
                currencyExchange.Id = model.Id;
                currencyExchange.CompanyId = CompanyId;
                currencyExchange.ConversionRate = "NotDefined";
                currencyExchange.CreatedDate = DateTime.Now.ToString();
                currencyExchange.CurrencyCode = "NotDefined";
                currencyExchange.Discription = "NotDefined";
             
                gstmart.CurrencyExchanges.Add(currencyExchange);
                gstmart.SaveChanges();
                return "Currency Exchange Inserted";
            }
            catch(Exception Ex)
            {
                string Exception = Ex.ToString();
                return Exception;
            }

        }

        public List<SelectListItem> GetCustomcodelist()
        {

            var Gstmart = new GSTMARTEntities();
            var query = (from p in Gstmart.CustomCodes select new { Id = p.Id, Customcode = p.Customcode }).ToList();
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
            foreach (var item in query)
            {
                list.Add(new SelectListItem { Text = item.Customcode, Value = item.Customcode });
            }
            return list;

        }
        public List<SelectListItem> GetCurrencyTypelist()
        {

            var Gstmart = new GSTMARTEntities();
            var query = (from p in Gstmart.CurrencyTypes select p).ToList();
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem{Text="Select Currency Type",Value=""});
            foreach (var item in query)
            {
                list.Add(new SelectListItem { Text = item.currencytype, Value = item.currencytype });
            }
            return list;

        }

        public List<SelectListItem> GetTransactionlist()
        {

            var Gstmart = new GSTMARTEntities();
            var query = (from p in Gstmart.TransactionTypes select new { Id = p.Id, Trantype = p.Transactiontype }).ToList();
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Please Select", Value = "Please Select" });
            foreach (var item in query)
            {
                list.Add(new SelectListItem { Text = item.Trantype, Value = item.Trantype });
            }
            return list;

        }
        public List<SelectListItem> getdateformetlist()
        {

            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Please Select", Value = "" });
            list.Add(new SelectListItem { Text = "M / d / yyyy", Value = "M / d / yyyy" });
            list.Add(new SelectListItem { Text = "M/d/yy", Value = "M/d/yy" });
            list.Add(new SelectListItem { Text = "MM/dd/yy", Value = "MM/dd/yy" });
            list.Add(new SelectListItem { Text = "MM/dd/yyyy", Value = "MM/dd/yyyy" });
            list.Add(new SelectListItem { Text = "yy/MM/dd", Value = "yy/MM/dd" });
            list.Add(new SelectListItem { Text = "yyyy-MM-dd", Value = "yyyy-MM-dd" });
            list.Add(new SelectListItem { Text = "dd-MMM-yy", Value = "dd-MMM-yy" });
            return list;

        }

        
        public List<SystemModel> Getsystembycompanyid(string id)
        {
            var Gstmart = new GSTMARTEntities();
            var CompanayId = (from p in gstmart.Companies where p.CompanyID == id select p).FirstOrDefault();
            var query = (from p in Gstmart.GstSystems where p.CompanyId == CompanayId.Id select new { Id = p.Id, Systemid = p.SystemID, Systemname = p.SystemName }).ToList();
            var list = new List<SystemModel>();
            foreach (var item in query)
            {
                var model = new SystemModel();
                model.Id = item.Id;
                model.SystemId = item.Systemid;
                model.SystemName = item.Systemname;
                list.Add(model);
            }
            return list;
        }


       public bool CreateTaxcode(TaxCodeModel model, string systemlist)
        {
            var Gstmart = new GSTMARTEntities();

            var originalcodetax = (from p in Gstmart.OriginalCodeTaxes where p.OriginalCode == model.OriginalCode && p.TaxRate == model.TaxRate select p).ToList();
            if (originalcodetax.Count==0)
            {
                //var qrycompany = (from p in Gstmart.Companies where p.Id == companylist select new { p.CompanyID }).SingleOrDefault();
                //var qrycustomcode = (from p in Gstmart.CustomCodes where p.Id == customcodelist select new { p.Customcode }).SingleOrDefault();
                //var qrytransactiontype = (from p in Gstmart.TransactionTypes where p.Id == transactiontypelist select new { p.Transactiontype }).SingleOrDefault();
                TaxCode taxcode = new TaxCode();
                taxcode.CompanyId = model.CompanyId;
                taxcode.SystemId = systemlist;
                taxcode.CustomCode = model.CustomCode;
                taxcode.OriginalCode = model.OriginalCode;
                taxcode.TransactionType = model.TransactionType;
                taxcode.TaxRate = model.TaxRate;
                taxcode.Description = model.Description;
                taxcode.ReferenceNumber = model.RefernceNumber;
                taxcode.GST03Fields = model.GST03Fields;
                taxcode.Remarks = model.Remarks;
                taxcode.DateType = model.DateType;

                OriginalCodeTax oct = new OriginalCodeTax();
                oct.TaxRate = model.TaxRate;
                oct.OriginalCode = model.OriginalCode;
                Gstmart.TaxCodes.Add(taxcode);
                Gstmart.OriginalCodeTaxes.Add(oct);

                int res = Gstmart.SaveChanges();
                if (res == 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                //var qrycompany = (from p in Gstmart.Companies where p.Id == companylist select new { p.CompanyID }).SingleOrDefault();
                //var qrycustomcode = (from p in Gstmart.CustomCodes where p.Id == customcodelist select new { p.Customcode }).SingleOrDefault();
                //var qrytransactiontype = (from p in Gstmart.TransactionTypes where p.Id == transactiontypelist select new { p.Transactiontype }).SingleOrDefault();
                TaxCode taxcode = new TaxCode();
                taxcode.CompanyId = model.CompanyId;
                taxcode.SystemId = systemlist;
                taxcode.CustomCode = model.CustomCode;
                taxcode.OriginalCode = model.OriginalCode;
                taxcode.TransactionType = model.TransactionType;
                taxcode.TaxRate = model.TaxRate;
                taxcode.Description = model.Description;
                taxcode.ReferenceNumber = model.RefernceNumber;
                taxcode.GST03Fields = model.GST03Fields;
                taxcode.Remarks = model.Remarks;
                taxcode.DateType = model.DateType;
                Gstmart.TaxCodes.Add(taxcode);
                int res = Gstmart.SaveChanges();
                if (res == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public string UpdateCurrencyExchange(CurrencyExchangeModel model, string CompanyId)
        {
            try
            {
                var CurrencyExchange = (from p in gstmart.CurrencyExchanges where p.Id == model.Id select p).FirstOrDefault();
                CurrencyExchange.CompanyId = CompanyId;
                CurrencyExchange.CreatedDate = DateTime.Now.ToString();
                gstmart.Entry(CurrencyExchange).State = System.Data.EntityState.Modified;
                gstmart.SaveChanges();
                return "Currency Exchange Updated";
            }
            catch (Exception Ex)
            {
                string Exception = Ex.ToString();
                return Exception;
            }
        }

        public string DeleteCurrencyExchange(int id)
        {
            var currencyexchange = (from p in gstmart.CurrencyExchanges where p.Id == id select p).FirstOrDefault();
            gstmart.Entry(currencyexchange).State = System.Data.EntityState.Deleted;
            gstmart.SaveChanges();
            return "Deleted";
        }

        public List<CurrencyExchange> GetCurrencyExchangeList()
        {
            var CurrencyExchangeList = (from p in gstmart.CurrencyExchanges select p).OrderByDescending(code=>code.Id).ToList();
            return CurrencyExchangeList;
            
        }

        public List<SelectListItem> GetCurrencyList()
        {
            var Currency = (from p in gstmart.CurrencyTypes select p).ToList();
            List<SelectListItem> List = new List<SelectListItem>();
            List.Add(new SelectListItem { Text = "Select Currency Type", Value = "" });
            foreach(var item in Currency)
            {
                List.Add(new SelectListItem { Text = item.currencytype, Value = item.currencytype });
            }
            return List;
        }

        public List<SelectListItem> CreateCustomcode(string customcode)
        {
            CustomCode cd = new CustomCode();
            cd.Customcode = customcode;
            gstmart.CustomCodes.Add(cd);
            gstmart.SaveChanges();
            var list = GetCustomcodelist();
            return list;
        }

        public List<SelectListItem> CreateCurrencyType(string CurrencyType)
        {
            CurrencyType cd = new CurrencyType();
            cd.currencytype = CurrencyType;
            gstmart.CurrencyTypes.Add(cd);
            gstmart.SaveChanges();
            var list = GetCurrencyTypelist();
            return list;
        }

        public List<SelectListItem> CreateTransactiontype(string transactiontype)
        {
            TransactionType tt = new TransactionType();
            tt.Transactiontype = transactiontype;
            gstmart.TransactionTypes.Add(tt);
            gstmart.SaveChanges();
            var list = GetTransactionlist();
            return list;

        }

        public string CreateCurrencyGroup(CurrencyGroupModel model)
        {
            try
            {
                var currencygroup = new CurrencyGroup();
                currencygroup.CreatedDate = DateTime.Now.ToString();
                currencygroup.CurrencyGroupName = model.CurrencyGroupName;
                currencygroup.CurrencyType = model.CurrencyType;
                currencygroup.Id = model.Id;
                currencygroup.TaxCode = model.TaxCode;
                gstmart.CurrencyGroups.Add(currencygroup);
                gstmart.SaveChanges();
                return "Currency Group Inserted";
            }
            catch(Exception Ex)
            {
                string Exc = Ex.ToString();
                return Exc;
            }
        }


        public string UpdateCurrencyGroup(CurrencyGroupModel model)
        {
            try
            {

                var currencygroup = (from p in gstmart.CurrencyGroups where p.Id == model.Id select p).FirstOrDefault();

                currencygroup.CreatedDate = DateTime.Now.ToString();
                currencygroup.CurrencyGroupName = model.CurrencyGroupName;
                currencygroup.CurrencyType = model.CurrencyType;
                currencygroup.TaxCode = model.TaxCode;
                gstmart.Entry(currencygroup).State = System.Data.EntityState.Modified;
                gstmart.SaveChanges();
                return "Currency Group Updated";
            }
            catch (Exception Ex)
            {
                string Exc = Ex.ToString();
                return Exc;
            }
        }

        public CurrencyGroupModel FindCurrencfGroupById(int id)
        {
            var currencygroup = (from p in gstmart.CurrencyGroups where p.Id == id select p).FirstOrDefault();
            var model = new CurrencyGroupModel();
            model.CreatedDate = currencygroup.CreatedDate;
            model.CurrencyGroupName = currencygroup.CurrencyGroupName;
            model.CurrencyType = currencygroup.CurrencyType;
            model.Id = currencygroup.Id;
            model.TaxCode = currencygroup.TaxCode;
            return model;
        }

        public string DeleteCurrencyGroup(int id)
        {
            var currencygroup = (from p in gstmart.CurrencyGroups where p.Id == id select p).FirstOrDefault();
            gstmart.Entry(currencygroup).State = System.Data.EntityState.Deleted;
            gstmart.SaveChanges();
            return "Currency Group Deleted";
        }

        public List<CurrencyGroup> GetCurrencyGroupList()
        {
            var currencyList = (from p in gstmart.CurrencyGroups select p).OrderByDescending(m=>m.Id).ToList();
                return currencyList;
        }

 

        public bool DeleteTaxModel(int id)
        {
            var query = (from p in gstmart.TaxCodes where p.Id == id select p).FirstOrDefault();
            if (query != null)
            {
                gstmart.Entry(query).State = System.Data.EntityState.Deleted;
                gstmart.SaveChanges();
                return true;
            }
            return false;
        }



        public List<TaxCodeModel> GetTAXCODEBYID(int id)
        {
            var Gstmart = new GSTMARTEntities();

            var query = (from p in Gstmart.TaxCodes where p.Id == id select new { Id = p.Id, CompanyId = p.CompanyId,SystemId=p.SystemId,Customcode=p.CustomCode,Originalcode=p.OriginalCode,Transactiontype=p.TransactionType,Taxrate=p.TaxRate,Description=p.Description,Referncenumber=p.ReferenceNumber,Gst03filed=p.GST03Fields,Remarks=p.Remarks,Datetype=p.DateType }).ToList();
            var list = new List<TaxCodeModel>();
            foreach (var item in query)
            {
                var model = new TaxCodeModel();
                model.Id = item.Id;
                model.CompanyId = item.CompanyId;
                model.SystemId = item.SystemId;
                model.CustomCode = item.Customcode;
                model.OriginalCode = item.Originalcode;
                model.TransactionType = item.Transactiontype;
                model.TaxRate = item.Taxrate;
                model.Description = item.Description;
                model.RefernceNumber = item.Referncenumber;
                model.GST03Fields = item.Gst03filed;
                model.Remarks = item.Remarks;
                model.DateType = item.Datetype;             
                list.Add(model);
            }
            return list;
        }


        public bool UpdateTaxcode(TaxCodeModel model,  string systemlist)
        {
            var Gstmart = new GSTMARTEntities();

            var originalcodetax = (from p in Gstmart.OriginalCodeTaxes where p.OriginalCode == model.OriginalCode && p.TaxRate == model.TaxRate select p).ToList();
            if (originalcodetax.Count == 0)
            {
                //var qrycompany = (from p in Gstmart.Companies where p.Id == companylist select new { p.CompanyID }).SingleOrDefault();
                //var qrycustomcode = (from p in Gstmart.CustomCodes where p.Id == customcodelist select new { p.Customcode }).SingleOrDefault();
                //var qrytransactiontype = (from p in Gstmart.TransactionTypes where p.Id == transactiontypelist select new { p.Transactiontype }).SingleOrDefault();

                TaxCode taxcode = Gstmart.TaxCodes.Where(x => x.Id == model.Id).Single<TaxCode>();
                taxcode.CompanyId = model.CompanyId;
                taxcode.SystemId = systemlist;
                taxcode.CustomCode = model.CustomCode;
                taxcode.OriginalCode = model.OriginalCode;
                taxcode.TransactionType = model.TransactionType;
                taxcode.TaxRate = model.TaxRate;
                taxcode.Description = model.Description;
                taxcode.ReferenceNumber = model.RefernceNumber;
                taxcode.GST03Fields = model.GST03Fields;
                taxcode.Remarks = model.Remarks;
                taxcode.DateType = model.DateType;
                Gstmart.Entry(taxcode).State = System.Data.EntityState.Modified;
                
                
                OriginalCodeTax oct = new OriginalCodeTax();
                oct.TaxRate = model.TaxRate;
                oct.OriginalCode = model.OriginalCode;
              
                Gstmart.OriginalCodeTaxes.Add(oct);

                int res = Gstmart.SaveChanges();
                if (res == 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                //var qrycompany = (from p in Gstmart.Companies where p.Id == companylist select new { p.CompanyID }).SingleOrDefault();
                //var qrycustomcode = (from p in Gstmart.CustomCodes where p.Id == customcodelist select new { p.Customcode }).SingleOrDefault();
                //var qrytransactiontype = (from p in Gstmart.TransactionTypes where p.Id == transactiontypelist select new { p.Transactiontype }).SingleOrDefault();
                TaxCode taxcode = Gstmart.TaxCodes.Where(x => x.Id == model.Id).Single<TaxCode>();
                taxcode.CompanyId = model.CompanyId;
                taxcode.SystemId = systemlist;
                taxcode.CustomCode = model.CustomCode;
                taxcode.OriginalCode = model.OriginalCode;
                taxcode.TransactionType = model.TransactionType;
                taxcode.TaxRate = model.TaxRate;
                taxcode.Description = model.Description;
                taxcode.ReferenceNumber = model.RefernceNumber;
                taxcode.GST03Fields = model.GST03Fields;
                taxcode.Remarks = model.Remarks;
                taxcode.DateType = model.DateType;

                OriginalCodeTax oct = Gstmart.OriginalCodeTaxes.Where(x => x.OriginalCode == model.OriginalCode).Single<OriginalCodeTax>();
                oct.TaxRate = model.TaxRate;
                oct.OriginalCode = model.OriginalCode;
             
                Gstmart.Entry(taxcode).State = System.Data.EntityState.Modified;
                Gstmart.Entry(oct).State = System.Data.EntityState.Modified;

                int res = Gstmart.SaveChanges();
                if (res == 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        public List<TaxCodeModel> gettaxmodellist()
        {
            var qry = (from p in gstmart.TaxCodes select p).OrderByDescending(id=>id.Id).ToList();

            var list = new List<TaxCodeModel>();
            foreach (var item in qry)
            {
                var model = new TaxCodeModel();
                model.Id = item.Id;
                model.OriginalCode = item.OriginalCode;
                model.CustomCode = item.CustomCode;
                model.TaxRate = item.TaxRate;
                model.TransactionType = item.TransactionType;
                model.RefernceNumber = item.ReferenceNumber;
                model.Description = item.Description;
                model.DateType = item.DateType;
                model.GST03Fields = item.GST03Fields;
                model.Remarks = item.Remarks;
                list.Add(model);
            }
            return list;


        }

        public string gettaxratebyoriginalcode(string id)
        {
            var Gstmart = new GSTMARTEntities();
            var qry = (from p in Gstmart.OriginalCodeTaxes where p.OriginalCode == id select new { p.TaxRate }).SingleOrDefault();
            if (qry != null)
            {

                return qry.TaxRate;
            }
            else
            {

                return "Fail";

            }

        }


        public string UpdateManualCurrencyExchange(CurrencyExchangeModel model, string CompanyId)
        {
            try
            {
                var CurrencyExchange = (from p in gstmart.CurrencyExchanges where p.Id == model.Id select p).FirstOrDefault();
               // CurrencyExchange.CompanyId = CompanyId;
                CurrencyExchange.ConversionRate = model.ConversionRate;
                CurrencyExchange.ConversionRate = model.ConversionRate;
                CurrencyExchange.CreatedDate = model.CreatedDate;
                CurrencyExchange.CurrencyCode = model.CurrencyCode;
                CurrencyExchange.Discription = model.Description;
                gstmart.Entry(CurrencyExchange).State = System.Data.EntityState.Modified;
                gstmart.SaveChanges();
                return "Currency Exchange Updated";
            }
            catch (Exception Ex)
            {
                string Exception = Ex.ToString();
                return Exception;
            }
        }

        public string InsertManualCurrencyExchange(CurrencyExchangeModel model, string CompanyId)
        {
            try
            {
                var currencyExchange = new CurrencyExchange();
                currencyExchange.Id = model.Id;
                currencyExchange.CompanyId = CompanyId;
                currencyExchange.ConversionRate = model.ConversionRate;
                currencyExchange.CreatedDate = model.CreatedDate;
                currencyExchange.CurrencyCode = model.CurrencyCode;
                currencyExchange.Discription = model.Description;
                gstmart.CurrencyExchanges.Add(currencyExchange);
                gstmart.SaveChanges();
                return "Currency Exchange Inserted";
            }
            catch (Exception Ex)
            {
                string Exception = Ex.ToString();
                return Exception;
            }

        }


        public List<UserPermissionModel> SearchUser(string UserId)
        {

            if (UserId != "All")
            {

                var Gstmart = new GSTMARTEntities();
                var ob = (from u in Gstmart.Users
                          join c in Gstmart.Permissions on u.Id equals c.User_Id
                          where u.UserId.Contains(UserId) || u.Email.Contains(UserId) || u.Status == UserId
                          select new { u.Id, u.Status, u.LastLoginDate, u.Createdate, u.UserId, u.Name, c }).ToList();

                var list = new List<UserPermissionModel>();
                foreach (var item in ob)
                {
                    var model = new UserPermissionModel();
                    model.Id = item.Id;
                    model.Date = item.Createdate;
                    model.Status = item.Status;
                    model.LastLoginDate = item.LastLoginDate;
                    model.Name = item.Name;
                    model.UserId = item.UserId;
                    model.Permission = item.c.CreateCycle + "," + item.c.AccessSetting + "," + item.c.DownloadData + "," + item.c.PrintReport;

                    list.Add(model);
                }

                return list;


            }
            else
            {
                var Gstmart = new GSTMARTEntities();
                var ob = (from u in Gstmart.Users
                          join c in Gstmart.Permissions on u.Id equals c.User_Id
                         
                          select new { u.Id, u.Status, u.LastLoginDate, u.Createdate, u.UserId, u.Name, c }).ToList();

                var list = new List<UserPermissionModel>();
                foreach (var item in ob)
                {
                    var model = new UserPermissionModel();
                    model.Id = item.Id;
                    model.Date = item.Createdate;
                    model.Status = item.Status;
                    model.LastLoginDate = item.LastLoginDate;
                    model.Name = item.Name;
                    model.UserId = item.UserId;
                    model.Permission = item.c.CreateCycle + "," + item.c.AccessSetting + "," + item.c.DownloadData + "," + item.c.PrintReport;

                    list.Add(model);
                }

                return list;
            }
        }


        public string auditLog(DateTime Date, string Ip, string Message, string UserId, string Name,string Status)
        {
            try
            {

                AuditLog Audit = new AuditLog();
                Audit.Id = 0;
                Audit.CreatedDate = Date;
                Audit.IpAddress = Ip;
                Audit.Message = Message;
                Audit.UserId = UserId;
                Audit.Name = Name;
                Audit.Status = Status;
                gstmart.AuditLogs.Add(Audit);
                gstmart.SaveChanges();
                return "Success";
            }
            catch (Exception Ex)
            {
                string ExMessage = Ex.ToString();
                return ExMessage;
            }
        }

        public List<AuditLog> GetAuditLogList()
        {
            var Audits = (from p in gstmart.AuditLogs select p).OrderByDescending(m=>m.Id).ToList();
            return Audits;
        }

        public string DeleteAuditLog(int id)
        {
            var AuditLog = (from p in gstmart.AuditLogs where p.Id == id select p).FirstOrDefault();
            gstmart.Entry(AuditLog).State = System.Data.EntityState.Deleted;
            gstmart.SaveChanges();
            return "Deleted";
        }

        public AuditLogModel FindAuditLogById(int id)
        {
            var AuditLog = (from p in gstmart.AuditLogs where p.Id == id select p).FirstOrDefault();
            var model = new AuditLogModel();
            model.CreatedDate = AuditLog.CreatedDate.ToString();
            model.Id = AuditLog.Id;
            model.IpAddress = AuditLog.IpAddress;
            model.Message = AuditLog.Message;
            model.UserId = AuditLog.UserId;
            model.Name = AuditLog.Name;
            model.Status = AuditLog.Status;
            return model;
        }

        public List<SelectListItem> GetCurrencyCodelist()
        {

            var Gstmart = new GSTMARTEntities();
            var query = (from p in Gstmart.CurrencyTypes select p).ToList();
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Select Currency Code", Value = "" });
            foreach (var item in query)
            {
                list.Add(new SelectListItem { Text = item.currencycode, Value = item.currencycode });
            }
            return list;

        }

        public List<SelectListItem> CreateCurrencyCode(string CurrencyType, string CurrencyCode)
        {
            CurrencyType cd = new CurrencyType();
            cd.currencytype = CurrencyType;
            cd.currencycode = CurrencyCode;
            gstmart.CurrencyTypes.Add(cd);
            gstmart.SaveChanges();
            var list = GetCurrencyCodelist();
            return list;
        }

        public List<SelectListItem> CreateCurrencyType(string CurrencyType, string CurrencyCode)
        {
            CurrencyType cd = new CurrencyType();
            cd.currencytype = CurrencyType;
            cd.currencycode = CurrencyCode;
            gstmart.CurrencyTypes.Add(cd);
            gstmart.SaveChanges();
            var list = GetCurrencyTypelist();
            return list;
        }

        public List<AuditLogModel> SearchAuditUserByDate(string StartDate, string Enddate)
        {
            
            
            var Audits =new List<AuditLog>();
         

            if(StartDate=="" && Enddate!="")
            {
                DateTime EDate = Convert.ToDateTime(Enddate);
                Audits = (from p in gstmart.AuditLogs where  p.CreatedDate <= EDate.Date select p).ToList();
            }
            else if(StartDate!="" && Enddate=="")
            {
                DateTime SDate = Convert.ToDateTime(StartDate);
                Audits = (from p in gstmart.AuditLogs where p.CreatedDate >= SDate.Date  select p).ToList();
            }
            else
            {
                DateTime EDate = Convert.ToDateTime(Enddate);
                EDate = EDate.AddHours(24);
                DateTime SDate = Convert.ToDateTime(StartDate);
            
                Audits = (from p in gstmart.AuditLogs where p.CreatedDate >= SDate.Date && p.CreatedDate <= EDate.Date select p).ToList();
            }
            var AuditmodelList = new List<AuditLogModel>();
            foreach (var item in Audits)
            {
                var auditmodel = new AuditLogModel();
                auditmodel.CreatedDate = item.CreatedDate.ToString();
                auditmodel.Id = item.Id;
                auditmodel.IpAddress = item.IpAddress;
                auditmodel.Message = item.Message;
                auditmodel.UserId = item.UserId;
                auditmodel.Name = item.Name;
                auditmodel.Status = item.Status;
                AuditmodelList.Add(auditmodel);
            }
            return AuditmodelList;
        }


        public bool InsertConfiguration(ConfigurationModel model)
        {
            Configuration config = new Configuration();
            config.logoname = model.comapnylogo;
            config.dateformat = model.Dateformat;
            config.timeformat = model.Timeformat;
            config.dbaddress = model.databaseaddress;
            config.dbname = model.databasename;
            config.dbuserid = model.databaseuserid;
            config.dbpwd = model.databasepassword;
            config.directorypath = model.directorypath;

            gstmart.Configurations.Add(config);
            int res=  gstmart.SaveChanges();
            if(res==1)
            {
                return true;

            }
            else
            {
                return false;
            }
        }


        public List<AuditLogModel> SearchAuditUser(string parameter)
        {
            var Audits = new List<AuditLog>();
            if (parameter != "All")
            {
                Audits = (from p in gstmart.AuditLogs where p.UserId.Contains(parameter) || p.IpAddress.Contains(parameter) || p.Status.Contains(parameter) select p).ToList();
            }
            else
            {
                Audits = (from p in gstmart.AuditLogs select p).ToList();
            }
            var AuditmodelList = new List<AuditLogModel>();
            foreach (var item in Audits)
            {
                var auditmodel = new AuditLogModel();
                auditmodel.CreatedDate = item.CreatedDate.ToString();
                auditmodel.Id = item.Id;
                auditmodel.IpAddress = item.IpAddress;
                auditmodel.Message = item.Message;
                auditmodel.UserId = item.UserId;
                auditmodel.Name = item.Name;
                auditmodel.Status = item.Status;
                AuditmodelList.Add(auditmodel);
            }
            return AuditmodelList;

        }

        public string GetAdminName(string AdminId)
        {
            var Username = (from p in gstmart.Users where p.AdminID == AdminId select p).FirstOrDefault();
            return Username.Name;
        }


        public List<SelectListItem> CycleIDList(string originalConnectionString, string Db)
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "All Cycle Id", Value = "All Cycle Id" });
            try
            {

                var ecsBuilder = new EntityConnectionStringBuilder(originalConnectionString);
                var sqlCsBuilder = new SqlConnectionStringBuilder(ecsBuilder.ProviderConnectionString)
                {
                    InitialCatalog = Db
                };

                var providerConnectionString = sqlCsBuilder.ToString();
                ecsBuilder.ProviderConnectionString = providerConnectionString;

                string contextConnectionString = ecsBuilder.ToString();
                using (var db = new DbContext(contextConnectionString))
                {
                    var Gstmart = new GSTMARTEntities(contextConnectionString);
                    var query = (from p in Gstmart.Cycles where p.Status != "Audit Deleted" || p.Status ==null select p).ToList();
                    foreach (var item in query)
                    {
                        list.Add(new SelectListItem { Text = item.CycleID, Value = item.CycleID });
                    }
                    return list;

                }
            }

            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

            return list;
        }

        public List<Cycle> CycleList(string originalConnectionString, string Db)
        {
            var List = new List<Cycle>();
            try
            {

                var ecsBuilder = new EntityConnectionStringBuilder(originalConnectionString);
                var sqlCsBuilder = new SqlConnectionStringBuilder(ecsBuilder.ProviderConnectionString)
                {
                    InitialCatalog = Db
                };

                var providerConnectionString = sqlCsBuilder.ToString();
                ecsBuilder.ProviderConnectionString = providerConnectionString;

                string contextConnectionString = ecsBuilder.ToString();
                using (var db = new DbContext(contextConnectionString))
                {
                    var Gstmart = new GSTMARTEntities(contextConnectionString);
                    List = (from p in Gstmart.Cycles where p.Status != "Audit Deleted" || p.Status == null select p).ToList();
                    return List;
                }
            }

            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

            return List;
        }

        public List<Cycle> SearchCycleId(string parameter, string originalConnectionString, string Db)
        {
            var Cycles = new List<Cycle>();
            try
            {

                var ecsBuilder = new EntityConnectionStringBuilder(originalConnectionString);
                var sqlCsBuilder = new SqlConnectionStringBuilder(ecsBuilder.ProviderConnectionString)
                {
                    InitialCatalog = Db
                };

                var providerConnectionString = sqlCsBuilder.ToString();
                ecsBuilder.ProviderConnectionString = providerConnectionString;

                string contextConnectionString = ecsBuilder.ToString();
                using (var db = new DbContext(contextConnectionString))
                {
                    var Gstmart = new GSTMARTEntities(contextConnectionString);
                    if (parameter != "All Cycle Id")
                    {
                        Cycles = (from p in Gstmart.Cycles where p.CycleID.Contains(parameter) && (p.Status != "Audit Deleted" || p.Status == null) select p).ToList();
                    }
                    else
                    {
                        Cycles = (from p in Gstmart.Cycles where p.Status != "Audit Deleted" || p.Status == null select p).ToList();
                    }

                    return Cycles;
                }
            }

            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

            return Cycles;

        }

        public List<Cycle> SearchCycleByDate(string StartDate, string Enddate, string originalConnectionString, string Db)
        {


            var Cycle = new List<Cycle>();
            try
            {

                var ecsBuilder = new EntityConnectionStringBuilder(originalConnectionString);
                var sqlCsBuilder = new SqlConnectionStringBuilder(ecsBuilder.ProviderConnectionString)
                {
                    InitialCatalog = Db
                };

                var providerConnectionString = sqlCsBuilder.ToString();
                ecsBuilder.ProviderConnectionString = providerConnectionString;

                string contextConnectionString = ecsBuilder.ToString();
                using (var db = new DbContext(contextConnectionString))
                {
                    var Gstmart = new GSTMARTEntities(contextConnectionString);
                    if (StartDate == "" && Enddate != "")
                    {
                        DateTime EDate = Convert.ToDateTime(Enddate);
                        Cycle = (from p in Gstmart.Cycles where (p.CreatedDate <= EDate.Date) && (p.Status != "Audit Deleted" || p.Status == null) select p).ToList();
                    }
                    else if (StartDate != "" && Enddate == "")
                    {
                        DateTime SDate = Convert.ToDateTime(StartDate);
                        Cycle = (from p in Gstmart.Cycles where (p.CreatedDate >= SDate.Date) && (p.Status != "Audit Deleted" || p.Status == null) select p).ToList();
                    }
                    else
                    {
                        DateTime EDate = Convert.ToDateTime(Enddate);
                        EDate = EDate.AddHours(24);
                        DateTime SDate = Convert.ToDateTime(StartDate);
                        Cycle = (from p in Gstmart.Cycles where (p.CreatedDate >= SDate.Date && p.CreatedDate <= EDate.Date) && (p.Status != "Audit Deleted" || p.Status == null) select p).ToList();
                    }
                    return Cycle;
                }
            }

            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

            return Cycle;
        }

        public Cycle FindCycleById(int Id, string originalConnectionString, string Db)
        {
            var Cycles = new Cycle();
            try
            {

                var ecsBuilder = new EntityConnectionStringBuilder(originalConnectionString);
                var sqlCsBuilder = new SqlConnectionStringBuilder(ecsBuilder.ProviderConnectionString)
                {
                    InitialCatalog = Db
                };

                var providerConnectionString = sqlCsBuilder.ToString();
                ecsBuilder.ProviderConnectionString = providerConnectionString;

                string contextConnectionString = ecsBuilder.ToString();
                using (var db = new DbContext(contextConnectionString))
                {
                    var Gstmart = new GSTMARTEntities(contextConnectionString);
                    Cycles = (from p in Gstmart.Cycles where p.Id == Id select p).FirstOrDefault();

                    return Cycles;
                }
            }

            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

            return Cycles;

        }
        
        public User FindAdminEmail()
        {
            var Adminuser = new User();
            Adminuser = (from p in gstmart.Users where p.Usertype == "Admin" select p).FirstOrDefault();
            if (Adminuser != null)
            {
                return Adminuser;
            }
            return Adminuser;
        }
       

    }
}