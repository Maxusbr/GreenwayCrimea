using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using System.Threading;

namespace AdvantShop.SeleniumTest.Admin.Marketing.Voting.Answers
{
    [TestFixture]
    public class VotingAnswersSortTest : BaseSeleniumTest
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
            Thread.Sleep(2000);
            GetGridCell(0, "ID", "Voting").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));
        }

        [Test]
        public void ByName()
        {
            testname = "VotingAnswersSortByName";
            VerifyBegin(testname);

            GetGridCell(-1, "Name", "Answers").Click();
            WaitForAjax();
            VerifyAreEqual("Answer 1", GetGridCell(0, "Name", "Answers").Text, "Name 1 asc");
            VerifyAreEqual("Answer 12", GetGridCell(9, "Name", "Answers").Text, "Name 10 asc");

            GetGridCell(-1, "Name", "Answers").Click();
            WaitForAjax();
            VerifyAreEqual("Answer 99", GetGridCell(0, "Name", "Answers").Text, "Name 1 desc");
            VerifyAreEqual("Answer 90", GetGridCell(9, "Name", "Answers").Text, "Name 10 desc");

            VerifyFinally(testname);
        }


        [Test]
        public void ByIsVisible()
        {
            testname = "VotingAnswersSortByIsVisible";
            VerifyBegin(testname);

            GetGridCell(-1, "IsVisible", "Answers").Click();
            WaitForAjax();
            VerifyIsFalse(GetGridCell(0, "IsVisible", "Answers").FindElement(By.TagName("input")).Selected, "IsVisible 1 asc");
            VerifyIsFalse(GetGridCell(9, "IsVisible", "Answers").FindElement(By.TagName("input")).Selected, "IsVisible 10 asc");

            string ascLine1 = GetGridCell(0, "Name", "Answers").Text;
            string ascLine10 = GetGridCell(9, "Name", "Answers").Text;

            VerifyIsFalse(ascLine1.Equals(ascLine10), "asc different answers");

            GetGridCell(-1, "IsVisible", "Answers").Click();
            WaitForAjax();
            VerifyIsTrue(GetGridCell(0, "IsVisible", "Answers").FindElement(By.TagName("input")).Selected, "IsVisible 1 desc");
            VerifyIsTrue(GetGridCell(9, "IsVisible", "Answers").FindElement(By.TagName("input")).Selected, "IsVisible 10 desc");

            string descLine1 = GetGridCell(0, "Name", "Answers").Text;
            string descLine10 = GetGridCell(9, "Name", "Answers").Text;

            VerifyIsFalse(descLine1.Equals(descLine10), "desc different answers");

            VerifyFinally(testname);
        }


        [Test]
        public void ByCountVoice()
        {
            testname = "VotingAnswersSortCountVoice";
            VerifyBegin(testname);

            GetGridCell(-1, "CountVoice", "Answers").Click();
            WaitForAjax();
            VerifyAreEqual("1", GetGridCell(0, "CountVoice", "Answers").Text, "CountVoice 1 asc");
            VerifyAreEqual("10", GetGridCell(9, "CountVoice", "Answers").Text, "CountVoice 10 asc");

            GetGridCell(-1, "CountVoice", "Answers").Click();
            WaitForAjax();
            VerifyAreEqual("105", GetGridCell(0, "CountVoice", "Answers").Text, "CountVoice 1 desc");
            VerifyAreEqual("96", GetGridCell(9, "CountVoice", "Answers").Text, "CountVoice 10 desc");

            VerifyFinally(testname);
        }

        [Test]
        public void BySortOrder()
        {
            testname = "VotingAnswersSortBySortOrder";
            VerifyBegin(testname);

            GetGridCell(-1, "SortOrder", "Answers").Click();
            WaitForAjax();
            VerifyAreEqual("1", GetGridCell(0, "SortOrder", "Answers").Text, "SortOrder 1 asc");
            VerifyAreEqual("10", GetGridCell(9, "SortOrder", "Answers").Text, "SortOrder 10 asc");

            GetGridCell(-1, "SortOrder", "Answers").Click();
            WaitForAjax();
            VerifyAreEqual("105", GetGridCell(0, "SortOrder", "Answers").Text, "SortOrder 1 desc");
            VerifyAreEqual("96", GetGridCell(9, "SortOrder", "Answers").Text, "SortOrder 10 desc");

            VerifyFinally(testname);
        }

        [Test]
        public void ByDateAdded()
        {
            testname = "VotingAnswersSortByDateAdded";
            VerifyBegin(testname);

            GetGridCell(-1, "DateAdded", "Answers").Click();
            WaitForAjax();
            VerifyAreEqual("05.09.2012 11:55", GetGridCell(0, "DateAdded", "Answers").Text, "DateAdded 1 asc");
            VerifyAreEqual("05.09.2012 21:00", GetGridCell(9, "DateAdded", "Answers").Text, "DateAdded 10 asc");

            GetGridCell(-1, "DateAdded", "Answers").Click();
            WaitForAjax();
            VerifyAreEqual("09.09.2012 23:45", GetGridCell(0, "DateAdded", "Answers").Text, "DateAdded 1 desc");
            VerifyAreEqual("09.09.2012 17:54", GetGridCell(9, "DateAdded", "Answers").Text, "DateAdded 10 desc");

            VerifyFinally(testname);
        }

        [Test]
        public void ByDateModify()
        {
            testname = "VotingAnswersSortByDateModify";
            VerifyBegin(testname);

            GetGridCell(-1, "DateModify", "Answers").Click();
            WaitForAjax();
            VerifyAreEqual("04.10.2014 12:23", GetGridCell(0, "DateModify", "Answers").Text, "DateModify 1 asc");
            VerifyAreEqual("18.02.2015 23:56", GetGridCell(9, "DateModify", "Answers").Text, "DateModify 10 asc");

            GetGridCell(-1, "DateModify", "Answers").Click();
            WaitForAjax();
            VerifyAreEqual("17.01.2019 13:30", GetGridCell(0, "DateModify", "Answers").Text, "DateModify 1 desc");
            VerifyAreEqual("04.09.2018 23:56", GetGridCell(9, "DateModify", "Answers").Text, "DateModify 10 desc");

            VerifyFinally(testname);
        }


    }
}
