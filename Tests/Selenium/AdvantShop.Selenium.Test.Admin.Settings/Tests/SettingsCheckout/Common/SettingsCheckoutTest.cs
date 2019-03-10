using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using AdvantShop.Selenium.Core.Infrastructure;
using System.Collections.ObjectModel;

namespace AdvantShop.Selenium.Test.Admin.Settings.Tests
{
    [TestFixture]
    public class SettingsCheckoutTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Orders);
            InitializeService.LoadData(
                         "Data\\Admin\\Settings\\SettingCheckout\\Catalog.Product.csv",
                         "Data\\Admin\\Settings\\SettingCheckout\\Catalog.ProductGifts.csv",
                         "Data\\Admin\\Settings\\SettingCheckout\\Catalog.Offer.csv",
                         "Data\\Admin\\Settings\\SettingCheckout\\Catalog.Category.csv",
                         "Data\\Admin\\Settings\\SettingCheckout\\Catalog.ProductCategories.csv",
                         "data\\Admin\\Settings\\SettingCheckout\\[Order].Certificate.csv",
                         "data\\Admin\\Settings\\SettingCheckout\\[Order].OrderContact.csv",
                         "data\\Admin\\Settings\\SettingCheckout\\[Order].OrderCurrency.csv",
                         "data\\Admin\\Settings\\SettingCheckout\\[Order].OrderItems.csv",
                         "data\\Admin\\Settings\\SettingCheckout\\[Order].OrderStatus.csv",
                         "data\\Admin\\Settings\\SettingCheckout\\[Order].[Order].csv",
                         "Data\\Admin\\Settings\\SettingCheckout\\[Order].OrderSource.csv"
                );
            Init();
        }
        [Test]
        public void ControlAmountLimitation()
        {
            testname = "ControlAmountLimitation";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");

            if (!driver.FindElement(By.Id("AmountLimitation")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"AmountLimitation\"]")).Click();
                Thread.Sleep(2000);
            }
            try
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
                Thread.Sleep(2000);
            }
            catch { }

            GoToClient("products/test-product1");
            driver.FindElement(By.CssSelector(".btn-confirm.icon-bag-before")).Click();

            GoToClient("cart");
            driver.FindElement(By.CssSelector("[data-value=\"item.Amount\"] input")).Clear();
            driver.FindElement(By.CssSelector("[data-value=\"item.Amount\"] input")).SendKeys("5");
            DropFocus("h1");
            Refresh();
            VerifyAreEqual("5", driver.FindElement(By.CssSelector("[data-value=\"item.Amount\"] input")).GetAttribute("value"), "Amount change cart");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".cart-full-buttons .btn-disabled")).Count == 1, " btn not enable");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".cart-full-error")).Displayed, "error masange");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".cart-amount-error")).Displayed, " error amount");
            VerifyAreEqual("доступно 1", driver.FindElement(By.CssSelector(".cart-amount-error")).Text, "Amount avalible");
            VerifyAreEqual("Заказ содержит недоступное количество товаров.", driver.FindElement(By.CssSelector(".cart-full-error")).Text, "error masange text");

            VerifyFinally(testname);
        }
        [Test]
        public void ControlAmountLimitationNo()
        {
            testname = "ControlAmountLimitationNo";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");

            if (driver.FindElement(By.Id("AmountLimitation")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"AmountLimitation\"]")).Click();
                Thread.Sleep(2000);
            }
            try
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
                Thread.Sleep(2000);
            }
            catch { }

            GoToClient("products/test-product1");
            driver.FindElement(By.CssSelector(".btn-confirm.icon-bag-before")).Click();

            GoToClient("cart");
            driver.FindElement(By.CssSelector("[data-value=\"item.Amount\"] input")).Clear();
            driver.FindElement(By.CssSelector("[data-value=\"item.Amount\"] input")).SendKeys("5");
            DropFocus("h1");
            Refresh();
            VerifyAreEqual("5", driver.FindElement(By.CssSelector("[data-value=\"item.Amount\"] input")).GetAttribute("value"), "Amount change cart");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".cart-full-buttons .btn-disabled")).Count == 0, " btn not enable");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".cart-full-error")).Count == 0, "error masange");
            VerifyIsFalse(driver.FindElement(By.CssSelector(".cart-amount-error")).Displayed, " error amount");

            driver.FindElement(By.CssSelector(".icon-cancel-circled-before")).Click();
            VerifyFinally(testname);
        }
        [Test]
        public void GoToPayment()
        {
            testname = "GoToPayment";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");

            if (!driver.FindElement(By.Id("ProceedToPayment")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"ProceedToPayment\"]")).Click();
                Thread.Sleep(2000);
            }
            try
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
                Thread.Sleep(2000);
            }
            catch { }

            GoToClient("cart");
            try
            {
                driver.FindElement(By.CssSelector(".icon-cancel-circled-before")).Click();
                Thread.Sleep(2000);
            }
            catch { }
            GoToClient("products/test-product5");
            driver.FindElement(By.CssSelector(".btn-confirm.icon-bag-before")).Click();

            GoToClient("checkout");
            Thread.Sleep(2000);
            ScrollTo(By.Id("checkoutpayment"));
            XPathContainsText("span", "Пластиковая карта");
            Thread.Sleep(2000);

            driver.FindElement(By.Name("checkoutForm")).FindElement(By.CssSelector(".btn.btn-big.btn-submit")).Click();
            Thread.Sleep(4000);

            VerifyIsTrue(driver.Url.Contains("walletone.com/checkout"), " url payment");

            VerifyFinally(testname);
        }
        [Test]
        public void GoToPaymentNo()
        {
            testname = "GoToPaymentNo";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");

            if (driver.FindElement(By.Id("ProceedToPayment")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"ProceedToPayment\"]")).Click();
                Thread.Sleep(2000);
            }
            try
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
                Thread.Sleep(2000);
            }
            catch { }

            GoToClient("cart");
            try
            {
                driver.FindElement(By.CssSelector(".icon-cancel-circled-before")).Click();
                Thread.Sleep(2000);
            }
            catch { }
            GoToClient("products/test-product5");
            driver.FindElement(By.CssSelector(".btn-confirm.icon-bag-before")).Click();

            GoToClient("checkout");
            Thread.Sleep(2000);
            ScrollTo(By.Id("checkoutpayment"));
            XPathContainsText("span", "Пластиковая карта");
            Thread.Sleep(2000);

            driver.FindElement(By.Name("checkoutForm")).FindElement(By.CssSelector(".btn.btn-big.btn-submit")).Click();
            Thread.Sleep(4000);

            VerifyIsTrue(driver.Url.Contains("checkout/success"), " url checkout");
            VerifyIsFalse(driver.Url.Contains("walletone.com/checkout"), " url payment");
            VerifyAreEqual("Спасибо, ваш заказ оформлен!", driver.FindElement(By.TagName("h1")).Text, "success");

            VerifyFinally(testname);
        }

        [Test]
        public void LinkCustomerGroup()
        {
            testname = "LinkCustomerGroup";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"MinimalPriceCustomerGroups\"]")).GetAttribute("href").Contains("/customergroups"), "link");
            driver.FindElement(By.CssSelector("[data-e2e=\"MinimalPriceCustomerGroups\"]")).Click();
            VerifyAreEqual("Группы покупателей", driver.FindElement(By.TagName("h1")).Text, "success");

            VerifyFinally(testname);
        }
        [Test]
        public void MinSumOder()
        {
            testname = "MinSumOder";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");

            driver.FindElement(By.Id("MinimalOrderPriceForDefaultGroup")).Clear();
            driver.FindElement(By.Id("MinimalOrderPriceForDefaultGroup")).SendKeys("1000");

            try
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
                Thread.Sleep(2000);
            }
            catch { }

            GoToClient("cart");
            try
            {
                driver.FindElement(By.CssSelector(".icon-cancel-circled-before")).Click();
                Thread.Sleep(2000);
            }
            catch { }
            GoToClient("products/test-product5");
            driver.FindElement(By.CssSelector(".btn-confirm.icon-bag-before")).Click();

            GoToClient("cart");

            VerifyIsTrue(driver.FindElements(By.CssSelector(".cart-full-buttons .btn-disabled")).Count == 1, " btn not enable");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".cart-full-error")).Displayed, "error masange");
            VerifyAreEqual("Минимальная сумма заказа: 1 000 руб.. Вам необходимо приобрести еще товаров на сумму: 500 руб.", driver.FindElement(By.CssSelector(".cart-full-error")).Text, "error masange text");

            GoToClient("products/test-product10");
            driver.FindElement(By.CssSelector(".btn-confirm.icon-bag-before")).Click();

            GoToClient("cart");
            
            VerifyIsTrue(driver.FindElements(By.CssSelector(".cart-full-buttons .btn-disabled")).Count == 0, " btn not enable");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".cart-full-error")).Count == 0, "error masange");


            GoToAdmin("settingscheckout");
            driver.FindElement(By.Id("MinimalOrderPriceForDefaultGroup")).Clear();
            driver.FindElement(By.Id("MinimalOrderPriceForDefaultGroup")).SendKeys("100");
            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
            Thread.Sleep(2000);

            VerifyFinally(testname);
        }

        [Test]
        public void NumberCart()
        {
            testname = "NumberCart";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");

            if (!driver.FindElement(By.Id("ShowClientId")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"ShowClientId\"]")).Click();
                Thread.Sleep(2000);
            }
            try
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
                Thread.Sleep(2000);
            }
            catch { }
           
            GoToClient();

            VerifyIsTrue(driver.FindElement(By.CssSelector(".site-head-userid")).Displayed, " dispday cart");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".site-head-userid")).Text.Contains("Номер корзины:"), " number cart");          

            VerifyFinally(testname);
        }

        [Test]
        public void NumberCartNo()
        {
            testname = "NumberCartNo";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");

            if (driver.FindElement(By.Id("ShowClientId")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"ShowClientId\"]")).Click();
                Thread.Sleep(2000);
            }
            try
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
                Thread.Sleep(2000);
            }
            catch { }

            GoToClient();

            VerifyIsTrue(driver.FindElements(By.CssSelector(".site-head-userid")).Count==0, " dispday cart");
            VerifyIsFalse(driver.PageSource.Contains("Номер корзины:"), " number cart");

            VerifyFinally(testname);
        }

        [Test]
        public void PaymentAfterManagerConfirmed()
        {
            testname = "ManagerConfirmed";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");

            if (!driver.FindElement(By.Id("ManagerConfirmed")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"ManagerConfirmed\"]")).Click();
                Thread.Sleep(2000);
            }
            try
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
                Thread.Sleep(2000);
            }
            catch { }

            GoToClient("products/test-product10");
            driver.FindElement(By.CssSelector(".btn-confirm.icon-bag-before")).Click();
            GoToClient("checkout");
            ScrollTo(By.Id("checkoutpayment"));
            XPathContainsText("span", "Пластиковая карта");
            driver.FindElement(By.CssSelector("input.btn.btn-big.btn-submit")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.PageSource.Contains("После подтверждения заказа менеджером, Вам будет отправлена ссылка на оплату."), "contain text");

            GoToClient("myaccount");
            driver.FindElement(By.CssSelector(".order-history-body-item")).Click();
            VerifyIsTrue(driver.FindElements(By.CssSelector(".btn.btn-confirm.btn-middle")).Count==0, "no btn");

            GoToAdmin("orders");
            GetGridCell(0, "StatusName").Click();
            Thread.Sleep(2000);

            VerifyIsTrue(driver.PageSource.Contains("Заказ подтвержден, разрешить оплату"), "contain text in order");
            VerifyIsFalse(driver.FindElement(By.Id("Order_ManagerConfirmed")).Selected, "no confirm in order");
            driver.FindElement(By.CssSelector(".edit-text .adv-checkbox-emul")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);

            GoToClient("myaccount");
            driver.FindElement(By.CssSelector(".order-history-body-item")).Click();
            VerifyIsTrue(driver.FindElement(By.CssSelector(".btn.btn-confirm.btn-middle")).Displayed, "yes btn");

            VerifyFinally(testname);
        }

        [Test]
        public void PaymentNoManagerConfirmed()
        {
            testname = "PaymentNoManagerConfirmed";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");

            if (driver.FindElement(By.Id("ManagerConfirmed")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"ManagerConfirmed\"]")).Click();
                Thread.Sleep(2000);
            }
            try
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
                Thread.Sleep(2000);
            }
            catch { }

            GoToClient("products/test-product10");
            driver.FindElement(By.CssSelector(".btn-confirm.icon-bag-before")).Click();
            GoToClient("checkout");
            ScrollTo(By.Id("checkoutpayment"));
            XPathContainsText("span", "Пластиковая карта");
            driver.FindElement(By.CssSelector("input.btn.btn-big.btn-submit")).Click();
            Thread.Sleep(2000);
            VerifyIsFalse(driver.PageSource.Contains("После подтверждения заказа менеджером, Вам будет отправлена ссылка на оплату."), "contain text");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".btn.btn-confirm.btn-middle")).Displayed, "btn payment");

            GoToClient("myaccount");
            driver.FindElement(By.CssSelector(".order-history-body-item")).Click();
            VerifyIsTrue(driver.FindElement(By.CssSelector(".btn.btn-confirm.btn-middle")).Displayed, "yes btn");

            GoToAdmin("orders");
            GetGridCell(0, "StatusName").Click();
            Thread.Sleep(2000);

            VerifyIsFalse(driver.PageSource.Contains("Заказ подтвержден, разрешить оплату"), "contain text in order");
            VerifyIsTrue(driver.FindElements(By.Id("Order_ManagerConfirmed")).Count==0, "no confirm in order");         

            VerifyFinally(testname);
        }

        [Test]
        public void PreOrder()
        {
            testname = "PreOrder";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");

            (new SelectElement(driver.FindElement(By.Id("OutOfStockAction")))).SelectByText("Создавать заказ");

            try
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
                Thread.Sleep(2000);
            }
            catch { }

            GoToClient("products/test-product99");
            VerifyIsTrue(driver.FindElements(By.XPath("//span[contains(text(), 'Под заказ')]")).Count==0, " no btn preorder");
            VerifyIsTrue(driver.FindElement(By.XPath("//span[contains(text(), 'Нет в наличии')]")).Displayed, "text  no preorder");

            GoToClient("products/test-product100");
            VerifyIsTrue(driver.FindElement(By.XPath("//span[contains(text(), 'Нет в наличии')]")).Displayed, "text preorder");
            VerifyIsTrue(driver.FindElement(By.XPath("//span[contains(text(), 'Под заказ')]")).Displayed, "btn preorder");
            XPathContainsText("span", "Под заказ");

            VerifyIsTrue(driver.Url.EndsWith("preorder?offerId=100&amount=1"), "url pre order");
            VerifyAreEqual("Оформление под заказ", driver.FindElement(By.TagName("h1")).Text, "h1 pre order");
            VerifyAreEqual("TestProduct100", driver.FindElement(By.CssSelector(".h1")).Text, "h1 pre order product");

            driver.FindElement(By.Id("name")).Clear();
            driver.FindElement(By.Id("name")).SendKeys("TestName");
            driver.FindElement(By.Id("email")).Clear();
            driver.FindElement(By.Id("email")).SendKeys("test@mail.mail");

            driver.FindElement(By.CssSelector(".btn.btn-small.btn-submit")).Click();

            VerifyAreEqual("Покупка товара под заказ", driver.FindElement(By.TagName("h1")).Text, "h1 after pre order");
            VerifyAreEqual("После получения Вашей заявки, наши менеджеры сообщат Вам сроки поставки и точную цену товара. В письмо также будет ссылка на оформление заказа, которая будет действовать в течение 24 часов. С ее помощью Вы сможете оплатить Заказ, тем самым завершив процедуру оформления.", driver.FindElement(By.CssSelector(".site-body-cell .static-block")).Text, "h1 pre order text");
            
            GoToAdmin("orders");
            VerifyAreEqual("Новый", GetGridCell(0, "StatusName").Text, "status");
            VerifyAreEqual("TestName", GetGridCell(0, "BuyerName").Text, "name");
            VerifyAreEqual("10 000 руб.", GetGridCell(0, "SumFormatted").Text, "sum");
            GetGridCell(0, "StatusName").Click();

            IWebElement selectCatalogView = driver.FindElement(By.Id("Order_OrderSourceId"));
            SelectElement select = new SelectElement(selectCatalogView);
            VerifyIsTrue(select.SelectedOption.Text.Contains("Под заказ"), "source order");

            VerifyFinally(testname);
        }

        [Test]
        public void PreOrderLead()
        {
            testname = "PreOrderLead";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");

            (new SelectElement(driver.FindElement(By.Id("OutOfStockAction")))).SelectByText("Создавать лид");

            try
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
                Thread.Sleep(2000);
            }
            catch { }

            GoToClient("products/test-product99");
            VerifyIsTrue(driver.FindElements(By.XPath("//span[contains(text(), 'Под заказ')]")).Count==0, " no btn preorder");
            VerifyIsTrue(driver.FindElement(By.XPath("//span[contains(text(), 'Нет в наличии')]")).Displayed, "text  no preorder");

            GoToClient("products/test-product100");
            VerifyIsTrue(driver.FindElement(By.XPath("//span[contains(text(), 'Нет в наличии')]")).Displayed, "text preorder");
            VerifyIsTrue(driver.FindElement(By.XPath("//span[contains(text(), 'Под заказ')]")).Displayed, "btn preorder");
            XPathContainsText("span", "Под заказ");

            VerifyIsTrue(driver.Url.EndsWith("preorder?offerId=100&amount=1"), "url pre order");
            VerifyAreEqual("Оформление под заказ", driver.FindElement(By.TagName("h1")).Text, "h1 pre order");
            VerifyAreEqual("TestProduct100", driver.FindElement(By.CssSelector(".h1")).Text, "h1 pre order product");

            driver.FindElement(By.Id("name")).Clear();
            driver.FindElement(By.Id("name")).SendKeys("TestNameLead");
            driver.FindElement(By.Id("email")).Clear();
            driver.FindElement(By.Id("email")).SendKeys("test@mail.mail");

            driver.FindElement(By.CssSelector(".btn.btn-small.btn-submit")).Click();

            VerifyAreEqual("Покупка товара под заказ", driver.FindElement(By.TagName("h1")).Text, "h1 after pre order");
            VerifyAreEqual("После получения Вашей заявки, наши менеджеры сообщат Вам сроки поставки и точную цену товара. В письмо также будет ссылка на оформление заказа, которая будет действовать в течение 24 часов. С ее помощью Вы сможете оплатить Заказ, тем самым завершив процедуру оформления.", driver.FindElement(By.CssSelector(".site-body-cell .static-block")).Text, "h1 pre order text");

            GoToAdmin("leads");
            VerifyAreEqual("Новый", GetGridCell(0, "DealStatusName").Text, "status");
            VerifyAreEqual("10000", GetGridCell(0, "Sum").Text, "sum");
            GetGridCell(0, "DealStatusName").Click();        

            IWebElement selectCatalogView = driver.FindElement(By.Id("Lead_OrderSourceId"));
            SelectElement select = new SelectElement(selectCatalogView);
            VerifyIsTrue(select.SelectedOption.Text.Contains("Под заказ"), "source lead");

            VerifyFinally(testname);
        }

        [Test]
        public void PreOrderToCart()
        {
            testname = "PreOrderToCart";
            VerifyBegin(testname);
            GoToAdmin("settingscheckout");

            (new SelectElement(driver.FindElement(By.Id("OutOfStockAction")))).SelectByText("Разрешить добавлять в корзину");

            try
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCheckoutSave\"]")).Click();
                Thread.Sleep(2000);
            }
            catch { }

            GoToClient("products/test-product99");
            VerifyIsTrue(driver.FindElements(By.XPath("//span[contains(text(), 'Под заказ')]")).Count == 0, " no btn preorder");
            VerifyIsTrue(driver.FindElement(By.XPath("//span[contains(text(), 'Нет в наличии')]")).Displayed, "text  no preorder");
            VerifyIsTrue(driver.FindElements(By.XPath("//a[contains(text(), 'Добавить')]")).Count > 0 , "  btn order");
            
            driver.FindElement(By.CssSelector(".btn-confirm.icon-bag-before")).Click();
            GoToClient("cart");
            VerifyAreEqual("Под заказ", driver.FindElements(By.CssSelector(".cart-amount-error"))[1].Text, "text pre order");

            GoToClient("checkout");
            ScrollTo(By.TagName("footer"));
            driver.FindElement(By.CssSelector("input.btn.btn-big.btn-submit")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Спасибо, ваш заказ оформлен!", driver.FindElement(By.TagName("h1")).Text, "success");

            GoToAdmin("orders");
            VerifyAreEqual("Новый", GetGridCell(0, "StatusName").Text, "status");
            VerifyAreEqual("9 900 руб.", GetGridCell(0, "SumFormatted").Text, "sum");

            VerifyFinally(testname);
        }
    }
}