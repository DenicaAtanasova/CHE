namespace CHE.Web.InputModels.Cooperatives
{
    using CHE.Data.Models;
    using CHE.Web.InputModels.Addresses;
    using CHE.Web.InputModels.Attributes.Validation;

    using System.ComponentModel.DataAnnotations;

    public class CooperativeCreateInputModel
    {
        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [Required]
        public string Info { get; set; }

        [Required]
        [Grade]
        public string Grade { get; set; }

        public AddressInputModel Address { get; set; }
    }
}