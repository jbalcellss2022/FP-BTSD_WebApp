using Microsoft.IdentityModel.Tokens;

namespace BusinessLogicLayer.Interfaces
{
    public interface IEncryptionService
    {
        public bool CheckBCryptPassword(string password, string hash);
        public string GenerateJWT(Guid userId);
        public SecurityToken? ValidateJWT(string jwt);
    }
}
