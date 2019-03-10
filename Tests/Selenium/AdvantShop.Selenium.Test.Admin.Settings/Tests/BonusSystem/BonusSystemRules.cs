using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Settings.BonusSystem.Rules
{
    [TestFixture]
    public class BonusSystemRules : BaseSeleniumTest
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
        public void CheckRulesGrid()
        {
            GoToAdmin("rules");
            testname = "CheckRulesGrid";
            VerifyBegin(testname);
            
            VerifyAreEqual("Правила", driver.FindElement(By.TagName("h1")).Text, " grade title h1");
            VerifyAreEqual("Ни одной записи не найдено", driver.FindElement(By.CssSelector(".ui-grid-empty-text")).Text, "no notes");          
            VerifyFinally(testname);
        }

        [Test]
        public void CheckzAddRuleCancelBonus()
        {
            testname = "CheckzAddRuleCancelBonus";
            VerifyBegin(testname);
            GoToAdmin("rules");

            driver.FindElement(By.CssSelector("[data-e2e=\"Add\"]")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-content"));
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"RuleSelect\"]")))).SelectByText("Аннулирование баллов");
            driver.FindElement(By.CssSelector("[data-e2e=\"addTemplate\"]")).Click();
            Thread.Sleep(2000);

            VerifyAreEqual("Правило - \"Аннулирование баллов\"", driver.FindElement(By.TagName("h1")).Text, " grade title h1");
            VerifyIsTrue(driver.Url.Contains("rules/edit/CancellationsBonus"),  "url");
            driver.FindElement(By.Id("Name")).SendKeys("1");
            driver.FindElement(By.Id("Enabled")).Click();
            driver.FindElement(By.Id("AgeCard")).Clear();
            driver.FindElement(By.Id("AgeCard")).SendKeys("12");
            driver.FindElement(By.Id("NotSendSms")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(driver.FindElements(By.Id("SmsDayBefore")).Count==0, "invisible field count day");
            driver.FindElement(By.Id("NotSendSms")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(driver.FindElements(By.Id("SmsDayBefore")).Count == 1, "visible field count day");
            driver.FindElement(By.Id("SmsDayBefore")).Clear();
            driver.FindElement(By.Id("SmsDayBefore")).SendKeys("1");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"SaveRules\"]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("rules");
            Thread.Sleep(2000);
            VerifyIsFalse(driver.PageSource.Contains("Ни одной записи не найдено"), "yes notes");

            VerifyAreEqual("Аннулирование баллов", GetGridCell(0, "RuleTypeStr").Text, "rule added RuleTypeStr");
            VerifyAreEqual("Аннулирование баллов1", GetGridCell(0, "Name").Text, "rule added Name");
            VerifyIsTrue(GetGridCell(0, "Enabled").FindElement(By.TagName("input")).Selected, " select enabled");

            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Аннулирование баллов1", driver.FindElement(By.Id("Name")).GetAttribute("value"), "name");
            VerifyAreEqual("12", driver.FindElement(By.Id("AgeCard")).GetAttribute("value"), "AgeCard");
            VerifyAreEqual("1", driver.FindElement(By.Id("SmsDayBefore")).GetAttribute("value"), "SmsDayBefore");
            VerifyIsTrue(driver.FindElement(By.Id("Enabled")).Selected, " select enabled");
            VerifyIsFalse(driver.FindElement(By.Id("NotSendSms")).Selected, " select NotSendSms");

            VerifyFinally(testname);
        }
        [Test]
        public void CheckzAddRuleNewCart()
        {
            testname = "CheckzAddRuleNewCart";
            VerifyBegin(testname);
            GoToAdmin("rules");
            DelItem();
            driver.FindElement(By.CssSelector("[data-e2e=\"Add\"]")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-content"));
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"RuleSelect\"]")))).SelectByText("Начисление баллов при получении карты");
            driver.FindElement(By.CssSelector("[data-e2e=\"addTemplate\"]")).Click();
            Thread.Sleep(2000);

            VerifyAreEqual("Правило - \"Начисление баллов при получении карты\"", driver.FindElement(By.TagName("h1")).Text, " grade title h1");
            VerifyIsTrue(driver.Url.Contains("rules/edit/NewCard"), "url");
            driver.FindElement(By.Id("Name")).SendKeys("2");
            driver.FindElement(By.Id("Enabled")).Click();
            driver.FindElement(By.Id("GiftBonus")).Clear();
            driver.FindElement(By.Id("GiftBonus")).SendKeys("200");
            driver.FindElement(By.Id("BonusAvailableDays")).Clear();
            driver.FindElement(By.Id("BonusAvailableDays")).SendKeys("20");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"SaveRules\"]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("rules");
            Thread.Sleep(2000);
            VerifyIsFalse(driver.PageSource.Contains("Ни одной записи не найдено"), "yes notes");

            VerifyAreEqual("Начисление баллов при получении карты", GetGridCell(0, "RuleTypeStr").Text, "rule added RuleTypeStr");
            VerifyAreEqual("Начисление баллов при получении карты2", GetGridCell(0, "Name").Text, "rule added Name");
            VerifyIsTrue(GetGridCell(0, "Enabled").FindElement(By.TagName("input")).Selected, " select enabled");
                       
            VerifyFinally(testname);
        }
        [Test]
        public void CheckzAddRulesChangeGrade()
        {
            testname = "CheckzAddRulesChangeGrade";
            VerifyBegin(testname);
            GoToAdmin("rules");
            DelItem();

            driver.FindElement(By.CssSelector("[data-e2e=\"Add\"]")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-content"));
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"RuleSelect\"]")))).SelectByText("Смена грейда");
            driver.FindElement(By.CssSelector("[data-e2e=\"addTemplate\"]")).Click();
            Thread.Sleep(2000);

            VerifyAreEqual("Правило - \"Смена грейда\"", driver.FindElement(By.TagName("h1")).Text, " grade title h1");
            VerifyIsTrue(driver.Url.Contains("rules/edit/ChangeGrade"), "url");
            driver.FindElement(By.Id("Name")).SendKeys("3");
            driver.FindElement(By.Id("Enabled")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(driver.FindElements(By.Id("Period")).Count == 0, "invisible field count day");
            driver.FindElement(By.Id("AllPeriod")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(driver.FindElements(By.Id("Period")).Count == 1, "visible field count day");
            driver.FindElement(By.Id("Period")).Clear();
            driver.FindElement(By.Id("Period")).SendKeys("30");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"SaveRules\"]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("rules");
            Thread.Sleep(2000);
            VerifyIsFalse(driver.PageSource.Contains("Ни одной записи не найдено"), "yes notes");

            VerifyAreEqual("Смена грейда", GetGridCell(0, "RuleTypeStr").Text, "rule added RuleTypeStr");
            VerifyAreEqual("Смена грейда3", GetGridCell(0, "Name").Text, "rule added Name");
            VerifyIsTrue(GetGridCell(0, "Enabled").FindElement(By.TagName("input")).Selected, " select enabled");
                     
            VerifyFinally(testname);
        }

        [Test]
        public void CheckzAddRulesCleanExpired()
        {
            testname = "CheckzAddRulesCleanExpired";
            VerifyBegin(testname);
            GoToAdmin("rules");
            DelItem();
            driver.FindElement(By.CssSelector("[data-e2e=\"Add\"]")).Click();
            Thread.Sleep(2000);
           // (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"RuleSelect\"]")))).SelectByText("Правило: списание истекших бонусов");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"RuleSelect\"]")))).SelectByText("Cписание истекших бонусов");
            driver.FindElement(By.CssSelector("[data-e2e=\"addTemplate\"]")).Click();
            Thread.Sleep(2000);

            VerifyAreEqual("Правило - \"Cписание истекших бонусов\"", driver.FindElement(By.TagName("h1")).Text, " grade title h1");
            VerifyIsTrue(driver.Url.Contains("rules/edit/CleanExpiredBonus"), "url");
            driver.FindElement(By.Id("Name")).SendKeys("4");
            VerifyIsTrue(driver.FindElements(By.Id("DayBefore")).Count == 0, "invisible field count day");
            driver.FindElement(By.Name("NeedSms")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(driver.FindElements(By.Id("DayBefore")).Count == 1, "visible field count day");
            driver.FindElement(By.Id("DayBefore")).Clear();
            driver.FindElement(By.Id("DayBefore")).SendKeys("40");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"SaveRules\"]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("rules");
            Thread.Sleep(2000);
            VerifyIsFalse(driver.PageSource.Contains("Ни одной записи не найдено"), "yes notes");

            VerifyAreEqual("Cписание истекших бонусов", GetGridCell(0, "RuleTypeStr").Text, "rule added RuleTypeStr");
            VerifyAreEqual("Cписание истекших бонусов4", GetGridCell(0, "Name").Text, "rule added Name");
            VerifyIsFalse(GetGridCell(0, "Enabled").FindElement(By.TagName("input")).Selected , " select enabled");

            GetGridCell(0, "RuleTypeStr").Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Cписание истекших бонусов4", driver.FindElement(By.Id("Name")).GetAttribute("value"), "name");
            VerifyAreEqual("40", driver.FindElement(By.Id("DayBefore")).GetAttribute("value"), "DayBefore");
            VerifyIsFalse(driver.FindElement(By.Id("Enabled")).Selected, " select enabled");
            VerifyIsTrue(driver.FindElement(By.Id("NeedSms")).Selected, " select NeedSms");

            driver.FindElement(By.CssSelector("[data-e2e=\"DelRules\"]")).Click();
            driver.FindElement(By.CssSelector(".swal2-confirm")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Ни одной записи не найдено", driver.FindElement(By.CssSelector(".ui-grid-empty-text")).Text, "no notes");

            VerifyFinally(testname);
        }

        [Test]
        public void CheckzAddRulesTwoRules()
        {
            testname = "CheckzAddRulesTwoRules";
            VerifyBegin(testname);
            GoToAdmin("rules");
            DelItem();
            driver.FindElement(By.CssSelector("[data-e2e=\"Add\"]")).Click();
            Thread.Sleep(2000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"RuleSelect\"]")))).SelectByText("Аннулирование баллов");
            driver.FindElement(By.CssSelector("[data-e2e=\"addTemplate\"]")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Правило - \"Аннулирование баллов\"", driver.FindElement(By.TagName("h1")).Text, " grade title h1");

            driver.FindElement(By.CssSelector("[data-e2e=\"ReturnRules\"]")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.FindElements(By.TagName("ui-grid-custom-delete")).Count == 1, "count row");
            driver.FindElement(By.CssSelector("[data-e2e=\"Add\"]")).Click();
            Thread.Sleep(2000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"RuleSelect\"]")))).SelectByText("Аннулирование баллов");
            driver.FindElement(By.CssSelector("[data-e2e=\"addTemplate\"]")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.FindElements(By.CssSelector(".modal-dialog.modal-md")).Count==1, " window not clode");
            driver.FindElement(By.CssSelector(".btn.btn-default.btn-cancel")).Click();
            VerifyIsTrue(driver.FindElements(By.TagName("ui-grid-custom-delete")).Count == 1, "count row after add");

            VerifyFinally(testname);
        }
        protected void DelItem()
        {
            if (driver.FindElements(By.TagName("ui-grid-custom-delete")).Count>0)
            {
                GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-grid-custom-delete")).Click();
                driver.FindElement(By.CssSelector(".swal2-confirm")).Click();
                Thread.Sleep(3000);
                VerifyAreEqual("Ни одной записи не найдено", driver.FindElement(By.CssSelector(".ui-grid-empty-text")).Text, "no notes");
            }
        }

    }
}
