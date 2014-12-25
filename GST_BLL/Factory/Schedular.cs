using GST_BLL.DTO;
using GST_BLL.Enum;
using GST_DB;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GST_BLL.Factory
{
    public class DataSchedular
    {
        GSTMARTEntities Gstmart = new GSTMARTEntities();
        string Message = string.Empty;
        Schedular entity = new Schedular();
        public string CreateCompnaySchedular(SchedularModel model)
        {

            
            try
            {
        
                entity.Frequency = model.CompanyFrequency;
                entity.LastRun = model.CompanyLastRun;
                entity.Module ="Company";
         
                entity.Unit= model.CompanyUnit;
                entity.STime = model.CompanyNextRun;
              
                entity.Unit = model.CompanyUnit;
                entity.LastRun = "Not Run Yet";
                entity.NextRun = model.CompanyNextRun;
                entity.Status = EnumClass.schedularStatus.Running.ToString();
                entity.Path = model.CompanyFilePath;
                entity.CreatedDate = DateTime.Now;
                entity.CompanyID = model.CmpnyID;


                if (model.Companyid == 0)
                {
                    Gstmart.Schedulars.Add(entity);
                   
                }
                else
                {
                    entity.Id = model.Companyid;
                    Gstmart.Entry(entity).State = System.Data.EntityState.Modified;
                }
                Gstmart.SaveChanges();
                


                Message = EnumClass.MessageFamily.Success.ToString();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                       Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                       return Message = validationError.ErrorMessage;
                    
                    }
                }

            }
            return Message;
            
        
        }

        public string CreateFooterSchedular(SchedularModel model)
        {


            try
            {

                entity.Frequency = model.FooterFrequency;
                entity.LastRun = model.FooterLastRun;
                entity.Module ="Footer";
                entity.Unit = model.FooterUnit;
                entity.STime = model.FooterNextRun; ;
                entity.Unit = model.FooterUnit;
                entity.LastRun = "Not Run Yet";
                entity.NextRun = model.FooterNextRun;
                entity.Status = EnumClass.schedularStatus.Running.ToString();
                entity.Path = model.FooterFilePath;
                entity.CreatedDate = DateTime.Now;
                entity.CompanyID = model.CmpnyID;


                if (model.Footerid == 0)
                {
                    Gstmart.Schedulars.Add(entity);

                }
                else
                {
                    entity.Id = model.Footerid;
                    Gstmart.Entry(entity).State = System.Data.EntityState.Modified;
                }
                Gstmart.SaveChanges();


                entity.Status = EnumClass.schedularStatus.Running.ToString();
                Message = EnumClass.MessageFamily.Success.ToString();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        return Message = validationError.ErrorMessage;

                    }
                }

            }
            return Message;


        }

        public string CreatePurchaseSchedular(SchedularModel model)
        {


            try
            {

                entity.Frequency = model.PurchaseFrequency;
                entity.LastRun = model.PurchaseLastRun;
                entity.Module = "Purchase";
                entity.Unit = model.PurchaseUnit;
                entity.STime = model.PurchaseNextRun;
                entity.Unit = model.PurchaseUnit;
                entity.LastRun = "Not Run Yet";
                entity.NextRun = model.PurchaseNextRun;
                entity.Status = EnumClass.schedularStatus.Running.ToString();
                entity.Path = model.PurchaseFilePath;
                entity.CreatedDate = DateTime.Now;
                entity.CompanyID = model.CmpnyID;


                if (model.Purchaseid == 0)
                {
                    Gstmart.Schedulars.Add(entity);

                }
                else
                {
                    entity.Id = model.Purchaseid;
                    Gstmart.Entry(entity).State = System.Data.EntityState.Modified;
                }
                Gstmart.SaveChanges();



                entity.Status = EnumClass.schedularStatus.Running.ToString();
                Message = EnumClass.MessageFamily.Success.ToString();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        return Message = validationError.ErrorMessage;

                    }
                }

            }
            return Message;


        }

        public string CreateSupplySchedular(SchedularModel model)
        {



            try
            {

                entity.Frequency = model.SupplyFrequency;
                entity.LastRun = model.SupplyLastRun;
                entity.Module = "Supply";
                entity.Unit = model.SupplyUnit;
                entity.STime = model.SupplyNextRun;
                entity.Unit = model.SupplyUnit;
                entity.LastRun = "Not Run Yet";
                entity.NextRun = model.SupplyNextRun;
                entity.Status = EnumClass.schedularStatus.Running.ToString();
                entity.Path = model.SupplyFilePath;
                entity.CreatedDate = DateTime.Now;
                entity.CompanyID = model.CmpnyID;

                if (model.Supplyid == 0)
                {
                    Gstmart.Schedulars.Add(entity);

                }
                else
                {
                    entity.Id = model.Supplyid;

                    Gstmart.Entry(entity).State = System.Data.EntityState.Modified;
                }
                Gstmart.SaveChanges();



                entity.Status = EnumClass.schedularStatus.Running.ToString();
                Message = EnumClass.MessageFamily.Success.ToString();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        return Message = validationError.ErrorMessage;

                    }
                }

            }
            return Message;


        }


        public string CreateLedgerSchedular(SchedularModel model)
        {

            try
            {

                entity.Frequency = model.LedgerFrequency;
                entity.LastRun = model.LedgerLastRun;
                entity.Module ="Ledger";
                entity.Unit = model.LedgerUnit;
                entity.STime = model.LedgerNextRun;
                entity.Unit = model.LedgerUnit;
                entity.LastRun = "Not Run Yet";
                entity.NextRun = model.LedgerNextRun;
                entity.Status = EnumClass.schedularStatus.Running.ToString();
                entity.Path = model.LedgerFilePath;
                entity.CreatedDate = DateTime.Now;
                entity.CompanyID = model.CmpnyID;


                if (model.Ledgerid == 0)
                {
                    Gstmart.Schedulars.Add(entity);

                }
                else
                {
                    entity.Id = model.Ledgerid;
                    Gstmart.Entry(entity).State = System.Data.EntityState.Modified;
                }
                Gstmart.SaveChanges();



                entity.Status = EnumClass.schedularStatus.Running.ToString();
                Message = EnumClass.MessageFamily.Success.ToString();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        return Message = validationError.ErrorMessage;

                    }
                }

            }
            return Message;



        }

        public int EditSchedularStatus(int id)
        {
            try
            {
                var query = (from p in Gstmart.Schedulars where p.Id == id select p).FirstOrDefault();
                if (query.Status == "Running")
                {
                    query.Status = "Stop";
                }
                else
                {
                    query.Status = "Running";
                }
                Gstmart.Entry(query).State = System.Data.EntityState.Modified;
                Gstmart.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public List<DfaultModulForSchedular> GetSchedular(int id = 0,string companyID="")
        {

            var data = (from p in Gstmart.Schedulars where p.CompanyID == companyID select p).ToList();
            var list = new List<DfaultModulForSchedular>();
            try
            {

                foreach (var item in data)
                {

                    var model = new DfaultModulForSchedular();
                    if (item.Module.Contains("Company"))
                    {

                        model.id = item.Id;
                        model.FilePath = item.Path;
                        model.Frequency = item.Frequency;
                        model.LastRun = item.LastRun;
                        model.NextRun = item.NextRun;
                        model.StartTime = item.STime;
                        model.Status = item.Status;
                        model.Time = item.STime;
                        model.Unit = item.Unit;
                        model.Module = item.Module;
                        model.CreateDate = item.CreatedDate;
                  
                        list.Add(model);

                    }
                    else if (item.Module.Contains("Ledger"))
                    {
                        model.id = item.Id;
                        model.FilePath = item.Path;
                        model.Frequency = item.Frequency;
                        model.LastRun = item.LastRun;
                        model.NextRun = item.NextRun;
                        model.StartTime = item.STime;
                        model.Status = item.Status;
                        model.Time = item.STime;
                        model.Unit = item.Unit;
                        model.Module = item.Module;
                        model.CreateDate = item.CreatedDate;
                        list.Add(model);
                    }
                    else if (item.Module.Contains("Supply"))
                    {
                        model.id = item.Id;
                        model.FilePath = item.Path;
                        model.Frequency = item.Frequency;
                        model.LastRun = item.LastRun;
                        model.NextRun = item.NextRun;
                        model.StartTime = item.STime;
                        model.Status = item.Status;
                        model.Time = item.STime;
                        model.Unit = item.Unit;
                        model.Module = item.Module;
                        model.CreateDate = item.CreatedDate;
                        list.Add(model);
                    }
                    else if (item.Module.Contains("Footer"))
                    {
                        model.id = item.Id;
                        model.FilePath = item.Path;
                        model.Frequency = item.Frequency;
                        model.LastRun = item.LastRun;
                        model.NextRun = item.NextRun;
                        model.StartTime = item.STime;
                        model.Status = item.Status;
                        model.Time = item.STime;
                        model.Unit = item.Unit;
                        model.Module = item.Module;
                        model.CreateDate = item.CreatedDate;
                        list.Add(model);
                    }
                    else if (item.Module.Contains("Purchase"))
                    {
                        model.id = item.Id;
                        model.FilePath = item.Path;
                        model.Frequency = item.Frequency;
                        model.LastRun = item.LastRun;
                        model.NextRun = item.NextRun;
                        model.StartTime = item.STime;
                        model.Status = item.Status;
                        model.Time = item.STime;
                        model.Unit = item.Unit;
                        model.Module = item.Module;
                        model.CreateDate = item.CreatedDate;
                        list.Add(model);
                    }

                }



                return list;
            }
            catch(Exception ex)
            {

                return list;

            }
        
        
        }

        public Schedular CheckExtingSchedular(string module,string companyID)
        {
            var data = Gstmart.Schedulars.ToList().Find(x => x.Module == module && x.CompanyID==companyID);
            return data;

        }

        public SchedularModel FindSchedularById(int id,string CompanyID)
        {

            Schedular CompanyData = CheckExtingSchedular("Company", CompanyID);
            Schedular LedgerData = CheckExtingSchedular("Ledger", CompanyID);
            Schedular SupplyData = CheckExtingSchedular("Supply", CompanyID);
            Schedular FooterData = CheckExtingSchedular("Footer", CompanyID);
            Schedular PurchaseData = CheckExtingSchedular("Purchase", CompanyID);

            var model = new SchedularModel();


            model.Companyid = CompanyData.Id;
            model.CompanyModule = CompanyData.Module;
            model.CompanyFilePath = CompanyData.Path;
            model.CompanyFrequency = CompanyData.Frequency;
            model.CompanyUnit = CompanyData.Unit;
            model.CompanyStartTime = CompanyData.STime;
            model.Companystatus = CompanyData.Status;
            model.CompanyLastRun = CompanyData.LastRun;
            model.CompanyNextRun = CompanyData.NextRun;
            model.CompanyCreateDate = CompanyData.CreatedDate;
          


            model.Ledgerid = LedgerData.Id;
            model.LedgerModule = LedgerData.Module;
            model.LedgerFilePath = LedgerData.Path;
            model.LedgerFrequency = LedgerData.Frequency;
            model.LedgerUnit = LedgerData.Unit;
            model.LedgerStartTime = LedgerData.STime;
            model.LedgerStatus = LedgerData.Status;
            model.LedgerLastRun = LedgerData.LastRun;
            model.LedgerNextRun = LedgerData.NextRun;
            model.LedgerCreateDate = LedgerData.CreatedDate;
           


            model.Supplyid = SupplyData.Id;
            model.SupplyModule = SupplyData.Module;
            model.SupplyFilePath = SupplyData.Path;
            model.SupplyFrequency = SupplyData.Frequency;
            model.SupplyUnit = SupplyData.Unit;
            model.SupplyStartTime = SupplyData.STime;
            model.SupplyStatus = SupplyData.Status;
            model.SupplyLastRun = SupplyData.LastRun;
            model.SupplyNextRun = SupplyData.NextRun;
            model.SupplyCreateDate = SupplyData.CreatedDate;



            model.Footerid = FooterData.Id;
            model.FooterModule = FooterData.Module;
            model.FooterFilePath = FooterData.Path;
            model.FooterFrequency = FooterData.Frequency;
            model.FooterUnit = FooterData.Unit;
            model.FooterStartTime = FooterData.STime;
            model.FooterStatus = FooterData.Status;
            model.FooterLastRun = FooterData.LastRun;
            model.FooterNextRun = FooterData.NextRun;
            model.FooterCreateDate = FooterData.CreatedDate;




            model.Purchaseid = PurchaseData.Id;
            model.PurchaseModule = PurchaseData.Module;
            model.PurchaseFilePath = PurchaseData.Path;
            model.PurchaseFrequency = PurchaseData.Frequency;
            model.PurchaseUnit = PurchaseData.Unit;
            model.PurchaseStartTime = PurchaseData.STime;
            model.PurchaseStatus = PurchaseData.Status;
            model.PurchaseLastRun = PurchaseData.LastRun;
            model.PurchaseNextRun = PurchaseData.NextRun;
            model.PurchaseCreateDate = PurchaseData.CreatedDate;

            return model;

        }


        public int UpdateSchedular(SchedularModel model)
        {
            try
            {
                var CompaneyData = (from p in Gstmart.Schedulars where p.Id == model.Companyid select p).FirstOrDefault();
                var LedgerData = (from p in Gstmart.Schedulars where p.Id == model.Ledgerid select p).FirstOrDefault();
                var FooterData = (from p in Gstmart.Schedulars where p.Id == model.Footerid select p).FirstOrDefault();
                var PurchaseData = (from p in Gstmart.Schedulars where p.Id == model.Purchaseid select p).FirstOrDefault();
                var SupplyData = (from p in Gstmart.Schedulars where p.Id == model.Supplyid select p).FirstOrDefault();



                
                CompaneyData.Path = model.CompanyFilePath;
                CompaneyData.Frequency = model.CompanyFrequency;
                CompaneyData.Unit = model.CompanyUnit;
                CompaneyData.NextRun = model.CompanyNextRun;
                CompaneyData.CreatedDate = DateTime.Now;
                CompaneyData.STime = model.CompanyNextRun;
                CompaneyData.CompanyID = model.CmpnyID;
                Gstmart.Entry(CompaneyData).State = System.Data.EntityState.Modified;
                Gstmart.SaveChanges();


                LedgerData.Path = model.LedgerFilePath;
                LedgerData.Frequency = model.LedgerFrequency;
                LedgerData.Unit = model.LedgerUnit;
                LedgerData.NextRun = model.LedgerNextRun;
                LedgerData.STime = model.LedgerNextRun;
                LedgerData.CreatedDate = DateTime.Now;
                LedgerData.CompanyID = model.CmpnyID;
                Gstmart.Entry(LedgerData).State = System.Data.EntityState.Modified;
                Gstmart.SaveChanges();

                FooterData.Path = model.FooterFilePath;
                FooterData.Frequency = model.FooterFrequency;
                FooterData.Unit = model.FooterUnit;
                FooterData.NextRun = model.FooterNextRun;
                FooterData.STime = model.FooterNextRun;
                FooterData.CreatedDate = DateTime.Now;
                FooterData.CompanyID = model.CmpnyID;
                Gstmart.Entry(FooterData).State = System.Data.EntityState.Modified;
                Gstmart.SaveChanges();

                PurchaseData.Path = model.PurchaseFilePath;
                PurchaseData.Frequency = model.PurchaseFrequency;
                PurchaseData.Unit = model.PurchaseUnit;
                PurchaseData.NextRun = model.PurchaseNextRun;
                PurchaseData.STime = model.PurchaseNextRun;
                PurchaseData.CreatedDate = DateTime.Now;
                PurchaseData.CompanyID = model.CmpnyID;
                Gstmart.Entry(PurchaseData).State = System.Data.EntityState.Modified;
                Gstmart.SaveChanges();

                SupplyData.Path = model.SupplyFilePath;
                SupplyData.Frequency = model.SupplyFrequency;
                SupplyData.Unit = model.SupplyUnit;
                SupplyData.NextRun = model.SupplyNextRun;
                SupplyData.STime = model.SupplyNextRun;
                SupplyData.CreatedDate = DateTime.Now;
                SupplyData.CompanyID = model.CmpnyID;
                Gstmart.Entry(SupplyData).State = System.Data.EntityState.Modified;
                Gstmart.SaveChanges();

                return 1;
            }
            catch
            {
                return 0;
            }

        }
    }
}
