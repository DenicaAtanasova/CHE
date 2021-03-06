﻿namespace CHE.Services.Data
{
    using System.Threading.Tasks;

    public interface ISchedulesService
    {
        Task<TEntity> GetByIdAsync<TEntity>(string id);

        Task<string> GetIdByUserAsync(string userId);

        Task<string> GetIdByCooperativeAsync(string cooperativeId);
    }
}