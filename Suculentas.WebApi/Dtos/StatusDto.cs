using System.Collections.Generic;

namespace Suculentas.WebApi.Dtos
{
    public class StatusDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SaleDTO> Sales { get; set; }
    }
}