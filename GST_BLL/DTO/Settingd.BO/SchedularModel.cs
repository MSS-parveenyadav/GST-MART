using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GST_BLL.DTO
{
    public class SchedularModel
    {
        public int Companyid { get; set; }
        public string  CmpnyID { get; set; }
        public string CompanyFilePath { get; set; }
        public string CompanyFrequency { get; set; }
        public string CompanyStartTime { get; set; }
        public string Companystatus { get; set; }
        public string CompanyLastRun { get; set; }
        public string CompanyNextRun { get; set; }
        public string CompanyAction { get; set; }
        public DateTime CompanyCreateDate { get; set; }
        public string CompanyUnit { get; set; }
        public string  CompanyTime { get; set; }
        public string CompanyModule { get; set; }


        public int Ledgerid { get; set; }
        public string LedgerFilePath { get; set; }
        public string LedgerFrequency { get; set; }
        public string LedgerStartTime { get; set; }
        public string LedgerStatus { get; set; }
        public string LedgerLastRun { get; set; }
        public string LedgerNextRun { get; set; }
        public string LedgerAction { get; set; }
        public string LedgerUnit { get; set; }
        public string LedgerTime { get; set; }
        public DateTime LedgerCreateDate { get; set; }
        public string LedgerModule { get; set; }


        public int Footerid { get; set; }
        public string FooterFilePath { get; set; }
        public string FooterFrequency { get; set; }
        public string FooterStartTime { get; set; }
        public string FooterStatus { get; set; }
        public string FooterLastRun { get; set; }
        public string FooterNextRun { get; set; }
        public string FooterAction { get; set; }
        public string FooterUnit { get; set; }
        public string FooterTime { get; set; }
        public DateTime FooterCreateDate { get; set; }
        public string FooterModule { get; set; }


        public int Supplyid { get; set; }
        public string SupplyFilePath { get; set; }
        public string SupplyFrequency { get; set; }
        public string SupplyStartTime { get; set; }
        public string SupplyStatus { get; set; }
        public string SupplyLastRun { get; set; }
        public string SupplyNextRun { get; set; }
        public string SupplyAction { get; set; }
        public string SupplyUnit { get; set; }
        public string SupplyTime { get; set; }
        public DateTime SupplyCreateDate { get; set; }
        public string SupplyModule { get; set; }


        public int Purchaseid { get; set; }
        public string PurchaseFilePath { get; set; }
        public string PurchaseFrequency { get; set; }
        public string PurchaseStartTime { get; set; }
        public string PurchaseStatus { get; set; }
        public string PurchaseLastRun { get; set; }
        public string PurchaseNextRun { get; set; }
        public string PurchaseAction { get; set; }
        public string PurchaseUnit { get; set; }
        public string PurchaseTime { get; set; }
        public DateTime PurchaseCreateDate { get; set; }
        public string PurchaseModule { get; set; }

 


        public int Errors { get; set; }

    }
}
