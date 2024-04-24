using System;
using System.Collections.Generic;

namespace KarnelTravels;

public partial class TblSpot
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<TblTouristPlace> TblTouristPlaces { get; set; } = new List<TblTouristPlace>();
}
