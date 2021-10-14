namespace Suculentas.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public Sale Sale { get; set; }
    }
}