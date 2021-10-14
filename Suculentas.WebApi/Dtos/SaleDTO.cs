using System.Collections.Generic;

namespace Suculentas.WebApi.Dtos
{
    public class SaleDTO
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string TransactionCode { get; set; }
        public string StatusPagSeguro { get; set; }
        public double Price { get; set; }
        public bool Shipping { get; set; }
        public double? ShippingValue { get; set; }
        public string TrackingCode { get; set; }
        public int StatusId { get; set; }
        public int UserId { get; set; }
        public string Address { get; set; }
        public UserDTO User { get; set; }
        public List<OrderDTO> Orders { get; set; }
    }
}
