namespace Entities.Models;

public partial class AppChats
{
    public Guid UserId { get; set; }

    public int IdxSec { get; set; }
    public string? UserName { get; set; }

    public bool Source { get; set; }

    public string Message { get; set; } = null!;

    public DateTime Datetime { get; set; }
}
