//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.Saas;
using Newtonsoft.Json;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Mails;
using RestSharp;
using AdvantShop.Helpers;
using AdvantShop.Core.Caching;

namespace AdvantShop.Trial
{
    public class TrialService
    {
        private const string Url = "http://modules.advantshop.net/";

        private const string UrlTrialInfo = "http://modules.advantshop.net/Trial/GetParams/{0}";
        private const string UrlTrialEvents = "http://modules.advantshop.net/Event/LogEvent?licKey={0}&eventName={1}&eventParams={2}";
        private const string UrlTrackEvents = "http://modules.advantshop.net/Event/TrackEvent?licKey={0}&eventName={1}";

        private const string UrlGetAchievements = "http://modules.advantshop.net/Shop/GetLicenseAchievement?licKey={0}";
        private const string UrlAchievementsGetPoints = "http://modules.advantshop.net/Shop/GetLicensePoints?licKey={0}";
        private const string UrlGetAchievementsDescription = "http://modules.advantshop.net/Shop/GetAchievementsDescription?licKey={0}";
        private const string UrlGetAchievementsPopUp = "http://modules.advantshop.net/Shop/GetAchievementsPopUp?licKey={0}";

        private const string UrlTrialCounter = "http://modules.advantshop.net/Trial/GetCounter/{0}";

        private static DateTime _lastUpdate;

        private static DateTime _trialTillCached = DateTime.MinValue;

        public static bool IsTrialEnabled
        {
            get { return ModeConfigService.IsModeEnabled(ModeConfigService.Modes.TrialMode); }
        }

        public static int LeftDay
        {
            get
            {
                return (TrialPeriodTill - DateTime.Now).Days + 1;
            }
        }

        public static string LeftDayString
        {
            get
            {
                return LeftDay + " " + Strings.Numerals(LeftDay,
                   LocalizationService.GetResource("AdvantShop.Trial.TrialService.LeftDay0"),
                   LocalizationService.GetResource("AdvantShop.TrialTrialService.LeftDay1"),
                   LocalizationService.GetResource("AdvantShop.TrialTrialService.LeftDay2"),
                   LocalizationService.GetResource("AdvantShop.TrialTrialService.LeftDay5"));
            }
        }

