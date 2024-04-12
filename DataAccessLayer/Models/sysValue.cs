using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

/// <summary>
/// Settings and Configuration values table
/// </summary>
public partial class sysValue
{
    /// <summary>
    /// Setting Key
    /// </summary>
    public string setting { get; set; } = null!;

    /// <summary>
    /// Seeting Value
    /// </summary>
    public string? value { get; set; }

    /// <summary>
    /// Key-Value description
    /// </summary>
    public string? description { get; set; }
}
