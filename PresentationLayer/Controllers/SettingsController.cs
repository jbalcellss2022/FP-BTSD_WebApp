using BusinessLogicLayer.Interfaces;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Authorize]

    public class SettingsController(IClaimsService ClaimsService, IProfileService ProfileService) : Controller
    {
        public IActionResult IdxSettings()
        {
            DashboardUserProfileDTO UserProfile = ProfileService.GetUserProfile(ClaimsService.GetClaimValue("UserId"));

            return View(UserProfile);
        }

    }
}
