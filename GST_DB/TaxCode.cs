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
    
    public partial class TaxCode
    {
        public int Id { get; set; }
        public string CompanyId { get; set; }
        public string SystemId { get; set; }
        public string CustomCode { get; set; }
        public string OriginalCode { get; set; }
        public string TransactionType { get; set; }
        public string TaxRate { get; set; }
        public string Description { get; set; }
        public string ReferenceNumber { get; set; }
        public string GST03Fields { get; set; }
        public string Remarks { get; set; }
        public string DateType { get; set; }
    }
}
