using System.Collections.Generic;
using Suculentas.PagSeguro.Dtos;

namespace Suculentas.PagSeguro
{
    public interface ISuculentasPagSeguro
    {
        string Checkout(string emailUsuario, string token, string urlCheckout, List<PagSeguroItemDTO> itens, string reference);
        ConsultaTransacaoPagSeguroTransactionDTO ConsultaPorCodigoReferencia(string emailUsuario, string token, string urlConsultaTransacao, string codigoReferencia);
        bool CancelarTransacao(string emailUsuario, string token, string urlCancelamento, string transactionCode);
    }
}