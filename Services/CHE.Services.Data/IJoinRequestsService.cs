namespace CHE.Services.Data
{
    using System.Threading.Tasks;

    public interface IJoinRequestsService
    {
        Task<bool> CreateAsync(string content, string cooperativeId, string senderId, string receiverId);

        Task<TEntity> GetByIdAsync<TEntity>(string id);

        Task<bool> SendAsync(string requestContent, string cooperativeId, string receiverId, string senderName);

        Task<bool> AcceptAsync(string requestId);

        Task<bool> RejectAsync(string requestId);
    }
}