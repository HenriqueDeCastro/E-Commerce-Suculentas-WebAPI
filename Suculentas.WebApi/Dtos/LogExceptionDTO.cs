namespace Suculentas.WebApi.Dtos
{
    public class LogExceptionDTO
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string InnerException { get; set; }
        public string InnerExceptionMessage { get; set; }
    }
}
