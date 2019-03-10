using System;
using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Loging.Events;
using AdvantShop.Core.Services.SEO;
using AdvantShop.Web.Admin.Models.Activity;
using AdvantShop.Customers;
using AdvantShop.Helpers;

namespace AdvantShop.Web.Admin.Handlers.Activity
{
    public class GetActions
    {
        private readonly Guid _customerId;

        public GetActions(Guid customerId)
        {
            _customerId = customerId;
        }

        public ActivityActionsModel Execute()
        {
            var model = new ActivityActionsModel() {DataItems = new List<ActivityActionsItemModel>()};

            var items =
                _customerId != Guid.Empty
                    ? CustomerService.GetEvent(_customerId)
                    : null;

            if (items == null)
                return model;

            foreach (var item in items)
            {
                var actionItem = new ActivityActionsItemModel()
                {
                    EventType = RenderEventType(item.EvenType),
                    CreateTime = item.CreateOn.ToString("s"),
                    CreateTimeTitle = item.CreateOn.ToString("g"),
                    Time = RenderTime(item.CreateOn),
                    Link = RenderEventLink(item)
                };

                model.DataItems.Add(actionItem);
            }

            return model;
        }


        private string RenderEventType(ePageType evenType)
        {
            switch (evenType)
            {
                case ePageType.cart:
                    return "Просмотр корзины";
                case ePageType.product:
                    return "Просмотр товара";
                case ePageType.category:
                    return "Просмотр категории";
                case ePageType.brand:
                    return "Просмотр производителя";
                case ePageType.home:
                    return "Просмотр ";
                case ePageType.other:
                    return "Просмотр страницы";
                case ePageType.addToCart:
                    return "Добавление в корзину";
                case ePageType.addToCompare:
                    return "Добавление в сравнение";
                case ePageType.addResponse:
                    return "Добавление отзыва";
                case ePageType.addToWishlist:
                    return "Добавление в желаемые";
                case ePageType.searchresults:
                    return "Поиск";
                case ePageType.buyOneClickForm:
                    return "Покупка в 1 клик";
                case ePageType.purchase:
                    return "Успешная продажа";
                case ePageType.order:
                    return "Переход на страницу оформления";
            }
            return "не указано сообщение для типа " + evenType;
        }

        private string RenderTime(DateTime createOn)
        {
            var current = DateTime.Now;

            var temp = new DateTime(current.Year, current.Month, current.Day)
                     - new DateTime(createOn.Year, createOn.Month, createOn.Day);
            if (temp.Days > 0)
                return string.Format("{0} {1} назад", temp.Days, Strings.Numerals(temp.Days, "дней", "день", "дня", "дней"));

            temp = new DateTime(current.Year, current.Month, current.Day, current.Hour, 0, 0)
                 - new DateTime(createOn.Year, createOn.Month, createOn.Day, createOn.Hour, 0, 0);
            if (temp.Hours > 0)
                return string.Format("{0} {1} назад", temp.Hours, Strings.Numerals(temp.Hours, "часов", "час", "часа", "часов"));

            temp = new DateTime(current.Year, current.Month, current.Day, current.Hour, current.Minute, 0)
                 - new DateTime(createOn.Year, createOn.Month, createOn.Day, createOn.Hour, createOn.Minute, 0);

            if (temp.Minutes > 0)
                return string.Format("{0} {1} назад", temp.Minutes, Strings.Numerals(temp.Minutes, "минут", "минуту", "минуты", "минут"));

            temp = current - createOn;
            return string.Format("{0} {1} назад", temp.Days, Strings.Numerals(temp.Seconds, "секунд", "секунда", "секунды", "секунд"));
        }

        private string RenderEventLink(Event item)
        {
            switch (item.EvenType)
            {
                case ePageType.cart:
                case ePageType.order:
                case ePageType.purchase:
                    return string.Empty;
                case ePageType.home:
                    return string.Format("<a href=\"{0}\">{1}</a>", item.Url, "главной страницы");
                default:
                    return string.Format("<a href=\"{0}\">{1}</a>", item.Url, string.IsNullOrWhiteSpace(item.Name) ? "Перейти" : item.Name);
            }
        }
    }
}
