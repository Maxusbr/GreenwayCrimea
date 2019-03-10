using System.Linq;
using System.Collections.Generic;
using AdvantShop.Web.Admin.Models.ExportFeeds;
using AdvantShop.ExportImport;
using AdvantShop.Web.Admin.ViewModels.ExportFeeds;
using AdvantShop.Saas;

namespace AdvantShop.Web.Admin.Handlers.ExportFeeds
{
    public class GetExportFeedsHandler
    {
        private readonly int? _id;
        private readonly EExportFeedType? _exportFeedType;

        public GetExportFeedsHandler(int? id, EExportFeedType? exportFeedType)
        {
            _id = id;
            _exportFeedType = exportFeedType;
        }

        public ExportFeedsModel Execute()
        {
            var model = new ExportFeedsModel();

            model.CurrentExportFeedsType = _exportFeedType;
            model.ExportFeeds = new List<ExportFeedModel>();
            var exportFeeds = _exportFeedType.HasValue
                ? ExportFeedService.GetExportFeeds((EExportFeedType)_exportFeedType)
                : ExportFeedService.GetExportFeeds();
                    

            foreach (var exportFeed in exportFeeds)
            {
                if(SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveExportFeeds 
                    && (exportFeed.Type == EExportFeedType.YandexMarket || exportFeed.Type == EExportFeedType.GoogleMerchentCenter))
                {
                    continue;
                }

                var exportFeedModel = new ExportFeedModel
                {
                    Id = exportFeed.Id,
                    Name = exportFeed.Name,
                    Description = exportFeed.Description,
                    LastExport = exportFeed.LastExport,
                    LastExportFileFullName = exportFeed.LastExportFileFullName,
                    Type = exportFeed.Type
                };
                model.ExportFeeds.Add(exportFeedModel);
            }

            if (model.ExportFeeds != null && model.ExportFeeds.Count > 0)
            {
                model.CurrentExportFeed = _id.HasValue
                    ? model.ExportFeeds.FirstOrDefault(item => item.Id == _id)
                    : model.ExportFeeds.FirstOrDefault();

                var handler = new GetExportFeedSettings(ExportFeedSettingsProvider.GetSettings(model.CurrentExportFeed.Id), model.CurrentExportFeed.Type);
                model.CurrentExportFeed.ExportFeedSettings = handler.Execute();

                model.CurrentExportFeed.ExportAllProducts = ExportFeedService.IsExportAllCategories(model.CurrentExportFeed.Id);
            }
            return model;
        }
    }
}
