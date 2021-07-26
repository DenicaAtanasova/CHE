namespace CHE.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICheUsersService
    {
        Task<TEntity> GetByIdAsync<TEntity>(string id);

        //TODO: Rename if used only for teachers
        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(
            int startIndex = 1,
            int endIndex = 0,
            string schoolLevelFilter = null,
            string cityFilter = null,
            string neighbourhoodFilter = null);

        Task<int> CountAsync(
            string schoolLevelFilter = null,
            string cityFilter = null,
            string neighbourhoodFilter = null);

        Task AcceptRequestAsync(string requestId);

        Task RejectRequestAsync(string requestId);

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