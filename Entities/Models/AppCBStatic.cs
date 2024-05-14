using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class AppCBStatic
{
    /// <summary>
    /// UserId
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Auto ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Product description
    /// </summary>
    public string? Description { get; set; }

    public string? CBType { get; set; }

    public string? CBValue { get; set; }

    public DateTime? IsoDateC { get; set; }

    public DateTime? IsoDateM { get; set; }
}
