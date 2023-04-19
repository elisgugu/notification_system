using System;
using System.Collections.Generic;

namespace notification_system.Models;

public partial class Notification
{
    public int Id { get; set; }

    public byte[] Name { get; set; } = null!;

    public Guid CriteriaId { get; set; }

    public Guid UserProfileId { get; set; }

    public Guid ChannelId { get; set; }

    public virtual Channel Channel { get; set; } = null!;

    public virtual Criterion Criteria { get; set; } = null!;

    public virtual UserProfile UserProfile { get; set; } = null!;
}
