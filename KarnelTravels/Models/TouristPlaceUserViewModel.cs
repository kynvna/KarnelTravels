namespace KarnelTravels.Models
{
    public class TouristPlaceUserViewModel
    {
        public int Id { get; set; }


        public string? Name { get; set; }

        public string? Description { get; set; }

        public List<string> ImageUrl { get; set; } // Changed from Imglink to ImageUrl

        public string? Status { get; set; }
        public string? Namespot { get; set; }

        public virtual TblSpot? Sport { get; set; }
    }
}
