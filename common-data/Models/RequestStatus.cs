using System;
using System.Collections.Generic;

namespace common_data.Models;

public partial class RequestStatus
{
    public Guid Id { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
