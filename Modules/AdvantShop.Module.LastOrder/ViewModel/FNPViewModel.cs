using AdvantShop.Module.LastOrder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.LastOrder.ViewModel
{
    public class FNPViewModel
    {
        public string Time { get; set; }
        public string Name { get; set; }
        public string City { get; set; }

        public FNPViewModel(FNPModel model)
        {
            Time = GetTime(model.FakeDateTime);
            Name = model.Name;
            City = model.City;
        }

        private string GetTime(DateTime fakeTime)
        {
            TimeSpan time = DateTime.Now - fakeTime;

            var result = "";

            if (time.TotalSeconds >= 60)
            {
                if (time.TotalMinutes >= 60)
                {
                    if (time.TotalHours >= 24)
                    {
                        result = string.Format("{0} д.", Math.Floor(time.TotalDays));
                    }
                    else
                    {
                        result = string.Format("{0} ч.", Math.Floor(time.TotalHours));
                    }
                }
                else
                {
                    result = string.Format("{0} мин.", Math.Floor(time.TotalMinutes));
                }
            }
            else
            {
                result = string.Format("{0} сек.", Math.Floor(time.TotalSeconds));
            }

            return result;
        }

    }
}
