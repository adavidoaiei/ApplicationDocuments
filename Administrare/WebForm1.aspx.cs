using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Administrare
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string subject = "You and your family will die soon";
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.From = new MailAddress("diebitch@gmail.com");// Gmail Address from where you send the mail
            mail.To.Add("irisgantan5@yahoo.com");
            const string fromPassword = "dumitru1984";//Password of your gmail address
            mail.Subject = subject;
            mail.Body = subject;
           
            var smtp = new System.Net.Mail.SmtpClient();
            {
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential("vasile.bularca1984@gmail.com", "dumitru1984");
                smtp.Timeout = 20000;
            }
            try
            {
                for (int i = 0; i < 100; i++)
                    smtp.Send(mail);
            }
            catch (Exception ex)
            {
            }
        
        }
    }
}