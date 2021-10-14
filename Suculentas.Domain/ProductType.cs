using System.Collections.Generic;

namespace Suculentas.Domain
{
    public class ProductType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}