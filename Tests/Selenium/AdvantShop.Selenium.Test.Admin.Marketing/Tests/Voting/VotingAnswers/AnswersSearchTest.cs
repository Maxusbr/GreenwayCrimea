using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using System.Threading;

namespace AdvantShop.SeleniumTest.Admin.Marketing.Voting.Answers
{
    [TestFixture]
    public class VotingAnswersSearchTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CMS);
            InitializeService.LoadData(
        "data\\Admin\\VotingAnswers\\Voice.Answer.csv",
        "data\\Admin\\VotingAnswers\\Voice.VoiceTheme.csv"
          );

            Init();

        }


        [Test]
        public void SearchExist()
        {
            testname = "VotingAnswersSearchExist";
            VerifyBegin(testname);

            GoToAdmin("voting");
            GetGridCell(0, "ID", "Voting").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Answer 95");

            VerifyAreEqual("Answer 95", GetGridCell(0, "Name", "Answers").Text, "search exist voting answer");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void SearchNotExist()
        {
            testname = "VotingAnswersSearchNotExist";
            VerifyBegin(testname);

            GoToAdmin("voting");
            GetGridCell(0, "ID", "Voting").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Voice Test Theme Text");

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search not exist voting answer");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void SearchMuchSymbols()
        {
            testname = "VotingAnswersSearchMuchSymbols";
            VerifyBegin(testname);

            GoToAdmin("voting");
            GetGridCell(0, "ID", "Voting").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));

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
            testname = "VotingAnswersSearchInvalidSymbols";
            VerifyBegin(testname);

            GoToAdmin("voting");
            GetGridCell(0, "ID", "Voting").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("########@@@@@@@@&&&&&&&******,,,,..");

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search invalid symbols");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }


    }
}
