using AdvantShop.Core.Services.Bonuses.Model;

namespace AdvantShop.Web.Admin.Models.Grades
{
    public class GradeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public decimal BonusPercent { get; set; }
        public decimal PurchaseBarrier { get; set; }

        public static explicit operator GradeModel(Grade grade)
        {
            return new GradeModel
            {
                Id = grade.Id,
                Name = grade.Name,
                SortOrder = grade.SortOrder,
                BonusPercent = grade.BonusPercent,
                PurchaseBarrier = grade.PurchaseBarrier
            };
        }
    }
}
