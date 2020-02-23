namespace CHE.Web.InputModels.Cooperatives
{
    using System.ComponentModel.DataAnnotations;
    using CHE.Web.InputModels.Attributes.Validation;
    public class CooperativeEditInputModel
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Info { get; set; }

        [Required]
        [Grade]
        public string Grade { get; set; }

        public CooperativeAddressInputModel Address { get; set; }
    }
}