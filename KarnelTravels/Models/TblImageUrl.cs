using System;
using System.Collections.Generic;

namespace KarnelTravels.Models;

public partial class TblImageUrl
{
    public string? Description { get; set; }

    public int Id { get; set; }

    public string? Url { get; set; }

    public virtual ICollection<TblHotelRestaurant> TblHotelRestaurants { get; set; } = new List<TblHotelRestaurant>();

    public virtual ICollection<TblNews> TblNews { get; set; } = new List<TblNews>();

    public virtual ICollection<TblTourPackage> TblTourPackages { get; set; } = new List<TblTourPackage>();

    public virtual ICollection<TblTouristPlace> TblTouristPlaces { get; set; } = new List<TblTouristPlace>();

    public virtual ICollection<TblTravel> TblTravels { get; set; } = new List<TblTravel>();
}
