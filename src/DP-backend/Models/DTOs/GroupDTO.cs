using DP_backend.Common.Enumerations;
using DP_backend.Domain.Employment;

namespace DP_backend.Models.DTOs
{
    public class GroupDTO
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public Grade Grade { get; set; }

        public GroupDTO() { }
        public GroupDTO(Group group) 
        {
            Id = group.Id;
            Number = group.Number;
            Grade = group.Grade;
        }
    }
}
