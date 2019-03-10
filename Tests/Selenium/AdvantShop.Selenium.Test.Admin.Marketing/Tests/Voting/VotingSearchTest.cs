using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Marketing.Voting
{
    [TestFixture]
    public class VotingSearchTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CMS);
            InitializeService.LoadData(
        "data\\Admin\\Voting\\Voice.Answer.csv",
        "data\\Admin\\Voting\\Voice.VoiceTheme.csv"
          );

            Init();
            GoToAdmin("voting");
        }


        [Test]
        public void SearchExist()
        {
            testname = "VotingSearchExist";
            VerifyBegin(testname);

            GoToAdmin("voting");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("VoiceTheme65");

            VerifyAreEqual("VoiceTheme65", GetGridCell(0, "Name", "Voting").Text, "search exist voting");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void SearchNotExist()
        {
            testname = "VotingSearchNotExist";
            VerifyBegin(testname);

            GoToAdmin("voting");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Voice Test Theme Text");

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search not exist voting");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void SearchMuchSymbols()
        {
            testname = "VotingSearchMuchSymbols";
            VerifyBegin(testname);

            GoToAdmin("voting");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search too much symbols");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void SearchInvalidSymbols()
        {
            testname = "VotingSearchInvalidSymbols";
            VerifyBegin(testname);

            GoToAdmin("voting");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("########@@@@@@@@&&&&&&&******,,,,..");

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search invalid symbols");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

    }
}
