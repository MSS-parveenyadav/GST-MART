using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GST_BLL.DTO
{
  public  class UserModel
    {
     
       [Required(ErrorMessage="Admin Name is required")]
        public string Name
        {
            get;
            set;
        }
      [Required(ErrorMessage = "Admin ID is required")]
       public string AdminID
       {
           get;
           set;
       }
        public string Usertype
        {
            get;
            set;
        }

        public string Createddate
        {
            get;
            set;
        }
        public string Status
        {
            get;
            set;
        }

       [Required(ErrorMessage = "Email is required")]
       [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
       
        public string Email
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Password is required")]
        public string Password
        {
            get;
            set;
        }

       // [CompareAttribute("EmailAddress", ErrorMessage = "Emails mismatch")]
      [Required(ErrorMessage = "Password is required")]
        public string ConfirmPwd
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Mobile Number is required")]
        public string MobileNumber
        {
            get;
            set;
        }


        [Required(ErrorMessage = "Mobile Number is required")]
        public string CnfrmMobileNumber
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string ConfirmEmail
        {
            get;
            set;
        }

        public int id
        {
            get;
            set;
        }

        
    }
}
