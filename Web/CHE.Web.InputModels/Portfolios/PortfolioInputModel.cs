namespace CHE.Web.InputModels.Portfolios
{
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;

    public class PortfolioInputModel
    {
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        public string Education { get; set; }

        public string Experience { get; set; }

        public string Skills { get; set; }

        public string Interests { get; set; }

        [Display(Name = "School level")]
        public string EducationLevel { get; set; }

        public IFormFile Image { get; set; }
    }
}