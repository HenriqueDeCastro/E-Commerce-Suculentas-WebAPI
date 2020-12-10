using System.Net;
using System.Net.Mail;

namespace Suculentas.WebApi.Utils
{
    public class EnvioEmail
    {
        public bool EnviarEmail(string EmailFrom, string Titulo, string Mensagem)
        {
            try
            {
                SmtpClient cliente = new SmtpClient();
                NetworkCredential credenciais = new NetworkCredential();
                MailMessage mensagem = new MailMessage();

                cliente.UseDefaultCredentials = false;
                cliente.Host = "smtp.gmail.com";
                cliente.Port = 587;
                cliente.EnableSsl = true;
                cliente.DeliveryMethod = SmtpDeliveryMethod.Network;

                credenciais.UserName = "suculentasRo";
                credenciais.Password = "su20cu20lent@$";
                
                cliente.Credentials = credenciais;

                mensagem.From = new MailAddress("suculentasRo@gmail.com");
                mensagem.Subject = Titulo;
                mensagem.Body = Mensagem;
                mensagem.IsBodyHtml = true;
                mensagem.To.Add(EmailFrom);

                cliente.Send(mensagem);

                return true;
            }
            catch (System.Exception e)
            {
                throw;
            }
        }   
    }
}