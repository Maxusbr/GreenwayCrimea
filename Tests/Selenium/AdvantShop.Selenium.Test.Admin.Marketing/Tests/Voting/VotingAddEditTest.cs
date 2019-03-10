using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using System.Threading;

namespace AdvantShop.SeleniumTest.Admin.Marketing.Voting
{
    [TestFixture]
    public class VotingAddEditTest : BaseSeleniumTest
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
        }

        [Test]
        public void aVotingAddIsDefault()
        {
            testname = "VotingAddIsDefault";
            VerifyBegin(testname);

            //pre check
            GoToClient();

            VerifyIsTrue(driver.PageSource.Contains("VoiceTheme7"), "pre check default voting on page");
            
            //test
            GoToAdmin("voting");
            driver.FindElement(By.CssSelector("[data-e2e=\"btnAdd\"]")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-content"));

            driver.FindElement(By.CssSelector("[data-e2e=\"voteName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"voteName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"voteName\"]")).SendKeys("New Voting");

            driver.FindElement(By.CssSelector("[data-e2e=\"voteIsDefault\"]")).FindElement(By.TagName("span")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"voteSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin grid
            GoToAdmin("voting");

            VerifyAreEqual("Найдено записей: 171", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "voting added count");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("New Voting");

            VerifyAreEqual("New Voting", GetGridCell(0, "Name", "Voting").Text, "added name grid");
            VerifyIsTrue(GetGridCell(0, "IsDefault", "Voting").FindElement(By.TagName("input")).Selected, "added IsDefault grid");
            VerifyIsFalse(GetGridCell(0, "IsHaveNullVoice", "Voting").FindElement(By.TagName("input")).Selected, "added IsHaveNullVoice grid");
            VerifyIsFalse(GetGridCell(0, "IsClose", "Voting").FindElement(By.TagName("input")).Selected, "added IsClose grid");
            VerifyAreEqual("0", GetGridCell(0, "CountAnswers", "Voting").Text, "added CountAnswers grid");

            //check admin pop up
            GetGridCell(0, "_serviceColumn", "Voting").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.CssSelector(".modal-content"));
            VerifyAreEqual("New Voting", driver.FindElement(By.CssSelector("[data-e2e=\"voteName\"]")).GetAttribute("value"), "added name pop up");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"voteIsDefault\"]")).FindElement(By.TagName("input")).Selected, "added IsDefault pop up");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"voteIsHaveNullVoice\"]")).FindElement(By.TagName("input")).Selected, "added IsHaveNullVoice pop up");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"voteIsClose\"]")).FindElement(By.TagName("input")).Selected, "added IsClose pop up");

            //add answers
            driver.FindElement(By.CssSelector("[data-e2e=\"answerNameAdd\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"answerNameAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"answerNameAdd\"]")).SendKeys("New Answer 1");

            driver.FindElement(By.CssSelector("[data-e2e=\"answerAdd\"]")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"answerNameAdd\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"answerNameAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"answerNameAdd\"]")).SendKeys("New Answer 2");

            driver.FindElement(By.CssSelector("[data-e2e=\"answerAdd\"]")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"voteSave\"]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("voting");

            //check previous default voting
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("VoiceTheme7");

            VerifyAreEqual("VoiceTheme7", GetGridCell(0, "Name", "Voting").Text, "previous default name grid");
            VerifyIsFalse(GetGridCell(0, "IsDefault", "Voting").FindElement(By.TagName("input")).Selected, "previous default IsDefault no");

            //check client
            GoToClient();

            VerifyIsFalse(driver.PageSource.Contains("VoiceTheme7"), "previous default name client no on page");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-question")).Text.Contains("New Voting"), "added name client");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-answers")).Text.Contains("New Answer 1"), "added answer 1 client");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-answers")).Text.Contains("New Answer 2"), "added answer 2 client"); 
            VerifyIsTrue(driver.FindElement(By.Name("votingForm")).FindElements(By.CssSelector(".btn.btn-small.btn-action")).Count == 1, "added has null voice not");

            VerifyFinally(testname);
        }

        [Test]
        public void VotingAdd()
        {
            testname = "VotingAdd";
            VerifyBegin(testname);

            GoToAdmin("voting");
            driver.FindElement(By.CssSelector("[data-e2e=\"btnAdd\"]")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-content"));

            driver.FindElement(By.CssSelector("[data-e2e=\"voteName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"voteName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"voteName\"]")).SendKeys("New Voting All Options");

            driver.FindElement(By.CssSelector("[data-e2e=\"voteIsDefault\"]")).FindElement(By.TagName("span")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"voteIsHaveNullVoice\"]")).FindElement(By.TagName("span")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"voteIsClose\"]")).FindElement(By.TagName("span")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"voteSave\"]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("voting");
            
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("New Voting All Options");

            VerifyAreEqual("New Voting All Options", GetGridCell(0, "Name", "Voting").Text, "added name grid");
            VerifyIsTrue(GetGridCell(0, "IsDefault", "Voting").FindElement(By.TagName("input")).Selected, "added IsDefault grid");
            VerifyIsTrue(GetGridCell(0, "IsHaveNullVoice", "Voting").FindElement(By.TagName("input")).Selected, "added IsHaveNullVoice grid");
            VerifyIsTrue(GetGridCell(0, "IsClose", "Voting").FindElement(By.TagName("input")).Selected, "added IsClose grid");
            VerifyAreEqual("0", GetGridCell(0, "CountAnswers", "Voting").Text, "added CountAnswers grid");
            
            VerifyFinally(testname);
        }
        
        [Test]
        public void VotingEditIsHaveNullVoice()
        {
            testname = "VotingEditIsHaveNullVoice";
            VerifyBegin(testname);

            GoToAdmin("voting");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("VoiceTheme10");
            
            VerifyAreEqual("VoiceTheme10", GetGridCell(0, "Name", "Voting").Text, "pre check voting name grid IsHaveNullVoice");
            VerifyIsFalse(GetGridCell(0, "IsHaveNullVoice", "Voting").FindElement(By.TagName("input")).Selected, "pre check voting grid IsHaveNullVoice");
            VerifyIsFalse(GetGridCell(0, "IsDefault", "Voting").FindElement(By.TagName("input")).Selected, "pre check voting grid IsDefault");

            GetGridCell(0, "_serviceColumn", "Voting").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("VoiceTheme10", driver.FindElement(By.CssSelector("[data-e2e=\"voteName\"]")).GetAttribute("value"), "pre check voting name grid pop up");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"voteIsHaveNullVoice\"]")).FindElement(By.TagName("input")).Selected, "pre check voting IsHaveNullVoice pop up");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"voteIsDefault\"]")).FindElement(By.TagName("input")).Selected, "pre check voting IsDefault pop up");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswersList\"]")).FindElements(By.CssSelector(".row.as-sortable-item")).Count == 11, "pre check voting answers count pop up"); //10 answers + add button field
            VerifyAreEqual("Answer 46", driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswersList\"]")).FindElement(By.CssSelector("[data-e2e=\"answer-Answer 46\"]")).Text, "pre check voting answers name pop up");

            driver.FindElement(By.CssSelector("[data-e2e=\"voteIsHaveNullVoice\"]")).FindElement(By.TagName("span")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"voteIsDefault\"]")).FindElement(By.TagName("span")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"voteName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"voteName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"voteName\"]")).SendKeys("Edited Voting Name");

            driver.FindElement(By.CssSelector("[data-e2e=\"voteSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin grid
            GoToAdmin("voting");
            
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("VoiceTheme10");

            VerifyIsFalse(driver.FindElement(By.CssSelector("[grid-unique-id=\"gridVoting\"]")).Text.Contains("VoiceTheme10 "), "previous edited name");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Edited Voting Name");

            VerifyAreEqual("Edited Voting Name", GetGridCell(0, "Name", "Voting").Text, "edited name grid");
            VerifyIsTrue(GetGridCell(0, "IsDefault", "Voting").FindElement(By.TagName("input")).Selected, "edited IsDefault grid");
            VerifyIsTrue(GetGridCell(0, "IsHaveNullVoice", "Voting").FindElement(By.TagName("input")).Selected, "edited IsHaveNullVoice grid");

            //check admin pop up
            GetGridCell(0, "_serviceColumn", "Voting").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("Edited Voting Name", driver.FindElement(By.CssSelector("[data-e2e=\"voteName\"]")).GetAttribute("value"), "edited voting name grid pop up");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"voteIsHaveNullVoice\"]")).FindElement(By.TagName("input")).Selected, "edited voting IsHaveNullVoice pop up");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"voteIsDefault\"]")).FindElement(By.TagName("input")).Selected, "edited voting IsDefault pop up");

            //check client
            GoToClient();
            
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-question")).Text.Contains("Edited Voting Name"), "edited name client");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-answers")).Text.Contains("Answer 46"), "edited answer 1 client");
            VerifyIsTrue(driver.FindElement(By.Name("votingForm")).FindElements(By.CssSelector(".btn.btn-small.btn-action")).Count == 2, "edited has null voice");

            //check IsHaveNullVoice change no
            GoToAdmin("voting");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Edited Voting Name");

            GetGridCell(0, "_serviceColumn", "Voting").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-content"));
            driver.FindElement(By.CssSelector("[data-e2e=\"voteIsHaveNullVoice\"]")).FindElement(By.TagName("span")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"voteSave\"]")).Click();
            Thread.Sleep(2000);

            //check client no IsHaveNullVoice
            GoToClient();

            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-question")).Text.Contains("Edited Voting Name"), "edited name client no IsHaveNullVoice");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-answers")).Text.Contains("Answer 46"), "edited answer 1 client no IsHaveNullVoice");
            VerifyIsTrue(driver.FindElement(By.Name("votingForm")).FindElements(By.CssSelector(".btn.btn-small.btn-action")).Count == 1, "edited has null voice no IsHaveNullVoice");

            VerifyFinally(testname);
        }


        [Test]
        public void VotingEditIsClose()
        {
            testname = "VotingEditIsClose";
            VerifyBegin(testname);

            GoToAdmin("voting");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("VoiceTheme11");

            VerifyAreEqual("VoiceTheme11", GetGridCell(0, "Name", "Voting").Text, "pre check voting name grid no IsClose");

            GetGridCell(0, "_serviceColumn", "Voting").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-content"));
            
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"voteIsClose\"]")).FindElement(By.TagName("input")).Selected, "pre check voting IsClose pop up");

            driver.FindElement(By.CssSelector("[data-e2e=\"voteIsClose\"]")).FindElement(By.TagName("span")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"voteIsDefault\"]")).FindElement(By.TagName("span")).Click();
            
            driver.FindElement(By.CssSelector("[data-e2e=\"voteSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin grid
            GoToAdmin("voting");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("VoiceTheme11");
            
            VerifyAreEqual("VoiceTheme11", GetGridCell(0, "Name", "Voting").Text, "name grid");
            VerifyIsTrue(GetGridCell(0, "IsDefault", "Voting").FindElement(By.TagName("input")).Selected, "edited IsDefault grid");
            VerifyIsTrue(GetGridCell(0, "IsClose", "Voting").FindElement(By.TagName("input")).Selected, "edited IsClose grid");

            //check admin pop up
            GetGridCell(0, "_serviceColumn", "Voting").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-content"));
            
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"voteIsClose\"]")).FindElement(By.TagName("input")).Selected, "edited voting IsClose pop up");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"voteIsDefault\"]")).FindElement(By.TagName("input")).Selected, "edited voting IsDefault pop up");

            //check client
            GoToClient();
            
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-question")).Text.Contains("VoiceTheme11"), "edited name IsClose client");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-results")).Text.Contains("Answer 56"), "edited answer 1 IsClose client");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-results-total")).Displayed, "edited IsClose result client");

            //check IsClose change no
            GoToAdmin("voting");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("VoiceTheme11");

            GetGridCell(0, "_serviceColumn", "Voting").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-content"));

            driver.FindElement(By.CssSelector("[data-e2e=\"voteIsClose\"]")).FindElement(By.TagName("span")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"voteSave\"]")).Click();
            Thread.Sleep(2000);

            //check client no IsClose
            GoToClient();

            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-question")).Text.Contains("VoiceTheme11"), "edited name client no IsClose");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-answers")).Text.Contains("Answer 56"), "edited answer 1 client no IsClose");
            VerifyIsTrue(driver.FindElement(By.Name("votingForm")).FindElements(By.CssSelector(".btn.btn-small.btn-action")).Count == 1, "edited no IsClose client");

            VerifyFinally(testname);
        }
    }
}
