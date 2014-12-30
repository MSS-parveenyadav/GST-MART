using GST_BLL.DTO;
using GST_BLL.Enum;
using GST_DB;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GST_BLL.AuthorizedUser
{
    public class AuthorizedUser
    {
        public bool CheckLogin(AdminUserModel model, string companylist, string Db, out string UserId, out string UserName,out string Message)
        {
            try
            {
                //var ecsBuilder = new EntityConnectionStringBuilder(originalConnectionString);
                //var sqlCsBuilder = new SqlConnectionStringBuilder(ecsBuilder.ProviderConnectionString)
                //{
                //    InitialCatalog = Db
                //};

                //var providerConnectionString = sqlCsBuilder.ToString();
                //ecsBuilder.ProviderConnectionString = providerConnectionString;

                //string contextConnectionString = ecsBuilder.ToString();
                //using (var db = new DbContext(contextConnectionString))
                //{
                   // var Gstmart = new GSTMARTEntities(contextConnectionString);
                    var Gstmart = new GSTMARTEntities();

                    var User = (from p in Gstmart.Users
                                 where p.UserId == model.UserId && p.Password == model.Password && p.Status=="Active"
                                 select p).FirstOrDefault();
                    if (User!=null)
                    {
                        
                            var CompaneyList = (from p in Gstmart.CompanyUsers where p.UID == User.UserId && p.CID == companylist   select p).ToList();
                            if (CompaneyList.Count > 0)
                            {

                                //****** Update Last Login Date ******//
                                User.LastLoginDate = DateTime.Now.ToString();
                                Gstmart.Entry(User).State = System.Data.EntityState.Modified;
                                Gstmart.SaveChanges();
                                //******End of Update Last Login Date ******//
                                Message = "Successfull Login";
                                UserName=User.Name;
                                UserId = User.UserId;
                                return true;
                                
                            }
                            Message = "Companey Not Found";
                            UserName=User.Name;
                            UserId = "";
                            return false;
                    }
                        
                    else
                    {
                        Message = "Crediantial Not Match";
                        UserName="";
                        UserId = "";
                        return false;
                    }

            }

            catch (Exception ex)
            {
                Message = ex.ToString();
                UserName="";
                UserId = "";
                return false;
            }
  
        }

        public string GetemailbyUserId(string UserId, string originalConnectionString, string Db)
        {
            var ecsBuilder = new EntityConnectionStringBuilder(originalConnectionString);
            var sqlCsBuilder = new SqlConnectionStringBuilder(ecsBuilder.ProviderConnectionString)
            {
                InitialCatalog = Db
            };

            //var providerConnectionString = sqlCsBuilder.ToString();
            //ecsBuilder.ProviderConnectionString = providerConnectionString;

            //string contextConnectionString = ecsBuilder.ToString();
            //using (var db = new DbContext(contextConnectionString))
            //{
            //    var Gstmart = new GSTMARTEntities(contextConnectionString);
              var Gstmart = new GSTMARTEntities();
                string email = "";
               
                var query = (from p in Gstmart.Users where p.UserId == UserId select p).ToList();
                foreach (var item in query)
                {
                    email = item.Email;
                }
                return email;

            //}
        }

        public List<GstSystem> GetSystemID(string companyid)
        { 
        var GST_MART=new GSTMARTEntities();
        var companydetail = (from p in GST_MART.Companies where p.CompanyID == companyid select p).SingleOrDefault();
        var id = companydetail.Id;
        var systemID = GST_MART.GstSystems.Where(m=>m.CompanyId==id).ToList();
        return systemID;
        
        }


        public bool AddCycle(CycleModel model, string originalConnectionString, string Db,string UserId)
        {

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

                    var cycle = new Cycle();

                    cycle.CreatedDate = DateTime.Now;

                    cycle.Company = model.CPath;
                    cycle.Description = model.Description;
                    cycle.Footer = model.FPath;
                    cycle.Ledger = model.Lpath;
                    cycle.Purchase = model.Ppath;
                    cycle.Supply = model.Spath;
                    cycle.SystemId = model.SystemId;
                    cycle.CycleID = model.CycleID;
                    cycle.UserId = UserId;
                    Gstmart.Cycles.Add(cycle);
                    Gstmart.SaveChanges();
                }
                return true;
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
                return false;
            }
        }

        public int GetLstCycleNumber(string originalConnectionString,string Db)
        {
            int result=0;
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

                   var data =  (from p in Gstmart.Cycles select p).OrderByDescending(mbox => mbox.Id).First();
                   result = Convert.ToInt32(data.CycleID);
                    //result = Gstmart.Cycles.LastOrDefault().CycleID;
                }
            }
            catch (Exception ex)
            {
              
                return 0;
            
            }
            return result;

        }

        public string FindUser(string Email)
        {
            
                var Gstmart = new GSTMARTEntities();
                string email = "";
                var query = (from p in Gstmart.Users where p.Email == Email && p.Usertype == "Normal User" select p).ToList();
                foreach (var item in query)
                {
                    email = item.Email;
                }
                return email;

        }

        public  UserModel FindUserbyUserid(string userid)
        {
            
                var Gstmart = new GSTMARTEntities();
                var query = (from p in Gstmart.Users where p.UserId == userid select p).FirstOrDefault();
            var user=new UserModel();
            user.id = query.Id;
            user.Email=query.Email;
            user.Name=query.Name;

         return user;
             
        
        }


        public UserModel FindUserByEmail(string Email, string Guid, out string message)
        {
                var Gstmart = new GSTMARTEntities();
                UserModel User = new UserModel();
                message = "";
                var query = (from p in Gstmart.Users where p.Email == Email && p.Usertype == "Normal User" && p.Guid == Guid select p).FirstOrDefault();
                if (query == null)
                {
                    message = EnumClass.MessageFamily.Error.ToString();
                    return User;

                }
  
           
                User.id = query.Id;
                User.Name = query.Name;
                User.Password = query.Password;
                User.ConfirmPwd = query.Password;
                return User;

               
            
        }

        public string GenerateLink(string URL, string email, string uniq)
        {


            var Gstmart = new GSTMARTEntities();

            var query = (from p in Gstmart.Users where p.Email == email && p.Usertype == "Normal User" select p).FirstOrDefault();
            if (query != null)
            {

                query.Guid = uniq;
                Gstmart.Entry(query).State = System.Data.EntityState.Modified;
                Gstmart.SaveChanges();
            }

            var url = URL + "/User/ChangePassword?Email=" + email + "&&Guid=" + uniq;
            string ResetLink = "<br /><br />Please click the following link to Reset your Password <a href = '" + url + "'>Click here</a>";




           
            return ResetLink;
        }
        
       
        public int UpdatePassword(UserModel Data,out string Email,out string Username)
        {
            
                var Gstmart = new GSTMARTEntities();
                var User = (from p in Gstmart.Users where p.Id == Data.id && p.Usertype == "Normal User" select p).FirstOrDefault();
                if (User != null)
                {
                    User.Password = Data.Password;
                      User.Guid = "";
                    Gstmart.Entry(User).State = System.Data.EntityState.Modified;
                    Gstmart.SaveChanges();
                    
                    Email=User.Email;
                    Username = User.Name;
                    return 1;
                }
                Email ="";
                Username = "";
                return 2;
        }

        public List<CycleModel> GetImportProcess(string originalConnectionString,string Db,string companyID,out string error)
        {
                var list =new List<CycleModel>();
                error = "";
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

                        var data = Gstmart.Cycles.ToList();
                        if(data==null)
                        {
                            return list;
                        }

                     
                        
                        
                        foreach (var item in data)
                        {
                            var model = new CycleModel();
                            var countCycleErrors = CountCycleErrors(item.CycleID, companyID);
                            var duplicateRecords = CountDuplicate(item.CycleID, companyID);
                            var missingRecords = CountMissingFields(item.CycleID, companyID);
                            model.CPath = item.Company;
                            model.CreatedDate = item.CreatedDate;
                            model.CycleID = item.CycleID;
                            model.Description = item.Description;
                            model.FPath = item.Footer;
                            model.id = item.Id;
                            model.Lpath = item.Ledger;
                            model.Ppath = item.Purchase;
                            model.Spath = item.Supply;
                            model.SystemId = item.SystemId;
                            model.Status = item.Status;
                            model.CycleErrors = countCycleErrors;
                            model.DuplicateRecords = duplicateRecords;
                            model.MissingRecords = missingRecords;
                            list.Add(model);
                        
                        }
                    
                        return list.OrderByDescending(x=>x.id).ToList();
                    }
                }
            catch(Exception ex)
                {
             
                if(ex.Message.ToString().Contains("The underlying provider failed on Open"))
                {
                    error = "error";
                }
             
                    return list;
                }
               // catch (DbEntityValidationException dbEx)
                //{
                //    foreach (var validationErrors in dbEx.EntityValidationErrors)
                //    {
                //        foreach (var validationError in validationErrors.ValidationErrors)
                //        {
                //            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                //        }
                //    }
                //    return list;
                //}

        }


        public bool TempDeleteCycle(string originalConnectionString, string Db,int id,string status)
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
                        //var query = (from p in Gstmart.Cycles where p.Id == id select p).FirstOrDefault();
                        Cycle cycle = Gstmart.Cycles.Where(x => x.Id == id).Single<Cycle>();
                         
                            cycle.Status=status;
                            Gstmart.Entry(cycle).State = System.Data.EntityState.Modified;
                            Gstmart.SaveChanges();
                            return true;
                        
                        return false;
                    }
        }

        


      
        public string CountCycleErrors(string cycleID,string companyID)
        {
            var list = new List<SchedularModel>();

            try
            {
                var stageDB = new STAGEDBGSTMASRTEntities();
                var data=(from p in stageDB.ErrorLoggers where p.CycleID==cycleID  && p.CompanyID==companyID select p).Count();
               
                if(data==0 || data ==null)
                {
                    data = 0;
                }
             
                return data.ToString();
               
            }
            catch (Exception ex)
            {
              
                return "Fail";
            }

        }



        #region Read Errors
        //Read Ledger Errors
        public List<LoogerModel> ReadRrrors(string cycleID, string companyID)
        {
            var list = new List<LoogerModel>();

            try

            {
                var stageDB = new STAGEDBGSTMASRTEntities();
                var data = (from p in stageDB.ErrorLoggers where p.CycleID == cycleID && p.CompanyID == companyID select p);

               
                foreach (var item in data)
                {
                    var model = new LoogerModel();
                    model.ErrorCode = item.ErrorCode;
                    model.ErrorColumn = item.ErrorColumn;
                    model.Flat_File_Source_Error_Output_Column = item.Flat_File_Source_Error_Output_Column;
                 
                    list.Add(model);
                }
                return list;
            }
            catch(Exception ex)
            {

                throw ex;
            }

            


        }

        //Read Footer Errors
        public List<LoogerModel> ReadFooterRrrors(string cycleID, string companyID)
        {
            var list = new List<LoogerModel>();
            var id=Convert.ToInt64(cycleID);
            try
            {
                var stageDB = new STAGEDBGSTMASRTEntities();
                var data = (from p in stageDB.tbl_GSTErrorFooter where p.CycleID ==id  && p.CompanyID == companyID select p);


                foreach (var item in data)
                {
                    var model = new LoogerModel();
                    model.ErrorCode = item.ErrorCode;
                    model.ErrorColumn = item.ErrorColumn;
                    model.Flat_File_Source_Error_Output_Column = item.Flat_File_Source_Error_Output_Column;

                    list.Add(model);
                }
                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }




        }
        //Read Company Errors
        public List<LoogerModel> ReadCompanyRrrors(string cycleID, string companyID)
        {
            var list = new List<LoogerModel>();
            var id=Convert.ToInt64(cycleID);
            try
            {
                var stageDB = new STAGEDBGSTMASRTEntities();
                var data = (from p in stageDB.tbl_GSTErrorCompany where p.CycleID ==id  && p.CompanyID == companyID select p);


                foreach (var item in data)
                {
                    var model = new LoogerModel();
                    model.ErrorCode = item.ErrorCode;
                    model.ErrorColumn = item.ErrorColumn;
                    model.Flat_File_Source_Error_Output_Column = item.Flat_File_Source_Error_Output_Column;

                    list.Add(model);
                }
                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }




        }

        //Read Supply Errors
        public List<LoogerModel> ReadSupplyRrrors(string cycleID, string companyID)
        {
            var list = new List<LoogerModel>();
            var id=Convert.ToInt64(cycleID);
            try
            {
                var stageDB = new STAGEDBGSTMASRTEntities();
                var data = (from p in stageDB.tbl_GSTSupplyError where p.CycleID ==id  && p.CompanyID == companyID select p);


                foreach (var item in data)
                {
                    var model = new LoogerModel();
                    model.ErrorCode = item.ErrorCode;
                    model.ErrorColumn = item.ErrorColumn;
                    model.Flat_File_Source_Error_Output_Column = item.Flat_File_Source_Error_Output_Column;

                    list.Add(model);
                }
                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }




        }

        //Read Purchase Errors
        public List<LoogerModel> ReadPurchaseRrrors(string cycleID, string companyID)
        {
            var list = new List<LoogerModel>();

            try
            {
                var id= Convert.ToInt64(cycleID);
                var stageDB = new STAGEDBGSTMASRTEntities();
                var data = (from p in stageDB.tbl_GSTPurchaseError where p.CycleID ==id && p.CompanyID == companyID select p);


                foreach (var item in data)
                {
                    var model = new LoogerModel();
                    model.ErrorCode = item.ErrorCode;
                    model.ErrorColumn = item.ErrorColumn;
                    model.Flat_File_Source_Error_Output_Column = item.Flat_File_Source_Error_Output_Column;

                    list.Add(model);
                }
                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }




        }

        #endregion


        #region Duplicate Records

        public string CountDuplicate(string cycleID, string companyID)
        {
            var list = new List<SchedularModel>();

            try
            {
                var stageDB = new STAGEDBGSTMASRTEntities();

                var cycle= Convert.ToInt64(cycleID);

                var purchase = (from q in stageDB.tbl_GSTDupPurchase where q.Cycle_ID == cycle  && q.CompanyID == companyID select q).Count();


                var supply = (from q in stageDB.tbl_GSTDupSupply where q.Cycle_ID == cycle && q.CompanyID == companyID select q).Count();


                var ledger = (from q in stageDB.tbl_DupLedger where q.CycleID == cycle && q.CompanyID == companyID select q).Count();

                var company = (from q in stageDB.tbl_GSTDupCompany where q.CycleID == cycle && q.CompanyID == companyID select q).Count();

              


                var countDuplicate = purchase + supply+ledger;

                return countDuplicate.ToString();

            }
            catch (Exception ex)
            {

                return "Fail";
            }

        }
        public List<LedgerModel> ReadDuplicateLedgerErrors(string cycleID, string companyID)
        {
            var list = new List<LedgerModel>();
            var id= Convert.ToInt64(cycleID);
            try
            {
                var stageDB = new STAGEDBGSTMASRTEntities();
                var ledger = (from p in stageDB.tbl_DupLedger where p.CycleID == id && p.CompanyID == companyID select p);


                foreach (var item in ledger)
                {
                    var model = new LedgerModel();
                    model.AccountID = item.Account_ID;
                    model.AccountName = item.Account_Name;
                    model.Balance = item.Balance;
                    model.CompanyCode = item.Company_Code;
                    model.CompanyID = item.CompanyID;
                    model.CreatedDate = item.CreatedDate;
                    model.Credit = item.Credit;
                    model.CycleID = item.CycleID;
                    model.Debit = item.Debit;
                    model.id = item.Ledger_ID;
                    model.Name = item.Name;
                    model.SourceDocumentID = item.Source_Document_ID;
                    model.SourceType = item.Source_Type;
                    model.TaxCode = item.Tax_Code;
                    model.TransactionDate = item.Transaction_Date;
                    model.TransactionDescription = item.Transaction_Description;
                    model.TransactionID = item.Transaction_ID;
                    model.TransactionType = item.Transaction_Type;
                
                    list.Add(model);
                }
                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public List<PurchaseModel> ReadPurchaseDuplicateErrors(string cycleID, string companyID)
        {
            var list = new List<PurchaseModel>();
            var id= Convert.ToInt64(cycleID);
            try
            {
                var stageDB = new STAGEDBGSTMASRTEntities();
                var purchase = (from p in stageDB.tbl_GSTDupPurchase where p.Cycle_ID == id && p.CompanyID == companyID select p);


                foreach (var item in purchase)
                {
                    var model = new PurchaseModel();

                    model.Company_Code = item.Company_Code;
                    model.CompanyID = item.CompanyID;
                    model.CreateDate = item.CreateDate;
                    model.CycleID = item.Cycle_ID;
                    model.Foreign_Currency_Code = item.Foreign_Currency_Code;
                    model.Import_Declaration_No = item.Import_Declaration_No;
                    model.Invoice_Date = item.Invoice_Date;
                    model.Invoice_No = item.Invoice_No;
                    model.Line_No = item.Line_No;
                    model.MTH = item.Invoice_Date.Value.Month;
                    model.Product_Code = item.Product_Code;
                    model.Product_Description = item.Product_Description;
                    model.Purchase_Currency_Purchase_Amount_GST = item.Purchase_Currency_Purchase_Amount_GST;
                    model.Purchase_Foreign_Currency_Amount = item.Purchase_Foreign_Currency_Amount;
                    model.Purchase_ID = item.Purchase_ID;
                    model.Purchase_Value_GST_Amount = item.Purchase_Value_GST_Amount;
                    model.Purchase_Value_MYR = item.Purchase_Value_MYR;
                    model.Record_Identifier = item.Record_Identifier;
                    model.Supplier_BRN = item.Supplier_BRN;
                    model.Supplier_Name = item.Supplier_Name;
                    model.Supplier_Number = item.Supplier_Number;
                    model.Tax_Code = item.Tax_Code;
                    model.Transaction_Type = item.Transaction_Type;
                    model.YR = item.Invoice_Date.Value.Month;

                    list.Add(model);
                }
                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public List<SupplyModel> ReadSupplyDuplicateErrors(string cycleID, string companyID)
        {
            var list = new List<SupplyModel>();
            var id=Convert.ToInt64(cycleID) ;
            try
            {
                var stageDB = new STAGEDBGSTMASRTEntities();
                var supply = (from p in stageDB.tbl_GSTDupSupply where p.Cycle_ID == id && p.CompanyID == companyID select p);


                foreach (var item in supply)
                {
                    var model = new SupplyModel();

                    model.Company_Code = item.Company_Code;
                    model.CompanyID = item.CompanyID;
                    model.Country = item.Country;
                    model.Customer_BRN = item.Customer_BRN;
                    model.Customer_Code = item.Customer_Code;
                    model.Customer_Name = item.Customer_Name;
                    model.Cycle_ID = item.Cycle_ID;
                    //model.Destination_Export = item.Destination_Export;
                    model.Foreign_Currency_Code = item.Foreign_Currency_Code;
                    model.Invoice_Date = item.Invoice_Date;
                    model.Invoice_No = item.Invoice_No;
                    model.Line_No = item.Line_No;
                    model.MTH = item. Invoice_Date.Value.Month;
                    model.Product_Code = item.Product_Code;
                    model.Product_Description = item.Product_Description;
                    model.Record_Identifier = item.Record_Identifier;
                    model.Sales_Currency_Sales_Amount_GST = item.Sales_Currency_Sales_Amount_GST;
                    model.Sales_Foreign_Currency_Amount = item.Sales_Foreign_Currency_Amount;
                    model.Sales_Value_GST_Amount = item.Sales_Value_GST_Amount;
                    model.Sales_Value_MYR = item.Sales_Value_MYR;
                    model.Supply_ID = item.Supply_ID;
                    model.Tax_Code = item.Tax_Code;
                    model.Transaction_Type = item.Transaction_Type;
                    model.YR = item.Invoice_Date.Value.Year;


                    list.Add(model);
                }
                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public List<CompanyErrorModel> ReadCompanyDuplicateErrors(string cycleID, string companyID)
        {

            var list = new List<CompanyErrorModel>();
            var id=Convert.ToInt64(cycleID);
            try
            {
                var stageDB = new STAGEDBGSTMASRTEntities();
                var supply = (from p in stageDB.tbl_GSTDupCompany where p.CycleID == id && p.CompanyID == companyID select p);


                foreach (var item in supply)
                {
                    var model = new CompanyErrorModel();

                    model.Company_BRN = item.Company_BRN;
                    model.Company_Code = item.Company_Code;
                    model.Company_GST_No = item.Company_GST_No;
                    model.Company_Name = item.Company_Name;
                    model.CompanyID = item.CompanyID;
                    model.CycleID = item.CycleID;
                    model.File_Creation_Date = item.File_Creation_Date;
                    model.GAF_Version = item.GAF_Version;
                    model.Id = item.Id;
                    model.Period_End = item.Period_End;
                    model.Period_Start = item.Period_Start;
                    model.Product_Version = item.Product_Version;
                    model.Record_Identifier = item.Record_Identifier;



                    list.Add(model);
                }
                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<FooterModel> ReadFooterDuplicateErrors(string cycleID, string companyID)
        {
            var list = new List<FooterModel>();
            var id=Convert.ToInt64(cycleID);
            try
            {
                var stageDB = new STAGEDBGSTMASRTEntities();
                var supply = (from p in stageDB.tbl_GSTFooter where p.CycleID ==id  && p.CompanyID == companyID select p);


                foreach (var item in supply)
                {
                    var model = new FooterModel();

                    model.Balance_Sum = item.Balance_Sum;
                    model.Company_Code = item.Company_Code;
                    model.CompanyID = item.CompanyID;
                    model.Credit_Sum = item.Credit_Sum;
                    model.CycleID = item.CycleID;
                    model.Debit_Sum = item.Debit_Sum;
                    model.id = item.id;
                    model.Ledger_Count = item.Ledger_Count;
                    model.Purchase_Amount_Sum = item.Purchase_Amount_Sum;
                    model.Purchase_Count = item.Purchase_Count;
                    model.Purchase_GST_Amount_GST = item.Purchase_GST_Amount_GST;
                    model.Record_Identifier = item.Record_Identifier;
                    model.Supply_Amount_Sum = item.Supply_Amount_Sum;
                    model.Supply_Count = item.Supply_Count;
                    model.Supply_GST_Amount_Sum=item.Supply_GST_Amount_Sum;
                   




                    list.Add(model);
                }
                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        
        }
        #endregion

        
        #region Missing Fields
        public string CountMissingFields(string cycleID, string companyID)
        {
            var ID = Convert.ToInt64(cycleID);
    

            try
            {
                var stageDB = new STAGEDBGSTMASRTEntities();

                var purchase = (from q in stageDB.tbl_GSTMissingPurchase where q.Cycle_ID == ID && q.CompanyID == companyID select q).Count();

                var supply = (from q in stageDB.tbl_GSTMissingSuplly where q.Cycle_ID == ID && q.CompanyID == companyID select q).Count();

                var ledger = (from q in stageDB.tbl_MissingLedger where q.CycleID == ID && q.CompanyID == companyID select q).Count();

                var company = (from q in stageDB.tbl_GSTErrorCompany where q.CycleID == ID && q.CompanyID == companyID select q).Count();

                var footer = (from q in stageDB.tbl_GSTMissingFooter where q.CycleID == ID && q.CompanyID == companyID select q).Count();

                 var countMissing = purchase + supply + ledger + company+footer;

                return countMissing.ToString();

            }
            catch (Exception ex)
            {

                return "Fail";
            }

        }

        public List<LedgerModel> ReadMissingLedgerErrors(string cycleID, string companyID)
        {
            var list = new List<LedgerModel>();
            var id=Convert.ToInt64(cycleID);
            try
            {
                var stageDB = new STAGEDBGSTMASRTEntities();
                var ledger = (from p in stageDB.tbl_MissingLedger where p.CycleID ==id  && p.CompanyID == companyID select p);


                foreach (var item in ledger)
                {
                    var model = new LedgerModel();
                    model.AccountID = item.Account_ID;
                    model.AccountName = item.Account_Name;
                    model.Balance = item.Balance;
                    model.CompanyCode = item.Company_Code;
                    model.CompanyID = item.CompanyID;
                    model.CreatedDate = item.CreatedDate;
                    model.Credit = item.Credit;
                    model.CycleID = item.CycleID;
                    model.Debit = item.Debit;
                    model.id = item.Ledger_ID;
                    model.Name = item.Name;
                    model.SourceDocumentID = item.Source_Document_ID;
                    model.SourceType = item.Source_Type;
                    model.TaxCode = item.Tax_Code;
                    model.TransactionDate = item.Transaction_Date;
                    model.TransactionDescription = item.Transaction_Description;
                    model.TransactionID = item.Transaction_ID;
                    model.TransactionType = item.Transaction_Type;
                    list.Add(model);
                }
                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public List<PurchaseModel> ReadPurchaseMissingErrors(string cycleID, string companyID)
        {
            var list = new List<PurchaseModel>();
            var id = Convert.ToInt64(cycleID);
            try
            {
                var stageDB = new STAGEDBGSTMASRTEntities();
                var purchase = (from p in stageDB.tbl_GSTMissingPurchase where p.Cycle_ID == id && p.CompanyID == companyID select p);


                foreach (var item in purchase)
                {
                    var model = new PurchaseModel();
                    model.Company_Code = item.Company_Code;
                    model.CompanyID = item.CompanyID;
                    model.CreateDate = item.CreateDate;
                    model.CycleID = item.Cycle_ID;
                    model.Foreign_Currency_Code = item.Foreign_Currency_Code;
                    model.Import_Declaration_No = item.Import_Declaration_No;
                    model.Invoice_Date = item.Invoice_Date;
                    model.Invoice_No = item.Invoice_No;
                    model.Line_No = item.Line_No;
                    model.MTH = item.Invoice_Date.Value.Month;
                    model.Product_Code = item.Product_Code;
                    model.Product_Description = item.Product_Description;
                    model.Purchase_Currency_Purchase_Amount_GST = item.Purchase_Currency_Purchase_Amount_GST;
                    model.Purchase_Foreign_Currency_Amount = item.Purchase_Foreign_Currency_Amount;
                    model.Purchase_ID = item.Purchase_ID;
                    model.Purchase_Value_GST_Amount = item.Purchase_Value_GST_Amount;
                    model.Purchase_Value_MYR = item.Purchase_Value_MYR;
                    model.Record_Identifier = item.Record_Identifier;
                    model.Supplier_BRN = item.Supplier_BRN;
                    model.Supplier_Name = item.Supplier_Name;
                    model.Supplier_Number = item.Supplier_Number;
                    model.Tax_Code = item.Tax_Code;
                    model.Transaction_Type = item.Transaction_Type;
                    model.YR = item.Invoice_Date.Value.Month;

                    list.Add(model);
                }
                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public List<SupplyModel> ReadSupplyMissingErrors(string cycleID, string companyID)
        {
            var list = new List<SupplyModel>();
            var id = Convert.ToInt64(cycleID);
            try
            {
                var stageDB = new STAGEDBGSTMASRTEntities();
                var supply = (from p in stageDB.tbl_GSTMissingSuplly where p.Cycle_ID == id && p.CompanyID == companyID select p);


                foreach (var item in supply)
                {
                    var model = new SupplyModel();


                    model.Company_Code = item.Company_Code;
                    model.CompanyID = item.CompanyID;
                    model.Country = item.Country;
                    model.Customer_BRN = item.Customer_BRN;
                    model.Customer_Code = item.Customer_Code;
                    model.Customer_Name = item.Customer_Name;
                    model.Cycle_ID = item.Cycle_ID;
                    //model.Destination_Export = item.Destination_Export;
                    model.Foreign_Currency_Code = item.Foreign_Currency_Code;
                    model.Invoice_Date = item.Invoice_Date;
                    model.Invoice_No = item.Invoice_No;
                    model.Line_No = item.Line_No;
                    model.MTH = item.Invoice_Date.Value.Month;
                    model.Product_Code = item.Product_Code;
                    model.Product_Description = item.Product_Description;
                    model.Record_Identifier = item.Record_Identifier;
                    model.Sales_Currency_Sales_Amount_GST = item.Sales_Currency_Sales_Amount_GST;
                    model.Sales_Foreign_Currency_Amount = item.Sales_Foreign_Currency_Amount;
                    model.Sales_Value_GST_Amount = item.Sales_Value_GST_Amount;
                    model.Sales_Value_MYR = item.Sales_Value_MYR;
                    model.Supply_ID = item.Supply_ID;
                    model.Tax_Code = item.Tax_Code;
                    model.Transaction_Type = item.Transaction_Type;
                    model.YR = item.Invoice_Date.Value.Year;


                    list.Add(model);
                }
                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public List<CompanyErrorModel> ReadCompanyMissingErrors(string cycleID, string companyID)
        {

            var list = new List<CompanyErrorModel>();
            var id = Convert.ToInt64(cycleID);
            try
            {
                var stageDB = new STAGEDBGSTMASRTEntities();
                var supply = (from p in stageDB.tbl_GSTMissingCompany where p.CycleID == id && p.CompanyID == companyID select p);


                foreach (var item in supply)
                {
                    var model = new CompanyErrorModel();

                    model.Company_BRN = item.Company_BRN;
                    model.Company_Code = item.Company_Code;
                    model.Company_GST_No = item.Company_GST_No;
                    model.Company_Name = item.Company_Name;
                    model.CompanyID = item.CompanyID;
                    model.CycleID = item.CycleID;
                    model.File_Creation_Date = item.File_Creation_Date;
                    model.GAF_Version = item.GAF_Version;
                    model.Id = item.Id;
                    model.Period_End = item.Period_End;
                    model.Period_Start = item.Period_Start;
                    model.Product_Version = item.Product_Version;
                    model.Record_Identifier = item.Record_Identifier;



                    list.Add(model);
                }
                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<FooterModel> ReadFooterMissingErrors(string cycleID, string companyID)
        {
            var list = new List<FooterModel>();
            var id = Convert.ToInt64(cycleID);
            try
            {
                var stageDB = new STAGEDBGSTMASRTEntities();
                var supply = (from p in stageDB.tbl_GSTMissingFooter where p.CycleID == id && p.CompanyID == companyID select p);


                foreach (var item in supply)
                {
                    var model = new FooterModel();

                    model.Balance_Sum = item.Balance_Sum;
                    model.Company_Code = item.Company_Code;
                    model.CompanyID = item.CompanyID;
                    model.Credit_Sum = item.Credit_Sum;
                    model.CycleID = item.CycleID;
                    model.Debit_Sum = item.Debit_Sum;
                    model.id = item.id;
                    model.Ledger_Count = item.Ledger_Count;
                    model.Purchase_Amount_Sum = item.Purchase_Amount_Sum;
                    model.Purchase_Count = item.Purchase_Count;
                    model.Purchase_GST_Amount_GST = item.Purchase_GST_Amount_GST;
                    model.Record_Identifier = item.Record_Identifier;
                    model.Supply_Amount_Sum = item.Supply_Amount_Sum;
                    model.Supply_Count = item.Supply_Count;
                    model.Supply_GST_Amount_Sum = item.Supply_GST_Amount_Sum;





                    list.Add(model);
                }
                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        #endregion
        

        #region UserPermissions

        public UserPermission GetPermission_ToUser(string UserId)
        {
            var Permissions = new UserPermission();
            try
            {
                var gstmart = new GSTMARTEntities();
                var user = (from p in gstmart.Users where p.UserId == UserId select p).FirstOrDefault();
                var userpermission = (from p in gstmart.Permissions where p.User_Id == user.Id select p).FirstOrDefault();
                Permissions.isAccessSetting = userpermission.AccessSetting;
                Permissions.isCreatecycle = userpermission.CreateCycle;
                Permissions.isDownlode = userpermission.DownloadData;
                Permissions.isPrint = userpermission.PrintReport;
                return Permissions;
            }
            catch(Exception Ex)
            {
                return Permissions;
            }
        }

        #endregion
    }
}
