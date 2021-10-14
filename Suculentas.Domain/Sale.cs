using System;
using System.Collections.Generic;
using Suculentas.Domain.Identity;

namespace Suculentas.Domain
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string TransactionCode { get; set; }
        public string StatusPagSeguro { get; set; }
        public double Price { get; set; }
        public bool Shipping { get; set; }
        public double? ShippingValue { get; set; }
        public string TrackingCode { get; set; }
        public int StatusId { get; set; }
        public int UserId { get; set; }
        public string Address { get; set; }
        public Status Status { get; set; }
        public User User { get; set; }
        public List<Order> Orders { get; set; }

    }
}