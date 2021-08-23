namespace CHE.Data.Models
{
    using CHE.Data.Models.Enums;
    using Common.Models;

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static CHE.Common.DataConstants.Cooperative;

    public class Cooperative : BaseModel<string>
    {
        public Cooperative()
        {
            this.JoinRequestsReceived = new HashSet<JoinRequest>();
            this.Members = new HashSet<ParentCooperative>();
        }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(InfoMaxLength)]
        public string Info { get; set; }

        [Required]
        public string AddressId { get; set; }

        public Address Address { get; set; }

        [Required]
        [EnumDataType(typeof(Grade))]
        public Grade Grade { get; set; }

        [Required]
        public string AdminId { get; set; }

        public Parent Admin { get; set; }

        public Schedule Schedule { get; init; }

        public Messenger Messenger { get; set; }

        public ICollection<ParentCooperative> Members { get; set; }

        public ICollection<JoinRequest> JoinRequestsReceived { get; set; }
    }
}