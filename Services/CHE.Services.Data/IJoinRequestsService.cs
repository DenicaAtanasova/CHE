namespace CHE.Services.Data
{
    using CHE.Web.InputModels.JoinRequests;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IJoinRequestsService
    {
        Task<TEntity> GetByIdAsync<TEntity>(string id);

        Task<IEnumerable<TEntity>> GetAllByTeacherAsync<TEntity>(string teacherId);

        Task<IEnumerable<TEntity>> GetAllByCooperativeAsync<TEntity>(string cooperativeId);

        Task<string> CreateAsync(string senderId, JoinRequestInputModel inputModel);
    }
}