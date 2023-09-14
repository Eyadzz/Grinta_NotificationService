using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Application.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userIdClaim = ((ClaimsIdentity) Context.User!.Identity!).Claims.SingleOrDefault(c => c.Type == "UserId");
        if (userIdClaim is null || string.IsNullOrEmpty(userIdClaim.Value))
            return;

        await Groups.AddToGroupAsync(Context.ConnectionId, userIdClaim.Value);

        await base.OnConnectedAsync();
    }
}