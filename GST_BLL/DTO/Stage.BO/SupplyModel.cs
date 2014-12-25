using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GST_BLL.DTO
{
   public class SupplyModel
    {
        public long Supply_ID { get; set; }
        public string Company_Code { get; set; }
        public string Record_Identifier { get; set; }
        public string Customer_Name { get; set; }
        public string Customer_BRN { get; set; }
        public Nullable<System.DateTime> Invoice_Date { get; set; }
        public string Invoice_No { get; set; }
        public string Line_No { get; set; }
        public string Product_Description { get; set; }
        public Nullable<decimal> Sales_Value_MYR { get; set; }
        public Nullable<decimal> Sales_Value_GST_Amount { get; set; }
        public string Tax_Code { get; set; }
        public string Country { get; set; }
        public string Foreign_Currency_Code { get; set; }
        public Nullable<decimal> Sales_Foreign_Currency_Amount { get; set; }
        public Nullable<decimal> Sales_Currency_Sales_Amount_GST { get; set; }
        public Nullable<int> YR { get; set; }
        public Nullable<int> MTH { get; set; }
        public Nullable<long> Cycle_ID { get; set; }
        public string Transaction_Type { get; set; }
        public string Customer_Code { get; set; }
        public string Product_Code { get; set; }
        public string Destination_Export { get; set; }
        public string CompanyID { get; set; }
    }
}
