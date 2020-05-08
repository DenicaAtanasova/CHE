namespace CHE.Web.ViewModels.JoinRequests
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class JoinRequestCooperativeSendViewModel : IMapFrom<Cooperative>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}