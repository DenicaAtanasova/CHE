namespace CHE.Data.Models
{
    using CHE.Data.Common.Models;
    using System.ComponentModel.DataAnnotations;

    public class Image : BaseModel<string>
    { 
        [Required]
        public string Url { get; set; }

        [Required]
        public string Caption { get; set; }

        [Required]
        public string ProfileId { get; init; }

        public Profile Profile { get; init; }
    }
}