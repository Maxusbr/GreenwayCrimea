using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using System.Threading;

namespace AdvantShop.SeleniumTest.Admin.Marketing.Voting.Answers
{
    [TestFixture]
    public class VotingAnswersPageTest : BaseSeleniumTest
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
            GoToAdmin("voting");
            GetGridCell(0, "ID", "Voting").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));
        }

        [Test]
        public void Page()
        {
            testname = "VotingAnswersPage";
            VerifyBegin(testname);

            VerifyAreEqual("Answer 1", GetGridCell(0, "Name", "Answers").Text, "page 1 line 1");
            VerifyAreEqual("Answer 12", GetGridCell(9, "Name", "Answers").Text, "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            VerifyAreEqual("Answer 13", GetGridCell(0, "Name", "Answers").Text, "page 2 line 1");
            VerifyAreEqual("Answer 21", GetGridCell(9, "Name", "Answers").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            VerifyAreEqual("Answer 22", GetGridCell(0, "Name", "Answers").Text, "page 3 line 1");
            VerifyAreEqual("Answer 30", GetGridCell(9, "Name", "Answers").Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            VerifyAreEqual("Answer 31", GetGridCell(0, "Name", "Answers").Text, "page 4 line 1");
            VerifyAreEqual("Answer 4", GetGridCell(9, "Name", "Answers").Text, "page 4 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            VerifyAreEqual("Answer 40", GetGridCell(0, "Name", "Answers").Text, "page 5 line 1");
            VerifyAreEqual("Answer 49", GetGridCell(9, "Name", "Answers").Text, "page 5 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            VerifyAreEqual("Answer 5", GetGridCell(0, "Name", "Answers").Text, "page 6 line 1");
            VerifyAreEqual("Answer 58", GetGridCell(9, "Name", "Answers").Text, "page 6 line 10");

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            VerifyAreEqual("Answer 1", GetGridCell(0, "Name", "Answers").Text, "page 1 line 1");
            VerifyAreEqual("Answer 12", GetGridCell(9, "Name", "Answers").Text, "page 1 line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void PageToPrevious()
        {
            testname = "VotingAnswersPageToPrevious";
            VerifyBegin(testname);

            VerifyAreEqual("Answer 1", GetGridCell(0, "Name", "Answers").Text, "page 1 line 1");
            VerifyAreEqual("Answer 12", GetGridCell(9, "Name", "Answers").Text, "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("Answer 13", GetGridCell(0, "Name", "Answers").Text, "page 2 line 1");
            VerifyAreEqual("Answer 21", GetGridCell(9, "Name", "Answers").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("Answer 22", GetGridCell(0, "Name", "Answers").Text, "page 3 line 1");
            VerifyAreEqual("Answer 30", GetGridCell(9, "Name", "Answers").Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            VerifyAreEqual("Answer 13", GetGridCell(0, "Name", "Answers").Text, "page 2 line 1");
            VerifyAreEqual("Answer 21", GetGridCell(9, "Name", "Answers").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            VerifyAreEqual("Answer 1", GetGridCell(0, "Name", "Answers").Text, "page 1 line 1");
            VerifyAreEqual("Answer 12", GetGridCell(9, "Name", "Answers").Text, "page 1 line 10");

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            VerifyAreEqual("Answer 95", GetGridCell(0, "Name", "Answers").Text, "last page line 1");
            VerifyAreEqual("Answer 99", GetGridCell(4, "Name", "Answers").Text, "last page line 5");

            VerifyFinally(testname);
        }

    }
}
