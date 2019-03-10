using AdvantShop.App.Landing.Controllers.Domain;
using AdvantShop.App.Landing.Controllers.Domain.Settings;
using AdvantShop.App.Landing.Domain;
using AdvantShop.App.Landing.Models.Inplace;

namespace AdvantShop.App.Landing.Handlers.Inplace
{
    public class GetSettings
    {
        private readonly int _landingId;
        private readonly LpService _lpService;

        public GetSettings(int landingId)
        {
            _landingId = landingId;
            _lpService = new LpService();
        }

        public InplaceSettingsModel Execute()
        {
            LpService.CurrentLanding = _lpService.Get(_landingId);
            if (LpService.CurrentLanding == null)
                return null;
            
            return new InplaceSettingsModel()
            {
                PageTitle = SeoSettings.PageTitle,
                PageKeywords = SeoSettings.PageKeywords,
                PageDescription = SeoSettings.PageDescription,
            };
        }
    }
}
