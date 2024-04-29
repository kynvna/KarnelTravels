namespace KarnelTravels.Models
{
    public class GetHotel_Res_Re
    {
        public IEnumerable<TblHotelRestaurant> All { get; set; }
        public IEnumerable<TblHotelRestaurant> HotelS{ get; set; }
        public IEnumerable<TblHotelRestaurant> Restaurants { get; set; }
        public IEnumerable<TblHotelRestaurant> Resortt {  get; set; }

        public IEnumerable<HrCategory> HrCategories { get; set; }

        public IEnumerable<TblImageUrl> HrImages { get; set; }

        public IEnumerable<TblSpot> tblSpots { get; set; }
    }
}
