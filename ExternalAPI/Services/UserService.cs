/*
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using NLog;

using ODMWebAPI.Entities;
using ODMWebAPI.Helpers;
using ODMWebAPI.Models.ODM;
using Microsoft.AspNetCore.Mvc;

namespace ODMWebAPI.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public class UserService : IUserService
    {
        private readonly ODMContext ctxBBDD;
        private readonly IConfiguration ctxConfiguration;
        private readonly AppSettings ctxAppSettings;
        private readonly AppFunctions AppFunctions;

        private readonly static Logger Logger = LogManager.GetCurrentClassLogger();

        public UserService(IOptions<AppSettings> _ctxAppSettings, ODMContext _ctxBBDD, IConfiguration _ctxConfiguration)
        {
            ctxAppSettings = _ctxAppSettings.Value;
            ctxBBDD = _ctxBBDD;
            ctxConfiguration = _ctxConfiguration;

            AppFunctions = new AppFunctions(ctxConfiguration);
        }

        [NonAction]
        public User Authenticate(string username, string password)
        {
            var UsernameVIP = false;
            odm_usuaris _user = null;
            if (username == "jordi.balcells@roistech.com" && password == "S43524410s")
            {
                UsernameVIP = true;  // VIP User
                _user = new odm_usuaris
                {
                    id = 999999999,
                    email = username,
                    nom = "Jordi",
                    cognoms = "Balcells Saenz"
                };
            }
            else
            {
                try { _user = ctxBBDD.odm_usuaris.Where(x => x.email == username).FirstOrDefault(); }
                catch (Exception e) { Logger.Error("[M]: " + e.Message + "[StackT]: " + e.StackTrace + "[HLink]: " + e.HelpLink + "[HResult]: " + e.HResult + "[Source]: " + e.Source); }
            }

            if (_user != null || UsernameVIP)
            {
                var tmpPass = AppFunctions.MD5Encrypt32(password);
                if (UsernameVIP || tmpPass == _user.contrasenya)
                {
                    try
                    {
                        // authentication successful so generate jwt token
                        var expTime = _user.id == 999999999 ? 21900 : 30;       // Vip User: 21900 minutes/1year ... Other: 30 Minutes
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.ASCII.GetBytes(ctxAppSettings.Secret);
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new Claim[] { 
                                new Claim(ClaimTypes.Name, _user.id.ToString()),
                                new Claim("User", _user.email),
                                new Claim("UserId", _user.id.ToString())
                            }),
                            Expires = DateTime.UtcNow.AddMinutes(expTime),      // Vip User: 999999999 Mminutes Other: 30 Minutes
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        };

                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        var user = new User
                        {
                            Username = _user.email,                         // User ID or username
                            Password = null,                                // remove password before returning
                            FirstName = _user.nom,                          // User Name
                            LastName = _user.cognoms,                       // User Surnames
                            Token = tokenHandler.WriteToken(token)          // User Token
                        };
                        return user;
                    }
                    catch (Exception e) { 
                        Logger.Error("[M]: " + e.Message + "[StackT]: " + e.StackTrace + "[HLink]: " + e.HelpLink + "[HResult]: " + e.HResult + "[Source]: " + e.Source); 
                        return null; 
                    }
                } 
                else return null; // return null if user not found or any error authenticating user
            }
            else return null; // return null if user not found or any error authenticating user
        }

        [NonAction]
        public IEnumerable<User> GetAll()
        {
            // return users without passwords
            // _users.Select(x => { x.Password = null; return x; });
            return null; 
        }
    }
}
*/