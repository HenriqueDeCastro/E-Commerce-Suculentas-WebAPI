using System.Collections.Generic;

namespace Suculentas.WebApi.Dtos
{
    public class ProdutoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public double Preco { get; set; }
        public string Imagem { get; set; }
        public int Estoque { get; set; }
        public bool Ativo { get; set; }
        public int CategoriaId { get; set; }
        public List<PedidoDto> Pedidos { get; set; }
    }
}