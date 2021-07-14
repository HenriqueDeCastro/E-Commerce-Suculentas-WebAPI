using System.Collections.Generic;

namespace Suculentas.WebApi.Dtos
{
    public class VendaDto
    {
        public int Id { get; set; }
        public string DataVenda { get; set; }
        public string CodigoTransacao { get; set; }
        public string StatusPagSeguro { get; set; }
        public double Valor { get; set; }
        public bool Frete { get; set; }
        public double? ValorFrete { get; set; }
        public string CodigoRastreio { get; set; }
        public int StatusId { get; set; }
        public int UserId { get; set; }
        public string Endereco { get; set; }
        public UserDto User { get; set; }
        public List<PedidoDto> Pedidos { get; set; }
    }
}
