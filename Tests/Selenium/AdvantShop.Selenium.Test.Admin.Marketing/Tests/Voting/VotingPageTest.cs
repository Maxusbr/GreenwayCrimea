using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Marketing.Voting
{
    [TestFixture]
    public class VotingPageTest : BaseSeleniumTest
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
        public void Page()
        {
            testname = "VotingPage";
            VerifyBegin(testname);

            VerifyAreEqual("VoiceTheme1", GetGridCell(0, "Name", "Voting").Text, "page 1 line 1");
            VerifyAreEqual("VoiceTheme10", GetGridCell(9, "Name", "Voting").Text, "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            VerifyAreEqual("VoiceTheme11", GetGridCell(0, "Name", "Voting").Text, "page 2 line 1");
            VerifyAreEqual("VoiceTheme20", GetGridCell(9, "Name", "Voting").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            VerifyAreEqual("VoiceTheme21", GetGridCell(0, "Name", "Voting").Text, "page 3 line 1");
            VerifyAreEqual("VoiceTheme30", GetGridCell(9, "Name", "Voting").Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            VerifyAreEqual("VoiceTheme31", GetGridCell(0, "Name", "Voting").Text, "page 4 line 1");
            VerifyAreEqual("VoiceTheme40", GetGridCell(9, "Name", "Voting").Text, "page 4 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            VerifyAreEqual("VoiceTheme41", GetGridCell(0, "Name", "Voting").Text, "page 5 line 1");
            VerifyAreEqual("VoiceTheme50", GetGridCell(9, "Name", "Voting").Text, "page 5 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            VerifyAreEqual("VoiceTheme51", GetGridCell(0, "Name", "Voting").Text, "page 6 line 1");
            VerifyAreEqual("VoiceTheme60", GetGridCell(9, "Name", "Voting").Text, "page 6 line 10");

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            VerifyAreEqual("VoiceTheme1", GetGridCell(0, "Name", "Voting").Text, "page 1 line 1");
            VerifyAreEqual("VoiceTheme10", GetGridCell(9, "Name", "Voting").Text, "page 1 line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void PageToPrevious()
        {
            testname = "VotingPageToPrevious";
            VerifyBegin(testname);

            VerifyAreEqual("VoiceTheme1", GetGridCell(0, "Name", "Voting").Text, "page 1 line 1");
            VerifyAreEqual("VoiceTheme10", GetGridCell(9, "Name", "Voting").Text, "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("VoiceTheme11", GetGridCell(0, "Name", "Voting").Text, "page 2 line 1");
            VerifyAreEqual("VoiceTheme20", GetGridCell(9, "Name", "Voting").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("VoiceTheme21", GetGridCell(0, "Name", "Voting").Text, "page 3 line 1");
            VerifyAreEqual("VoiceTheme30", GetGridCell(9, "Name", "Voting").Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            VerifyAreEqual("VoiceTheme11", GetGridCell(0, "Name", "Voting").Text, "page 2 line 1");
            VerifyAreEqual("VoiceTheme20", GetGridCell(9, "Name", "Voting").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            VerifyAreEqual("VoiceTheme1", GetGridCell(0, "Name", "Voting").Text, "page 1 line 1");
            VerifyAreEqual("VoiceTheme10", GetGridCell(9, "Name", "Voting").Text, "page 1 line 10");

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            VerifyAreEqual("VoiceTheme161", GetGridCell(0, "Name", "Voting").Text, "last page line 1");
            VerifyAreEqual("VoiceTheme170", GetGridCell(9, "Name", "Voting").Text, "last page line 10");

            VerifyFinally(testname);
        }

    }
}
