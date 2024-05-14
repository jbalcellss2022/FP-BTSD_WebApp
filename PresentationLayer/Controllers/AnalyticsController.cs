﻿using BusinessLogicLayer.Interfaces;
using Entities.Data;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PresentationLayer.Controllers
{
    [Authorize]

    public class AnalyticsController(IClaimsService ClaimsService, IProfileService ProfileService, IUserService UserService) : Controller
    {
        public IActionResult Index()
        {
            DashboardUserProfileDTO UserProfile = ProfileService.GetUserProfile(ClaimsService.GetClaimValue("UserId"));
            ViewBag.UserStats = UserService.GetAllUserStats().Take(5);
            
            return View(UserProfile);
        }
    }
}
