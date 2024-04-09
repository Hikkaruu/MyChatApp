
using Microsoft.AspNetCore.SignalR;
using ChatAppBW.Repositories;
using ChatModels.Models;
using ChatModels.Dto;

namespace ChatAppBW.Hubs
{
    public class ChatHub(ChatRepository chatRepository) : Hub
    {

        public async Task AddAvailableUserAsync(AvailableUsers availableUser)
        {
            availableUser.ConnectionId = Context.ConnectionId;
            var availableUsers = await chatRepository.AddAvailableUser(availableUser);
            await Clients.All.SendAsync("NotifyAllClients", availableUsers);
        }

        public async Task RemoveUserAsync(string userId)
        {
            var availableUsers = await chatRepository.RemoveUserAsync(userId);
            await Clients.All.SendAsync("NotifyAllClients", availableUsers);
        }

        public async Task AddIndividualChat(IndividualChat individualChat)
        {
            await chatRepository.AddIndividualChatAsync(individualChat);
            var requestDto = new RequestChatDto()
            {
                ReceiverId = individualChat.ReceiverId,
                SenderId = individualChat.SenderId
            };

            var getChats = await chatRepository.GetIndividualChatsAsync(requestDto);
            var prepareIndividualChat = new IndividualChatDto()
            {
                SenderId = individualChat.SenderId,
                ReceiverId = individualChat.ReceiverId,
                Message = individualChat.Message,
                Date = DateTime.Now,
                ReceiverName = getChats.Where(c => c.ReceiverId == individualChat.ReceiverId).FirstOrDefault()!.ReceiverName,
                SenderName = getChats.Where(c => c.SenderId == individualChat.SenderId).FirstOrDefault()!.SenderName
            };
            await Clients.User(individualChat.ReceiverId!).SendAsync("ReceiveIndividualMessage", prepareIndividualChat);
        }
    }
}
 