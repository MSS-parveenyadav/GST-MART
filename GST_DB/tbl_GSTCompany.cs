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
    
    public partial class tbl_GSTCompany
    {
        public string Company_Code { get; set; }
        public string Record_Identifier { get; set; }
        public string Company_Name { get; set; }
        public string Company_BRN { get; set; }
        public string Company_GST_No { get; set; }
        public Nullable<System.DateTime> Period_Start { get; set; }
        public Nullable<System.DateTime> Period_End { get; set; }
        public Nullable<System.DateTime> File_Creation_Date { get; set; }
        public string Product_Version { get; set; }
        public string GAF_Version { get; set; }
        public Nullable<long> CycleID { get; set; }
        public string CompanyID { get; set; }
        public long Id { get; set; }
    }
}
