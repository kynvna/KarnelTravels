namespace KarnelTravels.Models
{
    public class NewsDetailViewModel
    {
        public TblNewsWithImageUrls MainNewsWithImages { get; set; }
        public List<TblNewsWithImageUrls> RelatedNewsWithImages { get; set; }
    }
}
