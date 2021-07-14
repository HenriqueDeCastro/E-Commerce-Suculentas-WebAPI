using System;

namespace Suculentas.PagSeguro.Dtos
{
    public class ConsultaTransacaoPagSeguroTransactionDTO
    {
        public string Reference { get; set; }
        public string Code { get; set; }
        public int type { get; set; }
        public int Status { get; set; }
        public int PaymentMethodType { get; set; }
    }
}