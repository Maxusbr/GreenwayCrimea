//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.SQL;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;

namespace AdvantShop.Repository.Currencies
{
    public class CurrencyService
    {
        private const string CurrencyCookieName = "Currency";

        private static Currency GetCurrencyFromReader(IDataReader reader)
        {
            return new Currency()
            {
                CurrencyId = SQLDataHelper.GetInt(reader, "CurrencyID"),
                Rate = SQLDataHelper.GetFloat(reader, "CurrencyValue"),
                Iso3 = SQLDataHelper.GetString(reader, "CurrencyIso3"),
                Symbol = SQLDataHelper.GetString(reader, "Code"),
                Name = SQLDataHelper.GetString(reader, "Name"),
                IsCodeBefore = SQLDataHelper.GetBoolean(reader, "IsCodeBefore"),
                NumIso3 = SQLDataHelper.GetInt(reader, "CurrencyNumIso3"),
                RoundNumbers = SQLDataHelper.GetFloat(reader, "RoundNumbers"),
                EnablePriceRounding = SQLDataHelper.GetBoolean(reader, "EnablePriceRounding")
            };
        }

        public static Currency CurrentCurrency
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    var contextCurrency = HttpContext.Current.Items["Currency"] as Currency;
                    if (contextCurrency != null)
                        return contextCurrency;

                    var cookieCurrency = CommonHelper.GetCookie(CurrencyCookieName);
                    var isCookieCurrencyExist = cookieCurrency != null && !string.IsNullOrEmpty(cookieCurrency.Value);

                    var iso3 = isCookieCurrencyExist
                                ? cookieCurrency.Value
                                : SettingsCatalog.DefaultCurrencyIso3;

                    contextCurrency = Currency(iso3);
                    if (contextCurrency == null)
                    {
                        contextCurrency = Currency(SettingsCatalog.DefaultCurrencyIso3);
                    }

                    if (contextCurrency == null)
                    {
                        contextCurrency = GetAllCurrencies().FirstOrDefault();
                    }

                    if (contextCurrency != null && (!isCookieCurrencyExist || (contextCurrency.Iso3 != cookieCurrency.Value)))
                        CommonHelper.SetCookie(CurrencyCookieName, contextCurrency.Iso3.ToUpper());

                    HttpContext.Current.Items["Currency"] = contextCurrency;

                    return contextCurrency;
                }

