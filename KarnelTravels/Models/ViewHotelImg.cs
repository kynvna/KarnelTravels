using Microsoft.EntityFrameworkCore.Migrations;

namespace KarnelTravels.Models
{
    public class ViewHotelImg
    {

        public int HrId { get; set; }

        public int? CatId { get; set; }

        public string? Name { get; set; }

        public decimal? Price { get; set; }

        public int? SpotId { get; set; }

        public string? Imglink { get; set; }

        public string? Description { get; set; }

        public int? ImageLinkId { get; set; }
        public string? Status { get; set; }

        public string url { get; set; }

        public virtual HrCategory? Cat { get; set; }
    }
}
