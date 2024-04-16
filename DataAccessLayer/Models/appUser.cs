using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

/// <summary>
/// Application User table
/// </summary>
public partial class appUser
{
    /// <summary>
    /// UUID unique User ID
    /// </summary>
    public Guid userId { get; set; }

    /// <summary>
    /// User login email
    /// </summary>
    public string? login { get; set; }

    public string password { get; set; } = null!;

    /// <summary>
    /// User name
    /// </summary>
    public string name { get; set; } = null!;

    /// <summary>
    /// User surname
    /// </summary>
    public string surname { get; set; } = null!;

    /// <summary>
    /// User phone
    /// </summary>
    public string? phone { get; set; }

    /// <summary>
    /// User address
    /// </summary>
    public string? address { get; set; }

    /// <summary>
    /// Internal comments
    /// </summary>
    public string? comments { get; set; }

    public bool? isAdmin { get; set; }

    public virtual ICollection<appLogger> appLoggers { get; set; } = new List<appLogger>();

    public virtual ICollection<appProduct> appProducts { get; set; } = new List<appProduct>();

    public virtual ICollection<appUsersRole> appUsersRoles { get; set; } = new List<appUsersRole>();

    public virtual ICollection<appUsersStat> appUsersStats { get; set; } = new List<appUsersStat>();
}
