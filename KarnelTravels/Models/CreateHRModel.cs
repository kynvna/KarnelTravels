namespace KarnelTravels.Models
{
    public class CreateHRModel
    {

        public int? CatId { get; set; }

        public string? Name { get; set; }

        public decimal? Price { get; set; }

        public int? SpotId { get; set; }

        public IFormFile Imglink { get; set; }

        public string? Description { get; set; }

        public int? ImageLinkId { get; set; }

        public string? Status { get; set; }

        public virtual HrCategory? Cat { get; set; }
    }
}
