using AdvantShop.Core.Services.Localization;
using AdvantShop.Web.Infrastructure.Localization;

namespace AdvantShop.Web.Infrastructure.Razor
{
    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        public LocalizedString T(string resourceKey)
        {
            var resourceValue = LocalizationService.GetResource(resourceKey);

            if (string.IsNullOrEmpty(resourceValue))
                return new LocalizedString(resourceKey);

            return new LocalizedString(resourceValue);
        }

        public LocalizedString T(string resourceKey, params object[] args)
        {
            var resourceValue = LocalizationService.GetResource(resourceKey);

            if (string.IsNullOrEmpty(resourceValue))
                return new LocalizedString(resourceKey);

            return new LocalizedString((args == null || args.Length == 0) ? resourceValue : string.Format(resourceValue, args));
        }
    }

    public abstract class WebViewPage : WebViewPage<dynamic>
    {
    }
}
