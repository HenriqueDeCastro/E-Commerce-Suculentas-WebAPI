using System;
using System.Collections.Generic;
using System.Text;

namespace Suculentas.Domain
{
    public class LogExcecao
    {
        public int Id { get; set; }
        public string Mensagem { get; set; }
        public string StackTrace { get; set; }
        public string InnerException { get; set; }
        public string InnerExceptionMensagem { get; set; }
    }
}
