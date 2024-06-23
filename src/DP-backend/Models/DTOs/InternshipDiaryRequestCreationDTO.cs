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
        
        /// <summary>
        /// Руководитель практики от профильной организации; если <c>null</c> используется уполномоченный представитель из Трудоустройства
        /// </summary>
        public string? ManagerFromEmployment { get; set; }

        /// <summary>
        /// Индивидуальный задание на практике; если <c>null</c> используется имя компании 
        /// </summary>
        public string? StudentIndividualTask { get; set; }
    }
}
