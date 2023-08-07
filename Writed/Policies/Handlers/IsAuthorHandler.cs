using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Writed.Models;
using Writed.Models.Interfaces;
using Writed.Policies.Requirements;

namespace Writed.Policies.Handlers
{
    public class IsAuthorHandler : AuthorizationHandler<IsAuthorRequirement, IHasAuthor>
    {
        private readonly UserManager<User> userManager;

        public IsAuthorHandler(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAuthorRequirement requirement, IHasAuthor resource)
        {
            var user = await userManager.GetUserAsync(context.User);

            if(user == null)
            {
                return;
            }

            if(resource.Author != null && resource.Author.Id == user.Id)
            {
                context.Succeed(requirement);
            }

            //Access denied
        }
    }
}
