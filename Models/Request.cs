using System;
using System.Collections.Generic;

namespace notification_system.Models;

public partial class Request
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid TypeId { get; set; }

    public DateTime Date { get; set; }

    public Guid StatusId { get; set; }

    public virtual RequestStatus Status { get; set; } = null!;

    public virtual RequestType Type { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
