using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Settings.BonusSystem.Cards
{
    [TestFixture]
    public class BonusSystemCardAdd : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers |ClearType.Bonuses );
            InitializeService.LoadData(

             "Data\\Admin\\Settings\\BonusSystem\\Catalog.Product.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Catalog.Offer.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Catalog.Category.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Catalog.ProductCategories.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Customers.CustomerGroup.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Customers.Customer.csv",
            "Data\\Admin\\Settings\\BonusSystem\\CardsBonusToAll\\Bonus.Grade.csv",
            "Data\\Admin\\Settings\\BonusSystem\\Bonus.Card.csv"


                );
            Init();
        }

        [Test]
        public void AddNewCard()
        {
            GoToAdmin("cards");
            testname = "AddNewCard";
            VerifyBegin(testname);
            driver.FindElement(By.CssSelector("[data-e2e=\"AddCard\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".edit")).Click();
            Thread.Sleep(500);

            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"selectUserGrid[0]['_serviceColumn']\"]")).Click();
            Thread.Sleep(2000);
            (new SelectElement(driver.FindElement(By.CssSelector(".col-xs-9 select")))).SelectByText("Серебряный");
            driver.FindElement(By.CssSelector("[data-e2e=\"addCard\"]")).Click();
            VerifyAreEqual("Testsurename TestName", driver.FindElement(By.TagName("strong")).Text, " card title h1");
            IWebElement selectElem1 = driver.FindElement(By.CssSelector("[data-e2e=\"gradeBonus\"] select"));
            SelectElement select3 = new SelectElement(selectElem1);
            VerifyIsTrue(select3.AllSelectedOptions[0].Text.Contains("Серебряный"), "select item Gost");

            VerifyAreEqual("0 бонусов\r\n0 основных и 0 дополнительных", driver.FindElement(By.CssSelector("[data-e2e=\"countBonus")).Text, " in str card");
            VerifyAreEqual("Testsurename", driver.FindElement(By.CssSelector("[data-e2e=\"bonusCardSuname\"]")).Text, " bonusCardSuname in str card");
            VerifyAreEqual("TestName", driver.FindElement(By.CssSelector("[data-e2e=\"bonusCardName\"]")).Text, " bonusCardName in str card");
            VerifyAreEqual("999999", driver.FindElement(By.CssSelector("[data-e2e=\"bonusCardPhone\"]")).Text, " bonusCardPhone in str card");
            VerifyAreEqual("test@mail.ru", driver.FindElement(By.CssSelector("[data-e2e=\"bonusCardMail\"]")).Text, " bonusCardMail in str card");

            VerifyAreEqual("Ни одной записи не найдено", driver.FindElements(By.TagName("td"))[0].Text, " no bonus1");
            VerifyAreEqual("Ни одной записи не найдено", driver.FindElements(By.TagName("td"))[1].Text, " no bonus2");
            VerifyAreEqual("Ни одной записи не найдено", driver.FindElements(By.TagName("td"))[1].Text, " no bonus3");

            //Check client cart
            driver.FindElement(By.CssSelector(".edit.link-decoration-none")).Click();
            Thread.Sleep(4000);
            VerifyAreEqual("Testsurename TestName", driver.FindElement(By.TagName("h1")).Text, " client cart title h1");
            VerifyAreEqual("0 (Серебряный 7 %)", driver.FindElements(By.CssSelector(".block-additional-parameters-value"))[7].Text, " client cart type card");
            GoBack();
            //change grade
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gradeBonus\"] select")))).SelectByText("Золотой 10%");
            driver.FindElement(By.CssSelector("[data-e2e=\"SaveCard\"]")).Click();
            Thread.Sleep(2000);

            selectElem1 = driver.FindElement(By.CssSelector("[data-e2e=\"gradeBonus\"] select"));
            select3 = new SelectElement(selectElem1);
            VerifyIsTrue(select3.AllSelectedOptions[0].Text.Contains("Золотой"), "select item Gost");
            //Check client cart
            driver.FindElement(By.CssSelector(".edit.link-decoration-none")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Testsurename TestName", driver.FindElement(By.TagName("h1")).Text, " client cart title h1");
            VerifyAreEqual("0 (Золотой 10 %)", driver.FindElements(By.CssSelector(".block-additional-parameters-value"))[7].Text, " client cart type card");

            GoBack();
            //Check grid
            driver.FindElement(By.CssSelector(".breadcrumb a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Testsurename TestName", GetGridCell(0, "FIO").Text, "grid cart fio");
            VerifyAreEqual("Золотой", GetGridCell(0, "GradeName").Text, "grid cart GradeName");
            VerifyAreEqual("10", GetGridCell(0, "GradePersent").Text, "grid cart GradePersent");
            VerifyFinally(testname);
        }
        
        [Test]
        public void EditCardAddMainBonus()
        {
            testname = "EditCardAddMainBonus";
            VerifyBegin(testname);
            GoToAdmin("cards/edit/2c8fb106-8f07-499b-b06f-51b43076f3cf");
            //fhdghfghfg
            driver.FindElement(By.CssSelector("[data-e2e=\"MainBonus\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"MainBonusAdd\"]")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"Amount\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"Amount\"]")).SendKeys("1000");

            driver.FindElement(By.CssSelector("[data-e2e=\"Reason\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"Reason\"]")).SendKeys("Reason");

            driver.FindElement(By.CssSelector("[data-e2e=\"bonusAdd\"]")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"countBonus\"]")).Text.Contains("1 000 основных и 0 дополнительных"), "card additional bonuses added");
            VerifyAreEqual("+ 1 000", driver.FindElement(By.Id("viewTableTransaction")).FindElements(By.TagName("td"))[2].Text, " count bonus  table transaction");
            VerifyAreEqual("1 000", driver.FindElement(By.Id("viewTableTransaction")).FindElements(By.TagName("td"))[3].Text, "  saldo table transaction");
            VerifyAreEqual("Reason", driver.FindElement(By.Id("viewTableTransaction")).FindElements(By.TagName("td"))[7].Text, " number sell table transaction");

            driver.FindElement(By.CssSelector("[data-e2e=\"MainBonus\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"MainBonusSub\"]")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"Amount\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"Amount\"]")).SendKeys("100");

            driver.FindElement(By.CssSelector("[data-e2e=\"Reason\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"Reason\"]")).SendKeys("second reason");

            driver.FindElement(By.CssSelector("[data-e2e=\"bonussubtract\"]")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"countBonus\"]")).Text.Contains("900 основных и 0 дополнительных"), "card additional bonuses added");
            VerifyAreEqual("-100", driver.FindElement(By.Id("viewTableTransaction")).FindElements(By.TagName("td"))[1].Text, " count bonus  table transaction");
            VerifyAreEqual("900", driver.FindElement(By.Id("viewTableTransaction")).FindElements(By.TagName("td"))[3].Text, "  saldo table transaction");
            VerifyAreEqual("second reason", driver.FindElement(By.Id("viewTableTransaction")).FindElements(By.TagName("td"))[7].Text, " number sell table transaction");
            driver.FindElement(By.CssSelector("[data-e2e=\"MainBonus\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"MainBonusSub\"]")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"Amount\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"Amount\"]")).SendKeys("1000");

            driver.FindElement(By.CssSelector("[data-e2e=\"Reason\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"Reason\"]")).SendKeys("reason");

            driver.FindElement(By.CssSelector("[data-e2e=\"bonussubtract\"]")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.FindElement(By.CssSelector(".modal-dialog.modal-md")).Displayed, "display modal win");
            driver.FindElement(By.CssSelector(".btn.btn-default.btn-cancel")).Click();
            Thread.Sleep(2000);

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"countBonus\"]")).Text.Contains("900 основных и 0 дополнительных"), "card additional bonuses added");
            VerifyAreEqual("-100", driver.FindElement(By.Id("viewTableTransaction")).FindElements(By.TagName("td"))[1].Text, " count bonus  table transaction");
            VerifyAreEqual("900", driver.FindElement(By.Id("viewTableTransaction")).FindElements(By.TagName("td"))[3].Text, "  saldo table transaction");
            VerifyAreEqual("second reason", driver.FindElement(By.Id("viewTableTransaction")).FindElements(By.TagName("td"))[7].Text, " number sell table transaction");

            VerifyFinally(testname);
        }

        [Test]
        public void EditCardAddsAdditionalBonus()
        {
            testname = "EditCardAddsAdditionalBonus";
            VerifyBegin(testname);
            GoToAdmin("cards/edit/2c8fb106-8f07-499b-b06f-51b43076f3cf");
             driver.FindElement(By.CssSelector("[data-e2e=\"AdditionalBonus\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"AdditionalBonusAdd\"]")).Click();

            VerifyAreEqual("Начислить дополнительные бонусы", driver.FindElement(By.TagName("h2")).Text, "pop up header");

            driver.FindElement(By.CssSelector("[data-e2e=\"Name\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"Name\"]")).SendKeys("AdditionalBonuses");

            driver.FindElement(By.CssSelector("[data-e2e=\"addAdditionBonusEndDate\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"addAdditionBonusEndDate\"]")).SendKeys("28.03.2030");

            driver.FindElement(By.CssSelector("[data-e2e=\"addAdditionBonusStartDate\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"addAdditionBonusStartDate\"]")).SendKeys("28.03.2014");

            driver.FindElement(By.CssSelector("[data-e2e=\"Name\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"Reason\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"Reason\"]")).SendKeys("thid Reason");

            driver.FindElement(By.CssSelector("[data-e2e=\"Amount\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"Amount\"]")).SendKeys("1000");


            driver.FindElement(By.CssSelector("[data-e2e=\"bonusAdd\"]")).Click();
            Thread.Sleep(2000);

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"countBonus\"]")).Text.Contains("1 000 дополнительных"), "card additional bonuses added");

            VerifyAreEqual("AdditionalBonuses", driver.FindElement(By.Id("viewTableAdditionalBonus")).FindElements(By.TagName("td"))[0].Text, " name bonus  table additional");
            VerifyAreEqual("1 000", driver.FindElement(By.Id("viewTableAdditionalBonus")).FindElements(By.TagName("td"))[1].Text, "  count table additional");
            VerifyAreEqual("thid Reason", driver.FindElement(By.Id("viewTableAdditionalBonus")).FindElements(By.TagName("td"))[2].Text, " reason table additional");
            
            //через точки
            VerifyAreEqual("28 03 2014", driver.FindElement(By.Id("viewTableAdditionalBonus")).FindElements(By.TagName("td"))[3].Text, " dateFrom table additional");
            VerifyAreEqual("28 03 2030", driver.FindElement(By.Id("viewTableAdditionalBonus")).FindElements(By.TagName("td"))[4].Text, " dateTo table additional");

            VerifyAreEqual("+ 1 000", driver.FindElement(By.Id("viewTableTransaction")).FindElements(By.TagName("td"))[5].Text, " count bonus  table transaction");
            VerifyAreEqual("1 000", driver.FindElement(By.Id("viewTableTransaction")).FindElements(By.TagName("td"))[6].Text, "  saldo table transaction");
            VerifyAreEqual("thid Reason", driver.FindElement(By.Id("viewTableTransaction")).FindElements(By.TagName("td"))[7].Text, " number sell table transaction");
            
            driver.FindElement(By.CssSelector("[data-e2e=\"AdditionalBonus\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"AdditionalBonusSub\"]")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Списать дополнительные бонусы", driver.FindElement(By.TagName("h2")).Text, "pop up header 1");

            (new SelectElement(driver.FindElement(By.CssSelector(".col-xs-9 select")))).SelectByText("AdditionalBonuses (1000)");
            driver.FindElement(By.CssSelector("[data-e2e=\"amount\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"amount\"]")).SendKeys("100");

            driver.FindElement(By.CssSelector("[data-e2e=\"Reason\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"Reason\"]")).SendKeys("five Reason");

            driver.FindElement(By.CssSelector("[data-e2e=\"subctractBonus\"]")).Click();
            Thread.Sleep(2000);

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"countBonus\"]")).Text.Contains("900 дополнительных"), "card additional bonuses added");
                      
            VerifyAreEqual("-100", driver.FindElement(By.Id("viewTableTransaction")).FindElements(By.TagName("td"))[4].Text, " count bonus  table transaction");
            VerifyAreEqual("900", driver.FindElement(By.Id("viewTableTransaction")).FindElements(By.TagName("td"))[6].Text, "  saldo table transaction");
            VerifyAreEqual("five Reason", driver.FindElement(By.Id("viewTableTransaction")).FindElements(By.TagName("td"))[7].Text, " number sell table transaction");
            
             driver.FindElement(By.CssSelector("[data-e2e=\"AdditionalBonus\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"AdditionalBonusSub\"]")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Списать дополнительные бонусы", driver.FindElement(By.TagName("h2")).Text, "pop up header 2");

            (new SelectElement(driver.FindElement(By.CssSelector(".col-xs-9 select")))).SelectByText("AdditionalBonuses (900)");
            driver.FindElement(By.CssSelector("[data-e2e=\"amount\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"amount\"]")).SendKeys("1000");

            driver.FindElement(By.CssSelector("[data-e2e=\"Reason\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"Reason\"]")).SendKeys("six Reason");

            driver.FindElement(By.CssSelector("[data-e2e=\"subctractBonus\"]")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.FindElement(By.CssSelector(".modal-dialog.modal-md")).Displayed, "display modal win");
            driver.FindElement(By.CssSelector(".btn.btn-default.btn-cancel")).Click();
            Thread.Sleep(2000);

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"countBonus\"]")).Text.Contains("900 дополнительных"), "card additional bonuses added");

            VerifyAreEqual("-100", driver.FindElement(By.Id("viewTableTransaction")).FindElements(By.TagName("td"))[4].Text, " count bonus  table transaction");
            VerifyAreEqual("900", driver.FindElement(By.Id("viewTableTransaction")).FindElements(By.TagName("td"))[6].Text, "  saldo table transaction");
            VerifyAreEqual("five Reason", driver.FindElement(By.Id("viewTableTransaction")).FindElements(By.TagName("td"))[7].Text, " number sell table transaction");
            
            VerifyFinally(testname);
        }

        [Test]
        public void EditCustomer()
        {
            GoToAdmin("cards");
            testname = "EdittCustomer";
            VerifyBegin(testname);
            GetGridCell(0, "FIO").Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".edit.link-decoration-none")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("Customer_LastName")).Click();
            driver.FindElement(By.Id("Customer_LastName")).Clear();
            driver.FindElement(By.Id("Customer_LastName")).SendKeys("changedSurname");

            driver.FindElement(By.Id("Customer_FirstName")).Click();
            driver.FindElement(By.Id("Customer_FirstName")).Clear();
            driver.FindElement(By.Id("Customer_FirstName")).SendKeys("changedName");

            driver.FindElement(By.Id("Customer_EMail")).Click();
            driver.FindElement(By.Id("Customer_EMail")).Clear();
            driver.FindElement(By.Id("Customer_EMail")).SendKeys("editedtest@mail.ru");

            driver.FindElement(By.Id("Customer_Phone")).Click();
            driver.FindElement(By.Id("Customer_Phone")).Clear();
            driver.FindElement(By.Id("Customer_Phone")).SendKeys("+79308888888");

            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);
            GoToAdmin("cards");
            GetGridCell(0, "FIO").Click();
            Thread.Sleep(2000);
            VerifyAreEqual("changedSurname", driver.FindElement(By.CssSelector("[data-e2e=\"bonusCardSuname\"]")).Text, " bonusCardSuname in str card");
            VerifyAreEqual("changedName", driver.FindElement(By.CssSelector("[data-e2e=\"bonusCardName\"]")).Text, " bonusCardName in str card");
            VerifyAreEqual("79308888888", driver.FindElement(By.CssSelector("[data-e2e=\"bonusCardPhone\"]")).Text, " bonusCardPhone in str card");
            VerifyAreEqual("editedtest@mail.ru", driver.FindElement(By.CssSelector("[data-e2e=\"bonusCardMail\"]")).Text, " bonusCardMail in str card");

            VerifyFinally(testname);

        }

        [Test]
        public void EditCustomerAddCard()
        {
            GoToAdmin("customers/edit/cfc2c33b-1e84-415e-8482-e98156341601");
            testname = "EditCustomerAddCard";
            VerifyBegin(testname);
            driver.FindElement(By.CssSelector("[data-e2e=\"AddCard\"]")).Click();
            Thread.Sleep(2000);

            VerifyAreEqual("test testov", driver.FindElement(By.CssSelector(".m-l-xs.link-invert")).Text, " name customer cart");
            (new SelectElement(driver.FindElement(By.CssSelector(".col-xs-9 select")))).SelectByText("Серебряный");
            driver.FindElement(By.CssSelector("[data-e2e=\"addCard\"]")).Click();
            Thread.Sleep(2000);

            VerifyIsTrue(driver.Url.Contains("cards/edit/cfc2c33b-1e84-415e-8482-e98156341601"), "redirect to bonus card");
            VerifyAreEqual("0 бонусов\r\n0 основных и 0 дополнительных", driver.FindElement(By.CssSelector("[data-e2e=\"countBonus")).Text, " in str card");
            VerifyAreEqual("testov", driver.FindElement(By.CssSelector("[data-e2e=\"bonusCardSuname\"]")).Text, " bonusCardSuname in str card");
            VerifyAreEqual("test", driver.FindElement(By.CssSelector("[data-e2e=\"bonusCardName\"]")).Text, " bonusCardName in str card");
            VerifyAreEqual("qwe@qwe.qwe", driver.FindElement(By.CssSelector("[data-e2e=\"bonusCardMail\"]")).Text, " bonusCardMail in str card");

            driver.FindElement(By.CssSelector(".breadcrumb a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("testov test", GetGridCell(0, "FIO").Text, "grid cart fio");
            VerifyAreEqual("Серебряный", GetGridCell(0, "GradeName").Text, "grid cart GradeName");
            VerifyAreEqual("7", GetGridCell(0, "GradePersent").Text, "grid cart GradePersent");

            GetGridCell(0, "FIO").Click();
            Thread.Sleep(2000);

            //Check client cart
            driver.FindElement(By.CssSelector(".edit.link-decoration-none")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("testov test", driver.FindElement(By.TagName("h1")).Text, " client cart title h1");
            VerifyAreEqual("0 (Серебряный 7 %)", driver.FindElements(By.CssSelector(".block-additional-parameters-value"))[7].Text, " client cart type card");
           
            //del cart
            driver.FindElements(By.CssSelector(".block-additional-parameters-value"))[6].Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".link-danger")).Click();
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            GetGridFilter().SendKeys("testov");
            DropFocus("h1");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), " NO del cart");
            VerifyFinally(testname);
        }

        [Test]
        public void EditEnabledCard()
        {
            GoToAdmin("cards");
            testname = "EditEnabledCard";
            VerifyBegin(testname);
            GetGridFilter().SendKeys("LastName");
            DropFocus("h1");
            GetGridCell(0, "FIO").Click();
            Thread.Sleep(2000);
            driver.FindElements(By.CssSelector(".switcher-state-trigger"))[1].Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"SaveCard\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector(".edit.link-decoration-none")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.PageSource.Contains("Карта заблокирована"), " enabled card in cart customer");
            ReInitClient();
            GoToClient("login");
            Refresh();
            driver.FindElement(By.Id("email")).SendKeys("mail@mail.com");
            driver.FindElement(By.Id("password")).SendKeys("123123");
            driver.FindElement(By.CssSelector(".btn.btn-submit.btn-middle")).Click();
            Thread.Sleep(2000);

            GoToClient("products/test-product22");
            ScrollTo(By.CssSelector(".rating"));
            VerifyIsTrue(driver.FindElements(By.CssSelector(".bonus-string-sum")).Count == 0, "no bonus in product cart");
            driver.FindElement(By.CssSelector(".details-payment-inline a")).Click();
            Thread.Sleep(2000);

            GoToClient("checkout");
            Thread.Sleep(2000);
            VerifyAreEqual("Бонусная карта заблокирована", driver.FindElement(By.CssSelector(".custom-input-text")).Text, " enabled bonus cart");

            driver.FindElements(By.CssSelector(".btn.btn-big.btn-submit"))[1].Click();

            VerifyFinally(testname);
        }

      
       
    }
}
