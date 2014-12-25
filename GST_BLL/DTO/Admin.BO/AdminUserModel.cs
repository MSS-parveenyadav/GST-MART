using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace GST_BLL.DTO
{
  public class AdminUserModel
    {

      public int Id
      {
          get;
          set;
      }
      public string UserId 
      { 
          get; 
          set; 
      }
      public string Guid
      {
          get;
          set;
      }
      public string Usename
      {
          get;
          set;
      }
      public string Password
      {
          get;
          set;
      }
      public string ConfirmPassword
      {
          get;
          set;
      }
      public string Email
      {
          get;
          set;
      }

      public string ConfirmEmail
      {
          get;
          set;
      }

      public string MobileNumber
      {
          get;
          set;
      }
      public string ConfirmMobileNumber
      {

          get;
          set;
      }

      public string Createcycle
      {
          get;
          set;
      }

      public string AccessSetting
      {
          get;
          set;
      }
      public string DownloadData
      {
          get;
          set;
      }
      public string PrintReport
      {
          get;
          set;
      }
      public string CompanyId { get; set; }
      public string AdminID { get; set; }
      public List<Companylist> Company 
      {
          get; 
          set; 
      }

      public IEnumerable<string> Companies { get; set; }
      public IEnumerable<SelectListItem> listcompaney { get; set; }

    }

    public class Companylist
    {
        public string CompanyId
        {
            get;
            set;
        }
        public string CompanyName
        {
            get;
            set;
        }
        public int id
        {
            get;
            set;

        }
        public string companylist
        {
            get;
            set;

        }

    }

}