                var currency = Currency(SettingsCatalog.DefaultCurrencyIso3) ?? GetAllCurrencies().FirstOrDefault();
                return currency;
            }
            set
            {
                if (HttpContext.Current != null)
                    CommonHelper.SetCookie(CurrencyCookieName, value.Iso3.ToUpper());
            }
        }

        public static Currency Currency(string iso3)
        {
            return GetAllCurrencies(true).FirstOrDefault(x => x.Iso3 == iso3);
        }

        public static List<Currency> GetAllCurrencies(bool fromCache)
        {
            if (!fromCache) return GetAllCurrencies();

            var cacheName = CacheNames.GetCurrenciesCacheObjectName();
            List<Currency> res;

            if (!CacheManager.TryGetValue(cacheName, out res))
            {
                res = GetAllCurrencies();

                if (res != null)
                    CacheManager.Insert(cacheName, res, 360);
            }

            return res;
        }

        public static List<Currency> GetAllCurrencies()
        {
            return SQLDataAccess.ExecuteReadList<Currency>("SELECT Catalog.Currency.* FROM Catalog.Currency", CommandType.Text, GetCurrencyFromReader);
        }


        public static Currency GetCurrency(int idCurrency, bool fromCache)
        {
            if (!fromCache)
                return GetCurrency(idCurrency);

            return GetAllCurrencies(true).FirstOrDefault(cur => cur.CurrencyId == idCurrency);
        }

        public static Currency GetCurrency(int idCurrency)
        {
            return SQLDataAccess.ExecuteReadOne<Currency>("SELECT Catalog.Currency.* FROM Catalog.Currency where CurrencyID = @id", CommandType.Text, GetCurrencyFromReader, new SqlParameter("@id", idCurrency));
        }

        public static Currency GetCurrencyByIso3(string iso3)
        {
            return SQLDataAccess.ExecuteReadOne<Currency>("SELECT Catalog.Currency.* FROM Catalog.Currency where CurrencyIso3 = @iso3", CommandType.Text, GetCurrencyFromReader, new SqlParameter("@iso3", iso3));
        }

        public static void DeleteCurrency(int idCurrency)
        {
            var currentCurrency = GetCurrency(idCurrency);
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Catalog].[Currency] where CurrencyID = @id", CommandType.Text, new SqlParameter("@id", idCurrency));
            if (currentCurrency != null)
                ExportImport.ExportFeedSettingsProvider.UpdateCurrencyToDefault(currentCurrency.Iso3);

            CacheManager.RemoveByPattern(CacheNames.GetCurrenciesCacheObjectName());
        }

        public static void UpdateCurrency(Currency currency)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Update [Catalog].[Currency] " +
                "Set Name=@name, Code=@code, CurrencyValue=@value, CurrencyIso3=@ISO3, CurrencyNumIso3=@CurrencyNumIso3, IsCodeBefore=@isCodeBefore, RoundNumbers=@RoundNumbers, EnablePriceRounding=@EnablePriceRounding " +
                "Where CurrencyID = @id",
                CommandType.Text,
                new SqlParameter("@id", currency.CurrencyId),
                new SqlParameter("@name", currency.Name),
                new SqlParameter("@code", currency.Symbol),
                new SqlParameter("@value", currency.Rate),
                new SqlParameter("@ISO3", currency.Iso3),
                new SqlParameter("@CurrencyNumIso3", currency.NumIso3 != 0 ? currency.NumIso3 : new Random().Next(1, 999)),
                new SqlParameter("@isCodeBefore", currency.IsCodeBefore),
                new SqlParameter("@RoundNumbers", currency.RoundNumbers),
                new SqlParameter("@EnablePriceRounding", currency.EnablePriceRounding));

            CacheManager.RemoveByPattern(CacheNames.GetCurrenciesCacheObjectName());
        }

        public static void InsertCurrency(Currency currency)
        {
            currency.CurrencyId = SQLDataAccess.ExecuteScalar<int>(
                 "INSERT INTO [Catalog].[Currency] (Name, Code, CurrencyValue, CurrencyIso3, CurrencyNumIso3, IsCodeBefore, RoundNumbers, EnablePriceRounding) " +
                 "VALUES (@Name, @Code, @CurrencyValue, @CurrencyIso3, @CurrencyNumIso3, @IsCodeBefore, @RoundNumbers, @EnablePriceRounding);" +
                 "SELECT scope_identity();",
                 CommandType.Text,
                 new SqlParameter("@Name", currency.Name),
                 new SqlParameter("@Code", currency.Symbol),
                 new SqlParameter("@CurrencyValue", currency.Rate),
                 new SqlParameter("@CurrencyIso3", currency.Iso3),
                 new SqlParameter("@CurrencyNumIso3", currency.NumIso3 != 0 ? (object)currency.NumIso3 : DBNull.Value),
                 new SqlParameter("@isCodeBefore", currency.IsCodeBefore),
                 new SqlParameter("@RoundNumbers", currency.RoundNumbers),
                 new SqlParameter("@EnablePriceRounding", currency.EnablePriceRounding));

            CacheManager.RemoveByPattern(CacheNames.GetCurrenciesCacheObjectName());
        }

        public static void UpdateCurrenciesFromCentralBank()
        {
            try
            {
                var request = WebRequest.Create("http://www.cbr.ru/scripts/XML_daily.asp?date_req=" + DateTime.Now.ToString("dd/MM/yyyy")) as HttpWebRequest;
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";

                var responseContent = "";
                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                            using (var reader = new StreamReader(stream))
                            {
                                responseContent = reader.ReadToEnd();
                            }
                    }
                }

                if (responseContent.IsNullOrEmpty())
                    return;

                var doc = XDocument.Parse(responseContent);

                var rub = GetCurrencyByIso3("RUB");
                var rubScale = rub != null ? rub.Rate : 1;

                var currencies = SQLDataAccess.ExecuteTable("SELECT CurrencyIso3,CurrencyValue FROM Catalog.Currency", CommandType.Text);

                for (int i = 0; i < currencies.Rows.Count; i++)
                {
                    foreach (var el in doc.Root.Elements("Valute"))
                    {
                        if (currencies.Rows[i]["CurrencyIso3"].ToString().ToLower() != el.Element("CharCode").Value.ToLower()) continue;

                        SQLDataAccess.ExecuteNonQuery(
                            "UPDATE Catalog.Currency SET CurrencyValue=@CurrencyValue, CurrencyNumIso3=@CurrencyNumIso3 WHERE CurrencyIso3=@CurrencyIso3",
                            CommandType.Text,
                            new SqlParameter("@CurrencyValue",
                                el.Element("Value").Value.TryParseFloat() / el.Element("Nominal").Value.TryParseFloat() * rubScale),
                            new SqlParameter("@CurrencyIso3", el.Element("CharCode").Value),
                            new SqlParameter("@CurrencyNumIso3", el.Element("NumCode").Value));
                    }
                }

                CacheManager.RemoveByPattern(CacheNames.GetCurrenciesCacheObjectName());
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }

        public static float ConvertCurrency(float sum, float newCurrencyValue, float oldCurrencyValue)
        {
            return sum * oldCurrencyValue / newCurrencyValue;
        }
    }
}

