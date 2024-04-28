namespace KarnelTravels.Models
{
    public class GetCar_Plane_Train
    {
        public IEnumerable<TblTravel> All {  get; set; }
        public IEnumerable<TblTravel> Cars { get; set; }
        public IEnumerable<TblTravel> Planes { get; set; }
        public IEnumerable<TblTravel> Trains { get; set; }
        public IEnumerable<TblSpot> Spots { get; set; }

        public IEnumerable<TblTransportation> Transportations { get; set; }

    }
}
