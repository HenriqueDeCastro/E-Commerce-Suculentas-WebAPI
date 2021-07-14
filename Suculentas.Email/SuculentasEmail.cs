using System.Web;
using System;
using System.Net;
using System.Net.Mail;
using System.IO;

namespace Suculentas.Email
{
    public class SuculentasEmail: ISuculentasEmail
    {
        public bool EnviarEmailSuculentas(string EmailFrom, string Titulo, string Mensagem)
        {
            try
            {
                SmtpClient cliente = new SmtpClient();
                NetworkCredential credenciais = new NetworkCredential();
                MailMessage mensagem = new MailMessage();

                cliente.Host = "server.hostingnow.com.br";
                cliente.Port = 587;
                cliente.EnableSsl = true;
                cliente.UseDefaultCredentials = false;
                cliente.DeliveryMethod = SmtpDeliveryMethod.Network;

                credenciais.UserName = "suculentasdaro@suculentasdaro.com";
                credenciais.Password = "su20cu20lent@$";

                cliente.Credentials = credenciais;

                mensagem.From = new MailAddress("suculentasdaro@suculentasdaro.com", "Suculentas da Rô");
                mensagem.Subject = Titulo;
                mensagem.Body = Mensagem;
                mensagem.IsBodyHtml = true;
                mensagem.To.Add(new MailAddress(EmailFrom));

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                cliente.Send(mensagem);


                return true;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public bool EnviarEmailSuporte(string EmailFrom, string Titulo, string Mensagem)
        {
            try
            {
                SmtpClient cliente = new SmtpClient();
                NetworkCredential credenciais = new NetworkCredential();
                MailMessage mensagem = new MailMessage();

                cliente.Host = "server.hostingnow.com.br";
                cliente.Port = 587;
                cliente.EnableSsl = true;
                cliente.UseDefaultCredentials = false;
                cliente.DeliveryMethod = SmtpDeliveryMethod.Network;

                credenciais.UserName = "suporte@suculentasdaro.com";
                credenciais.Password = "su20cu20lent@$";

                cliente.Credentials = credenciais;

                mensagem.From = new MailAddress("suporte@suculentasdaro.com", "Suporte Suculentas");
                mensagem.Subject = Titulo;
                mensagem.Body = Mensagem;
                mensagem.IsBodyHtml = true;
                mensagem.To.Add(new MailAddress(EmailFrom));

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                cliente.Send(mensagem);


                return true;
            }
            catch (System.Exception e)
            {
                throw;
            }
        }

        public string BodyEsqueciSenha(string url, string userName)
        {
            using (StreamReader html = new StreamReader(@"Templates\BodyEsqueciSenha.html"))
            {
                string body = html.ReadToEnd();
                body = body.Replace("#UserName#", userName);
                body = body.Replace("#UrlRedfinirSenha#", url);

                return body;
            }
        }

        public string BodyVendaEnviada(int vendaId, string codigoRastreio, string userName)
        {
            using (StreamReader html = new StreamReader(@"Templates\BodyVendaEnviada.html"))
            {
                string body = html.ReadToEnd();
                body = body.Replace("#UserName#", userName);
                body = body.Replace("#VendaId#", vendaId.ToString());
                body = body.Replace("#CodigoRastreio#", codigoRastreio);

                return body;
            }
        }
    }
}