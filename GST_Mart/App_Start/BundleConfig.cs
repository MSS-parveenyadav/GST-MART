using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace GST_Mart.App_Start
{
    public class BundleConfig
    {
        public static void registerbundle(BundleCollection bundle)
        {




            bundle.Add(new StyleBundle("~/Content/css").Include("~/Content/css/font-awesome.css", "~/Content/css/style.css", "~/Content/css/bootstrapCopy.css", "~/Content/css/bootstrap-theme.css", "~/Content/css/bootstrap-datetimepicker.min.css", "~/Content/css/lessframe.css"));
            bundle.Add(new ScriptBundle("~/Content/js").Include("~/Content/js/bootstrap.js", "~/Content/js/bootstrap.min.js", "~/Content/js/respond.min.js"));
            bundle.Add(new ScriptBundle("~/Scripts/js").Include("~/Scripts/bootstrap.js", "~/Scripts/bootstrap.min.js", "~/Scripts/jquery-1.4.4-vsdoc.js", "~/Scripts/jquery-1.4.4.js", "~/ Scripts/jquery-1.4.4.min.js", "~/Scripts/jquery-1.5.1.min.js","~/Scripts/jquery.validate.unobtrusive.min.js","~/Scripts/jquery.validate-vsdoc.js","~/Scripts/jquery.validate.js","~/Scripts/jquery.validate.min.js"));
     
     
       
     
        
       
        
    
     
        
   
  
       
        
        }
     
    }
 

}