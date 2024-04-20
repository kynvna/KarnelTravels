using System;
using System.Collections.Generic;

namespace KarnelTravels.Models;

public partial class HrCategory
{
    public int CatId { get; set; }

    public string? CatName { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<TblHotelRestaurant> TblHotelRestaurants { get; set; } = new List<TblHotelRestaurant>();
}
