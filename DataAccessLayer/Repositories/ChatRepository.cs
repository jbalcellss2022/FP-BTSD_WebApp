using DataAccessLayer.Interfaces;
using Entities.Data;
using Entities.DTOs;
using Entities.Models;

namespace DataAccessLayer.Repositories
{
    public class ChatRepository(BBDDContext bbddcontext) : IChatRepository
    {
        public async Task<bool> AddChatMessage(string userId, string userChat, bool source, string message, DateTime datetime)
        {
            bool result = false;
            Guid userGuid;
            var user = bbddcontext.AppUsers.FirstOrDefault(AppUser => AppUser.Login == userId);
            if (user != null)
            {
                userGuid = user.UserId;
                try
                {
                    AppChat appChat = new()
                    {
                        UserId = userGuid,
                        UserName = userChat,
                        Source = source,
                        Message = message,
                        Datetime = datetime
                    };

                    bbddcontext.Add(appChat);
                    await bbddcontext.SaveChangesAsync();
                    result = true;
                }
                catch { }
            }

            return result;
        }

        public List<UserChatMessageDTO> GetUserChatMessagesAsync(string userId)
        {
            List<UserChatMessageDTO> messages = [];
            var user = bbddcontext.AppUsers.FirstOrDefault(AppUser => AppUser.Login == userId);
            if (user != null)
            {
                var chatMessages = bbddcontext.AppChats.Where(AppChats => AppChats.UserId == user.UserId).ToList();
                foreach (var chatMessage in chatMessages)
                {
                    messages.Add(new UserChatMessageDTO
                    {
                        IdxSec = chatMessage.IdxSec,
                        UserName = chatMessage.UserName,
                        Source = chatMessage.Source,
                        Message = chatMessage.Message,
                        Datetime = chatMessage.Datetime
                    });
                }
            }

            return messages;
        }
    }
}
