using DataAccessLayer.Classes;
using DataAccessLayer.Contracts;
using DataAccessLayer.Data;
using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories
{
    public class UserRepository(BBDDContext bbddcontext) : IUserRepository
	{
        public appUser? GetUserByEmail(string Username)
        {
            if (bbddcontext != null && bbddcontext.appUsers != null)
            {
                appUser? user = bbddcontext.appUsers.FirstOrDefault(appUser => appUser.login == Username);
                return user;
            }

            return null;
        }

        public Guid GetUserIdByEmail(string Username)
        {
            Guid userId = Guid.Empty;
            if (bbddcontext != null && bbddcontext.appUsers != null)
            {
                var user = bbddcontext.appUsers.FirstOrDefault(appUser => appUser.login == Username);
                if (user != null) userId = user.userId;
            }

            return userId;
        }

        public Task<int> AddUserDD(UserDDDTO userDDDTO)
        {
            appUsersStat appUsersStat = new()
            {
                userId = userDDDTO.userId,
                SRconnectionId  = null,
                SRconnected = true,
                IPv4 = userDDDTO.ipAddress,
                IPv6 = "",
                Location = userDDDTO.ddCity,
                DevId = "",
                DevName = "",
                DevOS = userDDDTO.ddOs!.Match.Name + " " + userDDDTO.ddOs.Match.Version + " (" + userDDDTO.ddOs.Match.ShortName + "," + userDDDTO.ddOs.Match.Platform + ")",
                DevBrowser = userDDDTO.ddBrowser!.Match.Name + " (" + userDDDTO.ddBrowser.Match.Version + "," + userDDDTO.ddBrowser.Match.ShortName + "," + userDDDTO.ddBrowser.Match.Type + ")",
                DevBrand = userDDDTO.ddBrand,
                DevBrandName = userDDDTO.ddBrand,
                DevType = userDDDTO.ddtype,
                IsoDateC = DateTime.Now,
                IsoDateM = DateTime.Now,
            };
            bbddcontext.Update(appUsersStat);
            return bbddcontext.SaveChangesAsync();
        }
    }
}
