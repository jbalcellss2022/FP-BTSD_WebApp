using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Classes;
using DataAccessLayer.Contracts;
using DataAccessLayer.Repositories;
using DeviceDetectorNET;
using DeviceDetectorNET.Cache;
using DeviceDetectorNET.Parser;
using MaxMind.Db;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace BusinessLogicLayer.Services
{
    public class UserDDService : IUserDDService
    {
        private static DatabaseReader? IpReader = null;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserRepository userRepository;

        public UserDDService(IHostingEnvironment HostingEnvironment, IHttpContextAccessor HttpContextAccessor, IUserRepository UserRepository)
        {
            hostingEnvironment = HostingEnvironment;
            httpContextAccessor = HttpContextAccessor;
            userRepository = UserRepository;
        }

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

        public void AddUserDeviceDetector(string? userId)
        {
            UserDDDTO userDDDTO = GetUserDeviceDetector(userRepository.GetUserIdByEmail(userId!));
            userRepository.AddUserDD(userDDDTO);
        }

        private UserDDDTO GetUserDeviceDetector(Guid? userId)
        {
            UserDDDTO userDDDTO = new()
            {
                userId = userId,
                ddClient = "",
                ddModel = "",
                ddBrand = "",
                ddBrandName = "",
                ddOs = "",
                ddBrowser = "",
                ddtype = "",
                ipAddress = "",
                ddCity = "<Private IP Address>",
            };

            try
            {
                DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);
                DeviceDetectorSettings.RegexesDirectory = hostingEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString();

                var userAgent = httpContextAccessor.HttpContext.Request.Headers["User-Agent"]; 
                var headers = httpContextAccessor.HttpContext.Request.Headers.ToDictionary(a => a.Key, a => a.Value.ToArray().FirstOrDefault());
                var clientHints = ClientHints.Factory(headers); 
                var dd = new DeviceDetector(userAgent, clientHints);

                dd.SetCache(new DictionaryCache());
                dd.DiscardBotInformation();
                dd.SkipBotDetection();
                dd.Parse();

                userDDDTO.ddClient = dd.GetClient();
                userDDDTO.ddModel = dd.GetModel();
                userDDDTO.ddBrand = dd.GetBrand();
                userDDDTO.ddBrandName = dd.GetBrandName();
                userDDDTO.ddOs = dd.GetOs();
                userDDDTO.ddBrowser = dd.GetBrowserClient();
                userDDDTO.ddtype = dd.GetDeviceName();
            }
            catch (Exception e) {
                Exception a = e;
                //Logger.Warn(System.Reflection.MethodBase.GetCurrentMethod().Name + "[M]: " + e.Message + "[StackT]: " + e.StackTrace + "[HLink]: " + e.HelpLink + "[HResult]: " + e.HResult + "[Source]: " + e.Source); 
            }

            // Ip Location

            userDDDTO.ipAddress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(); // Determine the IP Address of the request 
            if (userDDDTO.ipAddress == "127.0.0.1" || userDDDTO.ipAddress == "0.0.0.0" || userDDDTO.ipAddress == "::1") {
                userDDDTO.ddCity = "Local"; 
            }
            else
            {
                try
                {
                    // Get the city from the IP Address
                    try
                    {
                        userDDDTO.ddCity = IpReader!.City(userDDDTO.ipAddress).City.Name + " - " + IpReader.City(userDDDTO.ipAddress).Country.Name; 
                    } 
                    catch (AddressNotFoundException) { userDDDTO.ddCity = "<Private IP Address>"; }
                    catch (InvalidDatabaseException) { userDDDTO.ddCity = "<Private IP Address>"; }
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

