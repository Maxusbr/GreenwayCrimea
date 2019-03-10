namespace AdvantShop.Core.Services.Bonuses.Model
{
    public class Grade
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal BonusPercent { get; set; }
        public int SortOrder { get; set; }
        public decimal PurchaseBarrier { get; set; }
    }
}
