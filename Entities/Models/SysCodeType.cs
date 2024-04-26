using System;
using System.Collections.Generic;

namespace Entities.Models;

/// <summary>
/// Product codes availables
/// </summary>
public partial class SysCodeType
{
    /// <summary>
    /// Product Code
    /// </summary>
    public string CodeId { get; set; } = null!;

    /// <summary>
    /// Code description
    /// </summary>
    public string? Description { get; set; }

#pragma warning disable IDE0028 // Simplify collection initialization
    public virtual ICollection<AppProduct> AppProducts { get; set; } = new List<AppProduct>();
#pragma warning restore IDE0028 // Simplify collection initialization
}
