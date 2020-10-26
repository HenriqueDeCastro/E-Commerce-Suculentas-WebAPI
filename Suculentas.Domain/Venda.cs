using System;
using System.Collections.Generic;
using Suculentas.Domain.Identity;

namespace Suculentas.Domain
{
    public class Venda
    {
        public int Id { get; set; }
        public DateTime DataVenda { get; set; }
        public double Valor { get; set; }
        public double Frete { get; set; }
        public int StatusId { get; set; }
        public int UserId { get; set; }
        public int EmpresaFreteId { get; set; }
        public int EnderecoId { get; set; }
        public Status Status { get; set; }
        public User User { get; set; }
        public EmpresaFrete EmpresaFrete { get; set; }
        public Endereco Endereco { get; set; }
        public List<Pedido> Pedidos { get; set; }

    }
}