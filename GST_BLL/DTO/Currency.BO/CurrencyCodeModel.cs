using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GST_BLL.DTO
{
    public class CurrencyCodeModel
    {
        public int Id { get; set; }
        public string CreatedDate { get; set; }
        public string CurrencyCode { get; set; }
        public string FeedUrl { get; set; }
        public string Frequency { get; set; }
        public string Time { get; set; }
        public string Description { get; set; }
        public string ConversionRate { get; set; }
        public string CompanyId { get; set; }
    }
}
