namespace Suculentas.WebApi.Dtos
{
    public class PedidoDto
    {
        public int Id { get; set; }
        public int Quantidade { get; set; }
        public int ProdutoId { get; set; }
        public double Preco { get; set; }
        public int VendaId { get; set; }
        public ProdutoDto Produto { get; set; }        
    }
}