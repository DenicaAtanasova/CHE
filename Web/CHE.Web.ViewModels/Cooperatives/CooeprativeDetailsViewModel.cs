using System.Collections.Generic;

namespace CHE.Web.ViewModels.Cooperatives
{
    public class CooeprativeDetailsViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Info { get; set; }

        public string Grade { get; set; }

        public int MembersCount { get; set; }

        public string CreatorUserName { get; set; }

        public string ScheduleId { get; set; }

        public CooperativeAddressViewModel Address { get; set; }

        public IEnumerable<CooperativeJoinRequestDetailsViewModel> JoinRequestsReceived { get; set; }

        public IEnumerable<CooperativeUserDetailsViewModel> Members { get; set; }
    }
}