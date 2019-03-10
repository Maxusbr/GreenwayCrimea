
using AdvantShop.ExportImport;
using AdvantShop.Web.Admin.Models.ExportFeeds;
using System.Collections.Generic;

namespace AdvantShop.Web.Admin.ViewModels.ExportFeeds
{
    public class ExportFeedsModel
    {
        public List<ExportFeedModel> ExportFeeds { get; set; }

        public ExportFeedModel CurrentExportFeed { get; set; }

        public EExportFeedType? CurrentExportFeedsType { get; set; }
    }
}
