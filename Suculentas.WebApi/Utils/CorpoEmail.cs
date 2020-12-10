using Suculentas.WebApi.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Suculentas.WebApi.Utils
{
    public class CorpoEmail
    {
        public string BodyEsqueciSenha(string url, string userName)
        {
            string body = "<h1 style='color: black'>Olá, <span style='color: #77bfa3'>" + userName + ".</span></h1>" +
                           "<br>" +
                           "<h3 style='color: black'>Soubemos que você perdeu sua senha da <span style='color: #77bfa3'>Suculentas da Rô</span>, nos desculpe por isso!</h3>" +
                           "<h3 style='color: black'>Mas não se preocupe! Você pode usar o botão a seguir para acessar a página de redefinição de senha.</h3>" +
                           "<br>" +
                           "<div style='text-align: center;'>" +
                           "<a href='" + url + "'><button style='background: #77bfa3; border-radius: 4px; line-height: 36px; cursor: pointer; color: #fff; border: none; font-size: 16px; text-align: center; padding: 0 16px; font-size: 14px; font-weight: 100'>Redefinir senha</button></a> " +
                           "</div>" +
                           "<br> " +
                           "<h3 style='color: black'>Caso não utilize esse link em 3 horas, ele irá expirar. Para obter um novo link de redefinição de senha, visite https://suculentasdaro.com.br/esquecisenha</h3>" +
                           "<br>" +
                           "<h3 style='color: black'>Muito Obrigado!</h3>" +
                           "<h3 style='color: black'>Equipe Suculentas da Rô.</h3>";

            return body;
        }
    }
}
