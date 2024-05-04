namespace KarnelTravels.Models
{
    public class SearchViewModel
    {
        public IEnumerable<TblHotelRestaurant> HotelRestaurants { get; set; }
        public IEnumerable<TblTravel> Travels { get; set; }
        public IEnumerable<TblTouristPlace> Tours { get; set; }
        public IEnumerable<TblTourPackage> ToursPackage { get; set; }
    }
}
