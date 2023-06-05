using System;
using System.Collections.Generic;

namespace common_data.Models;

public partial class Channel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
