using System;
using System.Collections.Generic;

namespace Suculentas.Domain
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public double Preco { get; set; }
        public string Imagem { get; set; }
        public int? Estoque { get; set; }
        public int? QuantidadeMaxima { get; set; }
        public int TipoProdutoId { get; set; }
        public int CategoriaId { get; set; }
        public bool Ativo { get; set; }
        public TipoProduto TipoProduto { get; set; }
        public Categoria Categoria  { get; set; }
        public List<Pedido> Pedidos { get; set; }

    }
}