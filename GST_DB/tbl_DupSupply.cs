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
    
    public partial class tbl_DupSupply
    {
        public int id { get; set; }
        public string CompanyCode { get; set; }
        public string TransactionType { get; set; }
        public string CustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerBRN { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public string Numberofinvoiceline { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public Nullable<decimal> ValueofsupplEexcluding { get; set; }
        public Nullable<decimal> ValueofGSTMYR { get; set; }
        public string TaxCode { get; set; }
        public string CountryCode { get; set; }
        public string DestinationGoodsExported { get; set; }
        public string CurrencyCode { get; set; }
        public Nullable<decimal> ValueSupplyExcluding { get; set; }
        public Nullable<decimal> ValuGSTForex { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Invoice_number { get; set; }
        public string CycleID { get; set; }
        public string CompanyID { get; set; }
    }
}
