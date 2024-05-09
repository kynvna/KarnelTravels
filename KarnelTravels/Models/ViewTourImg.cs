namespace KarnelTravels.Models
{
    public class ViewTourImg
    {
        public int Id { get; set; }

        public int? SportId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? ImageLinkId { get; set; }

        public string? Status { get; set; }

        public virtual TblSpot? Sport { get; set; }
    }
}
