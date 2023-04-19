using System;
using System.Collections.Generic;

namespace notification_system.Models;

public partial class RequestType
{
    public Guid Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
