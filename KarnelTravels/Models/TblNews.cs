using System;
using System.Collections.Generic;

namespace KarnelTravels.Models;

public partial class TblNews
{
    public int NewsId { get; set; }

    public int? ObjectId { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public string? NewsObject { get; set; }

    public int? HotNews { get; set; }

    public DateTime? Date { get; set; }

    public string? NewsDetail { get; set; }
}
