using System.Collections.Generic;

namespace Suculentas.Domain
{
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Sale> Sales { get; set; }
    }
}