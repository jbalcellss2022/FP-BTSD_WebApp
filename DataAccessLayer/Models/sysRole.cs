using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

/// <summary>
/// Roles available table
/// </summary>
public partial class sysRole
{
    /// <summary>
    /// Code of the authorization role
    /// </summary>
    public string role { get; set; } = null!;

    /// <summary>
    /// Role description
    /// </summary>
    public string? description { get; set; }

    public virtual ICollection<appUsersRole> appUsersRoles { get; set; } = new List<appUsersRole>();
}
