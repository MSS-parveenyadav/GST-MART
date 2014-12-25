using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GST_BLL.DTO
{
   public class CompanyUserModel
    {

      public List<CompanyUSer> companyuserlist { get;set; }
    }


    public class CompanyUSer
    {

        public int CompanyId
        {
            get;
            set;
        }

        public string CompanyName
        {
            get;
            set;
        }

        public int UserId
        {
            get;
            set;
        }

        public string Username
        {

            get;
            set;

        }

        public string Permission
        {
            get;
            set;

        }

        public string CompnayId { get; set; }

    }
}
