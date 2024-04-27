namespace DP_backend.Models.DTOs
{
    public class StudentsWithPaginationDTO
    {
        public List<StudentDTO> Students { get; set; }
        public PaginationDTO Pagination { get; set; }
    }
}
