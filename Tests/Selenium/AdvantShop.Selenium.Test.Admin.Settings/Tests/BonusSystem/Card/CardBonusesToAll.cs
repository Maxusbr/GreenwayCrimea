using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.BonusSystem.Cards
{
    [TestFixture]
    public class BonusSystemCardToAll : BaseMultiSeleniumTest
    {
        [SetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.Bonuses);
            InitializeService.LoadData(

             "Data\\Admin\\Settings\\BonusSystem\\CardsBonusToAll\\Catalog.Product.csv",
           "Data\\Admin\\Settings\\BonusSystem\\CardsBonusToAll\\Catalog.Offer.csv",
           "Data\\Admin\\Settings\\BonusSystem\\CardsBonusToAll\\Catalog.Category.csv",
           "Data\\Admin\\Settings\\BonusSystem\\CardsBonusToAll\\Catalog.ProductCategories.csv",
           "Data\\Admin\\Settings\\BonusSystem\\CardsBonusToAll\\Customers.CustomerGroup.csv",
           "Data\\Admin\\Settings\\BonusSystem\\CardsBonusToAll\\Customers.Customer.csv",
            "Data\\Admin\\Settings\\BonusSystem\\CardsBonusToAll\\Bonus.Grade.csv",
           "Data\\Admin\\Settings\\BonusSystem\\CardsBonusToAll\\Bonus.Card.csv"


                );
            Init();
        }

        [Test]
        public void AddToAllAdditionalBonuses()
        {
            GoToAdmin("cards");
            testname = "AddToAllAdditionalBonuses";
            VerifyBegin(testname);
            driver.FindElement(By.CssSelector("[data-e2e=\"indexBonusesAddAll\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.LinkText("Начислить дополнительные")).Click();
            Thread.Sleep(2000);

            WaitForElem(By.CssSelector(".modal-content"));
            VerifyAreEqual("Начислить дополнительные бонусы", driver.FindElement(By.TagName("h2")).Text, "pop up header");

            driver.FindElement(By.CssSelector("[data-e2e=\"Name\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"Name\"]")).SendKeys("AdditionalBonuses");

            driver.FindElement(By.CssSelector("[data-e2e=\"addAdditionBonusStartDate\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"addAdditionBonusStartDate\"]")).SendKeys("28.03.2014");

            driver.FindElement(By.CssSelector("[data-e2e=\"addAdditionBonusEndDate\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"addAdditionBonusEndDate\"]")).SendKeys("28.03.2030");

            driver.FindElement(By.CssSelector("[data-e2e=\"Amount\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"Amount\"]")).SendKeys("1000");

            driver.FindElement(By.CssSelector("[data-e2e=\"Reason\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"Reason\"]")).SendKeys("Reason");

            driver.FindElement(By.CssSelector("[data-e2e=\"bonusAdd\"]")).Click();
            Thread.Sleep(5000);

            GoToAdmin("cards/edit/2C8FB106-8F07-499B-B06F-51B43076F3C1");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"countBonus\"]")).Text.Contains("1 основных и 1 000 дополнительных"), "card 1 additional bonuses added");

            GoToAdmin("cards/edit/2C8FB106-8F07-499B-B06F-51B43076F3C2");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"countBonus\"]")).Text.Contains("2 основных и 1 000 дополнительных"), "card 2 additional bonuses added");

            GoToAdmin("cards/edit/2C8FB106-8F07-499B-B06F-51B43076F3C3");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"countBonus\"]")).Text.Contains("3 основных и 1 000 дополнительных"), "card 3 additional bonuses added");

            GoToAdmin("cards/edit/2C8FB106-8F07-499B-B06F-51B43076F420");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"countBonus\"]")).Text.Contains("120 основных и 1 000 дополнительных"), "last card additional bonuses added");

            VerifyFinally(testname);
        }

        [Test]
        public void AddToAllMainBonuses()
        {
            GoToAdmin("cards");
            testname = "AddToAllMainBonuses";
            VerifyBegin(testname);
            driver.FindElement(By.CssSelector("[data-e2e=\"indexBonusesAddAll\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.LinkText("Начислить основные")).Click();
            Thread.Sleep(2000);

            WaitForElem(By.CssSelector(".modal-content"));
            VerifyAreEqual("Начислить основные бонусы", driver.FindElement(By.TagName("h2")).Text, "pop up header");

            driver.FindElement(By.CssSelector("[data-e2e=\"Amount\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"Amount\"]")).SendKeys("1000");
            
            driver.FindElement(By.CssSelector("[data-e2e=\"Reason\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"Reason\"]")).SendKeys("Reason");

            driver.FindElement(By.CssSelector("[data-e2e=\"bonusAdd\"]")).Click();
            Thread.Sleep(5000);

            GoToAdmin("cards/edit/2C8FB106-8F07-499B-B06F-51B43076F3C1");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"countBonus\"]")).Text.Contains("1 001 основных и 0 дополнительных"), "card 1 additional bonuses added");

            GoToAdmin("cards/edit/2C8FB106-8F07-499B-B06F-51B43076F3C2");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"countBonus\"]")).Text.Contains("1 002 основных и 0 дополнительных"), "card 2 additional bonuses added");

            GoToAdmin("cards/edit/2C8FB106-8F07-499B-B06F-51B43076F3C3");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"countBonus\"]")).Text.Contains("1 003 основных и 0 дополнительных"), "card 3 additional bonuses added");

            GoToAdmin("cards/edit/2C8FB106-8F07-499B-B06F-51B43076F420");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"countBonus\"]")).Text.Contains("1 120 основных и 0 дополнительных"), "last card additional bonuses added");

            VerifyFinally(testname);
        }
    }
}
