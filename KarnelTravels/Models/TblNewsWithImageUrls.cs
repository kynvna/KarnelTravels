namespace KarnelTravels.Models
{
    public class TblNewsWithImageUrls
    {
        public TblNews NewsItem { get; set; }
        public List<TblImageUrl> ImageUrls { get; set; }
    }
}
