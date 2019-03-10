using System;

using AdvantShop.ExportImport;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Web.Admin.Handlers.ExportFeeds
{
    public class AddExportFeed
    {
        private readonly string _name;
        private readonly string _description;
        private readonly EExportFeedType _type;

        public AddExportFeed(string name, string description, EExportFeedType type)
        {
            _name = name;
            _description = description;
            _type = type;
        }

        public int Execute()
        {

            var exportFeedId = ExportFeedService.AddExportFeed(new ExportFeed
            {
                Name = _name,
                Type = _type,
                Description = _description
            });

            if (exportFeedId == 0)
                return -1;

            ExportFeedService.InsertCategory(exportFeedId, 0);



            var type = ReflectionExt.GetTypeByAttributeValue<ExportFeedKeyAttribute>(typeof(BaseExportFeed), atr => atr.Value, _type.ToString());
            var currentExportFeed = (BaseExportFeed)Activator.CreateInstance(type);
            currentExportFeed.SetDefaultSettings(exportFeedId);
           
            return exportFeedId;
        }
    }
}
