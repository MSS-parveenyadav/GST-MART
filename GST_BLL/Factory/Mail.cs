using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace GST_BLL.SendMail
{
    public class Mail
    {
        public string SendMail(string To, string from, string smtp_port, string Smtp_Password, string  Smtp_Host,string subject,string body)
        {
            string Result="";
            MailMessage mail = new MailMessage();
            mail.To.Add(To);
            mail.From = new MailAddress(from);
            mail.Subject = subject;
       
           
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = Smtp_Host;
            smtp.Port = Convert.ToInt32(smtp_port);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential
            (from, Smtp_Password);
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(mail);
                Result = "Email sent Successfuly,please check your Email.";
            }
            catch (Exception ex)
            {
                Result=ex.ToString();
            }
            return Result;
        }


        public string Sendsecuritycode(string Email, string securitycode, string Usename, string ms_email, string smtp_port, string Smtp_Password, string Smtp_Host, string subject)
        {
            string Result = "";
            MailMessage mail = new MailMessage();
            mail.To.Add(Email);
            mail.From = new MailAddress(ms_email);
            mail.Subject = subject;
            string Body = "Dear : " + Usename + "<br/><br/>Your Security Code is : " + securitycode;
            mail.Body = Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = Smtp_Host;
            smtp.Port = Convert.ToInt32(smtp_port);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential
            (ms_email, Smtp_Password);
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(mail);
                Result = "Email sent Successfuly,please check your Email.";
            }
            catch (Exception ex)
            {
                Result = ex.ToString();
            }
            return Result;
        }
    }
}
