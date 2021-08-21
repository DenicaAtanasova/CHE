namespace CHE.Services.Data
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class MessengersService : IMessengersService
    {
        private readonly CheDbContext _dbContext;

        public MessengersService(CheDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<TEntity> GetCooperativeMessengerAsync<TEntity>(string cooperativeId) =>
            await this._dbContext.Messengers
                .Where(x => x.CooperativeId == cooperativeId)
                .To<TEntity>()
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<TEntity>> GetAllPrivateContactsByUserAsync<TEntity>(string userId) =>
            await this._dbContext.MessengersUsers
                .Where(x => x.Messenger.CooperativeId == null)
                .Where(x => x.Messenger.Users.Any(x => x.UserId == userId))
                .To<TEntity>()
                .ToListAsync();

        public async Task<TEntity> GetPrivateMessengerAsync<TEntity>(string senderId, string receiverId)
        {
            var messenger = await this._dbContext.Messengers
                .Where(x => x.CooperativeId == null)
                .Where(x => x.Users.Any(u => u.UserId == senderId))
                .Where(x => x.Users.Any(u => u.UserId == receiverId))
                .To<TEntity>()
                .SingleOrDefaultAsync();

            if (messenger == null)
            {
                var newMessenger = await this.CreateAndGetPrivateMessengerAsync(senderId, receiverId);
                messenger = newMessenger.Map<Messenger, TEntity>();
            }

            return messenger;
        }

        public async Task<string> GetPrivateMessengerIdAsync(string senderId, string receiverId)
        {
            var messengerId = await this._dbContext.Messengers
                .Where(x => x.CooperativeId == null)
                .Where(x => x.Users.Any(u => u.UserId == senderId))
                .Where(x => x.Users.Any(u => u.UserId == receiverId))
                .Select(x => x.Id)
                .SingleOrDefaultAsync();

            if (messengerId == null)
            {
                var newMessenger = await this.CreateAndGetPrivateMessengerAsync(senderId, receiverId);
                return newMessenger.Id;
            }

            return messengerId;
        }
        
        public async Task AddMemberAsync(string messengerId, string userId)
        {
            this._dbContext.MessengersUsers.Add(
                new MessengerUser
                {
                    MessengerId = messengerId,
                    UserId = userId
                });

            await this._dbContext.SaveChangesAsync();
        }

        public async Task RemoveMemberAsync(string messengerId, string userId) 
        {
            var messingerUser = await this._dbContext.MessengersUsers
                .SingleOrDefaultAsync(x => x.MessengerId == messengerId && x.UserId == userId);

            if (messingerUser == null)
            {
                return;
            }

            this._dbContext.MessengersUsers.Remove(messingerUser);
            await this._dbContext.SaveChangesAsync();
        }

        private async Task<Messenger> CreateAndGetPrivateMessengerAsync(string senderId, string receiverId)
        {
            var messenger = new Messenger
            {
                Users = new List<MessengerUser>
                {
                    new MessengerUser { UserId = senderId },
                    new MessengerUser { UserId = receiverId }
                },
                CreatedOn = DateTime.UtcNow
            };

            await this._dbContext.Messengers.AddAsync(messenger);
            await this._dbContext.SaveChangesAsync();

            return messenger;
        }
    }
}