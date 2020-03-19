namespace CHE.Data.Models
{
    using Common.Models;
    using System.Collections.Generic;

    public class Cooperative : BaseDeletableModel<string>
    {
        public Cooperative()
        {
            this.JoinRequestsReceived = new HashSet<JoinRequest>();
            this.Members = new HashSet<CheUserCooperative>();
        }

        public string Name { get; set; }

        public string Info { get; set; }

        public string AddressId { get; set; }

        public Address Address { get; set; }

        public string GradeId { get; set; }

        public Grade Grade { get; set; }

        public string CreatorId { get; set; }

        public CheUser Creator { get; set; }

        public Schedule Schedule { get; set; }

        public ICollection<CheUserCooperative> Members { get; set; }

        public ICollection<JoinRequest> JoinRequestsReceived { get; set; }
    }
}