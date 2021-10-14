namespace Suculentas.WebApi.Dtos
{
    public class AddressDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Road { get; set; }
        public int Number { get; set; }
        public string Complement { get; set; }
        public string CEP { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int UserId { get; set; }
    }
}