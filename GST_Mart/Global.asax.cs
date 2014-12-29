using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using GST_BLL;
using GST_DB;
using System.Web.Optimization;
using GST_Mart.App_Start;


namespace GST_Mart
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.registerbundle(BundleTable.Bundles);
        }


        protected void FormsAuthentication_OnAuthenticate(Object sender, FormsAuthenticationEventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        //let us take out the username now                
                        string loginID = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                        string roles = string.Empty;

                        using (GSTMARTEntities entities = new GSTMARTEntities())
                        {
                            User user = entities.Users.FirstOrDefault(u => u.AdminID == loginID);

                            roles = user.Usertype;
                        }
                        //let us extract the roles from our own custom cookie


                        //Let us set the Pricipal with our user specific details
                        e.User = new System.Security.Principal.GenericPrincipal(
                          new System.Security.Principal.GenericIdentity(loginID, "Forms"), roles.Split(';'));
                    }
                    catch (Exception)
                    {
                        //somehting went wrong
                    }
                }
            }
        }

        protected void Application_AuthenticateRequest(Object sender, FormsAuthenticationEventArgs e)
        {
            // Extract the forms authentication cookie
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Context.Request.Cookies[cookieName];

            if (null == authCookie)
            {
                // There is no authentication cookie.
                return;
            }
            FormsAuthenticationTicket authTicket = null;
            try
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch (Exception ex)
            {
                // Log exception details (omitted for simplicity)
                return;
            }

            if (null == authTicket)
            {
                // Cookie failed to decrypt.
                return;
            }
            // When the ticket was created, the UserData property was assigned a
            // pipe delimited string of group names.
            String[] groups = authTicket.UserData.Split(new char[] { '|' });
            // Create an Identity object
            System.Security.Principal.GenericIdentity id = new System.Security.Principal.GenericIdentity(authTicket.Name,
                                                     "LdapAuthentication");

            // This principal will flow throughout the request.
            System.Security.Principal.GenericPrincipal principal = new System.Security.Principal.GenericPrincipal(id, groups);
            // Attach the new principal object to the current HttpContext object
            Context.User = principal;

        } 
    }
}