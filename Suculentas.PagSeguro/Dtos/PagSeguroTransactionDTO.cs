using System;

namespace Suculentas.PagSeguro.Dtos
{
    public class PagSeguroTransactionDTO
    {
        public string Reference { get; set; }
        public string Code { get; set; }
        public int type { get; set; }
        public int Status { get; set; }
        public int PaymentMethodType { get; set; }
    }
}