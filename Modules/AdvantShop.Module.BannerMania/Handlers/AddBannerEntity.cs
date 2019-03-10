using System;
using AdvantShop.Core;
using AdvantShop.Diagnostics;
using AdvantShop.Module.BannerMania.Models;
using AdvantShop.Module.BannerMania.Service;
using AdvantShop.Web.Infrastructure.Handlers;

namespace AdvantShop.Module.BannerMania.Handlers
{
    public class AddUpdateBannerEntity : AbstractCommandHandler<bool>
    {
        private readonly BannerEntity _bannerEntity;

        public AddUpdateBannerEntity(BannerEntity bannerEntity)
        {
            _bannerEntity = bannerEntity;
        }

        protected override bool Handle()
        {
            try
            {
                var banner = new BannerEntity()
                {
                    EntityId = _bannerEntity.EntityId,
                    EntityName = _bannerEntity.EntityName,
                    EntityType = _bannerEntity.EntityType,
                    ImagePath = _bannerEntity.ImagePath,
                    Placement = _bannerEntity.Placement,
                    URL = _bannerEntity.URL,
                    NewWindow = _bannerEntity.NewWindow,
                    Enabled = _bannerEntity.Enabled
                };

                var updateSortOrder = true;

                if (_bannerEntity.EntityId <= 0)
                {
                    BMService.AddBannerEntity(banner);
                }
                else
                {
                    var b = BMService.GetBannerEntity(banner.EntityId, banner.EntityType, banner.Placement);
                    updateSortOrder = b != null;

                    BMService.UpdateBannerEntity(banner);
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return false;
            }

            return true;
        }

    }
}
