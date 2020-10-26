using System.Collections.Generic;

namespace Suculentas.WebApi.Dtos
{
    public class VendaDto
    {
        public int Id { get; set; }
        public string DataVenda { get; set; }
        public double Valor { get; set; }
        public double Frete { get; set; }
        public int StatusId { get; set; }
        public int UserId { get; set; }
        public int EmpresaFreteId { get; set; }
        public int EnderecoId { get; set; }
        public List<PedidoDto> Pedidos { get; set; }
    }
}
