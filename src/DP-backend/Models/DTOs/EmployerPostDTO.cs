namespace DP_backend.Models.DTOs
{
    public class EmployerPostDTO
    {
        public string CompanyName { get; set; }

        public EmployerPostDTO(string companyName)
        {
            CompanyName = companyName;
        }
    }
}
