using System;
using System.Collections.Generic;

namespace Suculentas.PagSeguro.Dtos
{
    public class TransactionConsultationPagSeguroDTO
    {
        public DateTime Date { get; set; }
        public int ResultsInThisPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public List<PagSeguroTransactionDTO> pagSeguroTransactions { get; set;}
    }
}