namespace EnhancedEpic.Models
{
    public class GameOffer
    {
        public string OfferId { get; set; }

        public ICollection<Region> Regions { get; set; }
    }
}
