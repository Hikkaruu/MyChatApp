using ChatAppBW.Authentication;
using ChatAppBW.Data;
using ChatModels.Dto;
using ChatModels.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBW.Repositories
{
    public class ChatRepository(AppDbContext appDbContext, UserManager<AppUser> userManager)
    {

        public async Task<List<AvailableUsersDto>> AddAvailableUser(AvailableUsers availableUsers) 
        {
            var list = new List<AvailableUsersDto>();

            var getUser = await appDbContext.AvailableUsers.FirstOrDefaultAsync(u => u.UserId == availableUsers.UserId);

            if (getUser != null)
            {
                getUser.ConnectionId = availableUsers.ConnectionId;
            }
            else
            {
                appDbContext.AvailableUsers.Add(availableUsers);
            }

            await appDbContext.SaveChangesAsync();

            var allUsers = await appDbContext.AvailableUsers.ToListAsync();
            foreach (var user in allUsers)
            {
                list.Add(new AvailableUsersDto()
                {
                    UserId = user.UserId,
                    Fullname = (await userManager.FindByIdAsync(user.UserId!))!.Fullname
                });
            }
            return list;
        }

        public async Task<List<AvailableUsersDto>> GetAvailableUsersAsync()
        {
            var list = new List<AvailableUsersDto>();

            var users = await appDbContext.AvailableUsers.ToListAsync();

            foreach(var user in users)
            {
                list.Add(new AvailableUsersDto()
                {
                    UserId = user.UserId,
                    Fullname = (await userManager.FindByIdAsync(user.UserId!))!.Fullname
                });
            }
            return list;
        }

        public async Task<List<AvailableUsersDto>> RemoveUserAsync(string userId)
        {
            var user = await appDbContext.AvailableUsers.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user != null)
            {
                appDbContext.AvailableUsers.Remove(user);
                await appDbContext.SaveChangesAsync();
            }

            var list = new List<AvailableUsersDto>();
            var users = await appDbContext.AvailableUsers.ToListAsync();

            foreach (var usr in users)
            {
                list.Add(new AvailableUsersDto()
                {
                    UserId = usr.UserId,
                    Fullname = (await userManager.FindByIdAsync(usr.UserId!))!.Fullname,
                });
            }
            return list;
        }
        
        public async Task AddIndividualChatAsync(IndividualChat individualChat)
        {
            appDbContext.IndividualChats.Add(individualChat);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<List<IndividualChatDto>> GetIndividualChatsAsync(RequestChatDto requestChatDto)
        {
            var chatList = new List<IndividualChatDto>();
            var chats = await appDbContext.IndividualChats
                .Where(c => c.SenderId == requestChatDto.SenderId && c.ReceiverId == requestChatDto.ReceiverId ||
                c.SenderId == requestChatDto.ReceiverId && c.ReceiverId == requestChatDto.SenderId).ToListAsync();

            if (chats != null)
            {
                foreach (var chat in chats) 
                {
                    chatList.Add(new IndividualChatDto()
                    {
                        SenderId = chat.SenderId,
                        ReceiverId = chat.ReceiverId,
                        SenderName = (await userManager.FindByIdAsync(chat.SenderId!))!.Fullname,
                        ReceiverName = (await userManager.FindByIdAsync(chat.ReceiverId!))!.Fullname,
                        Message = chat.Message,
                        Date = chat.Date
                    });
                }
                return chatList;
            }
            else
            {
                return null!;
            }
        }

    }
}
