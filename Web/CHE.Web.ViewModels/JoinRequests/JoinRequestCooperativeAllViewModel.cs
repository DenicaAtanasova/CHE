namespace CHE.Web.ViewModels.JoinRequests
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class JoinRequestCooperativeAllViewModel : IMapFrom<JoinRequest>
    {
        public string Id { get; init; }

        public string SenderUserName { get; init; }

        public string CooperativeAdminUserName { get; init; }
    }
}