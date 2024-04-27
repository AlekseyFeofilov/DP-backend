namespace DP_backend.Models.DTOs
{
    public class PaginationDTO
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public PaginationDTO() { }
        public PaginationDTO(int currentPage, int pageCount, int pageSize)
        {
            CurrentPage = currentPage;
            PageCount = pageCount;
            PageSize = pageSize;
        }
    }
}
