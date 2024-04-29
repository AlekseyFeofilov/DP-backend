namespace DP_backend.Models.DTOs
{
    public class ObjectWithDataDTO<T>
    {
        public T Object { get; set; }
        public DateTime? Date { get; set; }
    }
}
