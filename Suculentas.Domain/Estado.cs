using System;
using System.Collections.Generic;

namespace Suculentas.Domain
{
    public class Estado
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string UF { get; set; }
        public List<Cidade> Cidades { get; set; }
    }
}