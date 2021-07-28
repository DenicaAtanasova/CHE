namespace CHE.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICheUsersService
    {
        Task<TEntity> GetByIdAsync<TEntity>(string id);

        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(
            string role,
            int startIndex = 1,
            int endIndex = 0,
            string schoolLevelFilter = null,
            string cityFilter = null,
            string neighbourhoodFilter = null);

        Task<int> CountAsync(
            string role,
            string schoolLevelFilter = null,
            string cityFilter = null,
            string neighbourhoodFilter = null);

        Task AcceptRequestAsync(
            string requestId,
            string cooperativeId,
            string senderId,
            string receiverId);

        Task RejectRequestAsync(
            string requestId,
            string cooperativeId,
            string senderId,
            string receiverId);

        Task SendRequestAsync(
            string senderId,
            string content,
            string cooperativeId,
            string receiverId);

        Task SendReviewAsync(
            string senderId,
            string receiverId,
            string comment,
            int rating);
    }
}