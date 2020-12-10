using System.Collections.Generic;

namespace Suculentas.Domain
{
    public class TipoProduto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<Produto> Produtos { get; set; }
    }
}