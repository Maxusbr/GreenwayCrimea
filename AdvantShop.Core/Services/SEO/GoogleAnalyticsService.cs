//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Saas;
using AdvantShop.Security.OAuth;
using AdvantShop.Trial;
using Google.GData.Analytics;
using Google.GData.Client;
using Newtonsoft.Json;

namespace AdvantShop.SEO
{
    public static class GoogleAnalyticsService
    {
        private const string DataFeedUrl = "https://www.google.com/analytics/feeds/data";

        public static DateTime GetLastModifiedDate()
        {
            return SettingsSEO.GoogleAnalyticsCachedDate;
        }


        public static Dictionary<DateTime, GoogleAnalyticsData> GetData()
        {
            if (!SettingsSEO.GoogleAnalyticsApiEnabled)
                return null;
            
            if (SettingsSEO.GoogleAnalyticsCachedDate == DateTime.MinValue ||
                SettingsSEO.GoogleAnalyticsCachedDate.AddMinutes(30) <= DateTime.Now)
            {
                var service = new AnalyticsService("WebApp");
                service.RequestFactory = GetAuthRequestFactory();
                
                if (service.RequestFactory == null)
                    return null;

                if ((TrialService.IsTrialEnabled || SaasDataService.IsSaasEnabled) && GetTotalVisitors(service) > 100)
                {
                    TrialService.TrackEvent(TrialEvents.GetFirstThouthandVisitors, string.Empty);
                }

                var data = GetVisitsData(service);
                if (data != null)
                    return data;
            }

            // if something is wrong return cached data
            if (SettingsSEO.GoogleAnalyticsCachedData.IsNullOrEmpty())
                return null;

            try
            {
                var cachedData = JsonConvert.DeserializeObject<Dictionary<DateTime, GoogleAnalyticsData>>(SettingsSEO.GoogleAnalyticsCachedData);
                return cachedData;
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return null; // if there is no any data
            }
        }

