namespace EnhancedEpic.Models
{
    public class Promotion
    {
        public string PromotionId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double DiscountRate { get; set; }
        public ICollection<PromotionRegion> PromotionRegions { get; set; }
    }
}
