namespace CHE.Services.Data
{
    using CHE.Web.InputModels.Reviews;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IReviewsService
    {
        Task<string> CreateAsync(string senderId, ReviewCreateInputModel inputModel);

        Task<IEnumerable<TEntity>> GetAllByReceiverAsync<TEntity>(string receiverId);
    }
}