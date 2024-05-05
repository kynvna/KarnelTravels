namespace KarnelTravels.Models
{
    public class HRUserViewModel
    {
        public int HrId { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public string Description { get; set; }
        public List<string> ImageUrl { get; set; } // Changed from Imglink to ImageUrl

    }
}
