using System;
using System.Collections.Generic;

namespace notification_system.Models;

public partial class RequestStatus
{
    public Guid Id { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
