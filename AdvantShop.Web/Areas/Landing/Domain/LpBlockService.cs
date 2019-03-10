using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Core.SQL;

namespace AdvantShop.App.Landing.Domain
{
    public class LpBlockService
    {

        #region Block

        public int Add(LpBlock block)
        {
            block.Id = Convert.ToInt32(SQLDataAccess.ExecuteScalar(
                "INSERT INTO [CMS].[LandingBlock] ([LandingId],[Name],[ContentHtml],[Type],[Settings],[SortOrder],[Enabled]) VALUES (@LandingId,@Name,@ContentHtml,@Type,@Settings,@SortOrder,@Enabled); Select scope_identity(); ",
                CommandType.Text,
                new SqlParameter("@LandingId", block.LandingId),
                new SqlParameter("@Name", block.Name),
                new SqlParameter("@ContentHtml", block.ContentHtml ?? string.Empty),
                new SqlParameter("@Type", block.Type),
                new SqlParameter("@Settings", block.Settings ?? string.Empty),
                new SqlParameter("@SortOrder", block.SortOrder),
                new SqlParameter("@Enabled", block.Enabled)
                ));

            return block.Id;
        }

        public void Update(LpBlock block)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Update [CMS].[LandingBlock] Set [LandingId]=@LandingId, [Name]=@Name, [ContentHtml]=@ContentHtml, [Type]=@Type, [Settings]=@Settings, [SortOrder]=@SortOrder, [Enabled]=@Enabled Where Id = @Id ",
                CommandType.Text,
                new SqlParameter("@Id", block.Id),
                new SqlParameter("@LandingId", block.LandingId),
                new SqlParameter("@Name", block.Name),
                new SqlParameter("@ContentHtml", block.ContentHtml ?? string.Empty),
                new SqlParameter("@Type", block.Type),
                new SqlParameter("@Settings", block.Settings ?? string.Empty),
                new SqlParameter("@SortOrder", block.SortOrder),
                new SqlParameter("@Enabled", block.Enabled)
                );
        }

        public void Delete(int id)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Delete [CMS].[LandingBlock] Where Id = @Id ", CommandType.Text, new SqlParameter("@Id", id));
        }

        public LpBlock Get(int id)
        {
            return
                SQLDataAccess.Query<LpBlock>("Select * From [CMS].[LandingBlock] Where Id = @id",
                    new {id}).FirstOrDefault();
        }

        public LpBlock Get(int lpId, string name)
        {
            return
                SQLDataAccess.Query<LpBlock>(
                    "Select * From [CMS].[LandingBlock] Where LandingId = @lpId and Name = @name",
                    new {lpId, name}).FirstOrDefault();
        }

        public List<LpBlock> GetList(int lpId)
        {
            return
                SQLDataAccess.Query<LpBlock>("Select * From [CMS].[LandingBlock] Where LandingId = @lpId Order By SortOrder",
                    new { lpId }).ToList();
        }

        #endregion

        #region Sub block

        public int AddSubBlock(LpSubBlock block)
        {
            return Convert.ToInt32(SQLDataAccess.ExecuteScalar(
                "INSERT INTO [CMS].[LandingSubBlock] ([LandingBlockId],[Name],[ContentHtml],[Type],[Settings],[SortOrder]) VALUES (@LandingBlockId,@Name,@ContentHtml,@Type,@Settings,@SortOrder); Select scope_identity(); ",
                CommandType.Text,
                new SqlParameter("@LandingBlockId", block.LandingBlockId),
                new SqlParameter("@Name", block.Name),
                new SqlParameter("@ContentHtml", block.ContentHtml ?? string.Empty),
                new SqlParameter("@Type", block.Type),
                new SqlParameter("@Settings", block.Settings ?? string.Empty),
                new SqlParameter("@SortOrder", block.SortOrder)
                ));
        }

        public void UpdateSubBlock(LpSubBlock block)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Update [CMS].[LandingSubBlock] Set [LandingBlockId]=@LandingBlockId, [Name]=@Name, [ContentHtml]=@ContentHtml, [Type]=@Type, [Settings]=@Settings, [SortOrder]=@SortOrder Where Id = @Id ",
                CommandType.Text,
                new SqlParameter("@Id", block.Id),
                new SqlParameter("@LandingBlockId", block.LandingBlockId),
                new SqlParameter("@Name", block.Name),
                new SqlParameter("@ContentHtml", block.ContentHtml ?? string.Empty),
                new SqlParameter("@Type", block.Type),
                new SqlParameter("@Settings", block.Settings ?? string.Empty),
                new SqlParameter("@SortOrder", block.SortOrder)
                );
        }

        public void DeleteSubBlock(int id)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Delete From [CMS].[LandingBlock] Where Id = @Id ", CommandType.Text, new SqlParameter("@Id", id));
        }

        public LpSubBlock GetSubBlock(int id)
        {
            return SQLDataAccess.Query<LpSubBlock>("Select * From [CMS].[LandingSubBlock] Where Id = @id", new { id }).FirstOrDefault();
        }

        public LpSubBlock GetSubBlock(int blockId, string name)
        {
            return
                SQLDataAccess.Query<LpSubBlock>(
                    "Select * From [CMS].[LandingSubBlock] Where LandingBlockId = @blockId and Name=@name",
                    new {blockId, name}).FirstOrDefault();
        }


        #endregion
        
    }
}
