namespace CHE.Web.ViewModels.JoinRequests
{
    using System.Collections.Generic;

    public class JoinRequestCooperativeAllListViewModel
    {
        public string CooperativeId { get; set; }

        public IEnumerable<JoinRequestCooperativeAllViewModel> JoinRequests { get; set; }
    }
}