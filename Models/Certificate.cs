using System;
using System.Collections.Generic;

namespace notification_system.Models;

public partial class Certificate
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public virtual User User { get; set; } = null!;
}
