using System.Security.Claims;
using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusinessLogicLayer.Services
{
    public class ClaimsService(IHttpContextAccessor ContextAccessor) : IClaimsService
    {
        public string GetClaimValue(string ClaimType)
        {
            var ClaimsIdentity = ContextAccessor.HttpContext!.User.Identity as ClaimsIdentity;
            return ClaimsIdentity!.FindFirst(ClaimType)!.Value;
        }
    }
}
