using System;
using System.Collections.Generic;
using Suculentas.Domain.Identity;

namespace Suculentas.Domain
{
    public class Endereco
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Rua { get; set; }
        public int Numero { get; set; }
        public string Complemento { get; set; }
        public string CEP { get; set; }
        public int CidadeId { get; set; }
        public int UserId { get; set; }
        public Cidade Cidade { get; set; }
        public User User { get; set; }
    }
}