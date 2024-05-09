using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarnelTravels.Models;

public partial class TblImageUrl
{
    public string? Description { get; set; }

    public string? Url { get; set; }

    public int Id { get; set; }

    public string? UrlObject { get; set; }

    public int? ObjectId { get; set; }

    [NotMapped]
    public string? ObjectName { get; set; }
}
