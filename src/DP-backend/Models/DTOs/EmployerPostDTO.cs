using System.Text.Json.Serialization;
using DP_backend.Domain.Employment;

namespace DP_backend.Models.DTOs
{
    public class EmployerPostDTO
    {
        public string CompanyName { get; set; }
        public string? PlacesQuantity { get; set; }
        public string? CommunicationPlace { get; set; }
        public string? Contact { get; set; }
        public bool isPartner { get; set; }
        public string? Comment { get; set; }
        
        /// <summary>
        /// Используйте AuthorizedDelegate
        /// </summary>
        [Obsolete("Используйте AuthorizedDelegate")]
        [JsonPropertyName("tutor")]
        [JsonInclude]
        public string Tutor { get => AuthorizedDelegate; set => AuthorizedDelegate = value; }
        /// <summary>
        /// Уполномоченный представитель профильной организации
        /// </summary>
        public string AuthorizedDelegate {  get; set; }
        
        public string Vacancy { get; set; }
        public EmployerPostDTO() { }
        public EmployerPostDTO(Employer model)
        {
            CompanyName = model.CompanyName;
            PlacesQuantity = model.PlacesQuantity;
            CommunicationPlace = model.CommunicationPlace;
            Contact = model.Contact;
            isPartner = model.isPartner;
            Comment = model.Comment;
            AuthorizedDelegate = model.AuthorizedDelegate;
            Vacancy = model.Vacancy;
        }
    }
}
