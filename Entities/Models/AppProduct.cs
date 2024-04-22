using System;
using System.Collections.Generic;

namespace Entities.Models;

/// <summary>
/// Users Products table
/// </summary>
public partial class AppProduct
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
    /// Product reference
    /// </summary>
    public string Reference { get; set; } = null!;

    /// <summary>
    /// Product description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Product CodeType
    /// </summary>
    public string CodeId { get; set; } = null!;

    /// <summary>
    /// Product ActionType
    /// </summary>
    public string ActionId { get; set; } = null!;

    public virtual SysActionType Action { get; set; } = null!;

    public virtual SysCodeType Code { get; set; } = null!;

    public virtual AppUser? User { get; set; }
}
