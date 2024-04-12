using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class appUsersStat
{
    /// <summary>
    /// Auto ID
    /// </summary>
    public int id { get; set; }

    /// <summary>
    /// UserId
    /// </summary>
    public Guid? userId { get; set; }

    /// <summary>
    /// DateTime of the connection
    /// </summary>
    public DateTime? userDateTime { get; set; }

    /// <summary>
    /// Connection IPv6
    /// </summary>
    public string? userIPv6 { get; set; }

    /// <summary>
    /// Connection IPv4
    /// </summary>
    public string? userIPv4 { get; set; }

    /// <summary>
    /// Connection Device, City, OS...
    /// </summary>
    public string? userData { get; set; }

    public virtual appUser? user { get; set; }
}
