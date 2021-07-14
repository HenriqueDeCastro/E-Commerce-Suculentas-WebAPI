using System;
using System.Collections.Generic;
using System.Text;

namespace Suculentas.Domain.Pagination
{
    public class CategoriaPagination
    {
        public Categoria Categoria { get; set; }
        public Produto[] Produtos { get; set; }
        public bool UltimaPagina { get; set; }
    }
}
