namespace Suculentas.WebApi.Dtos
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public ProductDTO Product { get; set; }

    }
}