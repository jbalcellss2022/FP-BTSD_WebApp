﻿using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using DeviceDetectorNET;
using DeviceDetectorNET.Cache;
using DeviceDetectorNET.Parser;
using Entities.DTOs;
using MaxMind.Db;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using NLog;
using System.Diagnostics;

namespace BusinessLogicLayer.Services
{
    public class UserDDService(
        IWebHostEnvironment hostingEnvironment,
        IHttpContextAccessor httpContextAccessor,
        IUserRepository userRepository
            ) : IUserDDService
    {

        private readonly static Logger Logger = LogManager.GetCurrentClassLogger();
        private static DatabaseReader? IpReader = null;
        private string? webRootPath;
        private string? pathToDB;

        public void StartDeviceDetector()
        {
            webRootPath = hostingEnvironment.WebRootPath;
            webRootPath = webRootPath.Replace("BussinessLayer", "PresentationLayer");
            //webRootPath = "/var/www/FP-BTSD_WebApp/PresentationLayer/wwwroot/geoip2/";
            if (webRootPath.IsNullOrEmpty()) webRootPath = "/var/www/FP-BTSD_WebApp/PresentationLayer/wwwroot";
            pathToDB = webRootPath + Path.DirectorySeparatorChar.ToString() + "geoip2" + Path.DirectorySeparatorChar.ToString();
            // pathToDB = "/var/www/FP-BTSD_WebApp/PresentationLayer/wwwroot/geoip2/";
            try
            {
                IpReader = new DatabaseReader(pathToDB + "GeoLite2-City.Xmmdb");
            }
            catch (Exception ex)
            {
                if (!Debugger.IsAttached)
                {
                    Logger.Error(System.Reflection.MethodBase.GetCurrentMethod()!.Name + "[M]: " + ex.Message ?? "" + "[StackT]: " + ex.StackTrace ?? "" + "[HLink]: " + ex.HelpLink ?? "" + "[HResult]: " + ex.HResult ?? "" + "[Source]: " + ex.Source ?? "" + ex.Data ?? "" + "[InnerE]: " + ex.InnerException!.Message ?? "");
                }
            }
        }

        public async Task<bool> AddUserDeviceDetector(string? userId)
        {
            UserDDDTO userDDDTO = GetUserDeviceDetector(userRepository.GetUserIdByEmail(userId!));
            await userRepository.AddUserDD(userDDDTO);

            return true;
        }

        private UserDDDTO GetUserDeviceDetector(Guid? userId)
        {

            UserDDDTO userDDDTO = new()
            {
                UserId = userId,
                DDClient = "",
                DDModel = "",
                DDBrand = "",
                DDBrandName = "",
                DDOs = "",
                DDBrowser = "",
                DDtype = "",
                IPAddress = "",
                DDCity = "<Private IP Address>",
            };

            try
            {
                DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);
                DeviceDetectorSettings.RegexesDirectory = hostingEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString();

                var userAgent = httpContextAccessor.HttpContext!.Request.Headers.UserAgent;  // ["User-Agent"]
                var headers = httpContextAccessor.HttpContext.Request.Headers.ToDictionary(a => a.Key, a => a.Value.ToArray().FirstOrDefault());
                var clientHints = ClientHints.Factory(headers); 
                var dd = new DeviceDetector(userAgent, clientHints);

                dd.SetCache(new DictionaryCache());
                dd.DiscardBotInformation();
                dd.SkipBotDetection();
                dd.Parse();

                userDDDTO.DDClient = dd.GetClient();
                userDDDTO.DDModel = dd.GetModel();
                userDDDTO.DDBrand = dd.GetBrand();
                userDDDTO.DDBrandName = dd.GetBrandName();
                userDDDTO.DDOs = dd.GetOs();
                userDDDTO.DDBrowser = dd.GetBrowserClient();
                userDDDTO.DDtype = dd.GetDeviceName();
            }
            catch (Exception ex) {
                Logger.Warn(System.Reflection.MethodBase.GetCurrentMethod()!.Name + "[M]: " + ex.Message + "[StackT]: " + ex.StackTrace + "[HLink]: " + ex.HelpLink + "[HResult]: " + ex.HResult + "[Source]: " + ex.Source);
            }

            // Ip Location

            userDDDTO.IPAddress = httpContextAccessor.HttpContext!.Connection.RemoteIpAddress!.ToString(); // Determine the IP Address of the request 
            if (userDDDTO.IPAddress == "127.0.0.1" || userDDDTO.IPAddress == "0.0.0.0" || userDDDTO.IPAddress == "::1") {
                userDDDTO.DDCity = "Local"; 
            }
            else
            {
                try
                {
                    // Get the city from the IP Address
                    try
                    {
                        webRootPath = hostingEnvironment.WebRootPath;
                        webRootPath = webRootPath.Replace("BussinessLayer", "PresentationLayer");
                        if (webRootPath.IsNullOrEmpty()) webRootPath = "/var/www/FP-BTSD_WebApp/PresentationLayer/wwwroot";
                        pathToDB = webRootPath + Path.DirectorySeparatorChar.ToString() + "geoip2" + Path.DirectorySeparatorChar.ToString();
                        IpReader = new DatabaseReader(pathToDB + "GeoLite2-City.mmdb");
                        userDDDTO.DDCity = IpReader!.City(userDDDTO.IPAddress).City.Name + " - " + IpReader.City(userDDDTO.IPAddress).Country.Name; 
                    } 
                    catch (AddressNotFoundException ex) {
                        Logger.Warn(System.Reflection.MethodBase.GetCurrentMethod()!.Name + "[M]: " + ex.Message + "[StackT]: " + ex.StackTrace + "[HLink]: " + ex.HelpLink + "[HResult]: " + ex.HResult + "[Source]: " + ex.Source);
                        userDDDTO.DDCity = "<Private IP Address>";
                    }
                    catch (InvalidDatabaseException ex) {
                        Logger.Warn(System.Reflection.MethodBase.GetCurrentMethod()!.Name + "[M]: " + ex.Message + "[StackT]: " + ex.StackTrace + "[HLink]: " + ex.HelpLink + "[HResult]: " + ex.HResult + "[Source]: " + ex.Source);
                        userDDDTO.DDCity = "<Private IP Address>";
                    }
                }
                catch (Exception ex) {
                    Logger.Warn(System.Reflection.MethodBase.GetCurrentMethod()!.Name + "[M]: " + ex.Message + "[StackT]: " + ex.StackTrace + "[HLink]: " + ex.HelpLink + "[HResult]: " + ex.HResult + "[Source]: " + ex.Source);
                    userDDDTO.DDCity = "<Private IP Address>";
                }
            }

            return userDDDTO;
        }

    }
}
