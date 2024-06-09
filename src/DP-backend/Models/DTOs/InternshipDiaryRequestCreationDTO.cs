namespace DP_backend.Models.DTOs
{
    public class InternshipDiaryRequestCreationDTO
    {
        /// <summary>
        /// При null в качестве id берётся информация из токена авторизации
        /// </summary>
        public Guid? StudentId { get; set; }

        /// <summary>
        /// Допустимые значения: 5,6,7,8
        /// </summary>
        public int Semester { get; set; }
    }
}
