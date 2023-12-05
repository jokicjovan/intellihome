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
    public class SmartDeviceHub : Hub<ISmartDeviceClient>
    {
        private readonly IUserService _userService;
        private readonly ISmartDeviceService _smartDeviceService;
        public SmartDeviceHub(IUserService userService, ISmartDeviceService smartDeviceService) {
            _userService = userService;
            _smartDeviceService = smartDeviceService;
        }

        public async Task SubscribeToDevice(Guid smartDeviceId)
        {
            var userFromContext = Context.User;
            if (userFromContext == null 
                || userFromContext.Identity == null 
                || !userFromContext.Identity.IsAuthenticated
                || userFromContext.FindFirst(ClaimTypes.NameIdentifier) == null)
            {
                await Clients.Caller.ReceiveSubscriptionResult("Authentication problem!");
                return;
            }

            Guid userId = Guid.Parse(userFromContext.FindFirst(ClaimTypes.NameIdentifier).Value);
            User user = await _userService.Get(userId);
            if (user == null)
            {
                await Clients.Caller.ReceiveSubscriptionResult("User not found!");
                return;
            }

            bool isAllowed = await _smartDeviceService.IsUserAllowed(smartDeviceId, user.Id);
            if (!isAllowed)
            {
                await Clients.Caller.ReceiveSubscriptionResult("User does not have permission for this device!");
                return;
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, smartDeviceId.ToString());
            await Clients.Caller.ReceiveSubscriptionResult("Subscription successful!");
        }

        //proveri da li treba i da li je vidljivo svim klijentima
        public async Task SendSmartDeviceDataToAllClients(String groupName, object data)
        {
            await Clients.Group(groupName).ReceiveSmartDeviceData(data);
        }
    }
}
