using BusinessLogicLayer.Interfaces;
using Entities.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static BCrypt.Net.BCrypt;

namespace BusinessLogicLayer.Services
{
    internal class EncryptionService(IConfiguration Configuration) : IEncryptionService
    {
        public JwtSecurityToken JWT_CheckExternalToken(string jwtTokenString)
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

        public string JWT_GetEmailFromToken(JwtSecurityToken jwtToken)
        {
            string email = "";
            try
            {
                email = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "email")?.Value!;
            }
            catch { }   

            return email;
        }

        public JWTDTO JWT_GetDataFromToken(JwtSecurityToken jwtToken)
        {
            JWTDTO? JWT = new();
            try
            {
                JWT.Name = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "name")?.Value!;
                JWT.Email = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "email")?.Value!;
            }
            catch { }

            return JWT!;
        }

        public bool JWT_IsExpired(JwtSecurityToken jwtToken)
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

        public string JWT_Generate(Guid userId)
        {
            return JWT_GetGenerate(userId);
        }
        public SecurityToken? JWT_Validate(string jwt)
        {
            return JWT_GetValidate(jwt);
        }

        public bool BCrypt_CheckPassword(string password, string hash)
        {
            bool result = false;
            try
            {
                result = Verify(password, hash);
            }
            catch (Exception) { };
            
            return result;
        }

        public string BCrypt_EncryptPassword(string password)
        {
            string newPassword = "";
            try
            {
                newPassword = HashPassword(password);
            }
            catch { }   

            return newPassword;
        }

        //############################################################################################################//
        //############################################## PRIVATE METHODS ##############################################//
        //############################################################################################################//

        #region JWTToken Metohds
        private string JWT_GetGenerate(Guid userId)
        {
            string newToken = "";
            try
            {
                string? secretKey = Configuration["Encryption:SecretKey"];
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

        private SecurityToken? JWT_GetValidate(string jwt)
        {
            SecurityToken? securityToken = null;
            try
            {
                string secretKey = Configuration["Encryption:SecretKey"]!;
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
