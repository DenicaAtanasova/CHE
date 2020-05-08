namespace CHE.Web.ViewModels.JoinRequests
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class JoinRequestAllViewModel : IMapFrom<JoinRequest>
    {
        public string Id { get; set; }

        public string SenderUserName { get; set; }
    }
}