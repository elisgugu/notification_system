using System;
using System.Collections.Generic;

namespace common_data.Models;

public partial class Certificate
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public virtual User User { get; set; } = null!;
}
