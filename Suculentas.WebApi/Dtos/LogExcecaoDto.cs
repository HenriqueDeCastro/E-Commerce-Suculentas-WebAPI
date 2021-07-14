using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Suculentas.WebApi.Dtos
{
    public class LogExcecaoDto
    {
        public int Id { get; set; }
        public string Mensagem { get; set; }
        public string StackTrace { get; set; }
        public string InnerException { get; set; }
        public string InnerExceptionMensagem { get; set; }
    }
}
