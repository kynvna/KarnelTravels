namespace KarnelTravels.Models
{
    public class TourPackage
    {
        public int PackageId { get; set; }

        

        public DateOnly? StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public decimal? TotalPrice { get; set; }

        public int? ImageLinkId { get; set; }

        public string? Name { get; set; }

        public int? SportId { get; set; }
    }
}
