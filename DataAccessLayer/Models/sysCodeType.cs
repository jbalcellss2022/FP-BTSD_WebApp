namespace DataAccessLayer.Models;

/// <summary>
/// Product codes availables
/// </summary>
public partial class sysCodeType
{
    /// <summary>
    /// Product Code
    /// </summary>
    public string codeId { get; set; } = null!;

    /// <summary>
    /// Code description
    /// </summary>
    public string? description { get; set; }

    public virtual ICollection<appProduct> appProducts { get; set; } = new List<appProduct>();
}
