using DataAccessLayer.Interfaces;
using Entities.Data;
using Entities.DTOs;
using Entities.Models;

namespace DataAccessLayer.Repositories
{
    public class UserRepository(BBDDContext bbddcontext) : IUserRepository
	{
        public AppUser? GetUserByEmail(string Username)
        {
            if (bbddcontext != null && bbddcontext.AppUsers != null)
            {
                AppUser? user = bbddcontext.AppUsers.FirstOrDefault(AppUser => AppUser.Login == Username);
                return user;
            }

            return null;
        }

        public Guid GetUserIdByEmail(string Username)
        {
            Guid userId = Guid.Empty;
            if (bbddcontext != null && bbddcontext.AppUsers != null)
            {
                var user = bbddcontext.AppUsers.FirstOrDefault(AppUser => AppUser.Login == Username);
                if (user != null) userId = user.UserId;
            }

            return userId;
        }

        public async Task<bool> AddUserDD(UserDDDTO userDDDTO)
        {
            AppUsersStat AppUsersStat = new()
            {
                UserId = userDDDTO.UserId,
                SRconnectionId  = null,
                SRconnected = true,
                IPv4 = userDDDTO.IPAddress,
                IPv6 = "",
                Location = userDDDTO.DDCity,
                DevId = "",
                DevName = "",
                DevOS = userDDDTO.DDOs!.Match.Name + " " + userDDDTO.DDOs.Match.Version + " (" + userDDDTO.DDOs.Match.ShortName + "," + userDDDTO.DDOs.Match.Platform + ")",
                DevBrowser = userDDDTO.DDBrowser!.Match.Name + " (" + userDDDTO.DDBrowser.Match.Version + "," + userDDDTO.DDBrowser.Match.ShortName + "," + userDDDTO.DDBrowser.Match.Type + ")",
                DevBrand = userDDDTO.DDBrand,
                DevBrandName = userDDDTO.DDBrand,
                DevType = userDDDTO.DDtype,
                IsoDateC = DateTime.Now,
                IsoDateM = DateTime.Now,
            };
            bbddcontext.Add(AppUsersStat);
            await bbddcontext.SaveChangesAsync();

            return true;
        }
    }
}
