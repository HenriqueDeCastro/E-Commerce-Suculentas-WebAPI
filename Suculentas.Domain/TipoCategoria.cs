using System.Collections.Generic;

namespace Suculentas.Domain
{
    public class TipoCategoria
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<Categoria> Categorias { get; set; }
    }
}