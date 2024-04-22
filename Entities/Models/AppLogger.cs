using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class AppLogger
{
    /// <summary>
    /// Auto ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// UserId
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// message of the warning or error
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// type of the message (Warning, Info, Debug, Error...)
    /// </summary>
    public string? MessageType { get; set; }

    /// <summary>
    /// Additional info
    /// </summary>
    public string? MessageDetails { get; set; }

    public virtual AppUser? User { get; set; }
}
