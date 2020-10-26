using System;

namespace Suculentas.Domain
{
    public class Gastos
    {
        public int Id { get; set; }
        public int Descricao { get; set; }
        public int Valor { get; set; }
        public DateTime Data { get; set; }
    }
}