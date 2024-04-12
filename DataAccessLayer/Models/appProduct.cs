namespace DataAccessLayer.Models;

/// <summary>
/// Users Products table
/// </summary>
public partial class AppProduct
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
    /// Product reference
    /// </summary>
    public string reference { get; set; } = null!;

    /// <summary>
    /// Product description
    /// </summary>
    public string? description { get; set; }

    /// <summary>
    /// Product CodeType
    /// </summary>
    public string codeId { get; set; } = null!;

    /// <summary>
    /// Product ActionType
    /// </summary>
    public string actionId { get; set; } = null!;

    public virtual sysActionType action { get; set; } = null!;

    public virtual sysCodeType code { get; set; } = null!;

    public virtual appUser? user { get; set; }
}
