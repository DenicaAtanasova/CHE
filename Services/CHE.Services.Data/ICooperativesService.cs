﻿namespace CHE.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ICooperativesService
    {
        Task<bool> CreateAsync<TAddress>(string name, string info, string gradeValue, string creatorId, TAddress addres);

        Task<bool> UpdateAsync<TAddress>(string cooperativeId, string name, string info, string gradeValue, TAddress addres);

        Task<bool> DeleteAsync(string id);

        Task<TEntity> GetByIdAsync<TEntity>(string id);

        IQueryable<TEntity> GetAll<TEntity>();

        Task<IEnumerable<TEntity>> GetCreatorAllByUsernameAsync<TEntity>(string username);

        Task<bool> AddMemberAsync(string userId, string cooperativeId);

        Task<bool> RemoveMemberAsync(string memberId, string cooperativeId);

        Task<bool> CheckIfMemberAsync(string userId, string cooperativeId);
    }
}