using System;
using System.Collections.Generic;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Customers;
using System.Linq;
using AdvantShop.Web.Admin.ViewModels.Home;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.FilePath;
using AdvantShop.Core.UrlRewriter;

namespace AdvantShop.Web.Admin.Handlers.Home
{
    public class GetBirthdayDasboard
    {
        public GetBirthdayDasboard()
        {
        }

        public BirthdayDashboardViewModel Execute()
        {
            var customers = CustomerService.GetCustomerBirthdayInTheNextWeek();

            var model = new BirthdayDashboardViewModel() { Birthday = new List<BirtdayItem>() };

            foreach(var customer in customers)
            {
                var item = new BirtdayItem();
                item.SrcImage = customer.Avatar.IsNotEmpty()
                    ? FoldersHelper.GetPath(FolderType.Avatar, customer.Avatar, false)
                    : UrlService.GetAdminStaticUrl() + "images/no-avatar.jpg";
                item.CustomerId = customer.Id.ToString();
                item.Name = customer.LastName + " " + customer.FirstName + " ";
                var descr = customer.BirthDay.HasValue && DateTime.Now.Day == customer.BirthDay.Value.Day ?
                    LocalizationService.GetResource("Admin.Home.BirthdayDashboard.Description.Today") + " " : customer.BirthDay.Value.Day.ToString() + " " + GetMonth(customer.BirthDay.Value.Month);
                if(DateTime.Now.Day < customer.BirthDay.Value.Day)
                {
                    var temp = customer.BirthDay.Value.Day - DateTime.Now.Day;
                    descr += string.Format(LocalizationService.GetResource("Admin.Home.BirthdayDashboard.Description.Days"), temp + " " + GetVariantDay(temp));
                    item.Sorting = temp;
                }
                else if(DateTime.Now.Day != customer.BirthDay.Value.Day)
                {
                    var temp = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                    temp = temp + customer.BirthDay.Value.Day - DateTime.Now.Day;
                    descr += string.Format(LocalizationService.GetResource("Admin.Home.BirthdayDashboard.Description.Days"), temp + " " + GetVariantDay(temp));
                    item.Sorting = temp;
                } 
                descr += " " + LocalizationService.GetResource("Admin.Home.BirthdayDashboard.Description.HappyBirthday");
                item.Description = descr;
                model.Birthday.Add(item);
            }
            model.Birthday = model.Birthday.OrderBy(x => x.Sorting).ToList();
            return model;
        }

        private string GetVariantDay(int day)
        {
            if (day == 1)
            {
                return LocalizationService.GetResource("Core.Numerals.Days.One");
            } else if (day < 5)
            {
                return LocalizationService.GetResource("Core.Numerals.Days.Two");
            }
            else
            {
                return LocalizationService.GetResource("Core.Numerals.Days.Five");
            }
        }

        private string GetMonth(int Month)
        {
            var result = string.Empty;
            switch (Month)
            {
                case 1:
                    result = "января";
                    break;
                case 2:
                    result = "февраля";
                    break;
                case 3:
                    result = "марта";
                    break;
                case 4:
                    result = "апреля";
                    break;
                case 5:
                    result = "мая";
                    break;
                case 6:
                    result = "июня";
                    break;
                case 7:
                    result = "июля";
                    break;
                case 8:
                    result = "августа";
                    break;
                case 9:
                    result = "сентября";
                    break;
                case 10:
                    result = "октября";
                    break;
                case 11:
                    result = "ноября";
                    break;
                case 12:
                    result = "декабря";
                    break;
            }
            return result;
        }
    }
}
