using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Marketing.Voting
{
    [TestFixture]
    public class VotingPresentTest : BaseSeleniumTest
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
        public void Present10()
        {
            testname = "VotingPresent10";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("10");
            VerifyAreEqual("VoiceTheme1", GetGridCell(0, "Name", "Voting").Text, "line 1");
            VerifyAreEqual("VoiceTheme10", GetGridCell(9, "Name", "Voting").Text, "line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void Present20()
        {
            testname = "VotingPresent20";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("20");
            VerifyAreEqual("VoiceTheme1", GetGridCell(0, "Name", "Voting").Text, "line 1");
            VerifyAreEqual("VoiceTheme20", GetGridCell(19, "Name", "Voting").Text, "line 20");

            VerifyFinally(testname);
        }

        [Test]
        public void Present50()
        {
            testname = "VotingPresent50";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("50");
            VerifyAreEqual("VoiceTheme1", GetGridCell(0, "Name", "Voting").Text, "line 1");
            VerifyAreEqual("VoiceTheme50", GetGridCell(49, "Name", "Voting").Text, "line 50");

            VerifyFinally(testname);
        }

        [Test]
        public void Present100()
        {
            testname = "VotingPresent100";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("100");
            VerifyAreEqual("VoiceTheme1", GetGridCell(0, "Name", "Voting").Text, "line 1");
            VerifyAreEqual("VoiceTheme100", GetGridCell(99, "Name", "Voting").Text, "line 100");

            VerifyFinally(testname);
        }

    }
}
