using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GST_BLL.DTO;
using System.Data.Entity;
using GST_DB;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web;
using System.Configuration;

namespace GST_BLL.MasterAdmin
{
 
    public class MasterAdmin
    {
        GSTMARTEntities Gstmart = new GSTMARTEntities();
        static List<string> iList = new  List<string>();
        static List<string> industryList = new List<string>();


        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DbDetails"].ToString());
        public List<User> GetUsers()
        {
            var users = (from p in Gstmart.Users select p).ToList();
            return users;
        
        }
        public string GetUsersbyid(int id)
        {
            var users = (from p in Gstmart.Users where p.Id==id select p).SingleOrDefault();
            string userid = users.UserId;
            var qry=(from p in Gstmart.Users where p.UserId==userid select p).SingleOrDefault();
            return qry.Createdate;
            

        }

        public string SearchDublicateEmail(string Email)
        {
            var result = (from p in Gstmart.Users where p.Email == Email && p.Usertype == "Admin" select p).FirstOrDefault();
            if(result==null)
            {
                return "No";
            }
            else 
            {
                return "Yes";
            }
        }



        public int CreateCompany(string CompaneyId, string CompneyName, string Description, string Remarks,out string Exception)
        {

            try
            {

                Company company = new Company();
                company.CompanyName = CompneyName;
                company.CompanyID = CompaneyId;
                company.Permission = "Admin";
                company.Description = Description;
                company.Remarks = Remarks;
                company.CreatedDate = DateTime.Now.ToString();
                Gstmart.Companies.Add(company);
                Gstmart.SaveChanges();

                var Id = (from p in Gstmart.Companies where p.CompanyID == CompaneyId && p.CompanyName == CompneyName select p.Id).FirstOrDefault();
                Exception = "";
                return Id;
            }
            catch (Exception dbEx)
            {
                Exception = dbEx.ToString();
                return 0;
            }
            
        }

