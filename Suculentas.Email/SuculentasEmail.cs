using System.Web;
using System;
using System.Net;
using System.Net.Mail;
using System.IO;

namespace Suculentas.Email
{
    public class SuculentasEmail: ISuculentasEmail
    {
        public bool SendEmailSuculentas(string from, string title, string message)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                NetworkCredential credentials = new NetworkCredential();
                MailMessage mail = new MailMessage();

                client.Host = "server.hostingnow.com.br";
                client.Port = 587;
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                credentials.UserName = "suculentasdaro@suculentasdaro.com";
                credentials.Password = "su20cu20lent@$";

                client.Credentials = credentials;

                mail.From = new MailAddress("suculentasdaro@suculentasdaro.com", "Suculentas da R�");
                mail.Subject = title;
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.To.Add(new MailAddress(from));

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                client.Send(mail);


                return true;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public bool SendEmailSupport(string from, string title, string message)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                NetworkCredential credentials = new NetworkCredential();
                MailMessage mail = new MailMessage();

                client.Host = "server.hostingnow.com.br";
                client.Port = 587;
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                credentials.UserName = "suporte@suculentasdaro.com";
                credentials.Password = "su20cu20lent@$";

                client.Credentials = credentials;

                mail.From = new MailAddress("suporte@suculentasdaro.com", "Suporte Suculentas");
                mail.Subject = title;
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.To.Add(new MailAddress(from));

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                client.Send(mail);


                return true;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public string BodyForgotPassword(string url, string userName)
        {
            using (StreamReader html = new StreamReader(@"Templates\BodyEsqueciSenha.html"))
            {
                string body = html.ReadToEnd();
                body = body.Replace("#UserName#", userName);
                body = body.Replace("#UrlRedfinirSenha#", url);

                return body;
            }
        }

        public string BodySentSale(int saleId, string trackingCode, string userName)
        {
            using (StreamReader html = new StreamReader(@"Templates\BodyVendaEnviada.html"))
            {
                string body = html.ReadToEnd();
                body = body.Replace("#UserName#", userName);
                body = body.Replace("#VendaId#", saleId.ToString());
                body = body.Replace("#CodigoRastreio#", trackingCode);

                return body;
            }
        }
    }
}