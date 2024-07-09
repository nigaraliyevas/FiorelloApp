namespace FiorelloApp.ViewModels
{
    public class HeaderVM
    {
        public int Count { get; set; }
        public List<BasketVM> AllProducts { get; set; }
        public Dictionary<string, string> Settings { get; set; }
    }
}
