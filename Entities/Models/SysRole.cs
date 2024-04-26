using System;
using System.Collections.Generic;

namespace Entities.Models;

/// <summary>
/// Roles available table
/// </summary>
public partial class SysRole
{
    /// <summary>
    /// Code of the authorization role
    /// </summary>
    public string Role { get; set; } = null!;

    /// <summary>
    /// Role description
    /// </summary>
    public string? Description { get; set; }

#pragma warning disable IDE0028 // Simplify collection initialization
    public virtual ICollection<AppUsersRole> AppUsersRoles { get; set; } = new List<AppUsersRole>();
#pragma warning restore IDE0028 // Simplify collection initialization
}
