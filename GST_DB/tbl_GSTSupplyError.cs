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
    
    public partial class tbl_GSTSupplyError
    {
        public string Flat_File_Source_Error_Output_Column { get; set; }
        public Nullable<int> ErrorCode { get; set; }
        public Nullable<int> ErrorColumn { get; set; }
        public int id { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> CycleID { get; set; }
        public string CompanyID { get; set; }
        public string Error_Description { get; set; }
        public string Error_ColumnName { get; set; }
    }
}
