using System;
using System.Collections.Generic;

namespace Suculentas.Domain
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public int? Inventory { get; set; }
        public int? MaximumQuantity { get; set; }
        public int ProductTypeId { get; set; }
        public int CategoryId { get; set; }
        public bool Active { get; set; }
        public ProductType TipoProduto { get; set; }
        public Category Category  { get; set; }
        public List<Order> Orders { get; set; }

    }
}