using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace BusinessLogicLayer.Interfaces
{
    public interface IEncryptionService
    {
        public JwtSecurityToken CheckJWTExternalToken(string jwtTokenString);
        public string GetJWTEmailFromToken(JwtSecurityToken jwtToken);
        public bool IsExpiredJWT(JwtSecurityToken jwtToken);

        public bool CheckBCryptPassword(string password, string hash);
        public string GenerateJWT(Guid userId);
        public SecurityToken? ValidateJWT(string jwt);
    }
}
