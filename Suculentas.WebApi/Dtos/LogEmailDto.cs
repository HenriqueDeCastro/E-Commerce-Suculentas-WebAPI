using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Suculentas.WebApi.Dtos
{
    public class LogEmailDto
    {
        public int Id { get; set; }
        public string Para { get; set; }
        public string Assunto { get; set; }
        public string Corpo { get; set; }
        public string ExceptionMensagem { get; set; }
    }
}
