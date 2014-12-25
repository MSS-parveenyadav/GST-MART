using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GST_BLL.DTO
{
    public class CompanyErrorModel
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