        public static DateTime TrialPeriodTill
        {
            get
            {
                if (DateTime.Now > _lastUpdate.AddHours(1))
                {
                    try
                    {
                        var request = WebRequest.Create(string.Format(UrlTrialInfo, SettingsLic.LicKey));
                        request.Method = "GET";

                        using (var dataStream = request.GetResponse().GetResponseStream())
                        {
                            using (var reader = new StreamReader(dataStream))
                            {
                                var responseFromServer = reader.ReadToEnd();
                                if (!string.IsNullOrEmpty(responseFromServer))
                                {
                                    _trialTillCached = JsonConvert.DeserializeObject<DateTime>(responseFromServer);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Log.Error(ex);
                    }
                    _lastUpdate = DateTime.Now;
                }
                return _trialTillCached;
            }
        }

        public static string TrialCounter
        {
            get
            {
                string counter = "";

                counter = CacheManager.Get<string>("trialCounter", 60 * 24, () =>
                {
                    string data = "";
                    try
                    {
                        var request = WebRequest.Create(string.Format(UrlTrialCounter, SettingsLic.LicKey));
                        request.Method = "GET";

                        using (var dataStream = request.GetResponse().GetResponseStream())
                        {
                            using (var reader = new StreamReader(dataStream))
                            {
                                var responseFromServer = reader.ReadToEnd();
                                if (!string.IsNullOrEmpty(responseFromServer))
                                {
                                    data = JsonConvert.DeserializeObject<KeyValuePair<DateTime, string>>(responseFromServer).Value;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Log.Error(ex);
                    }
                    return data;
                });

                return counter;

            }
        }


        public static void TrackEvent(TrialEvents trialEvent, string eventParams)
        {
            if (!IsTrialEnabled && !SaasDataService.IsSaasEnabled)
                return;

            new System.Threading.Tasks.Task(() =>
            {
                try
                {
                    new WebClient().DownloadString(string.Format(UrlTrialEvents, SettingsLic.LicKey, trialEvent.ToString(), HttpUtility.UrlEncode(eventParams)));
                    UpdateAchievements();
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                }
            }).Start();

            var context = HttpContext.Current;
            if (context != null)
            {
                if (context.Items["TrialEvents"] == null)
                    context.Items["TrialEvents"] = new List<KeyValuePair<TrialEvents, string>>();

                ((List<KeyValuePair<TrialEvents, string>>)context.Items["TrialEvents"]).Add(new KeyValuePair<TrialEvents, string>(trialEvent, eventParams));
            }
        }

        public static void TrackEvent(ETrackEvent @event)
        {
            var currentCustomer = Customers.CustomerContext.CurrentCustomer;
            if (!IsTrialEnabled || currentCustomer.IsVirtual || @event == ETrackEvent.None || TrackEventIsCommitted(@event))
                return;

            var url = string.Format(UrlTrackEvents, SettingsLic.LicKey, @event.ToString());
            if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.UserHostAddress.IsNotEmpty())
                url += "&ip=" + HttpUtility.UrlEncode(HttpContext.Current.Request.UserHostAddress);

            new System.Threading.Tasks.Task(() =>
            {
                try
                {
                    new WebClient().DownloadString(url);
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                }
            }).Start();

            SetTrackEventCommitted(@event);
        }

        private static bool TrackEventIsCommitted(ETrackEvent @event)
        {
            return CommonHelper.GetCookieString("committedEvents").Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Any(x => x == @event.ToString());
        }

        private static void SetTrackEventCommitted(ETrackEvent @event)
        {
            var cookieName = "committedEvents";
            var committedEvents = CommonHelper.GetCookieString(cookieName).Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            committedEvents.Add(@event.ToString());
            CommonHelper.SetCookie(cookieName, committedEvents.AggregateString('.'), TimeSpan.FromDays(14), false);
        }

        public static void UpdateAchievements()
        {
            try
            {
                SettingsMain.Achievements = new WebClient() { Encoding = Encoding.UTF8 }
                    .DownloadString(string.Format(UrlGetAchievements, SettingsLic.LicKey))
                    .Replace("#SHOP_URL_LINK#", UrlService.GetAbsoluteLink("/"))
                    .Replace("#SHOP_URL#", SettingsMain.SiteUrl);

                SettingsMain.AchievementsPoints = new WebClient() { Encoding = Encoding.UTF8 }
                    .DownloadString(string.Format(UrlAchievementsGetPoints, SettingsLic.LicKey));

                SettingsMain.AchievementsDescription = JsonConvert.DeserializeObject<string>(new WebClient() { Encoding = Encoding.UTF8 }
                    .DownloadString(string.Format(UrlGetAchievementsDescription, SettingsLic.LicKey)).Replace("#LICENCE_KEY#", SettingsLic.LicKey));


                SettingsMain.AchievementsPopUp = JsonConvert.DeserializeObject<string>(new WebClient() { Encoding = Encoding.UTF8 }
                    .DownloadString(string.Format(UrlGetAchievementsPopUp, SettingsLic.LicKey)));
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

        }

        public static List<AchievementLevel> GetAchievements()
        {
            UpdateAchievements();

            try
            {
                return JsonConvert.DeserializeObject<List<AchievementLevel>>(SettingsMain.Achievements);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return new List<AchievementLevel>();
            }
        }

        public static Achievement GetAchievementHelp(int id)
        {

            List<AchievementLevel> achievementLevels = TrialService.GetAchievements();

            return achievementLevels.SelectMany(level => level.Achievements.Where(ach => ach.Id == id)).FirstOrDefault();
        }

        public static void SendMessage(string to, MailTemplate tpl)
        {
            try
            {
                var client = new RestClient(Url);
                var request = new RestRequest(string.Format("Trial/SendMail/{0}", SettingsLic.LicKey), Method.POST) { Timeout = 3000 };
                request.AddJsonBody(new
                {
                    to,
                    subject = tpl.Subject,
                    body = tpl.Body
                });

                client.Execute(request);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }
    }
}