using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text;

namespace BusinessLogicLayer.Helpers
{
    internal class EmailBodyHelper(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment) : IHelpersService
    {
        public StringBuilder EmailBodyPassword_New(string Username, string TokenURL) 
        {
            var pathToFile = hostingEnvironment.WebRootPath
                                        + Path.DirectorySeparatorChar.ToString()
                                        + "emailTemplates"
                                        + Path.DirectorySeparatorChar.ToString()
                                        + "password_reset.html";
            var bodyHtml = new StringBuilder();
            using (StreamReader sourceReader = File.OpenText(pathToFile))
            {
                bodyHtml.Append(sourceReader.ReadToEnd());
            }
            bodyHtml.Replace("{{username}}", Username);
            bodyHtml.Replace("{{userTokenURL}}", TokenURL);

            return bodyHtml;
        }

        public StringBuilder EmailBodyPassword_Change(string Username)
        {
            var pathToFile = hostingEnvironment.WebRootPath
                                        + Path.DirectorySeparatorChar.ToString()
                                        + "emailTemplates"
                                        + Path.DirectorySeparatorChar.ToString()
                                        + "password_change.html";
            var bodyHtml = new StringBuilder();
            using (StreamReader sourceReader = File.OpenText(pathToFile))
            {
                bodyHtml.Append(sourceReader.ReadToEnd());
            }
            bodyHtml.Replace("{{username}}", Username);

            return bodyHtml;
        }

        public StringBuilder EmailBodyAccount_Welcome(string Username, string Name)
        {
            var request = httpContextAccessor.HttpContext!.Request;
            var rawurl = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(request);
            var uri = new Uri(rawurl);
            var newTokenURL = uri.GetComponents(UriComponents.Scheme | UriComponents.Host | UriComponents.Port, UriFormat.UriEscaped) + "/SignIn/Login";
            var pathToFile = hostingEnvironment.WebRootPath
                                        + Path.DirectorySeparatorChar.ToString()
                                        + "emailTemplates"
                                        + Path.DirectorySeparatorChar.ToString()
                                        + "account_welcome.html";
            var bodyHtml = new StringBuilder();
            using (StreamReader sourceReader = File.OpenText(pathToFile))
            {
                bodyHtml.Append(sourceReader.ReadToEnd());
            }
            bodyHtml.Replace("{{username}}", Username);
            bodyHtml.Replace("{{buttonURL}}", newTokenURL);

            return bodyHtml;
        }

    }
}
