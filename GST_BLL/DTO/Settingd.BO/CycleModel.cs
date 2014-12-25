using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace GST_BLL.DTO
{
   public class CycleModel
    {

        public int id { get; set; }
       
         public string SystemId { get; set; }
        [Required(ErrorMessage = "Decription is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "File is required")]
        public string Spath { get; set; }
        
        [Required(ErrorMessage = "File is required")]
        public string Ppath { get; set; }

        [Required(ErrorMessage = "File is required")]
        public string Lpath { get; set; }

        [Required(ErrorMessage = "File is required")]
        public string FPath { get; set; }

        [Required(ErrorMessage = "File is required")]
        public string CPath { get; set; }

        public DateTime CreatedDate { get; set; }
        public string  CycleID { get; set; }
        public string Status { get; set; }

        public string CycleErrors { get; set; }
        public string DuplicateRecords { get; set; }
        public string MissingRecords { get; set; }
        public string CompanyID { get; set; }
        public string Error { get; set; }
       
    }
}
