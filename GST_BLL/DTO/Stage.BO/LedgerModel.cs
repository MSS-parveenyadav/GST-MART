using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GST_BLL.DTO
{
   public class LedgerModel
    {

        public Int64 id { get; set; }
        public string CompanyCode { get; set; }
        public string TransactionType { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public string AccountID { get; set; }
        public string AccountName { get; set; }
        public string TransactionDescription { get; set; }
        public string Name { get; set; }
        public string TransactionID { get; set; }
        public string SourceDocumentID { get; set; }
        public string SourceType { get; set; }
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public string TaxCode { get; set; }
        public Nullable<long> CycleID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CompanyID { get; set; }
    }
}
