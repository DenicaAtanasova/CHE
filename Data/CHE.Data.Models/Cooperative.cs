namespace CHE.Data.Models
{
    using Common.Models;
    using System.Collections.Generic;

    public class Cooperative : BaseModel<string>
    {
        public Cooperative()
        {
            this.JoinRequestsReceived = new HashSet<JoinRequest>();
            this.Members = new HashSet<ParentCooperative>();
        }

        public string Name { get; set; }

        public string Info { get; set; }

        public string AddressId { get; set; }

        public Address Address { get; set; }

        public string GradeId { get; set; }

        public Grade Grade { get; set; }

        public string AdminId { get; set; }

        public Parent Admin { get; set; }

        public Schedule Schedule { get; init; }

        public Messenger Messenger { get; set; }

        public ICollection<ParentCooperative> Members { get; set; }

        public ICollection<JoinRequest> JoinRequestsReceived { get; set; }
    }
}