        private static int GetTotalVisitors(AnalyticsService service)
        {
            var totalVisitorsQuerry = new DataQuery(DataFeedUrl)
            {
                Ids = "ga:" + SettingsSEO.GoogleAnalyticsAccountID,
                Metrics = "ga:visitors",
                Dimensions = "ga:year",
                GAStartDate = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd"),
                GAEndDate = DateTime.Now.ToString("yyyy-MM-dd"),
                StartIndex = 1
            };

            try
            {
                var totalVisitors = service.Query(totalVisitorsQuerry);
                return totalVisitors.Aggregates.Metrics[0].Value.TryParseInt();
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
            return 0;
        }
        
        private static Dictionary<DateTime, GoogleAnalyticsData> GetVisitsData(AnalyticsService service)
        {
            var data = new Dictionary<DateTime, GoogleAnalyticsData>();

            var query = new DataQuery(DataFeedUrl)
            {
                Ids = "ga:" + SettingsSEO.GoogleAnalyticsAccountID,
                Metrics = "ga:visitors,ga:visits,ga:pageviews",
                Dimensions = "ga:date",
                GAStartDate = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"),
                GAEndDate = DateTime.Now.ToString("yyyy-MM-dd"),
                StartIndex = 1
            };

            // try to get actual data from google

            try
            {
                var dataFeedVisits = service.Query(query);
                if (dataFeedVisits != null)
                {
                    foreach (DataEntry entry in dataFeedVisits.Entries)
                    {
                        data.Add(DateTime.ParseExact(entry.Title.Text.Split('=').LastOrDefault(), "yyyyMMdd", CultureInfo.InvariantCulture),
                                 new GoogleAnalyticsData
                                 {
                                     Visitors = entry.Metrics[0].Value.TryParseInt(),
                                     Visits = entry.Metrics[1].Value.TryParseInt(),
                                     PageViews = entry.Metrics[2].Value.TryParseInt(),
                                 });
                    }
                }

                // saving and return data if all is ok
                SettingsSEO.GoogleAnalyticsCachedData = JsonConvert.SerializeObject(data);
                SettingsSEO.GoogleAnalyticsCachedDate = DateTime.Now;
                return data;
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
            return null;
        }

        public static GOAuth2RequestFactory GetAuthRequestFactory()
        {
            if (string.IsNullOrWhiteSpace(SettingsOAuth.GoogleAnalyticsAccessToken))
                return null;

            OAuthToken token = null;
            try
            {
                token = JsonConvert.DeserializeObject<OAuthToken>(SettingsOAuth.GoogleAnalyticsAccessToken);

                if (token == null)
                    return null;

                var parameters = new OAuth2Parameters
                {
                    ClientId = SettingsOAuth.GoogleClientId,
                    ClientSecret = SettingsOAuth.GoogleClientSecret,
                    Scope = "https://www.googleapis.com/auth/analytics.readonly https://www.googleapis.com/auth/userinfo.email",
                    AccessToken = token.AccessToken,
                    RefreshToken = token.RefreshToken ?? "",
                    AccessType = "offline",
                    TokenType = token.TokenType ?? "refresh",
                    RedirectUri = StringHelper.MakeASCIIUrl(UrlService.GetUrl("login"))
                };

                var authUrl = OAuthUtil.CreateOAuth2AuthorizationUrl(parameters);
                return new GOAuth2RequestFactory(null, "WebApp", parameters);
            }
            catch(Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return null;
        }

        public static GaVortexStatistic GetVortexStatistics(DateTime dateFrom, DateTime dateTo)
        {
            var service = new AnalyticsService("WebApp");
            service.RequestFactory = GetAuthRequestFactory();

            if (service.RequestFactory == null)
                return null;

            return new GaVortexStatistic()
            {
                TotalUsersCount = GetTotalUsersCount(service, dateFrom, dateTo),
                AdvantShopEvents = GetAdvantshopEvents(service, dateFrom, dateTo),
                Sources = GetOrderSources(service, dateFrom, dateTo)
            };
        }
        
        public static Dictionary<string, int> GetAdvantshopEvents(AnalyticsService service, DateTime dateFrom, DateTime dateTo)
        {
            var query = new DataQuery(DataFeedUrl)
            {
                Ids = "ga:" + SettingsSEO.GoogleAnalyticsAccountID,
                Metrics = "ga:users",
                Dimensions = "ga:eventAction",
                Filters = "ga:eventCategory==Advantshop_events",
                GAStartDate = dateFrom.ToString("yyyy-MM-dd"),
                GAEndDate = dateTo.ToString("yyyy-MM-dd"),
                StartIndex = 1
            };

            var data = new Dictionary<string, int>();

            try
            {
                var dataFeed = service.Query(query);
                if (dataFeed != null)
                {
                    foreach (DataEntry entry in dataFeed.Entries)
                    {
                        data.Add(entry.Dimensions[0].Value, entry.Metrics[0].Value.TryParseInt());
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return data;
        }

        public static int GetTotalUsersCount(AnalyticsService service, DateTime dateFrom, DateTime dateTo)
        {
            var query = new DataQuery(DataFeedUrl)
            {
                Ids = "ga:" + SettingsSEO.GoogleAnalyticsAccountID,
                Metrics = "ga:users",
                GAStartDate = dateFrom.ToString("yyyy-MM-dd"),
                GAEndDate = dateTo.ToString("yyyy-MM-dd"),
            };

            var usersCount = 0;

            try
            {
                var dataFeed = service.Query(query);
                if (dataFeed != null)
                {
                    foreach (DataEntry entry in dataFeed.Entries)
                    {
                        usersCount = entry.Metrics[0].Value.TryParseInt();
                    }
                }
            }
            catch (Exception ex)
            {
                var e = ex as WebException;
                if (e != null)
                {
                    var result = "";
                    using (var eResponse = e.Response)
                    {
                        using (var eStream = eResponse.GetResponseStream())
                        {
                            if (eStream != null)
                                using (var reader = new StreamReader(eStream))
                                {
                                    result = reader.ReadToEnd();
                                }
                        }
                    }
                    Debug.Log.Error(result);
                }

                Debug.Log.Error(ex);
            }

            return usersCount;
        }

        private static List<GaOrderSourcesStatistic> GetOrderSources(AnalyticsService service, DateTime dateFrom, DateTime dateTo)
        {
            var query = new DataQuery(DataFeedUrl)
            {
                Ids = "ga:" + SettingsSEO.GoogleAnalyticsAccountID,
                Metrics = "ga:transactions",
                Dimensions = "ga:source,ga:medium",
                Segment = "dynamic::ga:transactions>0",
                Sort = "-ga:transactions",
                GAStartDate = dateFrom.ToString("yyyy-MM-dd"),
                GAEndDate = dateTo.ToString("yyyy-MM-dd"),
            };

            var list = new List<GaOrderSourcesStatistic>();

            try
            {
                var dataFeed = service.Query(query);
                if (dataFeed != null)
                {
                    foreach (DataEntry entry in dataFeed.Entries)
                    {
                        list.Add(new GaOrderSourcesStatistic()
                        {
                            Source = entry.Dimensions[0].Value,
                            Medium = entry.Dimensions[1].Value,
                            Transactions = entry.Metrics[0].Value
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return list;
        }

        public static GaOrderSourceData GetOrderSource(Order order)
        {
            var service = new AnalyticsService("WebApp");
            service.RequestFactory = GetAuthRequestFactory();

            if (service.RequestFactory == null)
                return null;

            var querry = new DataQuery(DataFeedUrl)
            {
                Ids = "ga:" + SettingsSEO.GoogleAnalyticsAccountID,
                Metrics = "ga:transactions,ga:transactionRevenue",
                Dimensions = "ga:referralPath,ga:campaign,ga:source,ga:medium",
                Filters = "ga:transactionId==" + order.OrderID,
                GAStartDate = order.OrderDate.AddDays(-10).ToString("yyyy-MM-dd"),
                GAEndDate = order.OrderDate.AddDays(10).ToString("yyyy-MM-dd"),
                StartIndex = 1
            };

            try
            {
                var dataFeed = service.Query(querry);
                if (dataFeed != null)
                {
                    foreach (DataEntry entry in dataFeed.Entries)
                    {
                        var data = new GaOrderSourceData()
                        {
                            ReferalPath = entry.Dimensions[0].Value,
                            Campaign = entry.Dimensions[1].Value,
                            Source = entry.Dimensions[1].Value,
                            Medium = entry.Dimensions[1].Value,
                            Revenue = entry.Metrics[1].Value
                        };
                        return data;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
            return null;
        }
    }
}