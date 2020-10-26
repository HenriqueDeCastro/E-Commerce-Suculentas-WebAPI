using System;
using System.Collections.Generic;

namespace Suculentas.Domain
{
    public class EmpresaFrete
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<Venda> Vendas { get; set; }
    }
}