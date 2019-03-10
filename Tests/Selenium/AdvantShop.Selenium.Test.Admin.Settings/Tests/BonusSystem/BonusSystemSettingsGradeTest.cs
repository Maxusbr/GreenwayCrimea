using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Settings.BonusSystem.SettingsGrade
{
    [TestFixture]
    public class BonusSystemSettingsGradeTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(

             "Data\\Admin\\Settings\\BonusSystem\\Catalog.Product.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Catalog.Offer.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Catalog.Category.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Catalog.ProductCategories.csv"

                );
            Init();
        }
         

        [Test]
        public void CheckaGuestBonusCart()
        {
            GoToAdmin("settingsbonus");
            testname = "CheckaGuestBonusCart";
            VerifyBegin(testname);

            VerifyIsFalse(Is404Page("settingsbonus"), " 404 error");
            VerifyAreEqual("Бонусная система", driver.FindElement(By.CssSelector(".nav-link")).Text, " bonus title");

            (new SelectElement(driver.FindElement(By.Id("BonusGradeId")))).SelectByText("Гостевой");
            driver.FindElement(By.Id("CardNumTo")).SendKeys("0");
            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsBonusSave\"]")).Click();
            Thread.Sleep(2000);

            IWebElement selectElem1 = driver.FindElement(By.Id("BonusGradeId"));
            SelectElement select3 = new SelectElement(selectElem1);
            VerifyIsTrue(select3.AllSelectedOptions[0].Text.Contains("Гостевой"), "select item Guess");
            
            ProductToCard("+3 руб. на бонусную карту");
            GoToClient("cart");
            VerifyAreEqual("Сумма баллов начисляемых на бонусную карту: +3 руб.", driver.FindElement(By.CssSelector(".bonus-card-block-inline")).Text, "icon adn text bonus card ");
            VerifyAreEqual("3 руб.", driver.FindElement(By.CssSelector(".bonus-card-block-inline span")).Text, "Count bonus to card");
           
            VerifyFinally(testname);
        }
        [Test]
        public void CheckBronseBonusCart()
        {
            GoToAdmin("settingsbonus");
            testname = "CheckBronseBonusCart";
            VerifyBegin(testname);

            (new SelectElement(driver.FindElement(By.Id("BonusGradeId")))).SelectByText("Бронзовый");
            driver.FindElement(By.Id("CardNumFrom")).Clear();
            driver.FindElement(By.Id("CardNumFrom")).SendKeys("20000");

            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsBonusSave\"]")).Click();
            Thread.Sleep(2000);

            IWebElement selectElem1 = driver.FindElement(By.Id("BonusGradeId"));
            SelectElement select3 = new SelectElement(selectElem1);
            VerifyIsTrue(select3.AllSelectedOptions[0].Text.Contains("Бронзовый"), "select item Bronse");
            ProductToCard("+5 руб. на бонусную карту");
            GoToClient("cart");
            VerifyAreEqual("Сумма баллов начисляемых на бонусную карту: +5 руб.", driver.FindElement(By.CssSelector(".bonus-card-block-inline")).Text, "icon adn text bonus card ");
            VerifyAreEqual("5 руб.", driver.FindElement(By.CssSelector(".bonus-card-block-inline span")).Text, "Count bonus to card");

            VerifyFinally(testname);
        }
        [Test]
        public void CheckSilverBonusCart()
        {
            GoToAdmin("settingsbonus");
            testname = "CheckSilverBonusCart";
            VerifyBegin(testname);

            (new SelectElement(driver.FindElement(By.Id("BonusGradeId")))).SelectByText("Серебряный");
            driver.FindElement(By.Id("CardNumFrom")).Clear();
            driver.FindElement(By.Id("CardNumFrom")).SendKeys("20000");

            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsBonusSave\"]")).Click();
            Thread.Sleep(2000);
            IWebElement selectElem1 = driver.FindElement(By.Id("BonusGradeId"));
            SelectElement select3 = new SelectElement(selectElem1);
            VerifyIsTrue(select3.AllSelectedOptions[0].Text.Contains("Серебряный"), "select item Silver");
            ProductToCard("+7 руб. на бонусную карту");
            GoToClient("cart");
            VerifyAreEqual("Сумма баллов начисляемых на бонусную карту: +7 руб.", driver.FindElement(By.CssSelector(".bonus-card-block-inline")).Text, "icon adn text bonus card ");
            VerifyAreEqual("7 руб.", driver.FindElement(By.CssSelector(".bonus-card-block-inline span")).Text, "Count bonus to card");

            VerifyFinally(testname);
        }

        [Test]
        public void ChecktGoldBonusCart()
        {
            GoToAdmin("settingsbonus");
            testname = "ChecktGoldBonusCart";
            VerifyBegin(testname);

            (new SelectElement(driver.FindElement(By.Id("BonusGradeId")))).SelectByText("Золотой");
            driver.FindElement(By.Id("CardNumFrom")).Clear();
            driver.FindElement(By.Id("CardNumFrom")).SendKeys("20000");

            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsBonusSave\"]")).Click();
            Thread.Sleep(2000); Thread.Sleep(2000);

            IWebElement selectElem1 = driver.FindElement(By.Id("BonusGradeId"));
            SelectElement select3 = new SelectElement(selectElem1);
            VerifyIsTrue(select3.AllSelectedOptions[0].Text.Contains("Золотой"), "select item Gold");
            ProductToCard("+10 руб. на бонусную карту");
            GoToClient("cart");
            VerifyAreEqual("Сумма баллов начисляемых на бонусную карту: +10 руб.", driver.FindElement(By.CssSelector(".bonus-card-block-inline")).Text, "icon adn text bonus card ");
            VerifyAreEqual("10 руб.", driver.FindElement(By.CssSelector(".bonus-card-block-inline span")).Text, "Count bonus to card");

            VerifyFinally(testname);
        }

        [Test]
        public void ChecktPlatinBonusCart()
        {
            GoToAdmin("settingsbonus");
            testname = "ChecktPlatinBonusCart";
            VerifyBegin(testname);

            (new SelectElement(driver.FindElement(By.Id("BonusGradeId")))).SelectByText("Платиновый");
            driver.FindElement(By.Id("CardNumFrom")).Clear();
            driver.FindElement(By.Id("CardNumFrom")).SendKeys("20000");

            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsBonusSave\"]")).Click();
            Thread.Sleep(2000);
            IWebElement selectElem1 = driver.FindElement(By.Id("BonusGradeId"));
            SelectElement select3 = new SelectElement(selectElem1);
            VerifyIsTrue(select3.AllSelectedOptions[0].Text.Contains("Платиновый"), "select item Platin");

            ProductToCard("+30 руб. на бонусную карту");
            GoToClient("cart");
            VerifyAreEqual("Сумма баллов начисляемых на бонусную карту: +30 руб.", driver.FindElement(By.CssSelector(".bonus-card-block-inline")).Text, "icon and text bonus card ");
            VerifyAreEqual("30 руб.", driver.FindElement(By.CssSelector(".bonus-card-block-inline span")).Text, "Count bonus to card");

            VerifyFinally(testname);
        }

        public void ProductToCard(string bonus)
        {
            GoToClient("products/test-product1");
            if (driver.FindElement(By.CssSelector(".cart-mini span")).Text.Contains("пусто"))
            {
                ScrollTo(By.CssSelector(".rating"));
                VerifyAreEqual(bonus, driver.FindElement(By.CssSelector(".bonus-string-sum")).Text, "Count bonus in product cart");
                driver.FindElement(By.CssSelector(".details-payment-inline a")).Click();
                Thread.Sleep(2000);
            }
        }
    }
    }
