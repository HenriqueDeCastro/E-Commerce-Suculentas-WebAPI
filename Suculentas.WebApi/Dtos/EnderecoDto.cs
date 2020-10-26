namespace Suculentas.WebApi.Dtos
{
    public class EnderecoDto
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Rua { get; set; }
        public int Numero { get; set; }
        public string Complemento { get; set; }
        public string CEP { get; set; }
        public int CidadeId { get; set; }
        public int UserId { get; set; }
    }
}