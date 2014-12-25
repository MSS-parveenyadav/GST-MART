using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GST_BLL.DTO
{
   public class FooterModel
    {
        public string Company_Code { get; set; }
        public string Record_Identifier { get; set; }
        public Nullable<decimal> Purchase_Count { get; set; }
        public Nullable<decimal> Purchase_Amount_Sum { get; set; }
        public Nullable<decimal> Purchase_GST_Amount_GST { get; set; }
        public Nullable<decimal> Supply_Count { get; set; }
        public Nullable<decimal> Supply_Amount_Sum { get; set; }
        public Nullable<decimal> Supply_GST_Amount_Sum { get; set; }
        public Nullable<decimal> Ledger_Count { get; set; }
        public Nullable<decimal> Debit_Sum { get; set; }
        public Nullable<decimal> Credit_Sum { get; set; }
        public Nullable<decimal> Balance_Sum { get; set; }
        public Nullable<long> CycleID { get; set; }
        public string CompanyID { get; set; }
        public long id { get; set; }
    }
}
