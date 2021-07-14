using System;
using System.Collections.Generic;
using System.Text;

namespace Suculentas.Domain
{
    public class LogEmail
    {
        public int Id { get; set; }
        public string Para { get; set; }
        public string Assunto { get; set; }
        public string Corpo { get; set; }
        public string ExceptionMensagem { get; set; }
    }
}
