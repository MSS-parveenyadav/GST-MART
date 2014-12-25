using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GST_BLL.DTO
{
    public class CurrencyGroupModel
    {
        public int Id { get; set; }
        public string CurrencyGroupName { get; set; }
        public string CurrencyType { get; set; }
        public string TaxCode { get; set; }
        public string CreatedDate { get; set; }

    }
}
