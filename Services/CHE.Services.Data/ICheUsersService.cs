namespace CHE.Services.Data
{
    using CHE.Services.Data.Enums;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICheUsersService
    {
        Task<TEntity> GetByIdAsync<TEntity>(string id);

        //TODO: Rename if used only for teachers
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