//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Services.Localization;
using System;

namespace AdvantShop.Core.Scheduler
{
    public enum TimeIntervalType
    {
        [Localize("Core.Scheduler.TimeIntervalType.None")]
        None,
        [Localize("Core.Scheduler.TimeIntervalType.Days")]
        Days,
        [Localize("Core.Scheduler.TimeIntervalType.Hours")]
        Hours,
        [Localize("Core.Scheduler.TimeIntervalType.Minutes")]
        Minutes
    }

    public class TimeInterval
    {
        public TimeIntervalType IntervalType { get; set; }
        public int Interval { get; set; }

        public string Numeral()
        {
            return Numeral(string.Empty);
        }

        public string Numeral(string emptyText)
        {
            switch (IntervalType)
            {
                case TimeIntervalType.Minutes:
                    return Strings.Numerals(Interval, emptyText,
                        LocalizationService.GetResource("Core.Numerals.Minutes.One"),
                        LocalizationService.GetResource("Core.Numerals.Minutes.Two"),
                        LocalizationService.GetResource("Core.Numerals.Minutes.Five"));
                case TimeIntervalType.Hours:
                    return Strings.Numerals(Interval, emptyText,
                        LocalizationService.GetResource("Core.Numerals.Hours.One"),
                        LocalizationService.GetResource("Core.Numerals.Hours.Two"),
                        LocalizationService.GetResource("Core.Numerals.Hours.Five"));
                case TimeIntervalType.Days:
                    return Strings.Numerals(Interval, emptyText,
                        LocalizationService.GetResource("Core.Numerals.Days.One"),
                        LocalizationService.GetResource("Core.Numerals.Days.Two"),
                        LocalizationService.GetResource("Core.Numerals.Days.Five"));
                default:
                    return string.Empty;
            }
        }

        public DateTime GetDateTime(DateTime from)
        {
            switch (IntervalType)
            {
                case TimeIntervalType.Minutes:
                    return from.AddMinutes(Interval);
                case TimeIntervalType.Hours:
                    return from.AddHours(Interval);
                case TimeIntervalType.Days:
                    return from.AddDays(Interval);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}