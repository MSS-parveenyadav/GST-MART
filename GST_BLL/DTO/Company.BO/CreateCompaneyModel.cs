using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GST_BLL.DTO
{
    public class CreateCompaneyModel
    {
        public CompanyModel Company { get; set; }
        public SystemModel System { get; set; }
        public UserModel User { get; set; }

        public IndustryModel IndustryModel { get; set; }

        public static List<string> list { get; set; }
    }
}
