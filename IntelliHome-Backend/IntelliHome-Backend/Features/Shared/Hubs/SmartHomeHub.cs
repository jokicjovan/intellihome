using Data.Models.Users;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.Hubs.Interfaces;
using IntelliHome_Backend.Features.Users.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace IntelliHome_Backend.Features.Shared.Hubs
{
    [Authorize]
    public class SmartHomeHub : Hub<ISmartHomeClient>
    {
        private readonly IUserService _userService;
        private readonly ISmartHomeService _smartHomeService;

        public SmartHomeHub(IUserService userService, ISmartHomeService smartHomeService)
        {
            _userService = userService;
            _smartHomeService = smartHomeService;
        }

        public async Task SubscribeToSmartHome(Guid smartHomeId)
        {
            var userFromContext = Context.User;
            if (userFromContext == null
                || userFromContext.Identity == null
                || !userFromContext.Identity.IsAuthenticated
                || userFromContext.FindFirst(ClaimTypes.NameIdentifier) == null)
            {
                await Clients.Caller.ReceiveSmartHomeSubscriptionResult("Authentication problem!");
                return;
            }

            Guid userId = Guid.Parse(userFromContext.FindFirst(ClaimTypes.NameIdentifier).Value);
            User user = await _userService.Get(userId);
            if (user == null)
            {
                await Clients.Caller.ReceiveSmartHomeSubscriptionResult("User not found!");
                return;
            }

            bool isAllowed = await _smartHomeService.IsUserAllowed(smartHomeId, user.Id);
            if (!isAllowed)
            {
                await Clients.Caller.ReceiveSmartHomeSubscriptionResult("User does not have permission for this device!");
                return;
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, smartHomeId.ToString());
            await Clients.Caller.ReceiveSmartHomeSubscriptionResult("Subscription successful!");
        }
    }
}
