namespace Suculentas.Domain
{
    public class Pedido
    {
        public int Id { get; set; }
        public int Quantidade { get; set; }
        public int ProdutoId { get; set; }
        public double Preco { get; set; }
        public int VendaId { get; set; }
        public Produto Produto { get; set; }
        public Venda Venda { get; set; }
    }
}