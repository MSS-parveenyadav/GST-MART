using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GST_BLL.DTO
{
   public class UserPermissionModel
    {
       public int Id
       {
           get;
           set;
       }

       public string Date
       {
           get;
           set;
       }
       public string UserId
       {
           get;
           set;

       }
       public string Name
       {
           get;
           set;
       }
       public string Permission
       {
           get;
           set;
       }
       public string Status { get; set; }
       public string LastLoginDate { get; set; }

    }
}
