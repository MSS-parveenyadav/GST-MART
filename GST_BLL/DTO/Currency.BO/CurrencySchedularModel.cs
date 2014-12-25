using GST_DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GST_BLL.DTO.Currency.BO
{
  public class CurrencySchedularModel
    {
        public int Id { get; set; }
        public string FeedUrl { get; set; }
        public string FrequencyUnit { get; set; }
        public string Time { get; set; }

        public DateTime CreateDate { get; set; }
        public virtual Company Company { get; set; }

    }
}
