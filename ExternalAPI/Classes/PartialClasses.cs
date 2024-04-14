using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ExternalAPI.Classes
{
    /// <summary>
    /// Represents the user authentication credentials model. This class is used to capture
    /// the username and password information from the client during authentication processes.
    /// </summary>
    /// <remarks>
    /// This model requires both username and password fields to be provided for a successful
    /// authentication attempt. The username is typically the user's email address, and the password
    /// should meet the application's security criteria for complexity. Ensure that password data is
    /// handled securely throughout the application to prevent unauthorized access.
    /// </remarks>
    public class UserAuth
    {
        /// <summary>The same user (E-mail) that you use to identify yourself in the QRFY portal.</summary>
        /// <example>john.doe@mycompany.org</example>
        [Required]
        public string? Username { get; set; }
        /// <summary>The same password that you use to identify yourself in the QRFY portal.</summary>
        /// <example>*********</example>
        [Required]
        public string? Password { get; set; }
    }

}