using System;
using System.Collections.Generic;
using Suculentas.Domain.Identity;

namespace Suculentas.Domain
{
    public class Venda
    {
        public int Id { get; set; }
        public DateTime DataVenda { get; set; }
        public string CodigoTransacao { get; set; }
        public string StatusPagSeguro { get; set; }
        public double Valor { get; set; }
        public bool Frete { get; set; }
        public double? ValorFrete { get; set; }
        public string CodigoRastreio { get; set; }
        public int StatusId { get; set; }
        public int UserId { get; set; }
        public string Endereco { get; set; }
        public Status Status { get; set; }
        public User User { get; set; }
        public List<Pedido> Pedidos { get; set; }

    }
}