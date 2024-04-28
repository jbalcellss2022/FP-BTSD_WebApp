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

        public AppUser? GetUserByToken(string Token)
        {
            if (bbddcontext != null && bbddcontext.AppUsers != null)
            {
                AppUser? user = bbddcontext.AppUsers.FirstOrDefault(AppUser => AppUser.TokenID == Token);
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

        public async Task<bool> IncreaseUserRetries(string Username)
        {
            bool result = false;
            try
            {
                var user = bbddcontext.AppUsers.FirstOrDefault(AppUser => AppUser.Login == Username);
                if (user != null){
                    user.Retries++;

                    bbddcontext.Update(user);
                    await bbddcontext.SaveChangesAsync();
                }

                result = true;
            }
            catch { }

            return result;
        }

        public async Task<bool> ResetUserRetries(string Username)
        {
            bool result = false;
            try
            {
                var user = bbddcontext.AppUsers.FirstOrDefault(AppUser => AppUser.Login == Username);
                if (user != null)
                {
                    user.IsBlocked = false;
                    user.Retries = 0;

                    bbddcontext.Update(user);
                    await bbddcontext.SaveChangesAsync();
                }

                result = true;
            }
            catch { }

            return result;
        }

        public async Task<bool> BlockUser(string Username)
        {
            bool result = false;

            try
            {
                var user = bbddcontext.AppUsers.FirstOrDefault(AppUser => AppUser.Login == Username);
                if (user != null)
                {
                    user.IsBlocked = true;

                    bbddcontext.Update(user);
                    await bbddcontext.SaveChangesAsync();
                }

                result = true;
            }
            catch { }

            return result;
        }
        public async Task<bool> CreateAccount(string Username, string Name, string Password)
        {
            bool result = false;

            try
            {
                AppUser appUser = new()
                {
                    Login = Username,
                    Password = Password,
                    Name = Name,
                    IsBlocked = false,
                    Retries = 0,
                };

                bbddcontext.Add(appUser);
                await bbddcontext.SaveChangesAsync();
                result = true;
            }
            catch { }

            return result;
        }

        public async Task<bool> UpdateUserToken(string Username, string newToken)
        {
            if (bbddcontext != null && bbddcontext.AppUsers != null)
            {
                AppUser? user = bbddcontext.AppUsers.FirstOrDefault(AppUser => AppUser.Login == Username);
                if (user != null)
                {
                    user.TokenID = newToken;
                    user.TokenIssuedUTC = DateTime.UtcNow;
                    user.TokenExpiresUTC = DateTime.UtcNow.AddMinutes(15);
                    user.TokenIsValid = true;

                    bbddcontext.Update(user);
                    await bbddcontext.SaveChangesAsync();
                }

                return true;
            }

            return false;
        }

        public async Task<bool> UpdateUserPassword(Guid UserId, string Password)
        {
            if (bbddcontext != null && bbddcontext.AppUsers != null)
            {
                AppUser? user = bbddcontext.AppUsers.FirstOrDefault(AppUser => AppUser.UserId == UserId);
                if (user != null)
                {
                    user.IsBlocked = false;
                    user.Retries = 0;
                    user.Password = Password;
                    user.TokenIsValid = false;

                    bbddcontext.Update(user);
                    await bbddcontext.SaveChangesAsync();
                }

                return true;
            }

            return false;
        }


        public async Task<bool> AddUserDD(UserDDDTO userDDDTO)
        {
            bool result = false;

            try
            {
                AppUsersStat AppUsersStat = new()
                {
                    UserId = userDDDTO.UserId,
                    SRconnectionId = null,
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
                result = true;
            }
            catch { }   

            return result;
        }
    }
}
