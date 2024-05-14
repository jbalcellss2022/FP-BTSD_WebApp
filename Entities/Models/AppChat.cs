using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class AppChat
{
    public Guid UserId { get; set; }

    public int IdxSec { get; set; }

    public string UserName { get; set; } = null!;

    public bool Source { get; set; }

    public string Message { get; set; } = null!;

    public DateTime Datetime { get; set; }
}
