using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GST_BLL.DTO
{
    public class AuditLogModel
    {
        public int Id { get; set; }
        public string CreatedDate { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
        public string IpAddress { get; set; }
        public string Name { get; set; }

        public string Status { get; set; }
    }
}
