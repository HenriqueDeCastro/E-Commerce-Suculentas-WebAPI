using System;
using System.Collections.Generic;
using System.Text;

namespace Suculentas.Domain.Pagination
{
    public class VendaPagination
    {
        public Venda[] Vendas { get; set; }
        public bool UltimaPagina { get; set; }
    }
}
