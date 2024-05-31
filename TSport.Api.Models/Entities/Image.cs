using System;
using System.Collections.Generic;

namespace TSport.Api.Models.Entities;

public partial class Image
{
    public int Id { get; set; }

    public string? Url { get; set; }

    public int ImageId { get; set; }

    public virtual Shirt ImageNavigation { get; set; } = null!;
}
