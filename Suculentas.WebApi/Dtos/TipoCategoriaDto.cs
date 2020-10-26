using System.Collections.Generic;

namespace Suculentas.WebApi.Dtos
{
    public class TipoCategoriaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<CategoriaDto> Categorias { get; set; }
    }
}