using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

/// <summary>
/// Applicaction Roles table
/// </summary>
public partial class appUsersRole
{
    /// <summary>
    /// Auto ID
    /// </summary>
    public int id { get; set; }

    /// <summary>
    /// UserId
    /// </summary>
    public Guid userId { get; set; }

    /// <summary>
    /// Role code
    /// </summary>
    public string role { get; set; } = null!;

    public virtual sysRole roleNavigation { get; set; } = null!;

    public virtual appUser user { get; set; } = null!;
}
