using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Writed.Policies.Requirements;

namespace Writed.Policies.Handlers
{
    public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            int age = Convert.ToInt32(context.User.Claims.First(c => c.Type == "Age").Value);

            if (context.User.HasClaim("Age", age.ToString()))
            {
                if (age >= requirement.MinimumAge)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
