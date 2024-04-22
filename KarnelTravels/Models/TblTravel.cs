using System;
using System.Collections.Generic;

namespace KarnelTravels.Models;

public partial class TblTravel
{
    public int TravelId { get; set; }

    public int? TransCategoryId { get; set; }

    public int? SpotDeparture { get; set; }

    public int? SpotDestination { get; set; }

    public decimal? Price { get; set; }

    public string? Description { get; set; }

    public int? ImageLinkId { get; set; }

    public string? Status { get; set; }

    public virtual TblImageUrl? ImageLink { get; set; }

    public virtual TblTransportation? TransCategory { get; set; }
}
