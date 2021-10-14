using System;
using System.Collections.Generic;
using System.Text;

namespace Suculentas.Domain
{
    public class LogEmail
    {
        public int Id { get; set; }
        public string To { get; set; }
        public string Topic { get; set; }
        public string Body { get; set; }
        public string ExceptionMessage { get; set; }
    }
}
