using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Marketing.Voting
{
    [TestFixture]
    public class VotingSortTest : BaseSeleniumTest
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
        public void ByName()
        {
            testname = "VotingSortByName";
            VerifyBegin(testname);

            GetGridCell(-1, "Name", "Voting").Click();
            WaitForAjax();
            VerifyAreEqual("VoiceTheme1", GetGridCell(0, "Name", "Voting").Text, "Name 1 asc");
            VerifyAreEqual("VoiceTheme107", GetGridCell(9, "Name", "Voting").Text, "Name 10 asc");

            GetGridCell(-1, "Name", "Voting").Click();
            WaitForAjax();
            VerifyAreEqual("VoiceTheme99", GetGridCell(0, "Name", "Voting").Text, "Name 1 desc");
            VerifyAreEqual("VoiceTheme90", GetGridCell(9, "Name", "Voting").Text, "Name 10 desc");

            VerifyFinally(testname);
        }


        [Test]
        public void ByIsDefault()
        {
            testname = "VotingSortByIsDefault";
            VerifyBegin(testname);

            GetGridCell(-1, "IsDefault", "Voting").Click();
            WaitForAjax();
            VerifyIsFalse(GetGridCell(0, "IsDefault", "Voting").FindElement(By.TagName("input")).Selected, "IsDefault 1 asc");
            VerifyIsFalse(GetGridCell(9, "IsDefault", "Voting").FindElement(By.TagName("input")).Selected, "IsDefault 10 asc");

            string ascLine1 = GetGridCell(0, "Name", "Voting").Text;
            string ascLine10 = GetGridCell(9, "Name", "Voting").Text;

            VerifyIsFalse(ascLine1.Equals(ascLine10), "asc different themes");

            GetGridCell(-1, "IsDefault", "Voting").Click();
            WaitForAjax();
            VerifyIsTrue(GetGridCell(0, "IsDefault", "Voting").FindElement(By.TagName("input")).Selected, "IsDefault 1 desc");
            VerifyAreEqual("VoiceTheme7", GetGridCell(0, "Name", "Voting").Text, "item IsDefault");
            VerifyIsFalse(GetGridCell(9, "IsDefault", "Voting").FindElement(By.TagName("input")).Selected, "IsDefault 10 desc");

            string descLine1 = GetGridCell(0, "Name", "Voting").Text;
            string descLine10 = GetGridCell(9, "Name", "Voting").Text;

            VerifyIsFalse(descLine1.Equals(descLine10), "desc different themes");

            VerifyFinally(testname);
        }

        [Test]
        public void ByIsHaveNullVoice()
        {
            testname = "VotingSortByIsHaveNullVoice";
            VerifyBegin(testname);

            GetGridCell(-1, "IsHaveNullVoice", "Voting").Click();
            WaitForAjax();
            VerifyIsFalse(GetGridCell(0, "IsHaveNullVoice", "Voting").FindElement(By.TagName("input")).Selected, "IsHaveNullVoice 1 asc");
            VerifyIsFalse(GetGridCell(9, "IsHaveNullVoice", "Voting").FindElement(By.TagName("input")).Selected, "IsHaveNullVoice 10 asc");

            string ascLine1 = GetGridCell(0, "Name", "Voting").Text;
            string ascLine10 = GetGridCell(9, "Name", "Voting").Text;

            VerifyIsFalse(ascLine1.Equals(ascLine10), "asc different themes");

            GetGridCell(-1, "IsHaveNullVoice", "Voting").Click();
            WaitForAjax();
            VerifyIsTrue(GetGridCell(0, "IsHaveNullVoice", "Voting").FindElement(By.TagName("input")).Selected, "IsHaveNullVoice 1 desc");
            VerifyIsTrue(GetGridCell(9, "IsHaveNullVoice", "Voting").FindElement(By.TagName("input")).Selected, "IsHaveNullVoice 10 desc");

            string descLine1 = GetGridCell(0, "Name", "Voting").Text;
            string descLine10 = GetGridCell(9, "Name", "Voting").Text;

            VerifyIsFalse(descLine1.Equals(descLine10), "desc different themes");

            VerifyFinally(testname);
        }

        [Test]
        public void ByIsClose()
        {
            testname = "VotingSortByIsClose";
            VerifyBegin(testname);

            GetGridCell(-1, "IsClose", "Voting").Click();
            WaitForAjax();
            VerifyIsFalse(GetGridCell(0, "IsClose", "Voting").FindElement(By.TagName("input")).Selected, "IsClose 1 asc");
            VerifyIsFalse(GetGridCell(9, "IsClose", "Voting").FindElement(By.TagName("input")).Selected, "IsClose 10 asc");

            string ascLine1 = GetGridCell(0, "Name", "Voting").Text;
            string ascLine10 = GetGridCell(9, "Name", "Voting").Text;

            VerifyIsFalse(ascLine1.Equals(ascLine10), "asc different themes");

            GetGridCell(-1, "IsClose", "Voting").Click();
            WaitForAjax();
            VerifyIsTrue(GetGridCell(0, "IsClose", "Voting").FindElement(By.TagName("input")).Selected, "IsClose 1 desc");
            VerifyIsTrue(GetGridCell(9, "IsClose", "Voting").FindElement(By.TagName("input")).Selected, "IsClose 10 desc");

            string descLine1 = GetGridCell(0, "Name", "Voting").Text;
            string descLine10 = GetGridCell(9, "Name", "Voting").Text;

            VerifyIsFalse(descLine1.Equals(descLine10), "desc different themes");

            VerifyFinally(testname);
        }

        
        [Test]
        public void ByDateAdded()
        {
            testname = "VotingSortByDateAdded";
            VerifyBegin(testname);

            GetGridCell(-1, "DateAdded", "Voting").Click();
            WaitForAjax();
            VerifyAreEqual("05.09.2012 11:52", GetGridCell(0, "DateAdded", "Voting").Text, "DateAdded 1 asc");
            VerifyAreEqual("14.09.2012 20:52", GetGridCell(9, "DateAdded", "Voting").Text, "DateAdded 10 asc");

            GetGridCell(-1, "DateAdded", "Voting").Click();
            WaitForAjax();
            VerifyAreEqual("28.02.2013 12:52", GetGridCell(0, "DateAdded", "Voting").Text, "DateAdded 1 desc");
            VerifyAreEqual("19.02.2013 23:52", GetGridCell(9, "DateAdded", "Voting").Text, "DateAdded 10 desc");

            VerifyFinally(testname);
        }

        [Test]
        public void ByDateModify()
        {
            testname = "VotingSortByDateModify";
            VerifyBegin(testname);

            GetGridCell(-1, "DateModify", "Voting").Click();
            WaitForAjax();
            VerifyAreEqual("15.01.2014 14:26", GetGridCell(0, "DateModify", "Voting").Text, "DateModify 1 asc");
            VerifyAreEqual("24.01.2014 23:26", GetGridCell(9, "DateModify", "Voting").Text, "DateModify 10 asc");

            GetGridCell(-1, "DateModify", "Voting").Click();
            WaitForAjax();
            VerifyAreEqual("10.07.2014 15:26", GetGridCell(0, "DateModify", "Voting").Text, "DateModify 1 desc");
            VerifyAreEqual("01.07.2014 16:26", GetGridCell(9, "DateModify", "Voting").Text, "DateModify 10 desc");

            VerifyFinally(testname);
        }

    }
}
