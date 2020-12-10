using System.Collections.Generic;

namespace Suculentas.WebApi.Dtos
{
    public class TipoProdutoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<ProdutoDto> Produtos { get; set; }
    }
}