namespace EnhancedEpic.Models
{
    public class Region
    {
        public string RegionCode { get; set; }
        public string CurrencyCode { get; set; }
        public GameOffer Game { get; set; }
        public ICollection<PriceHistoryEntry> PriceHistoryEntries { get; set; }
        public ICollection<PromotionRegion> PromotionRegions { get; set; }
    }
}
