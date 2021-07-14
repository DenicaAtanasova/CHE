namespace CHE.Services.Data
{
    using CHE.Web.InputModels.JoinRequests;
    using CHE.Web.InputModels.Reviews;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICheUsersService
    {
        Task<TEntity> GetByIdAsync<TEntity>(string id);

        //TODO: Rename if used only for teachers
        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(int startIndex, int endIndex, string schoolLevelFilter);

        Task<int> Count(string schoolLevelFilter);

        Task AcceptRequestAsync(string requestId);

        Task RejectRequestAsync(string requestId);

        Task SendRequestAsync(string senderId, JoinRequestCreateInputModel inputModel);

        Task SendReviewAsync(string senderId, ReviewCreateInputModel inputModel);
    }
}