using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class appLogger
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
    /// message of the warning or error
    /// </summary>
    public string? message { get; set; }

    /// <summary>
    /// type of the message (Warning, Info, Debug, Error...)
    /// </summary>
    public string? messageType { get; set; }

    /// <summary>
    /// Additional info
    /// </summary>
    public string? messageDetails { get; set; }

    public virtual appUser? user { get; set; }
}
