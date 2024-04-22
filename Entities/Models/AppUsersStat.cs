using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class AppUsersStat
{
    /// <summary>
    /// Auto ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// UserId
    /// </summary>
    public Guid? UserId { get; set; }

    public string? SRconnectionId { get; set; }

    public bool? SRconnected { get; set; }

    /// <summary>
    /// Connection IPv6
    /// </summary>
    public string? IPv6 { get; set; }

    /// <summary>
    /// Connection IPv4
    /// </summary>
    public string? IPv4 { get; set; }

    /// <summary>
    /// Connection Device, City, OS...
    /// </summary>
    public string? Location { get; set; }

    public string? DevId { get; set; }

    public string? DevName { get; set; }

    public string? DevOS { get; set; }

    public string? DevBrowser { get; set; }

    public string? DevBrand { get; set; }

    public string? DevBrandName { get; set; }

    public string? DevType { get; set; }

    public DateTime? IsoDateC { get; set; }

    public DateTime? IsoDateM { get; set; }

    public virtual AppUser? User { get; set; }
}
