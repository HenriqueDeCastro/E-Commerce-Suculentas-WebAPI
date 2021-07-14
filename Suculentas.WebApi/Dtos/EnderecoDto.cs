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
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public int UserId { get; set; }
    }
}