        public int UpdateCompany(string CompaneyId, string CompneyName, string Compneyid, string Description, string Remarks)
        {

            try
            {
                Int32 compId = Convert.ToInt32(Compneyid);
                var systemdata = (from p in Gstmart.GstSystems where p.CompanyId == compId select p).ToList();
                foreach (var item in systemdata)
                {
                    Gstmart.Entry(item).State = System.Data.EntityState.Deleted;
                    Gstmart.SaveChanges();
                }

                var industryId = (from p in Gstmart.Industries where p.CompanyId == compId select p).ToList();
                foreach (var item in industryId)
                {
                    Gstmart.Entry(item).State = System.Data.EntityState.Deleted;
                    Gstmart.SaveChanges();
                }

                var CompneyData = (from p in Gstmart.Companies where p.Id == compId select p).FirstOrDefault();
                if(CompneyData!=null)
                {
                    CompneyData.CompanyName = CompneyName;
                    CompneyData.CompanyID = CompaneyId;
                    CompneyData.Remarks = Remarks;
                    CompneyData.Description = Description;
                    Gstmart.Entry(CompneyData).State = System.Data.EntityState.Modified;
                    Gstmart.SaveChanges();
                }            
                var Id = (from p in Gstmart.Companies where p.CompanyID == CompaneyId && p.CompanyName == CompneyName select p.Id).FirstOrDefault();
                return Id;
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
            return 0;
        }

        public int CreateSystem(string ID, string SystemId, string SystemName)
        {
            try
            {
                GstSystem system = new GstSystem();
                system.CompanyId = Convert.ToInt32(ID);
                system.SystemID = SystemId;
                system.SystemName = SystemName;
                Gstmart.GstSystems.Add(system);
                Gstmart.SaveChanges();
                return 1;
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
            return 0;
        }


        public bool Check_duplicateEmail(string email)
        {
            var qry = (from p in Gstmart.Users where p.Email == email select p).Count();
            if (qry > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
      
        
        }


        public bool Check_duplicateAdminID(string AdminId)
        {
            var qry = (from p in Gstmart.Users where p.AdminID == AdminId select p).Count();
            if (qry > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
      

        public bool Check_duplicateCompanyID(string CompanyID)
        {
            var qry = (from p in Gstmart.Companies where p.CompanyID == CompanyID select p).Count();
            if (qry > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }


        public string DeleteSystems(string CompaneyId)
        {
            try
            {
                var Companey = (from p in Gstmart.Companies where p.CompanyID == CompaneyId select p).FirstOrDefault();
                var Systems = (from p in Gstmart.GstSystems where p.CompanyId == Companey.Id select p).ToList();
                foreach(var item in Systems)
                {
                    Gstmart.Entry(item).State = System.Data.EntityState.Deleted;
                    Gstmart.SaveChanges();
                }
                return "Success";
            }
            catch(Exception Ex)
            {
                return Ex.Message;
            }
        }


        public string DeleteIndustry(string CompaneyId)
        {
            try
            {
                var Companey = (from p in Gstmart.Companies where p.CompanyID == CompaneyId select p).FirstOrDefault();
                var Systems = (from p in Gstmart.Industries where p.CompanyId == Companey.Id select p).ToList();
                foreach (var item in Systems)
                {
                    Gstmart.Entry(item).State = System.Data.EntityState.Deleted;
                    Gstmart.SaveChanges();
                }
                return "Success";
            }
            catch (Exception Ex)
            {
                return Ex.Message;
            }
        }

        public bool Check_duplicateSystemID(string SystemID)
        {

            foreach (var item in iList)
            {

                if (item == SystemID)
                {

                    return false;
                }
                
            }
            iList.Add(SystemID);
            if (SystemID == "ForStaticListEmpty")
            {
                iList = new List<string>();
            }
            var qry = (from p in Gstmart.GstSystems where p.SystemID == SystemID select p).Count();
            if (qry > 0)
            {
                iList = new List<string>();
                return false;
            }
            else
            {
               // iList = new List<string>();
                return true;
            }
        }




        //check duplicate industry code


        public bool Check_duplicateIndustryid(string SystemID)
        {

            foreach (var item in industryList)
            {

                if (item == SystemID)
                {

                    return false;
                }

            }
            industryList.Add(SystemID);
            if (SystemID == "ForStaticListEmpty")
            {
                industryList = new List<string>();
            }
            var qry = (from p in Gstmart.Industries where p.Code == SystemID select p).Count();
            if (qry > 0)
            {
                industryList = new List<string>();
               return false;
           }
            else
            {
                // iList = new List<string>();
                return true;
            }
        }




        public bool Check_duplicateIndustryCode(string IndusCode)
        {
            var qry = (from p in Gstmart.Industries where p.Code == IndusCode select p).Count();
            if (qry > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<CreateCompaneyModel> getcompanydetails()
        {

            //var ob = (from u in Gstmart.Users
            //          join c in Gstmart.Companies on u.Id equals c.UserId

            //          join s in Gstmart.GstSystems on c.Id equals s.CompanyId

            //          select new { c }).ToList();
            var list = new List<CreateCompaneyModel>();
            var adminuser =(from u in Gstmart.Users where u.Usertype=="Admin"  select u).OrderByDescending(m=>m.Id).FirstOrDefault();
            if(adminuser==null)
            {
                return list;
            }
            var ob = (from c in Gstmart.Companies
                      join s in Gstmart.GstSystems on c.Id equals s.CompanyId
                      join i in Gstmart.Industries on c.Id equals i.CompanyId
                      select new { c }).OrderByDescending(m => m.c.Id).ToList();


            foreach (var item in ob.Distinct())
            {
                var model = new CreateCompaneyModel();

                model.Company = new CompanyModel();
          

                model.Company.CompanyName = item.c.CompanyName;
                model.Company.Permission = item.c.Permission;
                model.Company.Username = adminuser.Name;
                model.Company.CompanyId = item.c.CompanyID.ToString();
                string createddate = item.c.CreatedDate;
                string[] DateTime = createddate.Split(' ');
                model.Company.CreatedDate = DateTime[0] + ", " + DateTime[1] + " " + DateTime[2];
                model.Company.id = item.c.Id;

                model.Company.systemModel = new List<SystemModel>();

                foreach (var systemItem in item.c.GstSystems)
                {
                    var listSystem = new SystemModel();

                    listSystem.SystemId = systemItem.SystemID;
                    listSystem.SystemName = systemItem.SystemName;
                    model.Company.systemModel.Add(listSystem);
                
                }


               

                model.Company.userModel = new UserModel();

                model.Company.userModel.AdminID = adminuser.AdminID;
                model.Company.userModel.id = adminuser.Id;



                model.Company.industrymodel = new List<IndustryModel>();
                foreach (var idustry in item.c.Industries)
                {
                    var listindustry = new IndustryModel();
                    listindustry.Id = idustry.Id;
                    listindustry.Industrycode = idustry.Code;
                    listindustry.Companyid = idustry.CompanyId;
                    model.Company.industrymodel.Add(listindustry);

                  }
             

                list.Add(model);
            }

            return list;
        }

        public CreateCompaneyModel Findcompany(string CompanyId)
        {
            var model = new CreateCompaneyModel();
            var adminuser = (from u in Gstmart.Users where u.Usertype == "Admin" select u).FirstOrDefault();
            if(adminuser==null)
            {
                return model;
            }

            Int32 ID=Convert.ToInt32(CompanyId);
            var ob = (from c in Gstmart.Companies
                      join s in Gstmart.GstSystems on c.Id equals s.CompanyId
                      join p in Gstmart.Industries on c.Id equals p.CompanyId
                      where c.Id == ID
                      select new { c }).ToList();

            

            foreach (var item in ob.Distinct())
            {

                model.Company = new CompanyModel();
                model.Company.CompanyName = item.c.CompanyName;          
                model.Company.CompanyId = item.c.CompanyID.ToString();
                model.Company.id = item.c.Id;
                model.Company.Description = item.c.Description;
                model.Company.Remarks = item.c.Remarks;


                model.Company.systemModel = new List<SystemModel>();
                foreach (var systemItem in item.c.GstSystems)
                {
                    var listSystem = new SystemModel();

                    listSystem.SystemId = systemItem.SystemID;
                    listSystem.SystemName = systemItem.SystemName;
                    listSystem.Id = systemItem.Id;
                    model.Company.systemModel.Add(listSystem);

                }
                model.Company.industrymodel = new List<IndustryModel>();
                foreach (var idustry in item.c.Industries)
                {
                    var listindustry = new IndustryModel();
                    listindustry.Id = idustry.Id;
                    listindustry.Industrycode = idustry.Code;
                    listindustry.Companyid = idustry.CompanyId;
                    model.Company.industrymodel.Add(listindustry);



                }
                model.Company.userModel = new UserModel();
                model.Company.userModel.id = adminuser.Id;
                model.Company.userModel.AdminID = adminuser.AdminID;
                model.Company.userModel.Name = adminuser.Name;
                model.Company.userModel.Password = adminuser.Password;
                model.Company.userModel.ConfirmPwd = adminuser.Password;
                model.Company.userModel.MobileNumber = adminuser.Mobilenumber;
                model.Company.userModel.CnfrmMobileNumber = adminuser.Mobilenumber;
                model.Company.userModel.Email = adminuser.Email;
                model.Company.userModel.ConfirmEmail = adminuser.Email;
                model.Company.userModel.Createddate = adminuser.Createdate; 


            }

            return model;
        }

        [Obsolete]
        public void ClearModel()
        {
            var model = new CreateCompaneyModel();
            model.Company = new CompanyModel();
            model.System = new SystemModel();
            model.User = new UserModel();

            model.Company.CompanyId = string.Empty;
            model.Company.CompanyName = string.Empty;
            model.Company.Permission = string.Empty;
            model.Company.Username = string.Empty;

            model.User.AdminID = string.Empty;
            model.User.CnfrmMobileNumber = string.Empty;
            model.User.ConfirmEmail = string.Empty;
            model.User.ConfirmPwd = string.Empty;
            model.User.Email = string.Empty;
            model.User.MobileNumber = string.Empty;
            model.User.Name = string.Empty;


            model.System.SystemId = string.Empty;
            model.System.SystemName = string.Empty;

        }
      
 
        public bool DeleteCompany(int companyId)
        {

            try
            {
               

                var systemData = (from s in Gstmart.GstSystems where s.CompanyId == companyId select s).ToList();

                foreach (var item in systemData)
                {
                    var data = Gstmart.GstSystems.FirstOrDefault(x => x.CompanyId == companyId);
                    Gstmart.GstSystems.Remove(data);
                    Gstmart.SaveChanges();
                }

                var industryId = (from p in Gstmart.Industries where p.CompanyId == companyId select p).ToList();
                foreach (var item in industryId)
                {
                    Gstmart.Entry(item).State = System.Data.EntityState.Deleted;
                    Gstmart.SaveChanges();
                }

                var ComanyData = Gstmart.Companies.FirstOrDefault(x => x.Id == companyId);
                if (ComanyData != null)
                {
                    Gstmart.Companies.Remove(ComanyData);
                    Gstmart.SaveChanges();

                }




                return true;


            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public List<Company> GetCompanyByID(int comapnyid)
        {
            var qry = (from p in Gstmart.Companies where p.Id == comapnyid select p).ToList();
            return qry;

        }


        [Obsolete]
        public bool CreateAdminDatbase(string CompaneyId)
        {
            try
            {
             
                string strdbname = CompaneyId.Replace(" ", String.Empty);
                string strCreatecmd = "create database " + strdbname + "";
                SqlCommand cmd = new SqlCommand(strCreatecmd, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string UpdateAdmin(string AdminID, string AdminName, string Password, string Email, string Mobile, int Id)
        {
            try
            {
                var UserData = (from p in Gstmart.Users where p.Id == Id select p).FirstOrDefault();
                if (UserData != null)
                {
                    UserData.Name = AdminName;
                    UserData.Password = Password;
                    UserData.Email = Email;
                    UserData.Createdate = DateTime.Now.ToString();
                    UserData.Mobilenumber = Mobile;
                    UserData.AdminID = AdminID;
                    Gstmart.Entry(UserData).State = System.Data.EntityState.Modified;
                    Gstmart.SaveChanges();
                }
                return "Admin Data Updated Successfully";
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
            return "Database failure";
        }

        public string CreateAdmin(string AdminID, string AdminName, string Password, string Email, string Mobile)
        {

            try
            {

                User user = new User();
                user.Name = AdminName;
                user.Password = Password;
                user.Status = "Active";
                user.Usertype = "Admin";
                user.Email = Email;
                user.Createdate = DateTime.Now.ToString();
                user.Mobilenumber = Mobile;
                user.AdminID = AdminID;
                Gstmart.Users.Add(user);
                Gstmart.SaveChanges();

                return "Admin Data Inserted Successfully";
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
            return "Database failure";
        }

        public int CreateIndustry(string ID, string industrycode)
        {
            try
            {
                Industry indus = new Industry();
                indus.Code = industrycode;
                indus.CompanyId = Convert.ToInt32(ID);                  
                Gstmart.Industries.Add(indus);
                Gstmart.SaveChanges();
                return 1;
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
            return 0;
        }



    }
}
    
