using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Authorize]

    public class BarcodesController(IClaimsService ClaimsService, IProfileService ProfileService, IBarcodeService BarcodeService) : Controller
    {
        public IActionResult StaticBarcodes()
        {
            DashboardUserProfileDTO UserProfile = ProfileService.GetUserProfile(ClaimsService.GetClaimValue("UserId"));
            ViewBag.CodebarList = BarcodeService.GetAllCBStatic().Take(10);

            return View(UserProfile);
        }

        public IActionResult DynamicBarcodes()
        {
            DashboardUserProfileDTO UserProfile = ProfileService.GetUserProfile(ClaimsService.GetClaimValue("UserId"));
            ViewBag.CodebarList = BarcodeService.GetAllCBDynamic().Take(10);

            return View(UserProfile);
        }

        public IActionResult Management()
        {
            DashboardUserProfileDTO UserProfile = ProfileService.GetUserProfile(ClaimsService.GetClaimValue("UserId"));

            return View(UserProfile);
        }

    }
}
