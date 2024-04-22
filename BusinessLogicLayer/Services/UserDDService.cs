using BusinessLogicLayer.Interfaces;
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
using System.Diagnostics;

namespace BusinessLogicLayer.Services
{
    public class UserDDService(IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository) : IUserDDService
    {
        private static DatabaseReader? IpReader = null;

        public void StartDeviceDetector()
        {
            string webRootPath = hostingEnvironment.WebRootPath;
            webRootPath = webRootPath.Replace("BussinessLayer", "PresentationLayer");
            if (webRootPath.IsNullOrEmpty()) webRootPath = "/var/www/FP-BTSD_WebApp/PresentationLayer/wwwroot";
            var pathToDB = webRootPath + Path.DirectorySeparatorChar.ToString() + "geoip2" + Path.DirectorySeparatorChar.ToString();
            try
            {
                IpReader = new DatabaseReader(pathToDB + "GeoLite2-City.mmdb");
            }
            catch (Exception)
            {
                if (!Debugger.IsAttached)
                {
                    //Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + "[M]: " + e.Message ?? "" + "[StackT]: " + e.StackTrace ?? "" + "[HLink]: " + e.HelpLink ?? "" + "[HResult]: " + e.HResult ?? "" + "[Source]: " + e.Source ?? "" + e.Data ?? "" + "[InnerE]: " + ex.InnerException.Message ?? "");
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
            catch (Exception e) {
                Exception a = e;
                //Logger.Warn(System.Reflection.MethodBase.GetCurrentMethod().Name + "[M]: " + e.Message + "[StackT]: " + e.StackTrace + "[HLink]: " + e.HelpLink + "[HResult]: " + e.HResult + "[Source]: " + e.Source); 
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
                        userDDDTO.DDCity = IpReader!.City(userDDDTO.IPAddress).City.Name + " - " + IpReader.City(userDDDTO.IPAddress).Country.Name; 
                    } 
                    catch (AddressNotFoundException) { userDDDTO.DDCity = "<Private IP Address>"; }
                    catch (InvalidDatabaseException) { userDDDTO.DDCity = "<Private IP Address>"; }
                }
                catch (Exception e) {
                    Exception a = e;
                    //Logger.Warn(System.Reflection.MethodBase.GetCurrentMethod().Name + "[M]: " + e.Message + "[StackT]: " + e.StackTrace + "[HLink]: " + e.HelpLink + "[HResult]: " + e.HResult + "[Source]: " + e.Source); 
                }
            }

            return userDDDTO;
        }
    }
}

