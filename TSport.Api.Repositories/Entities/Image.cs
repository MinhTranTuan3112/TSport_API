using System;
using System.Collections.Generic;

namespace TSport.Api.Repositories.Entities;

public partial class Image
{
    public int Id { get; set; }

    public string? Url { get; set; }

    public int ShirtId { get; set; }

    public virtual Shirt Shirt { get; set; } = null!;
}
