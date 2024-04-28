namespace KarnelTravels.Models
{
    public class GetHotel_Res_Re
    {
        public IEnumerable<TblHotelRestaurant> All { get; set; }
        public IEnumerable<TblHotelRestaurant> HotelS{ get; set; }
        public IEnumerable<TblHotelRestaurant> Restaurants { get; set; }
        public IEnumerable<TblHotelRestaurant> Resortt {  get; set; }
        
    }
}
