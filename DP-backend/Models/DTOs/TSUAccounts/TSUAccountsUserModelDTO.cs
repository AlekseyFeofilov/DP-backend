using System.ComponentModel.DataAnnotations;

namespace DP_backend.Models.DTOs.TSUAccounts
{
    public class TSUAccountsUserModelDTO
    {
        public Guid AccountId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender? Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsVerified { get; set; }
        public string StaffId { get; set; }
        public string FullName { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public string AvatarUrl { get; set; }
        public Guid? NsiId { get; set; }
    }

    public enum Gender
    {
        [Display(Name = "Мужской")]
        Male,

        [Display(Name = "Женский")]
        Female

    }
}
