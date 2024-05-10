using Microsoft.AspNetCore.Mvc.Rendering;

namespace KarnelTravels.Models
{
    public class GetCar_Plane_Train
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<ViewTravelImg> All { get; set; }
        public IEnumerable<ViewTravelImg> Cars { get; set; }
        public IEnumerable<ViewTravelImg> Planes { get; set; }
        public IEnumerable<ViewTravelImg> Trains { get; set; }
        public IEnumerable<TblSpot> Spots { get; set; }

        public IEnumerable<TblTransportation> Transportations { get; set; }
        public IEnumerable<TblImageUrl> HrImages { get; set; }
        public TblTravel travel { get; set; }
        public bool Active { get; set; }
        public SelectList selectObject { get; set; }
    }
}
