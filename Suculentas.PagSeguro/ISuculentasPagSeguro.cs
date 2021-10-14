using System.Collections.Generic;
using Suculentas.PagSeguro.Dtos;

namespace Suculentas.PagSeguro
{
    public interface ISuculentasPagSeguro
    {
        string Checkout(string emailUser, string token, string urlCheckout, List<PagSeguroItemDTO> items, string reference);
        PagSeguroTransactionDTO ConsultByReferenceCode(string emailUser, string token, string urlConsultaTransaction, string referenceCode);
        bool CancelTransaction(string emailUser, string token, string urlCancel, string transactionCode);
    }
}