using System;

namespace Suculentas.Email
{
    public interface ISuculentasEmail
    {
        bool SendEmailSuculentas(string from, string title, string message);
        bool SendEmailSupport(string from, string title, string message);
        string BodyForgotPassword(string url, string userName);
        string BodySentSale(int saleId, string trackingCode, string userName);
    }
}