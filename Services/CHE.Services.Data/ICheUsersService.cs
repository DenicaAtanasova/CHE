namespace CHE.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICheUsersService
    {
        //Teachers
        Task<TEntity> GetByIdAsync<TEntity>(string id);
        //Teachers
        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(
            string role,
            int startIndex = 1,
            int endIndex = 0,
            string schoolLevelFilter = null,
            string cityFilter = null,
            string neighbourhoodFilter = null);
        //Teachers
        Task<int> CountAsync(
            string role,
            string schoolLevelFilter = null,
            string cityFilter = null,
            string neighbourhoodFilter = null);
        //Parent
        Task AcceptRequestAsync(
            string requestId,
            string cooperativeId,
            string senderId);
        //Parent
        Task RejectRequestAsync(
            string requestId,
            string cooperativeId,
            string senderId);
        //Parent
        Task SendRequestAsync(
            string senderId,
            string content,
            string cooperativeId);

        Task SendReviewAsync(
            string senderId,
            string receiverId,
            string comment,
            int rating);
    }
}