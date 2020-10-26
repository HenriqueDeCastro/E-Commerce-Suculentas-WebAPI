using System.Collections.Generic;

namespace Suculentas.WebApi.Dtos
{
    public class EstadoDto
    {
        
        public int Id { get; set; }
        public string Nome { get; set; }
        public string UF { get; set; }
        public List<CidadeDto> Cidades { get; set; }
    }
}