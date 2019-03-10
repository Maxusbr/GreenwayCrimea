using System.Collections.Generic;
using System.Linq;
using AdvantShop.App.Landing.Controllers.Domain;
using AdvantShop.App.Landing.Domain;
using AdvantShop.App.Landing.Models;

namespace AdvantShop.App.Landing.Handlers.Inplace
{
    public class GetBlocks
    {
        private readonly int _landingId;
        private readonly LpService _lpService;

        public GetBlocks(int landingId)
        {
            _landingId = landingId;
            _lpService = new LpService();
        }

        public List<BlockListViewModel> Execute()
        {
            var lp = _lpService.Get(_landingId);
            if (lp == null)
                return null;

            var lpTemplateService = new LpTemplateService();
            var blockConfigService = new LpBlockConfigService();

            var template = lpTemplateService.GetTemplate(lp.Template);
            if (template == null)
                return null;

            var lpBlockConfigList = template.Blocks.Select(t => blockConfigService.Get(t, lp.Template)).Where(x => x != null).ToList();

            var model = new List<BlockListViewModel>();

            foreach (var block in lpBlockConfigList)
            {
                var mBlock = model.Find(x => x.Category == block.Category);

                if (mBlock == null)
                {
                    mBlock = new BlockListViewModel() { Category = block.Category };
                    model.Add(mBlock);
                }

                mBlock.Blocks.Add(new BlockListItemViewModel()
                {
                    Name = block.Name,
                    Type = block.Type,
                    Description = block.Description,
                    Picture = block.Picture
                });
            }

            return model;
        }
    }
}
