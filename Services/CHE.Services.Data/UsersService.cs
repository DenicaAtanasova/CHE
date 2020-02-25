namespace CHE.Services.Data
{
    using System.Threading.Tasks;
    using AutoMapper;

    public class UsersService : IUsersService
    {
        private readonly IJoinRequestsService _joinRequestsService;

        public UsersService(IMapper mapper, IJoinRequestsService joinRequestsService)
        {
            this._joinRequestsService = joinRequestsService;
        }

        public Task<bool> AcceptRequest(string requestId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> RejectRequest(string requestId)
        {
            var result = await this._joinRequestsService.DeleteAsync(requestId);

            return result;
        }
    }
}