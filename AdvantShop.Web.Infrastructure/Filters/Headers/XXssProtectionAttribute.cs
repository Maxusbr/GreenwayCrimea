using System;
using System.Web.Mvc;

namespace AdvantShop.Web.Infrastructure.Filters.Headers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class XXssProtectionAttribute : HttpHeaderAttributeBase
    {
        private readonly XXssProtectionPolicy _policy;
        private readonly bool _blockMode;

        public XXssProtectionAttribute(XXssProtectionPolicy policy, bool blockMode)
        {
            _policy = policy;
            _blockMode = blockMode;
        }

        public override void SetHttpHeadersOnActionExecuted(ActionExecutedContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            string value;
            switch (_policy)
            {
                case XXssProtectionPolicy.Disabled:
                    return;
                case XXssProtectionPolicy.FilterDisabled:
                    value = "0";
                    break;
                case XXssProtectionPolicy.FilterEnabled:
                    value = (_blockMode ? "1; mode=block" : "1");
                    break;
                default:
                    throw new NotImplementedException("Wrong XFrameOptionsPolicy " + _policy);
            }
            if (string.IsNullOrEmpty(value)) return;
            response.Headers[HeaderConstants.XXssProtectionHeader] = value;
        }
    }

    public enum XXssProtectionPolicy
    {
        /// <summary>Specifies that the X-Xss-Protection header should not be set in the HTTP response.</summary>
        Disabled,

        /// <summary>
        ///     Specifies that the X-Xss-Protection header should be set in the HTTP response, explicitly disabling the IE XSS
        ///     filter.
        /// </summary>
        FilterDisabled,

        /// <summary>
        ///     Specifies that the X-Xss-Protection header should be set in the HTTP response, explicitly enabling the IE XSS
        ///     filter.
        /// </summary>
        FilterEnabled
    }
}
