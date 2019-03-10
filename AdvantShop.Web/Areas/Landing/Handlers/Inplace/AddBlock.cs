using System;
using AdvantShop.App.Landing.Controllers;
using AdvantShop.App.Landing.Controllers.Domain;
using AdvantShop.App.Landing.Domain;
using AdvantShop.App.Landing.Handlers.Install;
using AdvantShop.App.Landing.Models;
using AdvantShop.App.Landing.Models.Inplace;
using AdvantShop.Diagnostics;

namespace AdvantShop.App.Landing.Handlers.Inplace
{
    public class AddBlock
    {
        private readonly AddBlockModel _model;
        private LandingBaseController _controller;

        private readonly LpService _lpService;
        private readonly LpBlockService _lpBlockService;
        private readonly LpBlockConfigService _lpBlockConfigService;

        public AddBlock(AddBlockModel model, LandingBaseController controller)
        {
            _model = model;
            _controller = controller;

            _lpService = new LpService();
            _lpBlockService = new LpBlockService();
            _lpBlockConfigService = new LpBlockConfigService();
        }

        public AddBlockResultModel Execute()
        {
            var lp = _lpService.Get(_model.LpId);
            if (lp == null)
                return new AddBlockResultModel();

            if (string.IsNullOrWhiteSpace(_model.Name))
                return new AddBlockResultModel();

            var installBlockHandler = new InstallBlockHandler(_model.Name, lp.Template, lp.Id, _model.SortOrder, null);
            var result = installBlockHandler.Execute();
            if (!result.Result)
            {
                Debug.Log.Error("Can't add landing block " + _model.Name + " for template " + lp.Template);

                return new AddBlockResultModel();
            }

            var block = _lpBlockService.Get(result.BlockId);
            if (block != null)
            {
                var blockConfig = _lpBlockConfigService.Get(block.Name, lp.Template);
                if (blockConfig != null)
                {
                    var model = new BlockModel()
                    {
                        Block = block,
                        Config = blockConfig,
                        InPlace = LpService.Inplace,
                    };

                    string html = null;

                    try
                    {
                        html = _controller.RenderPartialToString("~/Areas/Landing/Views/Shared/_WrapBlock.cshtml", model);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log.Error(ex);
                    }

                    return new AddBlockResultModel() {Result = true, Html = html};
                }
            }

            return new AddBlockResultModel();
        }
    }
}
