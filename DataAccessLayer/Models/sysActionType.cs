// Ignore Spelling: app

namespace DataAccessLayer.Models;

/// <summary>
/// Product actions types availables
/// </summary>
public partial class sysActionType
{
    /// <summary>
    /// Product Code Type
    /// </summary>
    public string actionId { get; set; } = null!;

    /// <summary>
    /// Action description
    /// </summary>
    public string? description { get; set; }

    public virtual ICollection<appProduct> appProducts { get; set; } = new List<appProduct>();
}
