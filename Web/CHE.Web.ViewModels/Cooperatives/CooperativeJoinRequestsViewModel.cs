namespace CHE.Web.ViewModels.Cooperatives
{
    using CHE.Web.ViewModels.JoinRequests;
    using System.Collections.Generic;

    public class CooperativeJoinRequestsViewModel
    {
        public string Id { get; set; }

        public string ScheduleId { get; set; }

        public string CreatorUserName { get; set; }

        public IEnumerable<JoinRequestAllViewModel> JoinRequestsReceived { get; set; }
    }
}