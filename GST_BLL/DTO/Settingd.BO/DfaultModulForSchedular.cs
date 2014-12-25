using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GST_BLL.DTO
{
    public class DfaultModulForSchedular
    {
        public int id { get; set; }
        public string FilePath { get; set; }
        public string Frequency { get; set; }
        public string StartTime { get; set; }
        public string Status { get; set; }
        public string LastRun { get; set; }
        public string NextRun { get; set; }
        public string Action { get; set; }
        public string Unit { get; set; }
        public string Time { get; set; }
        public DateTime CreateDate { get; set; }
        public string Module { get; set; }
    }
}
