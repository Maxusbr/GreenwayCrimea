using System;
using System.Collections.Generic;
using AdvantShop.Core.Services.Loging.Events;
using AdvantShop.Core.Services.SEO;

namespace AdvantShop.Admin.UserControls.Customer
{
    public partial class ActivitiLog : System.Web.UI.UserControl
    {
        public List<Event> ActivityEvents;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected string RenderEventType(ePageType evenType)
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

        protected string RenderTime(DateTime createOn)
        {
            var current = DateTime.Now;

            var temp = new DateTime(current.Year, current.Month, current.Day)
                     - new DateTime(createOn.Year, createOn.Month, createOn.Day);
            if (temp.Days > 0)
                return temp.Days + " дней назад";

            temp = new DateTime(current.Year, current.Month, current.Day, current.Hour, 0, 0)
                 - new DateTime(createOn.Year, createOn.Month, createOn.Day, createOn.Hour, 0, 0);
            if (temp.Hours > 0)
                return temp.Hours + " часов назад";

            temp = new DateTime(current.Year, current.Month, current.Day, current.Hour, current.Minute, 0)
                 - new DateTime(createOn.Year, createOn.Month, createOn.Day, createOn.Hour, createOn.Minute, 0);

            if (temp.Minutes > 0)
                return temp.Minutes + " минут назад";

            temp = current - createOn;
            return temp.Seconds + " секунд назад";
        }

        protected string RenderEventLink(Event item)
        {
            switch (item.EvenType)
            {
                case ePageType.cart:
                case ePageType.order:
                case ePageType.purchase:
                    return string.Empty;
                case ePageType.home:
                    return string.Format("<a href=\"{0}\">{1}</a>", item.Url, "главной страницы" );
                default:
                    return string.Format("<a href=\"{0}\">{1}</a>", item.Url, string.IsNullOrWhiteSpace(item.Name) ? "Перейти" : item.Name);
            }
        }
    }
}