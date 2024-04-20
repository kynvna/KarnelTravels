using System;
using System.Collections.Generic;

namespace KarnelTravels.Models;

public partial class TblNews
{
    public int? ImageLinkIid { get; set; }

    public int NewsId { get; set; }

    public string? ObjectId { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public string? NewsObject { get; set; }

    public virtual TblImageUrl? ImageLinkI { get; set; }
}
