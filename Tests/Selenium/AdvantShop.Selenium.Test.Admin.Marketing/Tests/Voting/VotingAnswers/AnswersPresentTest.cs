using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using System.Threading;

namespace AdvantShop.SeleniumTest.Admin.Marketing.Voting.Answers
{
    [TestFixture]
    public class VotingAnswersPresentTest : BaseSeleniumTest
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
        public void Present10()
        {
            testname = "VotingAnswersPresent10";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("10");
            VerifyAreEqual("Answer 1", GetGridCell(0, "Name", "Answers").Text, "line 1");
            VerifyAreEqual("Answer 12", GetGridCell(9, "Name", "Answers").Text, "line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void Present20()
        {
            testname = "VotingAnswersPresent20";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("20");
            VerifyAreEqual("Answer 1", GetGridCell(0, "Name", "Answers").Text, "line 1");
            VerifyAreEqual("Answer 21", GetGridCell(19, "Name", "Answers").Text, "line 20");

            VerifyFinally(testname);
        }

        [Test]
        public void Present50()
        {
            testname = "VotingAnswersPresent50";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("50");
            VerifyAreEqual("Answer 1", GetGridCell(0, "Name", "Answers").Text, "line 1");
            VerifyAreEqual("Answer 49", GetGridCell(49, "Name", "Answers").Text, "line 50");

            VerifyFinally(testname);
        }

        [Test]
        public void Present100()
        {
            testname = "VotingAnswersPresent100";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("100");
            VerifyAreEqual("Answer 1", GetGridCell(0, "Name", "Answers").Text, "line 1");
            VerifyAreEqual("Answer 94", GetGridCell(99, "Name", "Answers").Text, "line 100");

            VerifyFinally(testname);
        }

    }
}
