using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KarnelTravels.Models
{
    public class GetHotel_Res_Re
    {
        public int TotalItems {  get; set; }
         public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<ViewHotelImg> All { get; set; }
        public IEnumerable<ViewHotelImg> HotelS { get; set; }
        public IEnumerable<ViewHotelImg> Restaurants { get; set; }
        public IEnumerable<ViewHotelImg> Resortt {  get; set; }

        public IEnumerable<TblHotelRestaurant> tblHotelRestaurants { get; set; }
        public IEnumerable<HrCategory> HrCategories { get; set; }

        public IEnumerable<TblImageUrl> HrImages { get; set; }

        public IEnumerable<TblSpot> tblSpots { get; set; }
         public TblHotelRestaurant tblHotel {  get; set; }
        public bool Active { get; set; }
        public SelectList selectObject { get; set; }
    }
}
