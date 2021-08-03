namespace CHE.Services.Data
{
    using System.Threading.Tasks;

    public interface IParentsService
    {
        Task<string> CreateAsync(string userId);

        Task SendRequestAsync(string userId, string cooperativeId, string content);

        Task AcceptRequestAsync(string senderId, string cooperativeId, string requestId);

        Task RejectRequestAsync(string senderId, string cooperativeId, string requestId);

        Task SendReviewAsync(string userId, string receiverId, string comment, int rating);
    }
}