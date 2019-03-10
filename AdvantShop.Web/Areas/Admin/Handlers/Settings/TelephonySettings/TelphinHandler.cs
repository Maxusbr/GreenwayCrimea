using System.Collections.Generic;
using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.IPTelephony.Telphin;
using AdvantShop.Core.Services.Localization;
using Newtonsoft.Json;

namespace AdvantShop.Web.Admin.Handlers.Settings.TelephonySettings
{
    public class TelphinHandler
    {
        private Telphin _telphin;

        public TelphinHandler()
        {
            _telphin = new Telphin();
        }

        public List<TelphinExtension> GetExtensions()
        {
            var extensions = _telphin.GetExtensions() ?? new List<TelphinExtension>();
            foreach (var extension in extensions)
            {
                extension.Events = _telphin.GetEvents(extension.Id);
            }
            SettingsTelephony.TelphinExtensions = JsonConvert.SerializeObject(extensions);
            return extensions;
        }

        private List<TelphinExtension> GetDBExtensions()
        {
            return SettingsTelephony.TelphinExtensions.IsNotEmpty()
                ? JsonConvert.DeserializeObject<List<TelphinExtension>>(SettingsTelephony.TelphinExtensions)
                : new List<TelphinExtension>();
        }

        public List<TelphinExtension> AddEvents(string extensionId)
        {
            var extensions = GetDBExtensions();
            var ext = extensions.FirstOrDefault(x => x.Id == extensionId);
            if (ext == null)
                throw new BlException(LocalizationService.GetResource("Admin.SettingsTelephony.Errors.TelphinExtension.NotFound"));
            foreach (var kvp in TelphinEvent.EventsDict)
            {
                var @event = _telphin.AddEvent(extensionId, new TelphinEvent
                {
                    EventType = kvp.Value,
                    Url = string.Format("{0}/telphin/pushnotification", SettingsMain.SiteUrl)
                });
                if (@event != null)
                    ext.Events.Add(@event);
            }
            SettingsTelephony.TelphinExtensions = JsonConvert.SerializeObject(extensions);
            return extensions;
        }

        public List<TelphinExtension> DeleteEvents(string extensionId)
        {
            var extensions = GetDBExtensions();
            var ext = extensions.FirstOrDefault(x => x.Id == extensionId);
            if (ext == null)
                throw new BlException(LocalizationService.GetResource("Admin.SettingsTelephony.Errors.TelphinExtension.NotFound"));

            _telphin.DeleteEvents(extensionId);
            ext.Events.Clear();
            SettingsTelephony.TelphinExtensions = JsonConvert.SerializeObject(extensions);
            return extensions;
        }

        public TelphinExtension GetExtension(string extension)
        {
            var extensions = GetDBExtensions();
            if (!extensions.Any())
                extensions = GetExtensions();
            return extensions.FirstOrDefault(x => x.Extension == extension);
        }
    }
}
