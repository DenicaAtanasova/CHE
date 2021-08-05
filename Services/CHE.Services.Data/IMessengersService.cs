namespace CHE.Services.Data
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using Microsoft.EntityFrameworkCore;

    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IMessengersService
    {
        Task<TEntity> GetCooperativeMessengerWithUsersAsync<TEntity>(string cooperativeId);

        Task<TEntity> GetCooperativeMessengerWithMessagesAsync<TEntity>(string cooperativeId);

        Task<IEnumerable<TEntity>> GetAllPrivateMessengersByUserAsync<TEntity>(string userId);

        Task<TEntity> GetPrivateMessengerWithMessagesAsync<TEntity>(string senderId, string receiverId);
    }

    public class MessengersService : IMessengersService
    {
        private readonly CheDbContext _dbContext;

        public MessengersService(CheDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<TEntity> GetCooperativeMessengerWithUsersAsync<TEntity>(string cooperativeId) =>
            await this._dbContext.Messengers
                .Where(x => x.CooperativeId == cooperativeId)
                .To<TEntity>()
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<TEntity>> GetAllPrivateMessengersByUserAsync<TEntity>(string userId) =>
            await this._dbContext.MessengersUsers
                .Where(x => x.Messenger.Users.Any(x => x.UserId == userId))
                .To<TEntity>()
                .ToListAsync();

        public async Task<TEntity> GetPrivateMessengerWithMessagesAsync<TEntity>(string senderId, string receiverId)
        {
            var messenger = await this._dbContext.Messengers
                .Where(x => x.CooperativeId == null)
                .Where(x => x.Users.Any(u => u.UserId == senderId))
                .Where(x => x.Users.Any(u => u.UserId == receiverId))
                .To<TEntity>()
                .SingleOrDefaultAsync();

            if (messenger == null)
            {
                var newMessenger = await this.CreatePrivateChatAsync(senderId, receiverId);
                messenger = newMessenger.Map<Messenger, TEntity>();
            }

            return messenger;
        }

        public async Task<TEntity> GetCooperativeMessengerWithMessagesAsync<TEntity>(string cooperativeId) =>
            await this._dbContext.Messengers
                .Where(x => x.CooperativeId == cooperativeId)
                .To<TEntity>()
                .SingleOrDefaultAsync();

        private async Task<Messenger> CreatePrivateChatAsync(string senderId, string receiverId)
        {
            var messenger = new Messenger
            {
                Users = new List<MessengerUser>
                {
                    new MessengerUser { UserId = senderId },
                    new MessengerUser { UserId = receiverId }
                }
            };

            await this._dbContext.Messengers.AddAsync(messenger);
            await this._dbContext.SaveChangesAsync();

            return messenger;
        }
    }
}