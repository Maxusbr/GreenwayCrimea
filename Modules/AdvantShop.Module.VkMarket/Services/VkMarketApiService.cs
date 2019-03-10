using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using AdvantShop.Core;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Services.Vk;
using AdvantShop.Diagnostics;
using AdvantShop.Module.VkMarket.Domain;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Exception;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace AdvantShop.Module.VkMarket.Services
{
    public class VkMarketApiService
    {

        public VkMarketApiService()
        {
            if (!Core.Modules.ModulesRepository.IsExistsModuleTable("Module", "VkCategory"))
            {
                InstallUpdateModuleService.Install();
            }
        }

        #region Auth

        public VkApi Auth()
        {
            if (string.IsNullOrEmpty(VkMarketSettings.AuthToken) || VkMarketSettings.UserId == 0)
                return null;

            try
            {
                var vk = new VkApi();
                vk.Authorize(new ApiAuthParams() { AccessToken = VkMarketSettings.AuthToken, UserId = VkMarketSettings.UserId });

                return vk;
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);

                var errors = VkMarketSettings.TokenErrorsCount;
                if (errors > 5)
                {
                    VkMarketSettings.TokenErrorsCount = 0;
                    VkMarketSettings.AuthToken = null;
                    VkMarketSettings.UserId = 0;
                }
                else
                {
                    VkMarketSettings.TokenErrorsCount = errors + 1;
                }

                throw new BlException("VkMarketApiService.Auth авторизация не прошла");
            }
        }

        #endregion

        public bool IsActive()
        {
            return !string.IsNullOrEmpty(VkMarketSettings.AuthToken) && VkMarketSettings.UserId != 0 && VkMarketSettings.Group != null;
        }

        #region Market Categories

        /// <summary>
        /// Список категорий из Вконтакте
        /// </summary>
        public List<VkMarketCategory> GetMarketCategories()
        {
            return CacheManager.Get("VkMarketApiService.MarketCategories", () =>
            {
                try
                {
                    var vk = Auth();
                    return vk.Markets.GetCategories(1000, 0).Select(x => new VkMarketCategory(x)).ToList();
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                }
                return null;
            });
        }

        #endregion

        #region Groups

        public List<VkGroup> GetGroups()
        {
            var vk = Auth();

            var groups = vk.Groups.Get(new GroupsGetParams()
            {
                Count = 1000,
                Extended = true,
                UserId = VkMarketSettings.UserId,
                Fields = GroupsFields.All,
                Filter = GroupsFilters.Moderator,
            }).Select(x => new VkGroup(x)).ToList();

            return groups;
        }

        public VkGroupSettings GetGroupSettings(long groupId)
        {
            var vk = Auth();

            try
            {
                // для настроек нужно stanalone приложение
                var settings = vk.Groups.GetSettings((ulong)groupId);
                if (settings == null)
                    return null;

                return new VkGroupSettings()
                {
                    IsMarketEnabled = settings.Market != null && settings.Market.Value,
                    MarketCurrency = settings.MarketCurrency.ToString()
                };
            }
            catch // Access denied: only group admin could access this method
            {
                // ignored
            }

            return null;
        }

        #endregion

        #region Categories

        public long AddAlbum(string name, VkApi vkApi = null)
        {
            var vk = vkApi ?? Auth();
            return vk.Markets.AddAlbum(-VkMarketSettings.Group.Id, name);
        }

        public bool UpdateAlbum(long albumId, string name, VkApi vkApi = null)
        {
            var vk = vkApi ?? Auth();
            try
            {
                return vk.Markets.EditAlbum(-VkMarketSettings.Group.Id, albumId, name);
            }
            catch (VkApiException ex)
            {
                Debug.Log.Error(ex);
            }
            return false;
        }

        public bool DeleteAlbum(long albumId, VkApi vkApi = null)
        {
            var vk = vkApi ?? Auth();
            try
            {
                return vk.Markets.DeleteAlbum(-VkMarketSettings.Group.Id, albumId);
            }
            catch (VkApiException ex)
            {
                Debug.Log.Error(ex);
            }
            return false;
        }

        public List<MarketAlbum> GetAllAlbums(VkApi vkApi = null)
        {
            var vk = vkApi ?? Auth();
            return vk.Markets.GetAlbums(-VkMarketSettings.Group.Id, 0, 100).ToList();
        }

        /// <summary>
        /// Изменяет положение подборки с товарами в списке.
        /// </summary>
        /// <param name="albumId">идентификатор подборки</param>
        /// <param name="before">идентификатор подборки, перед которой следует поместить текущую.</param>
        /// <param name="after">идентификатор подборки, после которой следует поместить текущую</param>
        /// <returns></returns>
        public bool ReorderAlbums(long albumId, long? before, long? after)
        {
            var vk = Auth();
            try
            {
                return vk.Markets.ReorderAlbums(-VkMarketSettings.Group.Id, albumId, before, after);
            }
            catch (VkApiException ex)
            {
                Debug.Log.Error(ex);
            }
            return false;
        }

        #endregion

        #region Products

        public long AddProduct(VkApi vk, VkProduct product)
        {
            product.Id = vk.Markets.Add(new MarketProductParams()
            {
                OwnerId = product.OwnerId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Deleted = product.Deleted,
                MainPhotoId = product.MainPhotoId,
                PhotoIds = product.PhotoIdsList,
            });

            vk.Markets.AddToAlbum(product.OwnerId, product.Id, new[] { product.AlbumId });

            return product.Id;
        }

        public bool UpdateProduct(VkApi vk, VkProduct product)
        {
            var result = vk.Markets.Edit(new MarketProductParams()
            {
                ItemId = product.Id,
                OwnerId = product.OwnerId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Deleted = product.Deleted,
                MainPhotoId = product.MainPhotoId,
                PhotoIds = product.PhotoIdsList,
            });

            return result;
        }

        public bool DeleteProduct(VkApi vk, long groupId, long id)
        {
            try
            {
                return vk.Markets.Delete(groupId, id);
            }
            catch (VkApiException ex)
            {
                Debug.Log.Error(ex);
            }
            return false;
        }

        public IEnumerable<Market> GetProducts(VkApi vk, long groupId, long? albumId)
        {
            var offset = 0;

            while (true)
            {
                var products = vk.Markets.Get(groupId, albumId, 200, offset, true);

                if (products == null || products.Count == 0)
                    break;

                foreach (var product in products)
                {
                    yield return product;
                }

                if (products.Count < 200)
                    break;

                offset += 200;
            }
        }

        public Market GetFirstProduct(VkApi vk, long groupId, long? albumId)
        {
            var product = vk.Markets.Get(groupId, albumId, 1, 0, true).FirstOrDefault();
            return product;
        }

        #endregion

        #region Photos

        public Photo AddPhoto(VkApi vk, long groupId, bool mainPhoto, string filePath)
        {
            // Получить адрес сервера для загрузки.
            var server = vk.Photo.GetMarketUploadServer(groupId, mainPhoto);

            // Загрузить фотографию.
            var wc = new WebClient();
            var responseImg = Encoding.ASCII.GetString(wc.UploadFile(server.UploadUrl, filePath));

            // Сохранить загруженную фотографию
            var photo = vk.Photo.SaveMarketPhoto(groupId, responseImg);

            return photo.FirstOrDefault();
        }

        #endregion
    }
}
