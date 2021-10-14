using System;
using System.Collections.Generic;
using System.Text;

namespace Suculentas.Domain
{
    public class LogException
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string InnerException { get; set; }
        public string InnerExceptionMessage { get; set; }
    }
}
