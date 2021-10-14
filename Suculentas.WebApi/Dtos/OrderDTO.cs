namespace Suculentas.WebApi.Dtos
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int Quantidade { get; set; }
        public int ProdutoId { get; set; }
        public double Preco { get; set; }
        public int VendaId { get; set; }
        public ProductDTO Product { get; set; }        
    }
}