using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Core.SQL;
using AdvantShop.Module.VkMarket.Domain;

namespace AdvantShop.Module.VkMarket.Services
{
    public class VkCategoryService
    {
        private readonly VkMarketApiService _apiService;

        public VkCategoryService()
        {
            _apiService = new VkMarketApiService();
        }

        public List<VkCategory> GetList()
        {
            return SQLDataAccess.Query<VkCategory>("Select * From Module.VkCategory Order by SortOrder").ToList();
        }

        public VkCategory Get(int id)
        {
            return SQLDataAccess.Query<VkCategory>("Select * From Module.VkCategory Where Id=@id", new {id}).FirstOrDefault();
        }
        
        public VkCategory GetByVkId(long vkId)
        {
            return SQLDataAccess.Query<VkCategory>("Select * From Module.VkCategory Where VkId=@vkId", new { vkId }).FirstOrDefault();
        }

        public void Add(VkCategory category)
        {
            if (category.VkId == 0)
                category.VkId = _apiService.AddAlbum(category.Name);
            
            category.Id =
                SQLDataAccess.ExecuteScalar<int>(
                    "Insert Into Module.VkCategory (VkId, VkCategoryId, Name, SortOrder) Values (@VkId, @VkCategoryId, @Name, @SortOrder); " +
                    "Select scope_identity();", 
                    CommandType.Text,
                    new SqlParameter("@VkId", category.VkId),
                    new SqlParameter("@VkCategoryId", category.VkCategoryId),
                    new SqlParameter("@Name", category.Name ?? ""),
                    new SqlParameter("@SortOrder", category.SortOrder));
        }

        public void Update(VkCategory category)
        {
            var c = Get(category.Id);
            if (c != null && c.Name != category.Name)
                _apiService.UpdateAlbum(category.VkId, category.Name);

            SQLDataAccess.ExecuteNonQuery(
                "Update Module.VkCategory SET VkId=@VkId, VkCategoryId=@VkCategoryId, Name=@Name, SortOrder=@SortOrder Where Id=@Id", CommandType.Text,
                new SqlParameter("@Id", category.Id),
                new SqlParameter("@VkId", category.VkId),
                new SqlParameter("@VkCategoryId", category.VkCategoryId),
                new SqlParameter("@Name", category.Name ?? ""),
                new SqlParameter("@SortOrder", category.SortOrder));
        }

        public void Delete(int id, long vkId)
        {
            _apiService.DeleteAlbum(vkId);

            new VkProductService().DeleteByAlbum(vkId);

            SQLDataAccess.ExecuteNonQuery(
                "Delete From Module.VkCategory Where Id=@Id", CommandType.Text,
                new SqlParameter("@Id", id));
        }


        public List<Category> GetLinkedCategories(int id)
        {
            return
                SQLDataAccess.Query<Category>(
                    "Select CategoryId, Name From Catalog.Category " +
                    "Where CategoryId in (Select CategoryId From Module.VkCategory_Category Where VkCategoryId=@id)",
                    new { id })
                    .ToList();
        }

        public void AddLink(int categoryId, int id)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Insert Into Module.VkCategory_Category (CategoryId, VkCategoryId) Values (@CategoryId, @VkCategoryId) ",
                CommandType.Text,
                new SqlParameter("@CategoryId", categoryId),
                new SqlParameter("@VkCategoryId", id));
        }

        public void RemoveLinks(int id)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Delete From Module.VkCategory_Category Where VkCategoryId=@VkCategoryId",
                CommandType.Text,
                new SqlParameter("@VkCategoryId", id));
        }

    }
}
