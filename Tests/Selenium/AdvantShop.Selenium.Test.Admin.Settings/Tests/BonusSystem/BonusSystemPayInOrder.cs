using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.BonusSystem.PayInOrder
{
    [TestFixture]
    public class BonusSystemPayInOrder : BaseMultiSeleniumTest
    {
        [SetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.Bonuses);
            InitializeService.LoadData(

             "Data\\Admin\\Settings\\BonusSystem\\PayByBonusInOrder\\Catalog.Product.csv",
           "Data\\Admin\\Settings\\BonusSystem\\PayByBonusInOrder\\Catalog.Offer.csv",
           "Data\\Admin\\Settings\\BonusSystem\\PayByBonusInOrder\\Catalog.Category.csv",
           "Data\\Admin\\Settings\\BonusSystem\\PayByBonusInOrder\\Catalog.ProductCategories.csv",
           "Data\\Admin\\Settings\\BonusSystem\\PayByBonusInOrder\\Customers.CustomerGroup.csv",
           "Data\\Admin\\Settings\\BonusSystem\\PayByBonusInOrder\\Customers.Customer.csv",
               "Data\\Admin\\Settings\\BonusSystem\\PayByBonusInOrder\\Bonus.Card.csv",
           "Data\\Admin\\Settings\\BonusSystem\\PayByBonusInOrder\\Bonus.Grade.csv",
              "Data\\Admin\\Settings\\BonusSystem\\PayByBonusInOrder\\Bonus.AdditionBonus.csv"
               


                );
            Init();
        }
         

        [Test]
        public void PayByMainBonusInOrder()
        {
            GoToAdmin("orders/add");
            testname = "PayByMainBonusInOrder";
            VerifyBegin(testname);

            driver.FindElement(By.LinkText("Выбрать покупателя")).Click();
            Thread.Sleep(3000);

            GetGridFilter().Click();
            GetGridFilter().SendKeys("FirstName1 LastName1");
            XPathContainsText("h2", "Выбор покупателя");

            XPathContainsText("a", "Выбрать");
            Thread.Sleep(2000);

            ScrollTo(By.Id("Order_OrderCustomer_Phone"));
            XPathContainsText("a", "Добавить товар");
            Thread.Sleep(2000);

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestProduct4");
            XPathContainsText("h2", "Выбор товара");

            GetGridCell(0, "selectionRowHeaderCol", "OffersSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();

            XPathContainsText("button", "Выбрать");
            Thread.Sleep(4000);

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(3000);

            VerifyAreEqual("530801", driver.FindElement(By.CssSelector("[data-e2e=\"numberBonusCart\"]")).Text, "card num");
            VerifyAreEqual("100 (Гостевой 3 %)", driver.FindElement(By.CssSelector("[data-e2e=\"percentBonus\"]")).Text, "card grade and percent");
            ScrollTo(By.CssSelector("[grid-unique-id=\"gridOrderItems\"]"));

            driver.FindElement(By.CssSelector("[data-e2e=\"BonusPay\"]")).Click();
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"BonusesAvailable\"]")).Text.Contains("доступно 100 бонусов"), "bonuses available");
            driver.FindElement(By.CssSelector("[data-e2e=\"BonusesUseAdd\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BonusesUseAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BonusesUseAdd\"]")).SendKeys("60");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"BonusesUse\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            Refresh();

            //check order
            VerifyAreEqual("40 (Гостевой 3 %)", driver.FindElement(By.CssSelector("[data-e2e=\"percentBonus\"]")).Text, "card num after bonus payment");
            VerifyAreEqual("- 60", driver.FindElement(By.CssSelector("[data-e2e=\"BonusCost\"]")).Text, "bonus cost after bonus payment");
            VerifyAreEqual("340 руб.", driver.FindElement(By.CssSelector("[data-e2e=\"OrderSum\"]")).Text, "order num after bonus payment");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"CountBonus\"]")).Text.Contains("10.20"), "bonus purchase amount");
            VerifyAreEqual("400", GetGridCell(0, "Price", "OrderItems").FindElement(By.TagName("input")).GetAttribute("value"), "order item price");
            VerifyIsTrue(GetGridCell(0, "Cost", "OrderItems").Text.Contains("400"), "order item cost");

            //check card purchase table
            GoToAdmin("cards/edit/2c8fb106-8f07-499b-b06f-51b43076f3c1");

            VerifyAreEqual("40 бонусов", driver.FindElement(By.CssSelector("[data-e2e=\"countBonus\"]")).FindElement(By.TagName("span")).Text, "bonus count in card");
            VerifyAreEqual("40 основных и 0 дополнительных", driver.FindElement(By.CssSelector("[data-e2e=\"countBonus\"]")).FindElement(By.TagName("div")).Text, "all bonuses count in card");

            VerifyAreEqual("2", driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseId\"]")).Text, "purchase PurchaseId");
            VerifyAreEqual("400", driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseFullAmount\"]")).Text, "purchase full amount");
            VerifyAreEqual("400", driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseDiscount\"]")).Text, "purchase PurchaseDiscount");
            VerifyAreEqual("340", driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseCash\"]")).Text, "purchase PurchaseCash");
            VerifyAreEqual("60", driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseMainBonuses\"]")).Text, "purchase PurchaseMainBonuses");
            VerifyAreEqual("0", driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseAdditionBonuses\"]")).Text, "purchase PurchaseAdditionBonuses");
            VerifyAreEqual("10,20", driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseAddNewBonuses\"]")).Text, "purchase PurchaseAddNewBonuses");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseStatus\"]")).Text.Contains("Ожидание платежа"), "purchase PurchaseStatus");

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"LinkToAllPurchases\"]")).FindElement(By.TagName("a")).GetAttribute("href").Contains("cards/allpurchase?CardId=2c8fb106-8f07-499b-b06f-51b43076f3c1"), "link to all purchases");
            
            //check card transaction table
            VerifyAreEqual("-60", driver.FindElement(By.CssSelector("[data-e2e=\"TransactionId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"TransactionMainBonusesUsed\"]")).Text, "Transaction MainBonuses Used");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"TransactionId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"TransactionMainBonusesAdded\"]")).Text, "Transaction MainBonuses Added");
            VerifyAreEqual("40", driver.FindElement(By.CssSelector("[data-e2e=\"TransactionId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"TransactionMainBonusesSaldo\"]")).Text, "Transaction MainBonuses Saldo");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"TransactionId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"TransactionAdditionBonusesUsed\"]")).Text, "Transaction AdditionBonuses Used");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"TransactionId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"TransactionAdditionBonusesAdded\"]")).Text, "Transaction AdditionBonuses Added");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"TransactionId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"TransactionAdditionBonusesSaldo\"]")).Text, "Transaction AdditionBonuses Saldo");
            VerifyAreEqual("2", driver.FindElement(By.CssSelector("[data-e2e=\"TransactionId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseId\"]")).Text, "Transaction PurchaseId");

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"LinkToAllTransactions\"]")).FindElement(By.TagName("a")).GetAttribute("href").Contains("cards/alltransaction?CardId=2c8fb106-8f07-499b-b06f-51b43076f3c1"), "link to all Transactions");

            //check card all purchases table
            GoToAdmin("cards/allpurchase?CardId=2c8fb106-8f07-499b-b06f-51b43076f3c1");

            VerifyAreEqual("2", driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseId\"]")).Text, "all purchase PurchaseId");
            VerifyAreEqual("400,00", driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseFullAmount\"]")).Text, "all purchase full amount");
            VerifyAreEqual("400,00", driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseDiscount\"]")).Text, "all purchase PurchaseDiscount");
            VerifyAreEqual("340,00", driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseCash\"]")).Text, "all purchase PurchaseCash");
            VerifyAreEqual("60,00", driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseMainBonuses\"]")).Text, "all purchase PurchaseMainBonuses");
            VerifyAreEqual("0,00", driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseAdditionBonuses\"]")).Text, "all purchase PurchaseAdditionBonuses");
            VerifyAreEqual("10,20", driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseAddNewBonuses\"]")).Text, "all purchase PurchaseAddNewBonuses");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseStatus\"]")).Text.Contains("Ожидание платежа"), "all purchase PurchaseStatus");

            //check card all transactions table
            GoToAdmin("cards/alltransaction?CardId=2c8fb106-8f07-499b-b06f-51b43076f3c1");

            VerifyAreEqual("-60,00", driver.FindElement(By.CssSelector("[data-e2e=\"TransactionId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"TransactionMainBonusesUsed\"]")).Text, "all Transaction MainBonuses Used");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"TransactionId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"TransactionMainBonusesAdded\"]")).Text, "all Transaction MainBonuses Added");
            VerifyAreEqual("40,00", driver.FindElement(By.CssSelector("[data-e2e=\"TransactionId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"TransactionMainBonusesSaldo\"]")).Text, "all Transaction MainBonuses Saldo");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"TransactionId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"TransactionAdditionBonusesUsed\"]")).Text, "all Transaction AdditionBonuses Used");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"TransactionId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"TransactionAdditionBonusesAdded\"]")).Text, "all Transaction AdditionBonuses Added");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"TransactionId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"TransactionAdditionBonusesSaldo\"]")).Text, "all Transaction AdditionBonuses Saldo");
            VerifyAreEqual("2", driver.FindElement(By.CssSelector("[data-e2e=\"TransactionId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseId\"]")).Text, "all Transaction PurchaseId");
            
            VerifyFinally(testname);
        }

        [Test]
        public void PayByAdditionBonusInOrder()
        {
            GoToAdmin("orders/add");
            testname = "PayByAdditionBonusInOrder";
            VerifyBegin(testname);

            driver.FindElement(By.LinkText("Выбрать покупателя")).Click();
            Thread.Sleep(3000);

            GetGridFilter().Click();
            GetGridFilter().SendKeys("FirstName8 LastName8");
            XPathContainsText("h2", "Выбор покупателя");

            XPathContainsText("a", "Выбрать");
            Thread.Sleep(2000);

            ScrollTo(By.Id("Order_OrderCustomer_Phone"));
            XPathContainsText("a", "Добавить товар");
            Thread.Sleep(2000);

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestProduct6");
            XPathContainsText("h2", "Выбор товара");

            GetGridCell(0, "selectionRowHeaderCol", "OffersSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();

            XPathContainsText("button", "Выбрать");
            Thread.Sleep(4000);

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(3000);

            VerifyAreEqual("530808", driver.FindElement(By.CssSelector("[data-e2e=\"numberBonusCart\"]")).Text, "card num");
            VerifyAreEqual("500 (Бронзовый 5 %)", driver.FindElement(By.CssSelector("[data-e2e=\"percentBonus\"]")).Text, "card grade and percent");
            ScrollTo(By.CssSelector("[grid-unique-id=\"gridOrderItems\"]"));

            driver.FindElement(By.CssSelector("[data-e2e=\"BonusPay\"]")).Click();
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"BonusesAvailable\"]")).Text.Contains("доступно 500 бонусов"), "bonuses available");
            driver.FindElement(By.CssSelector("[data-e2e=\"BonusesUseAdd\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"BonusesUseAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"BonusesUseAdd\"]")).SendKeys("100");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"BonusesUse\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            Refresh();

            //check order
            VerifyAreEqual("400 (Бронзовый 5 %)", driver.FindElement(By.CssSelector("[data-e2e=\"percentBonus\"]")).Text, "card num after bonus payment");
            VerifyAreEqual("- 100", driver.FindElement(By.CssSelector("[data-e2e=\"BonusCost\"]")).Text, "bonus cost after bonus payment");
            VerifyAreEqual("500 руб.", driver.FindElement(By.CssSelector("[data-e2e=\"OrderSum\"]")).Text, "order num after bonus payment");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"CountBonus\"]")).Text.Contains("25.00"), "bonus purchase amount");
            VerifyAreEqual("600", GetGridCell(0, "Price", "OrderItems").FindElement(By.TagName("input")).GetAttribute("value"), "order item price");
            VerifyIsTrue(GetGridCell(0, "Cost", "OrderItems").Text.Contains("600"), "order item cost");

            //check addition bonuses table
            GoToAdmin("cards/edit/2c8fb106-8f07-499b-b06f-51b43076f3c8");

            VerifyAreEqual("AdditionBonus Name 8", driver.FindElement(By.CssSelector("[data-e2e=\"AdditionBonusId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"AdditionBonusName\"]")).Text, "AdditionBonus Name");
            VerifyAreEqual("400", driver.FindElement(By.CssSelector("[data-e2e=\"AdditionBonusId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"AdditionBonusAmount\"]")).Text, "AdditionBonus Amount");
            VerifyAreEqual("Description8", driver.FindElement(By.CssSelector("[data-e2e=\"AdditionBonusId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"AdditionBonusDescription\"]")).Text, "AdditionBonus Description");
            VerifyAreEqual("05 05 2016", driver.FindElement(By.CssSelector("[data-e2e=\"AdditionBonusId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"AdditionBonusStartDate\"]")).Text, "AdditionBonus StartDate");
            VerifyAreEqual("05 05 2050", driver.FindElement(By.CssSelector("[data-e2e=\"AdditionBonusId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"AdditionBonusEndDate\"]")).Text, "AdditionBonus EndDate");

            //check card purchase table
            VerifyAreEqual("400 бонусов", driver.FindElement(By.CssSelector("[data-e2e=\"countBonus\"]")).FindElement(By.TagName("span")).Text, "bonus count in card");
            VerifyAreEqual("0 основных и 400 дополнительных", driver.FindElement(By.CssSelector("[data-e2e=\"countBonus\"]")).FindElement(By.TagName("div")).Text, "all bonuses count in card");

            VerifyAreEqual("2", driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseId\"]")).Text, "purchase PurchaseId");
            VerifyAreEqual("600", driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseFullAmount\"]")).Text, "purchase full amount");
            VerifyAreEqual("600", driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseDiscount\"]")).Text, "purchase PurchaseDiscount");
            VerifyAreEqual("500", driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseCash\"]")).Text, "purchase PurchaseCash");
            VerifyAreEqual("0", driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseMainBonuses\"]")).Text, "purchase PurchaseMainBonuses");
            VerifyAreEqual("100", driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseAdditionBonuses\"]")).Text, "purchase PurchaseAdditionBonuses");
            VerifyAreEqual("25", driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseAddNewBonuses\"]")).Text, "purchase PurchaseAddNewBonuses");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseStatus\"]")).Text.Contains("Ожидание платежа"), "purchase PurchaseStatus");

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"LinkToAllPurchases\"]")).FindElement(By.TagName("a")).GetAttribute("href").Contains("cards/allpurchase?CardId=2c8fb106-8f07-499b-b06f-51b43076f3c8"), "link to all purchases");

            //check card transaction table
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"TransactionId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"TransactionMainBonusesUsed\"]")).Text, "Transaction MainBonuses Used");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"TransactionId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"TransactionMainBonusesAdded\"]")).Text, "Transaction MainBonuses Added");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"TransactionId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"TransactionMainBonusesSaldo\"]")).Text, "Transaction MainBonuses Saldo");
            VerifyAreEqual("-100", driver.FindElement(By.CssSelector("[data-e2e=\"TransactionId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"TransactionAdditionBonusesUsed\"]")).Text, "Transaction AdditionBonuses Used");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"TransactionId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"TransactionAdditionBonusesAdded\"]")).Text, "Transaction AdditionBonuses Added");
            VerifyAreEqual("400", driver.FindElement(By.CssSelector("[data-e2e=\"TransactionId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"TransactionAdditionBonusesSaldo\"]")).Text, "Transaction AdditionBonuses Saldo");
            VerifyAreEqual("2", driver.FindElement(By.CssSelector("[data-e2e=\"TransactionId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseId\"]")).Text, "Transaction PurchaseId");

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"LinkToAllTransactions\"]")).FindElement(By.TagName("a")).GetAttribute("href").Contains("cards/alltransaction?CardId=2c8fb106-8f07-499b-b06f-51b43076f3c8"), "link to all Transactions");

            //check card all purchases table
            GoToAdmin("cards/allpurchase?CardId=2c8fb106-8f07-499b-b06f-51b43076f3c8");

            VerifyAreEqual("2", driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseId\"]")).Text, "all purchase PurchaseId");
            VerifyAreEqual("600,00", driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseFullAmount\"]")).Text, "all purchase full amount");
            VerifyAreEqual("600,00", driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseDiscount\"]")).Text, "all purchase PurchaseDiscount");
            VerifyAreEqual("500,00", driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseCash\"]")).Text, "all purchase PurchaseCash");
            VerifyAreEqual("0,00", driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseMainBonuses\"]")).Text, "all purchase PurchaseMainBonuses");
            VerifyAreEqual("100,00", driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseAdditionBonuses\"]")).Text, "all purchase PurchaseAdditionBonuses");
            VerifyAreEqual("25,00", driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseAddNewBonuses\"]")).Text, "all purchase PurchaseAddNewBonuses");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"PurchaseId-2\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseStatus\"]")).Text.Contains("Ожидание платежа"), "all purchase PurchaseStatus");

            //check card all transactions table
            GoToAdmin("cards/alltransaction?CardId=2c8fb106-8f07-499b-b06f-51b43076f3c8");

            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"TransactionId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"TransactionMainBonusesUsed\"]")).Text, "all Transaction MainBonuses Used");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"TransactionId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"TransactionMainBonusesAdded\"]")).Text, "all Transaction MainBonuses Added");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"TransactionId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"TransactionMainBonusesSaldo\"]")).Text, "all Transaction MainBonuses Saldo");
            VerifyAreEqual("-100,00", driver.FindElement(By.CssSelector("[data-e2e=\"TransactionId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"TransactionAdditionBonusesUsed\"]")).Text, "all Transaction AdditionBonuses Used");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"TransactionId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"TransactionAdditionBonusesAdded\"]")).Text, "all Transaction AdditionBonuses Added");
            VerifyAreEqual("400,00", driver.FindElement(By.CssSelector("[data-e2e=\"TransactionId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"TransactionAdditionBonusesSaldo\"]")).Text, "all Transaction AdditionBonuses Saldo");
            VerifyAreEqual("2", driver.FindElement(By.CssSelector("[data-e2e=\"TransactionId-1\"]")).FindElement(By.CssSelector("[data-e2e=\"PurchaseId\"]")).Text, "all Transaction PurchaseId");

            VerifyFinally(testname);
        }
    }
}
