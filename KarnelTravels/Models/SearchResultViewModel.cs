namespace KarnelTravels.Models
{
    public class SearchResultViewModel
    {
        public List<TblHotelRestaurant> Hotels { get; set; } = new List<TblHotelRestaurant>();
        public List<TblTouristPlace> TouristPlaces { get; set; } = new List<TblTouristPlace>();
        public List<TblSpot> Spots { get; set; } = new List<TblSpot>();
        public List<TblNews> News { get; set; } = new List<TblNews>();
        public List<TblTourPackage> Packages { get; set; } = new List<TblTourPackage>();
        public List<TblTravel> Transports { get; set; } = new List<TblTravel>();
    }
}
