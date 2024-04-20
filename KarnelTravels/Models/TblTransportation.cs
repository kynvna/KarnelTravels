using System;
using System.Collections.Generic;

namespace KarnelTravels.Models;

public partial class TblTransportation
{
    public int CatId { get; set; }

    public string? CatName { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<TblTravel> TblTravels { get; set; } = new List<TblTravel>();
}
