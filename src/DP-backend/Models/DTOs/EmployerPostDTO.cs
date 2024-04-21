using DP_backend.Domain.Employment;

namespace DP_backend.Models.DTOs
{
    public class EmployerPostDTO
    {
        public string CompanyName { get; set; }
        public string PlacesQuantity { get; set; }
        public string CommunicationPlace { get; set; }
        public string Contact { get; set; }
        public string Comment { get; set; }
        public EmployerPostDTO() { }
        public EmployerPostDTO(Employer model)
        {
            CompanyName = model.CompanyName;
            PlacesQuantity = model.PlacesQuantity;
            CommunicationPlace = model.CommunicationPlace;
            Contact = model.Contact;
            Comment = model.Comment;
        }
    }
}
