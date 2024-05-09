namespace KarnelTravels.Models
{
    public class GetAllPack
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<ViewPackageImg> Packages { get; set; }
    }
}
