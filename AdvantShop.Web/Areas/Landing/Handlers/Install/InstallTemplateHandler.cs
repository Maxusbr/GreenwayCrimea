using System;
using AdvantShop.App.Landing.Controllers.Domain;
using AdvantShop.Diagnostics;
using AdvantShop.App.Landing.Domain;

namespace AdvantShop.App.Landing.Handlers.Install
{
    public class InstallTemplateHandler
    {
        private Lp _lp;
        private LpConfiguration _configuration;
        
        private LpService _lpService;
        private LpTemplateService _lpTemplateService;

        public InstallTemplateHandler(Lp lp, LpConfiguration configuration)
        {
            _lp = lp;
            _configuration = configuration;

            _lpService = new LpService();
            _lpTemplateService = new LpTemplateService();
        }

        public int Execute()
        {
            if (_lp == null)
                return 0;

            _lp.Id = _lpService.Add(_lp);

            var template = _lpTemplateService.GetTemplate(_lp.Template);
            if (template == null)
                return 0;

            for(var i = 0; i < template.Blocks.Count; i++)
            {
                try
                {
                    var installBlockHandler = new InstallBlockHandler(template.Blocks[i], template.Key, _lp.Id, i * 100, _configuration);
                    var result = installBlockHandler.Execute();
                    //if (!result.Result)
                    //    Debug.Log.Error("Can't install landing block " + template.Blocks[i] + " for template " + template.Key);
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                }
            }

            return _lp.Id;
        }

    }
}
