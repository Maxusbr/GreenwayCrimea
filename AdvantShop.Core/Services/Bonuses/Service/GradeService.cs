using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Bonuses.Model;
using AdvantShop.Core.SQL;

namespace AdvantShop.Core.Services.Bonuses.Service
{
    public class GradeService
    {
        public static Grade Get(int id)
        {
            return SQLDataAccess2.Query<Grade>("Select * from Bonus.Grade where id=@id", new { id = id });
        }

        public static Grade Get(string name)
        {
            return SQLDataAccess2.Query<Grade>("Select * from Bonus.Grade where Name=@name", new { name = name });
        }

        public static int Add(Grade model)
        {
            var temp = SQLDataAccess2.ExecuteScalar<int>("insert into Bonus.Grade (Name,BonusPercent,SortOrder,PurchaseBarrier) values (@Name, @BonusPercent, @SortOrder, @PurchaseBarrier); select cast(scope_identity() as int)", model);
            return temp;
        }

        public static void Update(Grade model)
        {
            SQLDataAccess2.ExecuteNonQuery("Update Bonus.Grade set Name=@Name, BonusPercent=@BonusPercent, SortOrder=@SortOrder, PurchaseBarrier=@PurchaseBarrier where Id=@Id", model);
        }

        public static List<Grade> GetAll()
        {
            return SQLDataAccess2.ExecuteReadIEnumerable<Grade>("Select * from Bonus.Grade order by SortOrder ASC").ToList();
        }

        public static void Delete(int id)
        {
            SQLDataAccess2.ExecuteNonQuery("Delete from Bonus.Grade where Id=@id", new { id = id });
        }

        public static bool IsUsed(int gradeId)
        {
            return SQLDataAccess2.Query<bool>("Select top(1) 1 from Bonus.Card where GradeId=@id ", new { id = gradeId });
        }
    }
}
