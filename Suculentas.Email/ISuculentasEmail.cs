using System;

namespace Suculentas.Email
{
    public interface ISuculentasEmail
    {
        bool EnviarEmailSuculentas(string EmailFrom, string Titulo, string Mensagem);
        string BodyEsqueciSenha(string url, string userName);
        string BodyVendaEnviada(int vendaId, string codigoRastreio, string userName);
    }
}