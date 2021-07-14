using System.Collections.Generic;

namespace Suculentas.Domain
{
    public class Status
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<Venda> Vendas { get; set; }
    }
}