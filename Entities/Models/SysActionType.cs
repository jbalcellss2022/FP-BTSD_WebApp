using System;
using System.Collections.Generic;

namespace Entities.Models;

/// <summary>
/// Product actions types availables
/// </summary>
public partial class SysActionType
{
    /// <summary>
    /// Product Code Type
    /// </summary>
    public string ActionId { get; set; } = null!;

    /// <summary>
    /// Action description
    /// </summary>
    public string? Description { get; set; }

#pragma warning disable IDE0028 // Simplify collection initialization
    public virtual ICollection<AppProduct> AppProducts { get; set; } = new List<AppProduct>();
#pragma warning restore IDE0028 // Simplify collection initialization
}
