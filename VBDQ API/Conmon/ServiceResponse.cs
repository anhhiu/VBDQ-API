namespace VBDQ_API.Conmon
{
    public class ServiceResponse <T> where T : class
    {
        public T? Data { get; set; }
        public int StatusCode { get; set; }
        public string? Message { get; set; }
    }
}
