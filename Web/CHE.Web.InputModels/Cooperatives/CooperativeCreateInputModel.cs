namespace CHE.Web.InputModels.Cooperatives
{
    using CHE.Web.InputModels.Attributes.Validation;
    using System.ComponentModel.DataAnnotations;

    public class CooperativeCreateInputModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Info { get; set; }

        [Required]
        [Grade]
        public string Grade { get; set; }
    }
}