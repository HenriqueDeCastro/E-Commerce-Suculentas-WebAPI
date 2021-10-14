using System;
using System.Collections.Generic;
using System.Text;

namespace Suculentas.Domain.Pagination
{
    public class CategoryPagination
    {
        public Category Category { get; set; }
        public Product[] Products { get; set; }
        public bool LastPage { get; set; }
    }
}
