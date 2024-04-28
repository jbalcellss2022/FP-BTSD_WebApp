using Entities.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace BusinessLogicLayer.Interfaces
{
    public interface IEncryptionService
    {
        public JwtSecurityToken JWT_CheckExternalToken(string jwtTokenString);
        public string JWT_GetEmailFromToken(JwtSecurityToken jwtToken);
        public JWTDTO JWT_GetDataFromToken(JwtSecurityToken jwtToken);
        public bool JWT_IsExpired(JwtSecurityToken jwtToken);
        public string JWT_Generate(Guid userId);
        public SecurityToken? JWT_Validate(string jwt);

        public bool BCrypt_CheckPassword(string password, string hash);
        public string BCrypt_EncryptPassword(string password);
    }
}
