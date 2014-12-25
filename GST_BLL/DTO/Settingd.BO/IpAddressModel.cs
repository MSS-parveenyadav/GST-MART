using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace GST_BLL.DTO
{
    public class IpAddressModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage="Field Required")]
        public string IP { get; set; }
        public string CreatedDate { get; set; }
    }
}
