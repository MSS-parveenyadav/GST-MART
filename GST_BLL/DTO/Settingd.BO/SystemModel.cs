using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace GST_BLL.DTO
{
   public class SystemModel
    {
       public int Id
       {
           get;
           set;
       }
       [Required(ErrorMessage = "System Name is required")]
       public string SystemName
       {

           get;
           set;
       }

     
       [Required(ErrorMessage = "System Id is required")]
       public string SystemId
       {

           get;
           set;

       }

       public int Companyd
       {

           get;
           set;

       }
    }
}
