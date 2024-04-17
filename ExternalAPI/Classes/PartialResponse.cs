using System.ComponentModel;
using DataAccessLayer.Models;

namespace ExternalAPI.Classes
{
    public class PartialResponse
    {
        public class UserAuthResponse
        {
            /// <summary>Returns the usercode (E-mail) who made the request.</summary>
            /// <example>john.doe@mycompany.org</example>
            [Description("Returns the usercode (E-mail) who made the request.")]
            public string? Username { get; set; }
            /// <summary>Returns the username who made the request.</summary>
            /// <example>John Doe</example>
            [Description("Returns the username who made the request.")]
            public string? Name { get; set; }
            /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSIdIjk5OTk6OTk5OSIsIlVzZXIiOiJqb3JkaS5iYWxjZWxsc0Byb2lzdGVjaC5jb20iLCJVc2VySWQiOiI5OTk5OTk5OTkiLCJuYmYiOjE1OTAyNzEzNDgsImV4cCI6MTU5MTU4NTM0OCwiaWF0IjoxNTkwMjcxMzQ4fQ.6JkWJ1obup6eHtfmTuAVIkq55hsZqTMRMKW1itpt7t4</example>
            public string? Token { get; set; }
        }

        public class UserProducts
        {
            public List<appProduct>? Products { get; set; }
        }
    }
}
