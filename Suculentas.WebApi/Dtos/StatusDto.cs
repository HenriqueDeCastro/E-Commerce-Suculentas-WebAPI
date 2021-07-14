using System.Collections.Generic;

namespace Suculentas.WebApi.Dtos
{
    public class StatusDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<VendaDto> Vendas { get; set; }
    }
}