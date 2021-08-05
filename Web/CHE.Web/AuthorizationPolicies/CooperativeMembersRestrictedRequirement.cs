namespace CHE.Web.AuthorizationPolicies
{
    using CHE.Services.Data;
    using CHE.Web.Infrastructure;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.DependencyInjection;

    using System.Threading.Tasks;

    public class CooperativeMembersRestrictedRequirement : 
        AuthorizationHandler<CooperativeMembersRestrictedRequirement, HubInvocationContext>, 
        IAuthorizationRequirement
    {
        private readonly ICooperativesService _cooperativesService;

        public CooperativeMembersRestrictedRequirement(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            this._cooperativesService = serviceProvider.GetRequiredService<ICooperativesService>();
        }


        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            CooperativeMembersRestrictedRequirement requirement, 
            HubInvocationContext resource)
        {
            var cooperativeId = (string)resource.HubMethodArguments[1];
            var userId = context.User.GetId();

            if (await this._cooperativesService.CheckIfMemberAsync(userId, cooperativeId) ||
                await this._cooperativesService.CheckIfAdminAsync(userId, cooperativeId))
            {
                context.Succeed(requirement);
            }

            await Task.CompletedTask;
        }
    }
}