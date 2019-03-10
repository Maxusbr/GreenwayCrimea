using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using AdvantShop.Statistic;
using CsvHelper;
using CsvHelper.Configuration;

namespace AdvantShop.Admin.Handlers.Statistics
{
    public class AbcxyzAnalysisHandler
    {
        #region private

        private readonly DateTime _dateFrom;
        private readonly DateTime _dateTo;
        private string _groupBy;

        #endregion

        public AbcxyzAnalysisHandler(DateTime dateFrom, DateTime dateTo)
        {
            _dateFrom = dateFrom;
            _dateTo = dateTo;
        }

        public object GetData()
        {
            var items = GetDataItems();
            if (items == null || items.Count == 0)
                return null;

            //ToCsv(items);

            var totalCount = items.Count;
            var ax = items.Count(x => x.Abc == "A" && x.Xyz == "X");
            var axPercent = ((float)ax / totalCount * 100).ToString("F2");
            var bx = items.Count(x => x.Abc == "B" && x.Xyz == "X");
            var bxPercent = ((float)bx / totalCount * 100).ToString("F2");
            var cx = items.Count(x => x.Abc == "C" && x.Xyz == "X");
            var cxPercent = ((float)cx / totalCount * 100).ToString("F2");

            var ay = items.Count(x => x.Abc == "A" && x.Xyz == "Y");
            var ayPercent = ((float)ay / totalCount * 100).ToString("F2");
            var by = items.Count(x => x.Abc == "B" && x.Xyz == "Y");
            var byPercent = ((float)by / totalCount * 100).ToString("F2");
            var cy = items.Count(x => x.Abc == "C" && x.Xyz == "Y");
            var cyPercent = ((float)cy / totalCount * 100).ToString("F2");

            var az = items.Count(x => x.Abc == "A" && x.Xyz == "Z");
            var azPercent = ((float)az / totalCount * 100).ToString("F2");
            var bz = items.Count(x => x.Abc == "B" && x.Xyz == "Z");
            var bzPercent = ((float)bz / totalCount * 100).ToString("F2");
            var cz = items.Count(x => x.Abc == "C" && x.Xyz == "Z");
            var czPercent = ((float)cz / totalCount * 100).ToString("F2");

            return new
            {
                ax = Numerals(ax),
                axPercent,
                bx = Numerals(bx),
                bxPercent,
                cx = Numerals(cx),
                cxPercent,

                ay = Numerals(ay),
                ayPercent,
                by = Numerals(by),
                byPercent,
                cy = Numerals(cy),
                cyPercent,

                az = Numerals(az),
                azPercent,
                bz = Numerals(bz),
                bzPercent,
                cz = Numerals(cz),
                czPercent
            };
        }

        private string Numerals(float count)
        {
            return count + " " + Strings.Numerals(count, "товаров", "товар", "товара", "товаров");
        }

        private double StandardDeviation(List<AbcXyzAnalysisDateTime> values)
        {
            var avg = values.Average(x => x.Sum);
            return Math.Sqrt(values.Average(v => Math.Pow(v.Sum - avg, 2)));
        }

        public static double StandardDeviation(List<float> valueList)
        {
            double M = 0.0;
            double S = 0.0;
            int k = 1;
            foreach (float value in valueList)
            {
                double tmpM = M;
                M += (value - tmpM) / k;
                S += (value - tmpM) * (value - M);
                k++;
            }
            return Math.Sqrt(S / (k - 2));
        }

        public List<AbcXyzAnalysis> GetDataItems()
        {
            var resultList = new List<AbcXyzAnalysis>();

            var dates = new List<AbcXyzAnalysisDateTime>();
            var date = _dateFrom;

            _groupBy = date.AddMonths(1).AddDays(5) > _dateTo ? "wk" : "mm";

            while (date < _dateTo)
            {
                var nextdate = _groupBy == "wk" ? date.AddDays(7) : date.AddMonths(1);
                if (nextdate > _dateTo)
                    nextdate = _dateTo;

                dates.Add(new AbcXyzAnalysisDateTime() {From = date, To = nextdate});
                date = nextdate;
            }

            foreach (var dateTime in dates)
            {
                var items = OrderStatisticsService.GetAbcXyzOrderItems(dateTime.From, dateTime.To);

                if (items == null)
                    continue;

                foreach (var item in items)
                {
                    var listItem = resultList.Find(x => x.ArtNo == item.ArtNo);

                    if (listItem == null)
                    {
                        var newItem = new AbcXyzAnalysis()
                        {
                            ArtNo = item.ArtNo,
                            PriceSumList = new List<AbcXyzAnalysisDateTime>()
                        };

                        foreach (var dt in dates)
                        {
                            newItem.PriceSumList.Add(new AbcXyzAnalysisDateTime()
                            {
                                From = dt.From, 
                                To = dt.To,
                                Sum = dt.From == dateTime.From && dt.To == dateTime.To ? item.PriceSum : 0
                            });
                        }

                        resultList.Add(newItem);
                    }
                    else
                    {
                        var r = listItem.PriceSumList.Find(x => x.From == dateTime.From && x.To == dateTime.To);
                        if (r != null)
                            r.Sum = item.PriceSum;
                    }
                }
            }

            var totalSummary = 0f;

            foreach (var resultItem in resultList)
            {
                resultItem.PriceSummary = resultItem.PriceSumList.Sum(x => x.Sum);
                totalSummary += resultItem.PriceSummary;
            }

            foreach (var resultItem in resultList)
            {
                resultItem.Percent = resultItem.PriceSummary/totalSummary*100;

                var std = StandardDeviation(resultItem.PriceSumList.Select(x => x.Sum).ToList());
                var avg = resultItem.PriceSumList.Average(x => x.Sum);

                resultItem.StdDev = std/avg;
            }

            var cumulativeTotal = 0f;

            foreach (var resultItem in resultList.OrderByDescending(x => x.PriceSummary))
            {
                cumulativeTotal += resultItem.Percent;

                resultItem.Abc = cumulativeTotal > 80
                    ? "C"
                    : (cumulativeTotal > 50 ? "B" : "A");

                resultItem.Xyz = resultItem.StdDev < 0.1
                    ? "X"
                    : (resultItem.StdDev < 0.25 ? "Y" : "Z");
            }

            return resultList;
        }

        private void ToCsv(List<AbcXyzAnalysis> list)
        {
            if (list == null)
                return;

            try
            {
                using (
                    var writer =
                        new CsvWriter(
                            new StreamWriter(HttpContext.Current.Server.MapPath("~/abc.csv"), false, Encoding.UTF8),
                            new CsvConfiguration {Delimiter = ";"}))
                {
                    writer.WriteField("ArtNo");

                    foreach (var date in list[0].PriceSumList)
                        writer.WriteField(date.From + " - " + date.To);

                    writer.WriteField("PriceSummary");
                    writer.WriteField("Percent");
                    writer.WriteField("Abc");
                    writer.WriteField("StdDev");
                    writer.WriteField("Xyz");

                    writer.NextRecord();

                    foreach (var item in list)
                    {
                        writer.WriteField(item.ArtNo);

                        foreach (var date in item.PriceSumList)
                            writer.WriteField(date.Sum);

                        writer.WriteField(item.PriceSummary);
                        writer.WriteField(item.Percent);
                        writer.WriteField(item.Abc);

                        writer.WriteField(item.StdDev);
                        writer.WriteField(item.Xyz);

                        writer.NextRecord();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }
    }
}