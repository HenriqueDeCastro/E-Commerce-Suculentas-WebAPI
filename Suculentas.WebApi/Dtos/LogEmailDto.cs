namespace Suculentas.WebApi.Dtos
{
    public class LogEmailDTO
    {
        public int Id { get; set; }
        public string To { get; set; }
        public string Topic { get; set; }
        public string Body { get; set; }
        public string ExceptionMessage { get; set; }
    }
}
