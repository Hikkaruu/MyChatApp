using ChatModels.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace ChatAppBW.Authentication
{
    public class PersistingServerAuthenticationStateProvider : ServerAuthenticationStateProvider, IDisposable
    {
        private readonly PersistentComponentState pCState;
        private readonly IdentityOptions idOptions;
        private readonly PersistingComponentStateSubscription pCSSubscription;
        private Task<AuthenticationState>? authenticationState;

        public PersistingServerAuthenticationStateProvider(
            PersistentComponentState persistentComponentState, IOptions<IdentityOptions> identityOptions)
        {
            pCState = persistentComponentState;
            idOptions = identityOptions.Value;

            AuthenticationStateChanged += OnAuthenticationStateChanged;
            pCSSubscription = pCState.RegisterOnPersisting(OnPersistingAsync, RenderMode.InteractiveWebAssembly);
        }
      
        public void OnAuthenticationStateChanged(Task<AuthenticationState> task) => 
            authenticationState = task;

        private async Task OnPersistingAsync()
        {
            var authState = await authenticationState;
            var principal = authState.User;
            if (principal.Identity?.IsAuthenticated == true)
            {
                var userId = principal.FindFirst(idOptions.ClaimsIdentity.UserIdClaimType)?.Value;
                var fullname = principal.Claims.Where(f => f.Type == ClaimTypes.Name).Last().Value;
                var email = principal.FindFirst(idOptions.ClaimsIdentity.EmailClaimType)?.Value;

                if (userId != null && fullname != null && email != null)
                {
                    pCState.PersistAsJson(nameof(UserInfo), new UserInfo
                    {
                        Id = userId,
                        Fullname = fullname,
                        Email = email
                    });
                }
            }
            
        }

        public void Dispose()
        {
            pCSSubscription.Dispose();
            AuthenticationStateChanged -= OnAuthenticationStateChanged;
        }
    }
}
