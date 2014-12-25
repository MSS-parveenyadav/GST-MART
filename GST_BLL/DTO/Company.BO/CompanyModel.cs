using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace GST_BLL.DTO
{
    public class CompanyModel
    {
        [Required(ErrorMessage = "Company Name is required")]
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
        [Required(ErrorMessage = "Company Id is required")]
        public string CompanyId { get; set; }
        public int id { get; set; }

        public List<SystemModel> systemModel { get; set; }
        public UserModel userModel { get; set; }

        public List<IndustryModel> industrymodel { get; set; }

        public string Description
        {
            get;
            set;

        }
        public string Remarks
        {
            get;
            set;

        }

        public string companylist { get; set; }

    }
}
