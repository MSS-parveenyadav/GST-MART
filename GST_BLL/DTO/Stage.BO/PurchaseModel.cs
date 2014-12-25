using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GST_BLL.DTO
{
  public class PurchaseModel
    {
        public long Purchase_ID { get; set; }
        public string Company_Code { get; set; }
        public string Record_Identifier { get; set; }
        public string Supplier_BRN { get; set; }
        public string Supplier_Name { get; set; }
        public Nullable<System.DateTime> Invoice_Date { get; set; }
        public string Invoice_No { get; set; }
        public string Import_Declaration_No { get; set; }
        public string Line_No { get; set; }
        public string Product_Description { get; set; }
        public Nullable<decimal> Purchase_Value_MYR { get; set; }
        public Nullable<decimal> Purchase_Value_GST_Amount { get; set; }
        public string Tax_Code { get; set; }
        public string Foreign_Currency_Code { get; set; }
        public Nullable<decimal> Purchase_Foreign_Currency_Amount { get; set; }
        public Nullable<decimal> Purchase_Currency_Purchase_Amount_GST { get; set; }
        public Nullable<int> YR { get; set; }
        public Nullable<int> MTH { get; set; }
        public Nullable<long> CycleID { get; set; }
        public Nullable<decimal> Value_Exempt_Supplies { get; set; }
        public string Transaction_Type { get; set; }
        public string Supplier_Number { get; set; }
        public string Product_Code { get; set; }
        public string CompanyID { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    }
}
