namespace CHE.Web.ViewModels.JoinRequests
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class JoinRequestDetailsViewModel : IMapFrom<JoinRequest>
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public string SenderUserName { get; set; }

        public string ReceiverId { get; set; }

        public string CooperativeId { get; set; }

        public string CooperativeName { get; set; }
    }
}