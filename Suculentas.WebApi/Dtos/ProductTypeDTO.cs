using System.Collections.Generic;

namespace Suculentas.WebApi.Dtos
{
    public class ProductTypeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProductDTO> Products { get; set; }
    }
}