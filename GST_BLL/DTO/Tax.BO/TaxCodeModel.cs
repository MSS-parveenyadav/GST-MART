using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GST_BLL.DTO
{
 public  class TaxCodeModel
    {
     public int Id
     {
         get;
         set;
     }
     public string CompanyId
     {
         get;
         set;
     }
     public string SystemId
     {
         get;
         set;
     }
     public string CustomCode
     {
         get;
         set;
     }

     public string OriginalCode
     {
         get;
         set;

     }

     public string TransactionType
     {
         get;
         set;
     }
     public string TaxRate
     {
         get;
         set;
     }

     public string Description
     {
         get;
         set;
     }
     public string RefernceNumber
     {
         get;
         set;
     }

     public string GST03Fields
     {
         get;
         set;
     }
     public string Remarks
     {
         get;
         set;
     }
     public string DateType
     {
         get;
         set;
     }

    }
}
