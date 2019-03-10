using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Settings.BonusSystem.Client
{
    [TestFixture]
    public class BonusSystemClientTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog| ClearType.Orders| ClearType.Customers);
            InitializeService.LoadData(

             "Data\\Admin\\Settings\\BonusSystem\\Catalog.Product.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Catalog.Offer.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Catalog.Category.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Catalog.ProductCategories.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Customers.CustomerGroup.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Customers.Customer.csv",
              "Data\\Admin\\SettingTasks\\[Order].OrderSource.csv",
              "Data\\Admin\\SettingTasks\\[Order].OrderStatus.csv",
              "Data\\Admin\\SettingTasks\\[Order].PaymentMethod.csv",
              "Data\\Admin\\SettingTasks\\[Order].ShippingMethod.csv"
                );
            Init();
        }
        int num_order = 0;
        string numberCart = "";
        [Test]
        public void ChecktPlatinBonusCart()
        {
            GoToAdmin("settingsbonus");
            testname = "ChecktPlatinBonusCart";
            VerifyBegin(testname);

            (new SelectElement(driver.FindElement(By.Id("BonusGradeId")))).SelectByText("Платиновый");
            driver.FindElement(By.Id("CardNumTo")).SendKeys("1");

            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsBonusSave\"]")).Click();
            Thread.Sleep(2000);
            IWebElement selectElem1 = driver.FindElement(By.Id("BonusGradeId"));
            SelectElement select3 = new SelectElement(selectElem1);
            VerifyIsTrue(select3.AllSelectedOptions[0].Text.Contains("Платиновый"), "select item Platin");
        
            ProductToCard("+30 руб. на бонусную карту");
            GoToClient("cart");
            VerifyAreEqual("Сумма баллов начисляемых на бонусную карту: +30 руб.", driver.FindElement(By.CssSelector(".bonus-card-block-inline")).Text, "icon adn text bonus card ");
            VerifyAreEqual("30 руб.", driver.FindElement(By.CssSelector(".bonus-card-block-inline span")).Text, "Count bonus to card");

            ScrollTo(By.CssSelector(".bonus-card-block-inline span"));
            driver.FindElement(By.CssSelector("[data-ng-href=\"checkout\"]")).Click();
            Thread.Sleep(5000);
            ScrollTo(By.Name("WantBonusCard"));
            driver.FindElement(By.Name("WantBonusCard")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("+ 30 бонусов", driver.FindElement(By.CssSelector(".bonus-card-plus-price")).Text, "bonus count in checkout checkbox. Platin");

            //Сделать провеку в блоке
            //VerifyIsTrue(driver.FindElement(By.CssSelector(".checkout-cart-wrap")).Text.Contains("Бонусов на карту"), "bonus name in checkout. Platin");
            //VerifyIsTrue(driver.FindElement(By.CssSelector(".checkout-cart-wrap")).Text.Contains("30  руб."), "bonus count in checkout. Platin");

            ScrollTo(By.CssSelector(".footer-menu-head"));
            driver.FindElement(By.CssSelector(".js-checkout-form .btn.btn-big.btn-submit")).Click();
            Thread.Sleep(5000);
            VerifyIsTrue(driver.Url.Contains("checkout/success"), "success checkout order");
            VerifyIsTrue( driver.PageSource.Contains("30 бонусов"), "Count bonus after order");

            VerifyFinally(testname);
        }
        [Test]
        public void CheckzBeforePay()
        {
            GoToAdmin("cards");
            testname = "CheckzBeforePay";
            VerifyBegin(testname);

            numberCart = GetGridCell(0, "CardNumber").Text;
            VerifyAreEqual("Ad Admin", GetGridCell(0, "FIO").Text, " Grid  BuyerName");
            VerifyAreEqual("Платиновый", GetGridCell(0, "GradeName").Text, " Grid graid ");
            VerifyAreEqual("30", GetGridCell(0, "GradePersent").Text, " Grid percent");

            GetGridCell(0, "CardNumber").Click();
            Thread.Sleep(2000);

            VerifyAreEqual(numberCart, driver.FindElement(By.CssSelector("[data-e2e=\"numberCardBonus\"]")).Text, " in str card");

            IWebElement selectElem1 = driver.FindElement(By.CssSelector("[data-e2e=\"gradeBonus\"] select"));
            SelectElement select3 = new SelectElement(selectElem1);
            VerifyIsTrue(select3.AllSelectedOptions[0].Text.Contains("Платиновый"), "select item Platin");

            VerifyAreEqual("0 бонусов\r\n0 основных и 0 дополнительных", driver.FindElement(By.CssSelector("[data-e2e=\"countBonus")).Text, " in str card");

            VerifyAreEqual("Ad", driver.FindElement(By.CssSelector("[data-e2e=\"bonusCardSuname\"]")).Text, " bonusCardSuname in str card");
            VerifyAreEqual("Admin", driver.FindElement(By.CssSelector("[data-e2e=\"bonusCardName\"]")).Text, " bonusCardName in str card");
            VerifyAreEqual("1", driver.FindElement(By.Id("viewTableSells")).FindElements(By.TagName("td"))[0].Text, " number operition");
            //номер заказа на локале - 4, при выполнении на тачке  - 3
            VerifyAreEqual("3", driver.FindElement(By.Id("viewTableSells")).FindElements(By.TagName("td"))[1].Text, " number order");
            VerifyAreEqual("100", driver.FindElement(By.Id("viewTableSells")).FindElements(By.TagName("td"))[3].Text, " sum order");
            VerifyAreEqual("30", driver.FindElement(By.Id("viewTableSells")).FindElements(By.TagName("td"))[8].Text, " count bonus order");
            VerifyAreEqual("Ожидание платежа", driver.FindElement(By.Id("viewTableSells")).FindElements(By.TagName("td"))[10].Text, " complite order");

            VerifyAreEqual("Ни одной записи не найдено", driver.FindElement(By.Id("viewTableAdditionalBonus")).FindElements(By.TagName("td"))[0].Text, " no  bonus table additional");
            VerifyAreEqual("Ни одной записи не найдено", driver.FindElement(By.Id("viewTableTransaction")).FindElements(By.TagName("td"))[0].Text, " no  bonus table transaction");

            GoToClient("myaccount#?tab=bonusTab");

            VerifyAreEqual("0", driver.FindElement(By.CssSelector("[data-ng-bind=\"bonusInfo.bonusData.BonusAmount\"]")).Text, " amount Bonus in lk");
            VerifyAreEqual("30", driver.FindElement(By.CssSelector("[data-ng-bind=\"bonusInfo.bonusData.BonusPercent\"]")).Text, " percent Bonus in lk");
            VerifyAreEqual(numberCart, driver.FindElement(By.CssSelector("[data-ng-bind=\"bonusInfo.bonusData.CardNumber\"]")).Text, " number BonusCart in lk");

            VerifyFinally(testname);
        }

        [Test]
        public void CheckzInAdminPay()
        {
            GoToAdmin("orders");
            testname = "CheckzInAdminPay";
            VerifyBegin(testname);

            VerifyAreEqual("3", GetGridCell(0, "Number").Text, "Grid Gifts Number");
            VerifyAreEqual("Новый", GetGridCell(0, "StatusName").Text, " Grid orders StatusName");
            VerifyAreEqual("Admin Ad", GetGridCell(0, "BuyerName").Text, " Grid orders BuyerName");
          
            GetGridCell(0, "Number").Click();
            Thread.Sleep(2000);
            
            VerifyAreEqual("Ad", driver.FindElement(By.Id("Order_OrderCustomer_LastName")).GetAttribute("value"), "\r\n surename in cart");
            VerifyAreEqual("Admin", driver.FindElement(By.Id("Order_OrderCustomer_FirstName")).GetAttribute("value"), "\r\n name in cart");

            IWebElement selectElem = driver.FindElement(By.Id("Order_OrderSourceId"));
            SelectElement select = new SelectElement(selectElem);
            VerifyAreEqual("Корзина интернет магазина", (select.AllSelectedOptions[0].Text), "\r\n Source");

            VerifyAreEqual("TestProduct1\r\nАртикул: 1", GetGridCell(0, "Name", "OrderItems").Text, " Name product at order");
            VerifyAreEqual("100", GetGridCell(0, "Price", "OrderItems").FindElement(By.TagName("input")).GetAttribute("value").ToString(), " product at order");
            VerifyAreEqual("1", GetGridCell(0, "Amount", "OrderItems").FindElement(By.TagName("input")).GetAttribute("value").ToString(), " Count product at order");
            //VerifyAreEqual("в наличии", GetGridCell(0, "Available", "OrderItems").Text, " Available product at order");
            VerifyIsTrue(GetGridCell(0, "Cost", "OrderItems").Text.Contains("100 "), " Cost product at order");

            VerifyAreEqual(numberCart, driver.FindElement(By.CssSelector("[data-e2e=\"numberBonusCart\"]")).Text, " number Cart in order");
            VerifyAreEqual("Баллы", driver.FindElement(By.CssSelector("[data-e2e=\"countBonus\"]")).Text, " product at order");
            VerifyAreEqual("0 (Платиновый 30 %)", driver.FindElement(By.CssSelector("[data-e2e=\"percentBonus\"]")).Text, " percent Bonus in order");
            Refresh();
            driver.FindElement(By.CssSelector(".switcher-state-label span")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual(numberCart, driver.FindElement(By.CssSelector("[data-e2e=\"numberBonusCart\"]")).Text, " Name product at order");
            VerifyAreEqual("Баллы", driver.FindElement(By.CssSelector("[data-e2e=\"countBonus\"]")).Text, " product at order");
            VerifyAreEqual("30 (Платиновый 30 %)", driver.FindElement(By.CssSelector("[data-e2e=\"percentBonus\"]")).Text, " percent Bonus in order");

            VerifyFinally(testname);
        }
        [Test]
        public void CheckzInLkAfterPay()
        {
            GoToClient("myaccount#?tab=bonusTab");
            testname = "CheckzInLkAfterPay";
            VerifyBegin(testname);

            VerifyAreEqual("30", driver.FindElement(By.CssSelector("[data-ng-bind=\"bonusInfo.bonusData.BonusAmount\"]")).Text, " amount Bonus in lk");
            VerifyAreEqual("30", driver.FindElement(By.CssSelector("[data-ng-bind=\"bonusInfo.bonusData.BonusPercent\"]")).Text, " percent Bonus in lk");
            VerifyAreEqual(numberCart, driver.FindElement(By.CssSelector("[data-ng-bind=\"bonusInfo.bonusData.CardNumber\"]")).Text, " number BonusCart in lk");

            GoToAdmin("cards");

            numberCart = GetGridCell(0, "CardNumber").Text;
            VerifyAreEqual("Ad Admin", GetGridCell(0, "FIO").Text, " Grid  BuyerName");
            VerifyAreEqual("Платиновый", GetGridCell(0, "GradeName").Text, " Grid graid ");
            VerifyAreEqual("30", GetGridCell(0, "GradePersent").Text, " Grid percent");

            GetGridCell(0, "CardNumber").Click();
            Thread.Sleep(2000);

            VerifyAreEqual(numberCart, driver.FindElement(By.CssSelector("[data-e2e=\"numberCardBonus\"]")).Text, " in str card");

            IWebElement selectElem1 = driver.FindElement(By.CssSelector("[data-e2e=\"gradeBonus\"] select"));
            SelectElement select3 = new SelectElement(selectElem1);
            VerifyIsTrue(select3.AllSelectedOptions[0].Text.Contains("Платиновый"), "select item Platin");

            VerifyAreEqual("30 бонусов\r\n30 основных и 0 дополнительных", driver.FindElement(By.CssSelector("[data-e2e=\"countBonus")).Text, " in str card");

            VerifyAreEqual("Ad", driver.FindElement(By.CssSelector("[data-e2e=\"bonusCardSuname\"]")).Text, " bonusCardSuname in str card");
            VerifyAreEqual("Admin", driver.FindElement(By.CssSelector("[data-e2e=\"bonusCardName\"]")).Text, " bonusCardName in str card");

            VerifyAreEqual("1", driver.FindElement(By.Id("viewTableSells")).FindElements(By.TagName("td"))[0].Text, " number operition table sells");
            VerifyAreEqual("3", driver.FindElement(By.Id("viewTableSells")).FindElements(By.TagName("td"))[1].Text, " number order table sells");
            VerifyAreEqual("100", driver.FindElement(By.Id("viewTableSells")).FindElements(By.TagName("td"))[3].Text, " sum order table sells");
            VerifyAreEqual("30", driver.FindElement(By.Id("viewTableSells")).FindElements(By.TagName("td"))[8].Text, " count bonus order table sells");
            VerifyAreEqual("Завершена", driver.FindElement(By.Id("viewTableSells")).FindElements(By.TagName("td"))[10].Text, " complite order table sells");


            VerifyAreEqual("+30", driver.FindElement(By.Id("viewTableTransaction")).FindElements(By.TagName("td"))[2].Text, " count bonus  table transaction");
            VerifyAreEqual("30", driver.FindElement(By.Id("viewTableTransaction")).FindElements(By.TagName("td"))[3].Text, "  saldo table transaction");
            VerifyAreEqual("1", driver.FindElement(By.Id("viewTableTransaction")).FindElements(By.TagName("td"))[8].Text, " number sell table transaction");


            VerifyFinally(testname);
        }
  [Test]
        public void CheckzPaymentByBonus()
        {
            testname = "CheckzPaymentByBonus";
            VerifyBegin(testname);
            ReInitClient();
            Refresh();
            ProductToCard("+30 руб. на бонусную карту");
            GoToClient("cart");
            VerifyAreEqual("Сумма баллов начисляемых на бонусную карту: +30 руб.", driver.FindElement(By.CssSelector(".bonus-card-block-inline")).Text, "icon adn text bonus card ");
            VerifyAreEqual("30 руб.", driver.FindElement(By.CssSelector(".bonus-card-block-inline span")).Text, "Count bonus to card");

            ScrollTo(By.CssSelector(".bonus-card-block-inline span"));
            driver.FindElement(By.CssSelector("[data-ng-href=\"checkout\"]")).Click();
            Thread.Sleep(3000);
            Refresh();
            Thread.Sleep(2000);

            VerifyIsTrue(driver.FindElements(By.CssSelector(".bonus-card-plus-price")).Count == 0, "Undisplay sum bonus");
            VerifyIsTrue(driver.FindElements(By.Id("email")).Count == 0, "unDisplay email registration");
            VerifyIsTrue(driver.FindElements(By.Id("password")).Count == 0, "unDisplay password registration");
             VerifyIsFalse(driver.FindElement(By.CssSelector(".checkout-usertype-label input")).Selected, "Unselect checkBox registration");
            //Сделать!!!
            driver.FindElement(By.CssSelector(".checkout-usertype-label input")).Click();
            VerifyIsTrue(driver.FindElement(By.CssSelector(".checkout-usertype-label input")).Selected, "select checkBox registration");
            VerifyIsTrue(driver.FindElements(By.Id("email")).Count == 1, "Display email registration");
            VerifyIsTrue(driver.FindElements(By.Id("password")).Count == 1, "Display password registration");
            
            driver.FindElement(By.Id("email")).SendKeys("admin");
            driver.FindElement(By.Id("password")).SendKeys("123123");
            VerifyIsTrue(driver.FindElements(By.Id("Data_User_WantRegist")).Count == 0, "Display checkBox registration");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".btn.btn-submit.btn-middle")).Click();
            Thread.Sleep(2000);

            ScrollTo(By.Id("isBonusApply"));
            VerifyAreEqual("Бонусами по карте (у вас 30,0 бонусов)", driver.FindElement(By.CssSelector(".custom-input-text")).Text, "bonus count in checkout checkbox. Platin");
            driver.FindElement(By.Id("isBonusApply")).Click();
            Thread.Sleep(1000);
           
            ScrollTo(By.CssSelector(".footer-menu-head"));
            VerifyAreEqual("70 руб.", driver.FindElement(By.CssSelector(".checkout-result-price")).Text, " checkout rezult. Platin");
            VerifyAreEqual("21 руб.", driver.FindElement(By.CssSelector(".checkout-bonus-result")).FindElement(By.CssSelector(".checkout-result-price")).Text, "bonus count in checkout rezult. Platin");
            driver.FindElement(By.CssSelector(".js-checkout-form .btn.btn-big.btn-submit")).Click();
            Thread.Sleep(3000);
            VerifyAreEqual("21 бонусов", driver.FindElement(By.TagName("h2")).Text, "Count bonus after order");

            GoToAdmin("cards");
            GetGridCell(0, "CardNumber").Click();
            Thread.Sleep(2000);

            VerifyAreEqual("0 бонусов\r\n0 основных и 0 дополнительных", driver.FindElement(By.CssSelector("[data-e2e=\"countBonus")).Text, " in str card");
            VerifyAreEqual("Ad", driver.FindElement(By.CssSelector("[data-e2e=\"bonusCardSuname\"]")).Text, " bonusCardSuname in str card");
            VerifyAreEqual("Admin", driver.FindElement(By.CssSelector("[data-e2e=\"bonusCardName\"]")).Text, " bonusCardName in str card");
            VerifyAreEqual("2", driver.FindElement(By.Id("viewTableSells")).FindElements(By.TagName("td"))[0].Text, " number operition");
            VerifyAreEqual("4", driver.FindElement(By.Id("viewTableSells")).FindElements(By.TagName("td"))[1].Text, " number order");
            VerifyAreEqual("100", driver.FindElement(By.Id("viewTableSells")).FindElements(By.TagName("td"))[3].Text, " sum order");
            VerifyAreEqual("70", driver.FindElement(By.Id("viewTableSells")).FindElements(By.TagName("td"))[5].Text, " count pay money order");
            VerifyAreEqual("30", driver.FindElement(By.Id("viewTableSells")).FindElements(By.TagName("td"))[6].Text, " count pay bonus order");
            VerifyAreEqual("21", driver.FindElement(By.Id("viewTableSells")).FindElements(By.TagName("td"))[8].Text, " count bonus order");
            VerifyAreEqual("Ожидание платежа", driver.FindElement(By.Id("viewTableSells")).FindElements(By.TagName("td"))[10].Text, " complite order");

            VerifyAreEqual("-30", driver.FindElement(By.Id("viewTableTransaction")).FindElements(By.TagName("td"))[1].Text, " count bonus  table transaction");
            VerifyAreEqual("0", driver.FindElement(By.Id("viewTableTransaction")).FindElements(By.TagName("td"))[3].Text, "  saldo table transaction");

            GoToAdmin("orders");           
            VerifyAreEqual("Новый", GetGridCell(0, "StatusName").Text, " Grid orders StatusName");
            VerifyAreEqual("Admin Ad", GetGridCell(0, "BuyerName").Text, " Grid orders BuyerName");

            GetGridCell(0, "Number").Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Ad", driver.FindElement(By.Id("Order_OrderCustomer_LastName")).GetAttribute("value"), "\r\n surename in cart");
            VerifyAreEqual("Admin", driver.FindElement(By.Id("Order_OrderCustomer_FirstName")).GetAttribute("value"), "\r\n name in cart");
            VerifyAreEqual("Баллы", driver.FindElement(By.CssSelector("[data-e2e=\"countBonus\"]")).Text, " product at order");
            VerifyAreEqual("0 (Платиновый 30 %)", driver.FindElement(By.CssSelector("[data-e2e=\"percentBonus\"]")).Text, " percent Bonus in order");
            VerifyAreEqual("- 30", driver.FindElements(By.CssSelector(".col-xs.flex-grow-n.order-items-summary-col-value"))[2].Text, "\r\n bonus from order");
           
            GoToClient("myaccount#?tab=bonusTab");

            VerifyAreEqual("0", driver.FindElement(By.CssSelector("[data-ng-bind=\"bonusInfo.bonusData.BonusAmount\"]")).Text, " amount Bonus in lk");
            VerifyAreEqual("30", driver.FindElement(By.CssSelector("[data-ng-bind=\"bonusInfo.bonusData.BonusPercent\"]")).Text, " percent Bonus in lk");
            VerifyAreEqual(numberCart, driver.FindElement(By.CssSelector("[data-ng-bind=\"bonusInfo.bonusData.CardNumber\"]")).Text, " number BonusCart in lk");
            
            VerifyFinally(testname);
        }
       
        [Test]
        public void NewClientCheck()
        {
            testname = "NewClientCheck";
            VerifyBegin(testname);
            ReInitClient();
            Refresh();
            ProductToCard("+30 руб. на бонусную карту");
            GoToClient("cart");
            VerifyAreEqual("Сумма баллов начисляемых на бонусную карту: +30 руб.", driver.FindElement(By.CssSelector(".bonus-card-block-inline")).Text, "icon adn text bonus card ");
            VerifyAreEqual("30 руб.", driver.FindElement(By.CssSelector(".bonus-card-block-inline span")).Text, "Count bonus to card");

            ScrollTo(By.CssSelector(".bonus-card-block-inline span"));
            driver.FindElement(By.CssSelector("[data-ng-href=\"checkout\"]")).Click();
            Thread.Sleep(3000);
            Refresh();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("Data_User_Email")).SendKeys("newlogin@log.in");
            driver.FindElement(By.Id("Data_User_FirstName")).SendKeys("newname");
            driver.FindElement(By.Id("Data_User_LastName")).SendKeys("newsuname");
            driver.FindElement(By.Id("Data_User_Phone")).SendKeys("89998887766");

            VerifyIsTrue(driver.FindElements(By.Id("Data_User_WantRegist")).Count == 1, "Display checkBox registration");
            VerifyIsTrue(driver.FindElements(By.Id("password")).Count == 0, "Undisplay checkBox registration");
            VerifyIsTrue(driver.FindElements(By.Id("passwordRepeat")).Count == 0, "Undisplay checkBox registration");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".bonus-card-plus-price")).Count == 0, "Undisplay sum bonus");
            VerifyAreEqual("30", driver.FindElement(By.CssSelector(".nowrap")).Text, "display sum bonus in checkBox");

            VerifyIsFalse(driver.FindElement(By.Id("Data_User_WantRegist")).Selected, "Unselect checkBox registration");

            driver.FindElement(By.Id("Data_User_WantRegist")).Click();
            VerifyIsTrue(driver.FindElement(By.Id("Data_User_WantRegist")).Selected, "Select checkBox registration");
            VerifyIsTrue(driver.FindElements(By.Id("password")).Count == 1, "Display checkBox registration");
            VerifyIsTrue(driver.FindElements(By.Id("passwordRepeat")).Count == 1, "Display checkBox registration");

            driver.FindElement(By.Id("password")).SendKeys("123123");
            driver.FindElement(By.Id("passwordRepeat")).SendKeys("123123");

            ScrollTo(By.Name("CustomerComment"));

            VerifyAreEqual("30 руб.", driver.FindElement(By.CssSelector(".checkout-bonus-result")).FindElement(By.CssSelector(".checkout-result-price")).Text, "bonus count in checkout rezult. Platin");
            driver.FindElement(By.CssSelector(".js-checkout-form .btn.btn-big.btn-submit")).Click();
            Thread.Sleep(5000);

            VerifyAreEqual("30 бонусов", driver.FindElement(By.TagName("h2")).Text, "Count bonus after order");          

            VerifyFinally(testname);
        }

        public void ProductToCard(string bonus)
        {
            GoToClient("products/test-product1");
            if (driver.FindElement(By.CssSelector(".cart-mini span")).Text.Contains("пусто"))
            {
                Refresh();
                ScrollTo(By.CssSelector(".rating"));
                VerifyAreEqual(bonus, driver.FindElement(By.CssSelector(".bonus-string-sum")).Text, "Count bonus in product cart");
                driver.FindElement(By.CssSelector(".details-payment-inline a")).Click();
                Thread.Sleep(2000);
            }
        }
    }
}
