using BusinessLogicLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static BCrypt.Net.BCrypt;

namespace BusinessLogicLayer.Services
{
    internal class EncryptionService(IConfiguration configuration): IEncryptionService
    {
        public bool CheckBCryptPassword(string password, string hash)
        {
            bool result = false;
            try
            {
                result = Verify(password, hash);
            }
            catch (Exception) { };
            
            return result;
        }

        public string GenerateJWT(Guid userId)
        {
            return GetGenerateJWT(userId);
        }
        public SecurityToken? ValidateJWT(string jwt)
        {
            return GetValidateJWT(jwt);
        }

        //############################################################################################################//
        //############################################## PRIVATE METHODS ##############################################//
        //############################################################################################################//

        #region JWTToken Metohds
        private string GetGenerateJWT(Guid userId)
        {
            string newToken = "";
            try
            {
                string? secretKey = configuration["Encryption:SecretKey"];
                if (secretKey == null)
                {
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: "QRFY",
                        audience: "userAuth",
                        claims: [
                            new Claim("userpasswordlink", userId.ToString())
                        ],
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: creds);
                    newToken = new JwtSecurityTokenHandler().WriteToken(token).ToString();
                }
            }
            catch (Exception) { };

            return newToken;
        }

        private SecurityToken? GetValidateJWT(string jwt)
        {
            SecurityToken? securityToken = null;
            try
            {
                string secretKey = configuration["Encryption:SecretKey"]!;
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                var claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(jwt, tokenValidationParameters, out securityToken);
            }
            catch (Exception) { };

            return securityToken!;
        }

        #endregion


    }
}
