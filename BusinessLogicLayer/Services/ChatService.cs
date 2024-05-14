using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using Entities.DTOs;

namespace BusinessLogicLayer.Services
{
    public class ChatService (IChatRepository ChatRepository) : IChatService
    {
        public async Task<bool> AddUserChatMessage(string user, string userChat, bool source, string message, DateTime datetime)
        {
            bool result = await ChatRepository.AddChatMessage(user, userChat, source, message, datetime); 
            return result;
        }

        public List<UserChatMessageDTO> GetAllUserChatMessages(string user)
        {
            var messages = ChatRepository.GetUserChatMessagesAsync(user);
            return messages;
        }
    }
}
