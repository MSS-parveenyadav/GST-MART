using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace GST_BLL.DTO
{
    public class MultipleCompanyModel
    {
        public IEnumerable<string> Companies { get; set; }
        public IEnumerable<SelectListItem> companylist { get; set; }

    }
}
