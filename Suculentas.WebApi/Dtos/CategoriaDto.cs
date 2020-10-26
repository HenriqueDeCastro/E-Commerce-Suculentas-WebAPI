using System.Collections.Generic;

namespace Suculentas.WebApi.Dtos
{
    public class CategoriaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public int TipoCategoriaId { get; set; }
        public List<ProdutoDto> Produtos { get; set; }
    }
}