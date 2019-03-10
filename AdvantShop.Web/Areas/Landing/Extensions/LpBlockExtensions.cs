using System;
using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Diagnostics;
using AdvantShop.App.Landing.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Web;
using AdvantShop.App.Landing.Controllers.Domain;

namespace AdvantShop.App.Landing.Extensions
{
    public static class LpBlockExtensions
    {
        public static dynamic TryGetSettingsValue(this LpBlock block, string key)
        {
            if (block.Settings == null)
                return null;

            if (block.MappedSettings == null)
            {
                try
                {
                    block.MappedSettings = JObject.Parse(block.Settings);
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                }
            };

            return block.MappedSettings != null ? block.MappedSettings[key] : null;
        }

        public static bool TrySetSettingsValue(this LpBlock block, string key, object value)
        {
            if (block.Settings == null)
                return false;

            try
            {
                if (block.MappedSettings == null)
                {
                    block.MappedSettings = JObject.Parse(block.Settings);
                }

                if (block.MappedSettings != null)
                {
                    block.MappedSettings[key].Value = value;
                    block.Settings = JsonConvert.SerializeObject(block.MappedSettings);

                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return false;
        }

        public static HtmlString TryGetStyleString(this LpBlock block)
        {
            var styleObj = TryGetSettingsValue(block, "style");
            var result = new StringBuilder();

            if (styleObj != null)
            {
                var stylesArray = styleObj.ToObject<Dictionary<string, string>>();

                foreach (KeyValuePair<string, string> entry in stylesArray)
                {
                    result.AppendFormat("{0}:{1};", entry.Key, entry.Value);
                }
            }

            return new HtmlString(result.ToString());
        }

        public static dynamic TryGetSettingsValue(this LpSubBlock subBlock, string key)
        {
            if (subBlock.Settings == null)
                return null;

            if (subBlock.MappedSettings == null)
            {
                try
                {
                    subBlock.MappedSettings = JObject.Parse(subBlock.Settings);
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                }
            };

            return subBlock.MappedSettings != null ? subBlock.MappedSettings[key] : null;
        }

        public static HtmlString TryGetStyleString(this LpSubBlock subBlock)
        {
            var styleObj = TryGetSettingsValue(subBlock, "style");
            var result = new StringBuilder();

            if (styleObj != null)
            {
                var stylesArray = styleObj.ToObject<Dictionary<string, string>>();

                foreach (KeyValuePair<string, string> entry in stylesArray)
                {
                    result.AppendFormat("{0}:{1};", entry.Key, entry.Value);
                }
            }

            return new HtmlString(result.ToString());
        }

        public static HtmlString RenderInplaceNgStyle(this LpSubBlock subBlock, string name)
        {
            return new HtmlString(LpService.Inplace ? String.Format("data-ng-style=\"{0}.settings.style\"", name) : string.Empty);
        }

    }
}
