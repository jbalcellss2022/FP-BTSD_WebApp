using System;
using System.Collections.Generic;

namespace Entities.Models;

/// <summary>
/// Application User table
/// </summary>
public partial class AppUser
{
    /// <summary>
    /// UUID unique User ID
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// User login email
    /// </summary>
    public string? Login { get; set; }

    public string Password { get; set; } = null!;

    /// <summary>
    /// User name
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// User surname
    /// </summary>
    public string Surname { get; set; } = null!;

    /// <summary>
    /// User phone
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// User address
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Internal comments
    /// </summary>
    public string? Comments { get; set; }

    public bool? IsAdmin { get; set; }

    public bool Is2FAEnabled { get; set; }

    public virtual ICollection<AppLogger> AppLoggers { get; set; } = new List<AppLogger>();

    public virtual ICollection<AppProduct> AppProducts { get; set; } = new List<AppProduct>();

    public virtual ICollection<AppUsersRole> AppUsersRoles { get; set; } = new List<AppUsersRole>();

    public virtual ICollection<AppUsersStat> AppUsersStats { get; set; } = new List<AppUsersStat>();
}
