﻿namespace CHE.Services.Data
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Data.Enums;
    using CHE.Services.Mapping;

    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CooperativesService : ICooperativesService
    {
        private readonly CheDbContext _dbContext;
        private readonly IGradesService _gradesService;
        private readonly IAddressesService _addressesService;

        public CooperativesService(
            CheDbContext dbContext,
            IGradesService gradesService,
            IAddressesService addressesService)
        {
            this._dbContext = dbContext;
            this._gradesService = gradesService;
            this._addressesService = addressesService;
        }

        public async Task<string> CreateAsync(
            string userId,
            string name,
            string info,
            string grade,
            string city,
            string neighbourhood)
        {
            var adminId = await this._dbContext.Parents
                .Where(x => x.UserId == userId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            var cooperative = new Cooperative
            {
                AdminId = adminId,
                Name = name,
                Info = info,
                GradeId = await this._gradesService.GetGardeIdAsync(grade),
                Schedule = new Schedule { CreatedOn = DateTime.UtcNow },
                Messenger = new Messenger 
                { 
                    CreatedOn = DateTime.UtcNow,
                    Users = new List<MessengerUser>
                    {
                        new MessengerUser 
                        {
                            UserId = userId 
                        }
                    }
                },
                AddressId = await this._addressesService
                    .GetAddressIdAsync(city, neighbourhood),
                CreatedOn = DateTime.UtcNow,
            };

            this._dbContext.Add(cooperative);
            await this._dbContext.SaveChangesAsync();

            return cooperative.Id;
        }

        public async Task UpdateAsync(
            string id,
            string name,
            string info,
            string grade,
            string city,
            string neighbourhood)
        {
            var cooperativeToUpdate = await this._dbContext.Cooperatives
                .SingleOrDefaultAsync(x => x.Id == id);

            if (cooperativeToUpdate == null)
            {
                return;
            }

            cooperativeToUpdate.Name = name;
            cooperativeToUpdate.Info = info;
            cooperativeToUpdate.GradeId = await this._gradesService.GetGardeIdAsync(grade);
            cooperativeToUpdate.AddressId = await this._addressesService
                .GetAddressIdAsync(city, neighbourhood);
            cooperativeToUpdate.ModifiedOn = DateTime.UtcNow;

            this._dbContext.Cooperatives.Update(cooperativeToUpdate);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var cooperativeFromDb = await this._dbContext.Cooperatives
                .SingleOrDefaultAsync(x => x.Id == id);

            if (cooperativeFromDb == null)
            {
                return;
            }

            this._dbContext.Remove(cooperativeFromDb);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(string id) =>
            await this._dbContext.Cooperatives
                .AsNoTracking()
                .Where(x => x.Id == id)
                .To<TEntity>()
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(
            int startIndex = 1, 
            int endIndex = 0, 
            string gradeFilter = null,
            string cityFilter = null,
            string neighbourhoodFilter = null)
        {
            var filteredCooperatives = this.GetFilteredCollection(gradeFilter, cityFilter, neighbourhoodFilter);

            var count = await this.GetCollectionCountAsync(filteredCooperatives, endIndex);

            return await this.GetCollectionPerPageAsync<TEntity>(filteredCooperatives, startIndex, count);
        }

        public async Task<IEnumerable<TEntity>> GetAllByUserAsync<TEntity>(
            string userId,
            CooperativeUserType userType,
            int startIndex = 1,
            int endIndex = 0)
        {
            var userCoperatives = this.GetCollectionByUser(userId, userType);

            var count = await this.GetCollectionCountAsync(userCoperatives, endIndex);

            return await this.GetCollectionPerPageAsync<TEntity>(userCoperatives, startIndex, count);
        }

        public async Task AddMemberAsync(string parentId, string cooperativeId)
        {
            this._dbContext.ParentsCooperatives.Add(
                new ParentCooperative
                {
                    CooperativeId = cooperativeId,
                    ParentId = parentId
                });

            //TODO: Move to messengerService
            var messengerId = await this._dbContext.Messengers
                .Where(x => x.CooperativeId == cooperativeId)
                .Select(x => x.Id)
                .SingleOrDefaultAsync();

            var userId = await this._dbContext.Parents
                .Where(x => x.Id == parentId)
                .Select(x => x.UserId)
                .SingleOrDefaultAsync();

            this._dbContext.MessengersUsers.Add(
                new MessengerUser
                {
                    MessengerId = messengerId,
                    UserId = userId
                });

            await this._dbContext.SaveChangesAsync();
        }

        public async Task RemoveMemberAsync(string memberId, string cooperativeId)
        {
            var member = await this._dbContext.ParentsCooperatives
                .SingleOrDefaultAsync(x => (x.ParentId == memberId || x.Parent.UserId == memberId) && 
                                            x.CooperativeId == cooperativeId);

            if (member == null)
            {
                return;
            }

            this._dbContext.ParentsCooperatives.Remove(member);

            var messengerId = await this._dbContext.Messengers
                .Where(x => x.CooperativeId == cooperativeId)
                .Select(x => x.Id)
                .SingleOrDefaultAsync();

            var userId = await this._dbContext.Parents
                .Where(x => x.Id == memberId)
                .Select(x => x.UserId)
                .SingleOrDefaultAsync();

            var messingerUser = await this._dbContext.MessengersUsers
                .SingleOrDefaultAsync(x => x.MessengerId == messengerId && x.UserId == userId);

            this._dbContext.MessengersUsers.Remove(messingerUser);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task ChangeAdminAsync(string parentId, string cooperativeId)
        {
            var cooperative = await this._dbContext.Cooperatives
                .SingleOrDefaultAsync(x => x.Id == cooperativeId);

            if (cooperative == null)
            {
                return;
            }

            var newAdmin = cooperative.Members
                .SingleOrDefault(x => x.ParentId == parentId);

            if (newAdmin == null)
            {
                return;
            }

            this._dbContext.ParentsCooperatives.Remove(newAdmin);

            this._dbContext.ParentsCooperatives.Add(
                new ParentCooperative
                {
                    CooperativeId = cooperativeId,
                    ParentId = cooperative.AdminId
                });

            cooperative.AdminId = parentId;

            this._dbContext.Cooperatives.Update(cooperative);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task<bool> CheckIfAdminAsync(string userId, string cooperativeId) =>
            await this._dbContext.Cooperatives
                .AnyAsync(x => x.Admin.UserId == userId &&
                               x.Id == cooperativeId);

        public async Task<bool> CheckIfMemberAsync(string userId, string cooperativeId) =>
            await this._dbContext.ParentsCooperatives
                .AnyAsync(x => x.Parent.UserId == userId && x.CooperativeId == cooperativeId);

        public async Task<int> CountByUserAsync(string userId) =>
            await this._dbContext.Cooperatives
                .AsNoTracking()
                .Where(x => x.Admin.UserId == userId ||
                            x.Members.Any(x => x.Parent.UserId == userId))
                .CountAsync();

        public async Task<int> CountAsync(
            string gradeFilter = null,
            string cityFilter = null,
            string neighbourhoodFilter = null) => 
                await this.GetFilteredCollection(gradeFilter, cityFilter, neighbourhoodFilter)
                    .CountAsync();

        private IQueryable<Cooperative> GetFilteredCollection(
            string gradeFilter = null,
            string cityFilter = null,
            string neighbourhoodFilter = null)
        {
            var cooperatives = this._dbContext.Cooperatives.AsNoTracking();

            if (gradeFilter != null)
            {
                cooperatives = cooperatives.Where(x => x.Grade.Value == gradeFilter);
            }

            if (cityFilter != null)
            {
                cooperatives = cooperatives.Where(x => x.Address.City == cityFilter);
            }

            if (neighbourhoodFilter != null)
            {
                cooperatives = cooperatives.Where(x => x.Address.Neighbourhood == neighbourhoodFilter);
            }

            return cooperatives;
        }

        private async Task<int> GetCollectionCountAsync(IQueryable<Cooperative> cooperatives, int endIndex)
            => endIndex == 0
                ? await cooperatives.CountAsync()
                : endIndex;

        private async Task<IEnumerable<TEntity>> GetCollectionPerPageAsync<TEntity>(
            IQueryable<Cooperative> cooperatives, 
            int startIndex, 
            int count) =>
            await cooperatives
                .Skip((startIndex - 1) * count)
                .Take(count)
                .To<TEntity>()
                .ToListAsync();

        private IQueryable<Cooperative> GetCollectionByUser(string userId, CooperativeUserType participant) =>
            participant switch
            {
                CooperativeUserType.Admin => 
                    _dbContext.Cooperatives
                        .AsNoTracking()
                        .Where(x => x.Admin.UserId == userId),
                CooperativeUserType.Member | CooperativeUserType.Admin => 
                    _dbContext.Cooperatives
                        .AsNoTracking()
                        .Where(x => x.Admin.UserId == userId || x.Members.Any(x => x.Parent.UserId == userId)),
                CooperativeUserType.Other => _dbContext.Cooperatives.AsNoTracking(),
                _ => null
            };
    }
}