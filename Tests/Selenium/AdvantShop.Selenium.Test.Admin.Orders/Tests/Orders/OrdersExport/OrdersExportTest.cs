using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Threading;

namespace AdvantShop.SeleniumTest.Admin.Orders.OrderStatus
{
    [TestFixture]
    public class OrdersExportTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.Orders);
            InitializeService.LoadData(

             "Data\\Admin\\Orders\\OrdersExport\\Catalog.Product.csv",
           "Data\\Admin\\Orders\\OrdersExport\\Catalog.Offer.csv",
           "Data\\Admin\\Orders\\OrdersExport\\Catalog.Category.csv",
          "Data\\Admin\\Orders\\OrdersExport\\Catalog.ProductCategories.csv",
           "Data\\Admin\\Orders\\OrdersExport\\Customers.CustomerGroup.csv",
           "data\\Admin\\Orders\\OrdersExport\\Customers.Country.csv",
            "data\\Admin\\Orders\\OrdersExport\\Customers.Region.csv",
            "data\\Admin\\Orders\\OrdersExport\\Customers.City.csv",
            "data\\Admin\\Orders\\OrdersExport\\Customers.Customer.csv",
            "data\\Admin\\Orders\\OrdersExport\\Customers.Contact.csv",
            "data\\Admin\\Orders\\OrdersExport\\[Order].OrderCustomer.csv",
            "Data\\Admin\\Orders\\OrdersExport\\[Order].OrderSource.csv",
            "data\\Admin\\Orders\\OrdersExport\\[Order].OrderContact.csv",
            "data\\Admin\\Orders\\OrdersExport\\[Order].OrderCurrency.csv",
             "data\\Admin\\Orders\\OrdersExport\\[Order].OrderItems.csv",
             "data\\Admin\\Orders\\OrdersExport\\[Order].OrderStatus.csv",
             "data\\Admin\\Orders\\OrdersExport\\[Order].[Order].csv"


                );

            Init();
            
        }

        [Test]
        public void ExportAll()
        {
            testname = "ExportAll";
            VerifyBegin(testname);

            GoToAdmin("analytics/exportorders");

            if (driver.FindElement(By.Id("UseStatus")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"exportOrdersUseStatus\"]")).Click();
            }

            VerifyIsFalse(driver.FindElement(By.Id("ddlOrderStatuses")).Enabled, "use status field disabled");

            if (driver.FindElement(By.Id("UseDate")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"exportOrdersUseDate\"]")).Click();
            }
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Enabled, "use date from field disabled");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Enabled, "use date to field disabled");

            (new SelectElement(driver.FindElement(By.Id("ddlEncodings")))).SelectByText("UTF-8");

            driver.FindElement(By.CssSelector("[data-e2e=\"Export\"]")).Click();
            WaitForAjax();
            WaitForElem(By.CssSelector(".progress-bar"));
            Thread.Sleep(1000);
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"exportCountValue\"]")).Text.Contains("30"), "export count value");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"exportCountTotal\"]")).Text.Contains("30"), "export count total");

            string linkExport = driver.FindElement(By.LinkText("Скачать файл")).GetAttribute("href");
            
            driver.Navigate().GoToUrl(linkExport + "&openinbrowser=true");
            WaitForElem(By.TagName("pre"));

            VerifyIsTrue(driver.Url.Contains("csv"), "export file format");
            VerifyIsTrue(driver.PageSource.Contains("Номер заказа;Статус;Дата заказа;ФИО;Email;Телефон;Товары;Итоговая стоимость;Валюта;Налог;Стоимость товаров;Выгода;Метод оплаты;Метод доставки;Адрес доставки;Комментарий пользователя;Комментарй администратора;Комментарий к статусу;Оплачен\r\n1;Новый;04.08.2016 00:00:00;LastName1 FirstName1;;;[1 - TestProduct1 - 1шт.];1;\" руб.\";0;0;1;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment1;Нет\r\n2;Новый;05.08.2016 00:00:00;LastName2 FirstName2;;;[2 - TestProduct2 - 2шт.];2;\" руб.\";0;0;2;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment2;Нет\r\n3;Новый;06.08.2016 00:00:00;LastName3 FirstName3;;;[3 - TestProduct3 - 3шт.];3;\" руб.\";0;0;3;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment3;Нет\r\n4;Новый;07.08.2016 00:00:00;LastName4 FirstName4;;;[4 - TestProduct4 - 4шт.];4;\" руб.\";0;0;4;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment4;Нет\r\n5;Новый;08.08.2016 00:00:00;LastName5 FirstName5;;;[5 - TestProduct5 - 5шт.];5;\" руб.\";0;0;5;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment5;Нет\r\n6;В обработке;09.08.2016 00:00:00;LastName1 FirstName1;;;[6 - TestProduct6 - 6шт.];6;\" руб.\";0;0;6;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment6;Нет\r\n7;В обработке;10.08.2016 00:00:00;LastName2 FirstName2;;;[7 - TestProduct7 - 7шт.];7;\" руб.\";0;0;7;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment7;Нет\r\n8;В обработке;11.08.2016 00:00:00;LastName3 FirstName3;;;[8 - TestProduct8 - 8шт.];8;\" руб.\";0;0;8;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment8;Нет\r\n9;В обработке;12.08.2016 00:00:00;LastName4 FirstName4;;;[9 - TestProduct9 - 9шт.];9;\" руб.\";0;0;9;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment9;Нет\r\n10;В обработке;13.08.2016 00:00:00;LastName5 FirstName5;;;[10 - TestProduct10 - 10шт.];10;\" руб.\";0;0;10;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment10;Нет\r\n11;Отправлен;14.08.2016 00:00:00;LastName1 FirstName1;;;[11 - TestProduct11 - 11шт.];11;\" руб.\";0;0;11;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment11;Нет\r\n12;Отправлен;15.08.2016 00:00:00;LastName2 FirstName2;;;[12 - TestProduct12 - 12шт.];12;\" руб.\";0;0;12;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment12;Нет\r\n13;Отправлен;16.08.2016 00:00:00;LastName3 FirstName3;;;[13 - TestProduct13 - 13шт.];13;\" руб.\";0;0;13;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment13;Нет\r\n14;Отправлен;17.08.2016 00:00:00;LastName4 FirstName4;;;[14 - TestProduct14 - 14шт.];14;\" руб.\";0;0;14;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment14;Нет\r\n15;Отправлен;18.08.2016 00:00:00;LastName5 FirstName5;;;[15 - TestProduct15 - 15шт.];15;\" руб.\";0;0;15;Подарочный сертификат;Самовывоз - 0;;;;comment15;Да\r\n16;Доставлен;19.08.2016 00:00:00;LastName1 FirstName1;;;[16 - TestProduct16 - 16шт.];16;\" руб.\";0;0;16;Подарочный сертификат;Самовывоз - 0;;;;comment16;Да\r\n17;Доставлен;20.08.2016 00:00:00;LastName2 FirstName2;;;[17 - TestProduct17 - 17шт.];17;\" руб.\";0;0;17;Наложенный платеж;Самовывоз - 0;;;;comment17;Да\r\n18;Доставлен;21.08.2016 00:00:00;LastName3 FirstName3;;;[18 - TestProduct18 - 18шт.];18;\" руб.\";0;0;18;Наложенный платеж;Самовывоз - 0;;;;comment18;Да\r\n19;Доставлен;22.08.2016 00:00:00;LastName4 FirstName4;;;[19 - TestProduct19 - 19шт.];19;\" руб.\";0;0;19;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment19;Да\r\n20;Доставлен;23.08.2016 00:00:00;LastName5 FirstName5;;;[20 - TestProduct20 - 20шт.];20;\" руб.\";0;0;20;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment20;Да\r\n21;Отменён;24.08.2016 00:00:00;LastName5 FirstName5;;;[21 - TestProduct21 - 21шт.];21;\" руб.\";0;0;21;Наличными курьеру;Самовывоз - 0;;;;comment21;Да\r\n22;Отменён;25.08.2016 00:00:00;LastName5 FirstName5;;;[22 - TestProduct22 - 22шт.];22;\" руб.\";0;0;22;Наличными курьеру;Самовывоз - 0;;;;comment22;Да\r\n23;Отменён;26.08.2016 00:00:00;LastName5 FirstName5;;;[23 - TestProduct23 - 23шт.];23;\" руб.\";0;0;23;Банковский перевод для физ. лиц;Самовывоз - 0;;;;comment23;Да\r\n24;Отменён;27.08.2016 00:00:00;LastName5 FirstName5;;;[24 - TestProduct24 - 24шт.];24;\" руб.\";0;0;24;Банковский перевод для физ. лиц;Самовывоз - 0;;;;comment24;Да\r\n25;Отменён;28.08.2016 00:00:00;LastName5 FirstName5;;;[25 - TestProduct25 - 25шт.];25;\" руб.\";0;0;25;Банковский перевод для юр. лиц;Самовывоз - 0;;;;comment25;Да\r\n26;Отменен навсегда;29.08.2016 00:00:00;LastName5 FirstName5;;;[26 - TestProduct26 - 26шт.];26;\" руб.\";0;0;26;Банковский перевод для юр. лиц;Самовывоз - 0;;;;comment26;Да\r\n27;Отменен навсегда;30.08.2016 00:00:00;LastName5 FirstName5;;;[27 - TestProduct27 - 27шт.];27;\" руб.\";0;0;27;Пластиковая карта;Самовывоз - 0;;;;comment27;Да\r\n28;Отменен навсегда;31.08.2016 10:00:00;LastName5 FirstName5;;;[28 - TestProduct28 - 28шт.];28;\" руб.\";0;0;28;Пластиковая карта;Самовывоз - 0;;;;comment28;Да\r\n29;Отменен навсегда;31.08.2016 15:00:00;LastName5 FirstName5;;;[29 - TestProduct29 - 29шт.];29;\" руб.\";0;0;29;Электронные деньги (Яндекс Деньги, WebMoney, Qiwi);Самовывоз - 0;;;;comment29;Да\r\n30;Отменен навсегда;31.08.2016 23:00:00;LastName5 FirstName5;;;[30 - TestProduct30 - 30шт.];30;\" руб.\";0;0;30;Терминалы оплаты;Самовывоз - 0;;;;comment30;Да"), "export file content");
      
            VerifyFinally(testname);
        }

        [Test]
        public void ExportUseStatus()
        {
            testname = "ExportUseStatus";
            VerifyBegin(testname);

            GoToAdmin("analytics/exportorders");

            if (!driver.FindElement(By.Id("UseStatus")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"exportOrdersUseStatus\"]")).Click();
            }

            VerifyIsTrue(driver.FindElement(By.Id("ddlOrderStatuses")).Enabled, "use status field enabled");

            (new SelectElement(driver.FindElement(By.Id("ddlOrderStatuses")))).SelectByText("Доставлен");

            if (driver.FindElement(By.Id("UseDate")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"exportOrdersUseDate\"]")).Click();
            }
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Enabled, "use date from field disabled");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Enabled, "use date to field disabled");

            (new SelectElement(driver.FindElement(By.Id("ddlEncodings")))).SelectByText("UTF-8");

            driver.FindElement(By.CssSelector("[data-e2e=\"Export\"]")).Click();
            WaitForAjax();
            WaitForElem(By.CssSelector(".progress-bar"));
            Thread.Sleep(1000);
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"exportCountValue\"]")).Text.Contains("5"), "export count value");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"exportCountTotal\"]")).Text.Contains("5"), "export count total");

            string linkExport = driver.FindElement(By.LinkText("Скачать файл")).GetAttribute("href");

            driver.Navigate().GoToUrl(linkExport + "&openinbrowser=true");
            WaitForElem(By.TagName("pre"));

            VerifyIsTrue(driver.Url.Contains("csv"), "export file format");
            VerifyIsTrue(driver.PageSource.Contains("Номер заказа;Статус;Дата заказа;ФИО;Email;Телефон;Товары;Итоговая стоимость;Валюта;Налог;Стоимость товаров;Выгода;Метод оплаты;Метод доставки;Адрес доставки;Комментарий пользователя;Комментарй администратора;Комментарий к статусу;Оплачен\r\n16;Доставлен;19.08.2016 00:00:00;LastName1 FirstName1;;;[16 - TestProduct16 - 16шт.];16;\" руб.\";0;0;16;Подарочный сертификат;Самовывоз - 0;;;;comment16;Да\r\n17;Доставлен;20.08.2016 00:00:00;LastName2 FirstName2;;;[17 - TestProduct17 - 17шт.];17;\" руб.\";0;0;17;Наложенный платеж;Самовывоз - 0;;;;comment17;Да\r\n18;Доставлен;21.08.2016 00:00:00;LastName3 FirstName3;;;[18 - TestProduct18 - 18шт.];18;\" руб.\";0;0;18;Наложенный платеж;Самовывоз - 0;;;;comment18;Да\r\n19;Доставлен;22.08.2016 00:00:00;LastName4 FirstName4;;;[19 - TestProduct19 - 19шт.];19;\" руб.\";0;0;19;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment19;Да\r\n20;Доставлен;23.08.2016 00:00:00;LastName5 FirstName5;;;[20 - TestProduct20 - 20шт.];20;\" руб.\";0;0;20;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment20;Да"), "export file content");

            VerifyFinally(testname);
        }


        [Test]
        public void ExportUseDate()
        {
            testname = "ExportUseDate";
            VerifyBegin(testname);

            GoToAdmin("analytics/exportorders");

            if (driver.FindElement(By.Id("UseStatus")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"exportOrdersUseStatus\"]")).Click();
            }

            VerifyIsFalse(driver.FindElement(By.Id("ddlOrderStatuses")).Enabled, "use status field disabled");
            
            if (!driver.FindElement(By.Id("UseDate")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"exportOrdersUseDate\"]")).Click();
            }

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Enabled, "use date from field enabled");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Enabled, "use date to field enabled");

            /*check set date by calender*/
            //set date from
            driver.FindElement(By.Id("gridFilterDateFrom")).FindElement(By.CssSelector(".glyphicon.glyphicon-calendar")).Click();
            driver.FindElement(By.CssSelector("[colspan=\"5\"]")).Click();
            driver.FindElement(By.CssSelector("[colspan=\"5\"]")).Click();
            XPathContainsText("span", "2016");
            XPathContainsText("span", "авг.");
            XPathContainsText("td", "20");
            XPathContainsText("span", "00:00");
            XPathContainsText("span", "00:00");
            DropFocus("h1");

            VerifyAreEqual("20.08.2016 00:00", driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).GetAttribute("value"), "date filed from by calender");

            //set date to
            driver.FindElement(By.Id("gridFilterDateTo")).FindElement(By.CssSelector(".glyphicon.glyphicon-calendar")).Click();
            driver.FindElements(By.CssSelector("[colspan=\"5\"]"))[1].Click();
            driver.FindElements(By.CssSelector("[colspan=\"5\"]"))[1].Click();
            XPathContainsText("span", "2016");
            XPathContainsText("span", "авг.");
            driver.FindElements(By.CssSelector(".table.table-condensed.day-view"))[1].FindElements(By.XPath("//td[contains(text(), '27')]"))[1].Click();
            XPathContainsText("span", "00:00");
            XPathContainsText("span", "00:00");
            DropFocus("h1");
            
            VerifyAreEqual("27.08.2016 00:00", driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).GetAttribute("value"), "date filed to by calender");

            /*check set date by print*/
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("20.08.2016 00:00");
            DropFocus("h1");

            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("27.08.2016 00:00");
            DropFocus("h1");

            VerifyAreEqual("20.08.2016 00:00", driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).GetAttribute("value"), "date filed from by print");
            VerifyAreEqual("27.08.2016 00:00", driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).GetAttribute("value"), "date filed to by print");

            (new SelectElement(driver.FindElement(By.Id("ddlEncodings")))).SelectByText("UTF-8");

            driver.FindElement(By.CssSelector("[data-e2e=\"Export\"]")).Click();
            WaitForAjax();
            WaitForElem(By.CssSelector(".progress-bar"));
            Thread.Sleep(1000);
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"exportCountValue\"]")).Text.Contains("8"), "export count value");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"exportCountTotal\"]")).Text.Contains("8"), "export count total");

            string linkExport = driver.FindElement(By.LinkText("Скачать файл")).GetAttribute("href");

            driver.Navigate().GoToUrl(linkExport + "&openinbrowser=true");
            WaitForElem(By.TagName("pre"));

            VerifyIsTrue(driver.Url.Contains("csv"), "export file format");
            VerifyIsTrue(driver.PageSource.Contains("Номер заказа;Статус;Дата заказа;ФИО;Email;Телефон;Товары;Итоговая стоимость;Валюта;Налог;Стоимость товаров;Выгода;Метод оплаты;Метод доставки;Адрес доставки;Комментарий пользователя;Комментарй администратора;Комментарий к статусу;Оплачен\r\n17;Доставлен;20.08.2016 00:00:00;LastName2 FirstName2;;;[17 - TestProduct17 - 17шт.];17;\" руб.\";0;0;17;Наложенный платеж;Самовывоз - 0;;;;comment17;Да\r\n18;Доставлен;21.08.2016 00:00:00;LastName3 FirstName3;;;[18 - TestProduct18 - 18шт.];18;\" руб.\";0;0;18;Наложенный платеж;Самовывоз - 0;;;;comment18;Да\r\n19;Доставлен;22.08.2016 00:00:00;LastName4 FirstName4;;;[19 - TestProduct19 - 19шт.];19;\" руб.\";0;0;19;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment19;Да\r\n20;Доставлен;23.08.2016 00:00:00;LastName5 FirstName5;;;[20 - TestProduct20 - 20шт.];20;\" руб.\";0;0;20;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment20;Да\r\n21;Отменён;24.08.2016 00:00:00;LastName5 FirstName5;;;[21 - TestProduct21 - 21шт.];21;\" руб.\";0;0;21;Наличными курьеру;Самовывоз - 0;;;;comment21;Да\r\n22;Отменён;25.08.2016 00:00:00;LastName5 FirstName5;;;[22 - TestProduct22 - 22шт.];22;\" руб.\";0;0;22;Наличными курьеру;Самовывоз - 0;;;;comment22;Да\r\n23;Отменён;26.08.2016 00:00:00;LastName5 FirstName5;;;[23 - TestProduct23 - 23шт.];23;\" руб.\";0;0;23;Банковский перевод для физ. лиц;Самовывоз - 0;;;;comment23;Да\r\n24;Отменён;27.08.2016 00:00:00;LastName5 FirstName5;;;[24 - TestProduct24 - 24шт.];24;\" руб.\";0;0;24;Банковский перевод для физ. лиц;Самовывоз - 0;;;;comment24;Да"), "export file content");

            VerifyFinally(testname);
        }

        [Test]
        public void ExportUseStatusAndDate()
        {
            testname = "ExportUseStatusAndDate";
            VerifyBegin(testname);

            GoToAdmin("analytics/exportorders");

            if (!driver.FindElement(By.Id("UseStatus")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"exportOrdersUseStatus\"]")).Click();
            }

            VerifyIsTrue(driver.FindElement(By.Id("ddlOrderStatuses")).Enabled, "use status field enabled");

            (new SelectElement(driver.FindElement(By.Id("ddlOrderStatuses")))).SelectByText("Отменён");

            if (!driver.FindElement(By.Id("UseDate")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"exportOrdersUseDate\"]")).Click();
            }

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Enabled, "use date from field enabled");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Enabled, "use date to field enabled");
            
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("20.08.2016 00:00");
            DropFocus("h1");

            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("27.08.2016 00:00");
            DropFocus("h1");

            VerifyAreEqual("20.08.2016 00:00", driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).GetAttribute("value"), "date filed from by print");
            VerifyAreEqual("27.08.2016 00:00", driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).GetAttribute("value"), "date filed to by print");

            (new SelectElement(driver.FindElement(By.Id("ddlEncodings")))).SelectByText("UTF-8");

            driver.FindElement(By.CssSelector("[data-e2e=\"Export\"]")).Click();
            WaitForAjax();
            WaitForElem(By.CssSelector(".progress-bar"));
            Thread.Sleep(1000);
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"exportCountValue\"]")).Text.Contains("4"), "export count value");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"exportCountTotal\"]")).Text.Contains("4"), "export count total");

            string linkExport = driver.FindElement(By.LinkText("Скачать файл")).GetAttribute("href");

            driver.Navigate().GoToUrl(linkExport + "&openinbrowser=true");
            WaitForElem(By.TagName("pre"));

            VerifyIsTrue(driver.Url.Contains("csv"), "export file format");
            VerifyIsTrue(driver.PageSource.Contains("Номер заказа;Статус;Дата заказа;ФИО;Email;Телефон;Товары;Итоговая стоимость;Валюта;Налог;Стоимость товаров;Выгода;Метод оплаты;Метод доставки;Адрес доставки;Комментарий пользователя;Комментарй администратора;Комментарий к статусу;Оплачен\r\n21;Отменён;24.08.2016 00:00:00;LastName5 FirstName5;;;[21 - TestProduct21 - 21шт.];21;\" руб.\";0;0;21;Наличными курьеру;Самовывоз - 0;;;;comment21;Да\r\n22;Отменён;25.08.2016 00:00:00;LastName5 FirstName5;;;[22 - TestProduct22 - 22шт.];22;\" руб.\";0;0;22;Наличными курьеру;Самовывоз - 0;;;;comment22;Да\r\n23;Отменён;26.08.2016 00:00:00;LastName5 FirstName5;;;[23 - TestProduct23 - 23шт.];23;\" руб.\";0;0;23;Банковский перевод для физ. лиц;Самовывоз - 0;;;;comment23;Да\r\n24;Отменён;27.08.2016 00:00:00;LastName5 FirstName5;;;[24 - TestProduct24 - 24шт.];24;\" руб.\";0;0;24;Банковский перевод для физ. лиц;Самовывоз - 0;;;;comment24;Да"), "export file content");

            VerifyFinally(testname);
        }

        [Test]
        public void ExportUseStatusAndDateNotPass()
        {
            testname = "ExportUseStatusAndDateNotPass";
            VerifyBegin(testname);

            GoToAdmin("analytics/exportorders");

            if (!driver.FindElement(By.Id("UseStatus")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"exportOrdersUseStatus\"]")).Click();
            }

            VerifyIsTrue(driver.FindElement(By.Id("ddlOrderStatuses")).Enabled, "use status field enabled");

            (new SelectElement(driver.FindElement(By.Id("ddlOrderStatuses")))).SelectByText("Отменен навсегда");

            if (!driver.FindElement(By.Id("UseDate")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"exportOrdersUseDate\"]")).Click();
            }

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Enabled, "use date from field enabled");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Enabled, "use date to field enabled");

            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("20.08.2016 00:00");
            DropFocus("h1");

            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("27.08.2016 00:00");
            DropFocus("h1");

            VerifyAreEqual("20.08.2016 00:00", driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).GetAttribute("value"), "date filed from by print");
            VerifyAreEqual("27.08.2016 00:00", driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).GetAttribute("value"), "date filed to by print");

            (new SelectElement(driver.FindElement(By.Id("ddlEncodings")))).SelectByText("UTF-8");

            driver.FindElement(By.CssSelector("[data-e2e=\"Export\"]")).Click();
            WaitForAjax();
            WaitForElem(By.CssSelector(".progress-bar"));
            Thread.Sleep(1000);
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"exportCountValue\"]")).Text.Contains("0"), "export count value");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"exportCountTotal\"]")).Text.Contains("0"), "export count total");

            string linkExport = driver.FindElement(By.LinkText("Скачать файл")).GetAttribute("href");

            driver.Navigate().GoToUrl(linkExport + "&openinbrowser=true");
            WaitForElem(By.TagName("pre"));

            VerifyIsTrue(driver.Url.Contains("csv"), "export file format");
            VerifyIsTrue(driver.PageSource.Contains("Номер заказа;Статус;Дата заказа;ФИО;Email;Телефон;Товары;Итоговая стоимость;Валюта;Налог;Стоимость товаров;Выгода;Метод оплаты;Метод доставки;Адрес доставки;Комментарий пользователя;Комментарй администратора;Комментарий к статусу;Оплачен"), "export file content");

            VerifyFinally(testname);
        }

        [Test]
        public void ExportUseStatusAndDateDisabledAfterPrint()
        {
            testname = "ExportUseStatusAndDateDisabledAfterPrint";
            VerifyBegin(testname);

            GoToAdmin("analytics/exportorders");

            if (!driver.FindElement(By.Id("UseStatus")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"exportOrdersUseStatus\"]")).Click();
            }

            VerifyIsTrue(driver.FindElement(By.Id("ddlOrderStatuses")).Enabled, "use status field enabled");

            (new SelectElement(driver.FindElement(By.Id("ddlOrderStatuses")))).SelectByText("Отменен навсегда");

            if (!driver.FindElement(By.Id("UseDate")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"exportOrdersUseDate\"]")).Click();
            }

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Enabled, "use date from field enabled");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Enabled, "use date to field enabled");

            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("20.08.2016 00:00");
            DropFocus("h1");

            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("27.08.2016 00:00");
            DropFocus("h1");

            VerifyAreEqual("20.08.2016 00:00", driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).GetAttribute("value"), "date filed from by print");
            VerifyAreEqual("27.08.2016 00:00", driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).GetAttribute("value"), "date filed to by print");

            IWebElement selectElem = driver.FindElement(By.Id("ddlOrderStatuses"));
            SelectElement select = new SelectElement(selectElem);
            VerifyIsTrue(select.SelectedOption.Text.Contains("Отменен навсегда"), "order status selected");

            (new SelectElement(driver.FindElement(By.Id("ddlEncodings")))).SelectByText("UTF-8");

            driver.FindElement(By.CssSelector("[data-e2e=\"exportOrdersUseStatus\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"exportOrdersUseDate\"]")).Click();

            VerifyIsFalse(driver.FindElement(By.Id("ddlOrderStatuses")).Enabled, "use status field disabled with value");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Enabled, "use date from field disabled with value");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Enabled, "use date to field disabled with value");

            driver.FindElement(By.CssSelector("[data-e2e=\"Export\"]")).Click();
            WaitForAjax();
            WaitForElem(By.CssSelector(".progress-bar"));
            Thread.Sleep(1000);
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"exportCountValue\"]")).Text.Contains("30"), "export count value");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"exportCountTotal\"]")).Text.Contains("30"), "export count total");

            string linkExport = driver.FindElement(By.LinkText("Скачать файл")).GetAttribute("href");

            driver.Navigate().GoToUrl(linkExport + "&openinbrowser=true");
            WaitForElem(By.TagName("pre"));

            VerifyIsTrue(driver.Url.Contains("csv"), "export file format");
            VerifyIsTrue(driver.PageSource.Contains("Номер заказа;Статус;Дата заказа;ФИО;Email;Телефон;Товары;Итоговая стоимость;Валюта;Налог;Стоимость товаров;Выгода;Метод оплаты;Метод доставки;Адрес доставки;Комментарий пользователя;Комментарй администратора;Комментарий к статусу;Оплачен\r\n1;Новый;04.08.2016 00:00:00;LastName1 FirstName1;;;[1 - TestProduct1 - 1шт.];1;\" руб.\";0;0;1;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment1;Нет\r\n2;Новый;05.08.2016 00:00:00;LastName2 FirstName2;;;[2 - TestProduct2 - 2шт.];2;\" руб.\";0;0;2;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment2;Нет\r\n3;Новый;06.08.2016 00:00:00;LastName3 FirstName3;;;[3 - TestProduct3 - 3шт.];3;\" руб.\";0;0;3;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment3;Нет\r\n4;Новый;07.08.2016 00:00:00;LastName4 FirstName4;;;[4 - TestProduct4 - 4шт.];4;\" руб.\";0;0;4;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment4;Нет\r\n5;Новый;08.08.2016 00:00:00;LastName5 FirstName5;;;[5 - TestProduct5 - 5шт.];5;\" руб.\";0;0;5;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment5;Нет\r\n6;В обработке;09.08.2016 00:00:00;LastName1 FirstName1;;;[6 - TestProduct6 - 6шт.];6;\" руб.\";0;0;6;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment6;Нет\r\n7;В обработке;10.08.2016 00:00:00;LastName2 FirstName2;;;[7 - TestProduct7 - 7шт.];7;\" руб.\";0;0;7;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment7;Нет\r\n8;В обработке;11.08.2016 00:00:00;LastName3 FirstName3;;;[8 - TestProduct8 - 8шт.];8;\" руб.\";0;0;8;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment8;Нет\r\n9;В обработке;12.08.2016 00:00:00;LastName4 FirstName4;;;[9 - TestProduct9 - 9шт.];9;\" руб.\";0;0;9;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment9;Нет\r\n10;В обработке;13.08.2016 00:00:00;LastName5 FirstName5;;;[10 - TestProduct10 - 10шт.];10;\" руб.\";0;0;10;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment10;Нет\r\n11;Отправлен;14.08.2016 00:00:00;LastName1 FirstName1;;;[11 - TestProduct11 - 11шт.];11;\" руб.\";0;0;11;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment11;Нет\r\n12;Отправлен;15.08.2016 00:00:00;LastName2 FirstName2;;;[12 - TestProduct12 - 12шт.];12;\" руб.\";0;0;12;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment12;Нет\r\n13;Отправлен;16.08.2016 00:00:00;LastName3 FirstName3;;;[13 - TestProduct13 - 13шт.];13;\" руб.\";0;0;13;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment13;Нет\r\n14;Отправлен;17.08.2016 00:00:00;LastName4 FirstName4;;;[14 - TestProduct14 - 14шт.];14;\" руб.\";0;0;14;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment14;Нет\r\n15;Отправлен;18.08.2016 00:00:00;LastName5 FirstName5;;;[15 - TestProduct15 - 15шт.];15;\" руб.\";0;0;15;Подарочный сертификат;Самовывоз - 0;;;;comment15;Да\r\n16;Доставлен;19.08.2016 00:00:00;LastName1 FirstName1;;;[16 - TestProduct16 - 16шт.];16;\" руб.\";0;0;16;Подарочный сертификат;Самовывоз - 0;;;;comment16;Да\r\n17;Доставлен;20.08.2016 00:00:00;LastName2 FirstName2;;;[17 - TestProduct17 - 17шт.];17;\" руб.\";0;0;17;Наложенный платеж;Самовывоз - 0;;;;comment17;Да\r\n18;Доставлен;21.08.2016 00:00:00;LastName3 FirstName3;;;[18 - TestProduct18 - 18шт.];18;\" руб.\";0;0;18;Наложенный платеж;Самовывоз - 0;;;;comment18;Да\r\n19;Доставлен;22.08.2016 00:00:00;LastName4 FirstName4;;;[19 - TestProduct19 - 19шт.];19;\" руб.\";0;0;19;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment19;Да\r\n20;Доставлен;23.08.2016 00:00:00;LastName5 FirstName5;;;[20 - TestProduct20 - 20шт.];20;\" руб.\";0;0;20;Оплата наличными при получении заказа в пункте выдачи;Самовывоз - 0;;;;comment20;Да\r\n21;Отменён;24.08.2016 00:00:00;LastName5 FirstName5;;;[21 - TestProduct21 - 21шт.];21;\" руб.\";0;0;21;Наличными курьеру;Самовывоз - 0;;;;comment21;Да\r\n22;Отменён;25.08.2016 00:00:00;LastName5 FirstName5;;;[22 - TestProduct22 - 22шт.];22;\" руб.\";0;0;22;Наличными курьеру;Самовывоз - 0;;;;comment22;Да\r\n23;Отменён;26.08.2016 00:00:00;LastName5 FirstName5;;;[23 - TestProduct23 - 23шт.];23;\" руб.\";0;0;23;Банковский перевод для физ. лиц;Самовывоз - 0;;;;comment23;Да\r\n24;Отменён;27.08.2016 00:00:00;LastName5 FirstName5;;;[24 - TestProduct24 - 24шт.];24;\" руб.\";0;0;24;Банковский перевод для физ. лиц;Самовывоз - 0;;;;comment24;Да\r\n25;Отменён;28.08.2016 00:00:00;LastName5 FirstName5;;;[25 - TestProduct25 - 25шт.];25;\" руб.\";0;0;25;Банковский перевод для юр. лиц;Самовывоз - 0;;;;comment25;Да\r\n26;Отменен навсегда;29.08.2016 00:00:00;LastName5 FirstName5;;;[26 - TestProduct26 - 26шт.];26;\" руб.\";0;0;26;Банковский перевод для юр. лиц;Самовывоз - 0;;;;comment26;Да\r\n27;Отменен навсегда;30.08.2016 00:00:00;LastName5 FirstName5;;;[27 - TestProduct27 - 27шт.];27;\" руб.\";0;0;27;Пластиковая карта;Самовывоз - 0;;;;comment27;Да\r\n28;Отменен навсегда;31.08.2016 10:00:00;LastName5 FirstName5;;;[28 - TestProduct28 - 28шт.];28;\" руб.\";0;0;28;Пластиковая карта;Самовывоз - 0;;;;comment28;Да\r\n29;Отменен навсегда;31.08.2016 15:00:00;LastName5 FirstName5;;;[29 - TestProduct29 - 29шт.];29;\" руб.\";0;0;29;Электронные деньги (Яндекс Деньги, WebMoney, Qiwi);Самовывоз - 0;;;;comment29;Да\r\n30;Отменен навсегда;31.08.2016 23:00:00;LastName5 FirstName5;;;[30 - TestProduct30 - 30шт.];30;\" руб.\";0;0;30;Терминалы оплаты;Самовывоз - 0;;;;comment30;Да"), "export file content");

            VerifyFinally(testname);
        }
    }
}
