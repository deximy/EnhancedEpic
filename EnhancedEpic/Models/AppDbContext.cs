using Microsoft.EntityFrameworkCore;

namespace EnhancedEpic.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<GameOffer> Games { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<PriceHistoryEntry> PriceHistoryEntries { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<PromotionRegion> PromotionRegions { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder model_builder)
        {
            model_builder
                .Entity<GameOffer>()
                .HasKey(game => game.OfferId);

            model_builder
                .Entity<Region>()
                .HasKey(region => region.RegionCode);

            model_builder
                .Entity<PriceHistoryEntry>()
                .HasKey(price_history_entry => price_history_entry.PriceHistoryEntryId);

            model_builder
                .Entity<Promotion>()
                .HasKey(promotion => promotion.PromotionId);

            // 仅配置复合主键
            model_builder
                .Entity<PromotionRegion>()
                .HasKey(promotion_region => new { promotion_region.PromotionId, promotion_region.RegionId });
        }
    }
}
