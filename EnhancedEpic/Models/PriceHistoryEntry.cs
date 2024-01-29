namespace EnhancedEpic.Models
{
    public class PriceHistoryEntry
    {
        public string PriceHistoryEntryId { get; set; }
        public DateTime Time { get; set; }
        public decimal Price { get; set; }
        public Region Region { get; set; }
    }
}
