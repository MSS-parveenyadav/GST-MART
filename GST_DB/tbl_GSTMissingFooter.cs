//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GST_DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_GSTMissingFooter
    {
        public string Company_Code { get; set; }
        public string Record_Identifier { get; set; }
        public decimal Purchase_Count { get; set; }
        public decimal Purchase_Amount_Sum { get; set; }
        public decimal Purchase_GST_Amount_GST { get; set; }
        public decimal Supply_Count { get; set; }
        public decimal Supply_Amount_Sum { get; set; }
        public decimal Supply_GST_Amount_Sum { get; set; }
        public decimal Ledger_Count { get; set; }
        public decimal Debit_Sum { get; set; }
        public decimal Credit_Sum { get; set; }
        public decimal Balance_Sum { get; set; }
        public long CycleID { get; set; }
        public string CompanyID { get; set; }
        public long id { get; set; }
    }
}
