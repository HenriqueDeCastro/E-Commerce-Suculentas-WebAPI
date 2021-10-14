using System;
using System.Collections.Generic;
using System.Text;

namespace Suculentas.Domain.Pagination
{
    public class SalePagination
    {
        public Sale[] Sales { get; set; }
        public bool LastPage { get; set; }
    }
}
