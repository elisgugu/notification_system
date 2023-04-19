using System;
using System.Collections.Generic;

namespace notification_system.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid ProfileId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();

    public virtual UserProfile Profile { get; set; } = null!;

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
