namespace KarnelTravels.Models
{
    public class SearchViewModel
    {
        public IEnumerable<ViewHotelImg> HotelRestaurants { get; set; }
        public IEnumerable<ViewTravelImg> Travels { get; set; }
        public IEnumerable<TblTouristPlace> Tours { get; set; }
        public IEnumerable<ViewPackageImg> ToursPackage { get; set; }
    }
}
