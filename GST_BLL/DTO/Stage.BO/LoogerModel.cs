using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GST_BLL.DTO
{
    public class LoogerModel
    {

        public string Flat_File_Source_Error_Output_Column { get; set; }
        public Nullable<int> ErrorCode { get; set; }
        public Nullable<int> ErrorColumn { get; set; }
        public int id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CycleID { get; set; }
        public string CompanyID { get; set; }
        public string Duplicate { get; set; }

    }
}
