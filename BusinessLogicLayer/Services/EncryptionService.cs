using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using static BCrypt.Net.BCrypt;

namespace BusinessLogicLayer.Services
{
    internal class EncryptionService(IConfiguration configuration) : IEncryptionService
    {
        public JwtSecurityToken CheckJWTExternalToken(string jwtTokenString)
        {
            JwtSecurityToken? jwtToken = null;
            try
            {
                var token = jwtTokenString;
                var handler = new JwtSecurityTokenHandler();
                jwtToken = handler.ReadJwtToken(token);
            }
            catch (Exception) { };

            return jwtToken!;
        }

        public string GetJWTEmailFromToken(JwtSecurityToken jwtToken)
        {
            string email = "";
            try
            {
                email = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "email")?.Value!;
            }
            catch { }   

            return email;
        }

        public bool IsExpiredJWT(JwtSecurityToken jwtToken)
        {
            bool IsExpiredJWT = true;

            try
            {
                var expClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "exp")?.Value;
                var expiryDateUnix = long.Parse(expClaim!);
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var expiryDate = epoch.AddSeconds(expiryDateUnix);
                IsExpiredJWT = expiryDate <= DateTime.UtcNow;
            }
            catch { }

            return IsExpiredJWT;
        }

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
