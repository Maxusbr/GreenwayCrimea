using System;
using System.Collections.Generic;
using AdvantShop.Configuration;
using Newtonsoft.Json;

namespace AdvantShop.Core.Services.IPTelephony.CallBack
{
    public class WorkTime
    {
        public bool Enabled { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }

        public WorkTime(bool enabled, int hoursFrom, int hoursTo)
        {
            Enabled = enabled;
            From = new TimeSpan(hoursFrom, 0, 0);
            To = new TimeSpan(hoursTo, 0, 0);
        }
    }

    public class WorkSchedule : Dictionary<DayOfWeek, WorkTime>
    {
        public static WorkSchedule Schedule
        {
            get
            {
                var schedule =
                    JsonConvert.DeserializeObject<WorkSchedule>(SettingsTelephony.CallBackWorkSchedule ?? string.Empty)
                    ?? new WorkSchedule();
                foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
                {
                    if (!schedule.ContainsKey(day))
                        schedule.Add(day, new WorkTime(false, 8, 20));
                }
                return schedule;
            }
            set
            {
                SettingsTelephony.CallBackWorkSchedule = JsonConvert.SerializeObject(value);
            }
        }

        public WorkTime Get(DayOfWeek key)
        {
            return this.ContainsKey(key) ? this[key] : new WorkTime(false, 8, 20);
        }

        public void Set(DayOfWeek key, WorkTime value)
        {
            if (this.ContainsKey(key))
                this[key] = value;
            else
                this.Add(key, value);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(Schedule);
        }
    }
}
