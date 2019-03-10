using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Core.SQL;
using AdvantShop.Module.VkMarket.Domain;
using VkNet;

namespace AdvantShop.Module.VkMarket.Services
{
    public class VkProductService
    {
        private readonly VkMarketApiService _apiService;

        public VkProductService()
        {
            _apiService = new VkMarketApiService();
        }

        public VkProduct Get(long id)
        {
            return SQLDataAccess
                .Query<VkProduct>("Select * From Module.VkProduct Where Id=@id", new {id})
                .FirstOrDefault();
        }

        public long GetIdByProductId(int productId)
        {
            return SQLDataAccess
                .Query<long>("Select top(1) id From Module.VkProduct Where ProductId=@productId", new { productId })
                .FirstOrDefault();
        }

        public int GetProductIdByMarketId(long marketId)
        {
            var productId =
                Convert.ToInt32(SQLDataAccess.ExecuteScalar(
                    "Select top(1) ProductId From Module.VkProduct Where Id=@Id", CommandType.Text, new SqlParameter("@Id", marketId)));

            return productId;
        }

        public void Add(VkProduct product)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Insert Into Module.VkProduct (Id, ProductId, MainPhotoId, PhotoIds, AlbumId) Values (@Id, @ProductId, @MainPhotoId, @PhotoIds, @AlbumId) ",
                CommandType.Text,
                new SqlParameter("@Id", product.Id),
                new SqlParameter("@ProductId", product.ProductId),
                new SqlParameter("@MainPhotoId", product.MainPhotoId),
                new SqlParameter("@PhotoIds", product.PhotoIdsList != null ? String.Join(",", product.PhotoIdsList) : ""),
                new SqlParameter("@AlbumId", product.AlbumId));
        }

        public void Delete(long id, int productId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Delete From Module.VkProduct Where Id=@Id and ProductId=@ProductId", 
                CommandType.Text,
                new SqlParameter("@Id", id),
                new SqlParameter("@ProductId", productId));
        }

        public void DeleteByAlbum(long albumId)
        {
            var vkProducts =
                SQLDataAccess.Query<VkProduct>("Select Id, ProductId From Module.VkProduct Where AlbumId=@albumId", new { albumId }).ToList();
            
            if (vkProducts.Count == 0)
                return;

            var vk = _apiService.Auth();
            var groupId = -VkMarketSettings.Group.Id;

            foreach (var p in vkProducts)
            {
                _apiService.DeleteProduct(vk, groupId, p.Id);
                Delete(p.Id, p.ProductId);
            }
        }

        public void DeleteByAlbumAndNotInList(VkApi vk, long albumId, List<long> ids)
        {
            if (ids == null || ids.Count == 0)
                return;

            var vkProducts =
                SQLDataAccess.Query<VkProduct>(
                    "Select Id, ProductId From Module.VkProduct Where AlbumId=@albumId and Id not in (" + String.Join(", ", ids) + ")", 
                    new { albumId })
                    .ToList();

            if (vkProducts.Count == 0)
                return;
            
            var groupId = -VkMarketSettings.Group.Id;

            foreach (var p in vkProducts)
            {
                _apiService.DeleteProduct(vk, groupId, p.Id);
                Delete(p.Id, p.ProductId);
            }
        }

        public void DeleteAllProducts()
        {
            var vkProducts = SQLDataAccess.Query<VkProduct>("Select Id, ProductId From Module.VkProduct").ToList();
            
            if (vkProducts.Count == 0)
                return;

            var vk = _apiService.Auth();
            var groupId = -VkMarketSettings.Group.Id;

            foreach (var p in vkProducts)
            {
                _apiService.DeleteProduct(vk, groupId, p.Id);
                Delete(p.Id, p.ProductId);
            }
        }

    }
}
