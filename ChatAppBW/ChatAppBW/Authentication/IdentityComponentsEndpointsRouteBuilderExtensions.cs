using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ChatAppBW.Authentication
{
    internal static class IdentityComponentsEndpointsRouteBuilderExtensions
    {
        public static IEndpointConventionBuilder MapAdditionalIdentityEndpoints(this IEndpointRouteBuilder builder)
        {
            var accountGroup = builder.MapGroup("/account");
            accountGroup.MapPost("/logout", async (ClaimsPrincipal user, SignInManager<AppUser> signInManager) => 
            {
                await signInManager.SignOutAsync();
                return TypedResults.LocalRedirect("/");
            });
            return accountGroup;
        }
    }
}
