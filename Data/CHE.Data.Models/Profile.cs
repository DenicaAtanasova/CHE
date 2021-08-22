namespace CHE.Data.Models
{
    using CHE.Data.Models.Enums;
    using Common.Models;

    using System.ComponentModel.DataAnnotations;

    using static CHE.Common.DataConstants.Profile;

    public class Profile : BaseModel<string>
    {
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; }

        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; }

        [MaxLength(EducationMaxLength)]
        public string Education { get; set; }

        [MaxLength(ExperienceMaxLength)]
        public string Experience { get; set; }

        [MaxLength(SkillsMaxLength)]
        public string Skills { get; set; }

        [MaxLength(InterestsMaxLength)]
        public string Interests { get; set; }

        [EnumDataType(typeof(SchoolLevel))]
        public SchoolLevel SchoolLevel { get; set; }

        [Required]
        public string OwnerId { get; init; }

        public Teacher Owner { get; init; }

        public Image Image { get; init; }

        public string AddressId { get; set; }

        public Address Address { get; set; }
    }
}