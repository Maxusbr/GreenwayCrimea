using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using System.Threading;

namespace AdvantShop.SeleniumTest.Admin.Marketing.Voting.Answers
{
    [TestFixture]
    public class VotingAnswersAddEditTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CMS);
            InitializeService.LoadData(
        "data\\Admin\\VotingAnswers\\VotingAnswersAddEdit\\Voice.Answer.csv",
        "data\\Admin\\VotingAnswers\\VotingAnswersAddEdit\\Voice.VoiceTheme.csv"
          );

            Init();
        }
        
        [Test]
        public void AnswersAddFromPage()
        {
            testname = "VotingAnswersAddFromPage";
            VerifyBegin(testname);

            //set default
            GoToAdmin("voting");

            GetGridCell(0, "_serviceColumn", "Voting").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.CssSelector(".modal-content"));

            if (!driver.FindElement(By.CssSelector("[data-e2e=\"voteIsDefault\"]")).FindElement(By.TagName("input")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"voteIsDefault\"]")).FindElement(By.TagName("span")).Click();
            }

            driver.FindElement(By.CssSelector("[data-e2e=\"voteSave\"]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("voting");
            GetGridCell(0, "ID", "Voting").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));

            driver.FindElement(By.CssSelector("[data-e2e=\"btnAddAnswers\"]")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-content"));

            driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswerName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswerName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswerName\"]")).SendKeys("New Answer Test");

            driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswerSortOrder\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswerSortOrder\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswerSortOrder\"]")).SendKeys("2");

            driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswerIsVisible\"]")).FindElement(By.TagName("span")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswerSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin grid
            GoToAdmin("voting");
            GetGridCell(0, "ID", "Voting").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));
            
            VerifyAreEqual("Найдено записей: 2", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "grid count all");
            VerifyAreEqual("New Answer Test", GetGridCell(1, "Name", "Answers").Text, "grid name");
            VerifyAreEqual("0", GetGridCell(1, "CountVoice", "Answers").Text, "grid CountVoice");
            VerifyAreEqual("2", GetGridCell(1, "SortOrder", "Answers").Text, "grid SortOrder");
            VerifyIsTrue(GetGridCell(1, "IsVisible", "Answers").FindElement(By.TagName("input")).Selected, "grid IsVisible");

            //check admin edit pop up
            GetGridCell(1, "_serviceColumn", "Answers").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("VoiceTheme1", driver.FindElement(By.CssSelector("[data-e2e=\"voteTheme\"]")).GetAttribute("value"), "voting name edit pop up");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"voteTheme\"]")).GetAttribute("readonly").Equals("true"), "voting name readonly edit pop up");
            VerifyAreEqual("New Answer Test", driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswerName\"]")).GetAttribute("value"), "answer name edit pop up");
            VerifyAreEqual("2", driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswerSortOrder\"]")).GetAttribute("value"), "answer sort order edit pop up");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswerIsVisible\"]")).FindElement(By.TagName("input")).Selected, "answer is visible edi pop up");

            //check client
            GoToClient();
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-question")).Text.Contains("VoiceTheme1"), "voting name client");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-answers")).Text.Contains("Answer 1"), "voting answer 1 client");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-answers")).Text.Contains("New Answer Test"), "added voting answer 2 client");
            
            VerifyFinally(testname);
        }

        [Test]
        public void AnswersVote()
        {
            testname = "VotingAnswersVote";
            VerifyBegin(testname);

            //set default
            GoToAdmin("voting");

            GetGridCell(1, "_serviceColumn", "Voting").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.CssSelector(".modal-content"));

            if (!driver.FindElement(By.CssSelector("[data-e2e=\"voteIsDefault\"]")).FindElement(By.TagName("input")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"voteIsDefault\"]")).FindElement(By.TagName("span")).Click();
            }

            driver.FindElement(By.CssSelector("[data-e2e=\"voteSave\"]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("voting");

            GetGridCell(1, "ID", "Voting").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswersStat-answer-Answer 2\"]")).Text.Contains("Answer 2"), "answers statistics 1 answer");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswersStat-answer-Answer 2\"]")).FindElement(By.CssSelector("[data-e2e=\"voteAnswersStat-answer-Answer 2-percent\"]")).Text.Contains("2 голосов(40%)"), "answers statistics 1 answer count voices");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswersStat-answer-Answer 3\"]")).Text.Contains("Answer 3"), "answers statistics 2 answer");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswersStat-answer-Answer 3\"]")).FindElement(By.CssSelector("[data-e2e=\"voteAnswersStat-answer-Answer 3-percent\"]")).Text.Contains("3 голосов(60%)"), "answers statistics 2 answer count voices");

            VerifyAreEqual("2", GetGridCell(0, "CountVoice", "Answers").Text, "grid CountVoice 1 answer");
            VerifyAreEqual("3", GetGridCell(1, "CountVoice", "Answers").Text, "grid CountVoice 2 answer");

            //check client voting
            GoToClient();
            ScrollTo(By.CssSelector(".h2.h-inline.products-specials-best-h"));
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-question")).Text.Contains("VoiceTheme2"), "voting name client");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-answers")).Text.Contains("Answer 2"), "voting answer 1 client");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-answers")).Text.Contains("Answer 3"), "voting answer 2 client");
            driver.FindElement(By.CssSelector(".voting-answers")).FindElements(By.CssSelector(".voting-answers-row"))[1].FindElement(By.CssSelector("[type=\"radio\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Name("votingForm")).FindElement(By.CssSelector(".btn.btn-small.btn-action")).Click();
            Thread.Sleep(2000);

            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-question")).Text.Contains("VoiceTheme2"), "voting name client after vote");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-results")).Text.Contains("Answer 2"), "answer 1 client after vote");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-results")).Text.Contains("Answer 3"), "answer 2 client after vote");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-results-total")).Text.Contains("6"), "all result count client after vote");

            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-results")).FindElements(By.CssSelector(".voting-results-progress"))[0].Text.Contains("33%"), "result answer 1 count client after vote");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-results")).FindElements(By.CssSelector(".voting-results-progress"))[1].Text.Contains("67%"), "result answer 2 count client after vote");
            
            //check admin after vote
            GoToAdmin("voting");

            GetGridCell(1, "ID", "Voting").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);
            
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswersStat-answer-Answer 2\"]")).Text.Contains("Answer 2"), "answers statistics 1 admin after vote");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswersStat-answer-Answer 2\"]")).FindElement(By.CssSelector("[data-e2e=\"voteAnswersStat-answer-Answer 2-percent\"]")).Text.Contains("2 голосов(33%)"), "answers statistics 1 answer count voices after vote");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswersStat-answer-Answer 3\"]")).Text.Contains("Answer 3"), "answers statistics 2 admin after vote");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswersStat-answer-Answer 3\"]")).FindElement(By.CssSelector("[data-e2e=\"voteAnswersStat-answer-Answer 3-percent\"]")).Text.Contains("4 голосов(67%)"), "answers statistics 2 answer count voices after vote");

            VerifyAreEqual("2", GetGridCell(0, "CountVoice", "Answers").Text, "grid CountVoice 1 answer after vote");
            VerifyAreEqual("4", GetGridCell(1, "CountVoice", "Answers").Text, "grid CountVoice 2 answer after vote");

            VerifyFinally(testname);
        }

        [Test]
        public void AnswersAddFromVoteEdit()
        {
            testname = "VotingAnswersAddFromVoteEdit";
            VerifyBegin(testname);

            GoToAdmin("voting");
            GetGridCell(2, "_serviceColumn", "Voting").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("Answer 4", driver.FindElement(By.CssSelector("[data-e2e=\"answer-Answer 4\"]")).Text, "pop up voting answer 1");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".payment-text")).Count == 1, "pop up count answers"); 

            driver.FindElement(By.CssSelector("[data-e2e=\"answerNameAdd\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"answerNameAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"answerNameAdd\"]")).SendKeys("New Answer Test From Voting Edit");

            driver.FindElement(By.CssSelector("[data-e2e=\"answerAdd\"]")).Click();

            if (!driver.FindElement(By.CssSelector("[data-e2e=\"voteIsDefault\"]")).FindElement(By.TagName("input")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"voteIsDefault\"]")).FindElement(By.TagName("span")).Click();
            }

            driver.FindElement(By.CssSelector("[data-e2e=\"voteSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("voting");

            GetGridCell(2, "_serviceColumn", "Voting").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("New Answer Test From Voting Edit", driver.FindElement(By.CssSelector("[data-e2e=\"answer-New Answer Test From Voting Edit\"]")).Text, "pop up voting answer added");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".payment-text")).Count == 2, "pop up count answers with added");

            XPathContainsText("button", "Отмена");

            GetGridCell(2, "ID", "Voting").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));

            VerifyAreEqual("Найдено записей: 2", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "grid count all");
            VerifyAreEqual("New Answer Test From Voting Edit", GetGridCell(1, "Name", "Answers").Text, "grid name added");
            VerifyAreEqual("0", GetGridCell(1, "CountVoice", "Answers").Text, "grid CountVoice added");
            VerifyAreEqual("0", GetGridCell(1, "SortOrder", "Answers").Text, "grid SortOrder added");
            VerifyIsTrue(GetGridCell(1, "IsVisible", "Answers").FindElement(By.TagName("input")).Selected, "grid IsVisible added");

            //check client
            GoToClient();
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-question")).Text.Contains("VoiceTheme3"), "voting name client");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-answers")).Text.Contains("Answer 4"), "voting answer 1 client");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-answers")).Text.Contains("New Answer Test From Voting Edit"), "added voting answer 2 client");

            VerifyFinally(testname);
        }

        [Test]
        public void AnswersEditSort()
        {
            testname = "VotingAnswersEditSort";
            VerifyBegin(testname);
            
            //set default
            GoToAdmin("voting");
            GetGridCell(3, "_serviceColumn", "Voting").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.CssSelector(".modal-content"));

            if (!driver.FindElement(By.CssSelector("[data-e2e=\"voteIsDefault\"]")).FindElement(By.TagName("input")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"voteIsDefault\"]")).FindElement(By.TagName("span")).Click();
            }

            driver.FindElement(By.CssSelector("[data-e2e=\"voteSave\"]")).Click();
            Thread.Sleep(2000);

            //pre check sort
            GoToClient();
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-answers")).FindElements(By.CssSelector(".voting-answers-row"))[0].Text.Contains("Answer 5"), "pre check sort client 1");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-answers")).FindElements(By.CssSelector(".voting-answers-row"))[1].Text.Contains("Answer 6"), "pre check sort client 2");

            //test
            GoToAdmin("voting");

            GetGridCell(3, "ID", "Voting").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));
            
            VerifyAreEqual("Answer 5", GetGridCell(0, "Name", "Answers").Text, "grid name");
            VerifyAreEqual("5", GetGridCell(0, "CountVoice", "Answers").Text, "grid CountVoice");
            VerifyAreEqual("5", GetGridCell(0, "SortOrder", "Answers").Text, "grid SortOrder");
            VerifyIsTrue(GetGridCell(0, "IsVisible", "Answers").FindElement(By.TagName("input")).Selected, "grid IsVisible");

            GetGridCell(0, "_serviceColumn", "Answers").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.CssSelector(".modal-content"));

            driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswerSortOrder\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswerSortOrder\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswerSortOrder\"]")).SendKeys("10");

            driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswerSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("voting");

            GetGridCell(3, "ID", "Voting").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);

            VerifyAreEqual("10", GetGridCell(0, "SortOrder", "Answers").Text, "grid SortOrder edited");

            //check client
            GoToClient();
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-answers")).FindElements(By.CssSelector(".voting-answers-row"))[0].Text.Contains("Answer 6"), "sort client 1 edited");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-answers")).FindElements(By.CssSelector(".voting-answers-row"))[1].Text.Contains("Answer 5"), "sort client 2 edited");

            VerifyFinally(testname);
        }

        [Test]
        public void AnswersEditName()
        {
            testname = "VotingAnswersEditName";
            VerifyBegin(testname);

            //set default
            GoToAdmin("voting");
            GetGridCell(4, "_serviceColumn", "Voting").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.CssSelector(".modal-content"));

            if (!driver.FindElement(By.CssSelector("[data-e2e=\"voteIsDefault\"]")).FindElement(By.TagName("input")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"voteIsDefault\"]")).FindElement(By.TagName("span")).Click();
            }

            driver.FindElement(By.CssSelector("[data-e2e=\"voteSave\"]")).Click();
            Thread.Sleep(2000);
            
            //test
            GoToAdmin("voting");

            GetGridCell(4, "ID", "Voting").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));

            VerifyAreEqual("Answer 7", GetGridCell(0, "Name", "Answers").Text, "grid name");

            GetGridCell(0, "_serviceColumn", "Answers").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.CssSelector(".modal-content"));

            driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswerName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswerName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswerName\"]")).SendKeys("Edited Answer Name");

            driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswerSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("voting");

            GetGridCell(4, "ID", "Voting").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);

            VerifyAreEqual("Edited Answer Name", GetGridCell(0, "Name", "Answers").Text, "grid name edited");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[grid-unique-id=\"gridAnswers\"]")).Text.Contains("Answer 7"), "previous edited name");

            //check client
            GoToClient();
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-answers")).Text.Contains("Edited Answer Name"), "edited answer name client");
            VerifyIsFalse(driver.FindElement(By.CssSelector(".voting-answers")).Text.Contains("Answer 7"), "previous edited answer name client");

            VerifyFinally(testname);
        }
        [Test]
        public void AnswersEditVisible()
        {
            testname = "VotingAnswersEditVisible";
            VerifyBegin(testname);

            //set default
            GoToAdmin("voting");
            GetGridCell(5, "_serviceColumn", "Voting").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.CssSelector(".modal-content"));

            if (!driver.FindElement(By.CssSelector("[data-e2e=\"voteIsDefault\"]")).FindElement(By.TagName("input")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"voteIsDefault\"]")).FindElement(By.TagName("span")).Click();
            }

            driver.FindElement(By.CssSelector("[data-e2e=\"voteSave\"]")).Click();
            Thread.Sleep(2000);

            //pre check client
            GoToClient();
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-answers")).Text.Contains("Answer 8"), "pre check client 1 answer is visible");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".voting-answers-row")).Count == 1, "pre check client 2 answer not visible count");
            VerifyIsFalse(driver.FindElement(By.CssSelector(".voting-answers")).Text.Contains("Answer 9"), "pre check client 2 answer not visible name");

            //test
            GoToAdmin("voting");

            GetGridCell(5, "ID", "Voting").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));

            VerifyAreEqual("Answer 8", GetGridCell(0, "Name", "Answers").Text, "grid name 1");
            VerifyIsTrue(GetGridCell(0, "IsVisible", "Answers").FindElement(By.TagName("input")).Selected, "grid IsVisible 1");
            VerifyAreEqual("Answer 9", GetGridCell(1, "Name", "Answers").Text, "grid name 2");
            VerifyIsFalse(GetGridCell(1, "IsVisible", "Answers").FindElement(By.TagName("input")).Selected, "grid IsVisible 2");

            GetGridCell(1, "_serviceColumn", "Answers").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswerIsVisible\"]")).FindElement(By.TagName("input")).Selected, "pop up IsVisible 2");
            driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswerIsVisible\"]")).FindElement(By.TagName("span")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswerSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("voting");

            GetGridCell(5, "ID", "Voting").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);

            GetGridCell(1, "_serviceColumn", "Answers").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"voteAnswerIsVisible\"]")).FindElement(By.TagName("input")).Selected, "pop up IsVisible 2 edited");

            VerifyAreEqual("Answer 9", GetGridCell(1, "Name", "Answers").Text, "grid name 2");
            VerifyIsTrue(GetGridCell(1, "IsVisible", "Answers").FindElement(By.TagName("input")).Selected, "grid IsVisible 2 edited");

            //check client
            GoToClient();
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-answers")).Text.Contains("Answer 8"), "client 1 answer is visible");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".voting-answers-row")).Count == 2, "client 2 answer edited visible count");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-answers")).Text.Contains("Answer 9"), "client 2 answer edited visible name");

            VerifyFinally(testname);
        }

        [Test]
        public void AnswersEditDeleteFromVoteEdit()
        {
            testname = "VotingAnswersEditDeleteFromVoteEdit";
            VerifyBegin(testname);

            GoToAdmin("voting");
            GetGridCell(6, "_serviceColumn", "Voting").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("Answer 10", driver.FindElement(By.CssSelector("[data-e2e=\"answer-Answer 10\"]")).Text, "pop up voting answer 1");
            VerifyAreEqual("Answer 11", driver.FindElement(By.CssSelector("[data-e2e=\"answer-Answer 11\"]")).Text, "pop up voting answer 2");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".payment-text")).Count == 2, "pop up count answers");

            driver.FindElement(By.CssSelector("[data-e2e=\"answer-id-11-delete\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            VerifyIsTrue(driver.FindElements(By.CssSelector(".payment-text")).Count == 1, "pop up count answers after deleting");

            if (!driver.FindElement(By.CssSelector("[data-e2e=\"voteIsDefault\"]")).FindElement(By.TagName("input")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"voteIsDefault\"]")).FindElement(By.TagName("span")).Click();
            }

            driver.FindElement(By.CssSelector("[data-e2e=\"voteSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("voting");

            GetGridCell(6, "_serviceColumn", "Voting").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("Answer 10", driver.FindElement(By.CssSelector("[data-e2e=\"answer-Answer 10\"]")).Text, "edit pop up voting answer 1 left after deleting");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".payment-text")).Count == 1, "edit pop up count answers after deleting");

            XPathContainsText("button", "Отмена");

            GetGridCell(6, "ID", "Voting").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));

            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "grid count all");
            VerifyAreEqual("Answer 10", GetGridCell(0, "Name", "Answers").Text, "grid name left after deleting");

            //check client
            GoToClient();
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-question")).Text.Contains("VoiceTheme7"), "voting name client");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".voting-answers")).Text.Contains("Answer 10"), "voting answer 1 client");
            VerifyIsFalse(driver.FindElement(By.CssSelector(".voting-answers")).Text.Contains("Answer 11"), "voting answer 2 client deleted");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".voting-answers-row")).Count == 1, "voting answer 2 answer deleted count");

            VerifyFinally(testname);
        }

        [Test]
        public void AnswersEditDeleteFromPage()
        {
            testname = "VotingAnswersEditDeleteFromPage";
            VerifyBegin(testname);

            //set default
            GoToAdmin("voting");

            GetGridCell(7, "_serviceColumn", "Voting").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.CssSelector(".modal-content"));

            if (!driver.FindElement(By.CssSelector("[data-e2e=\"voteIsDefault\"]")).FindElement(By.TagName("input")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"voteIsDefault\"]")).FindElement(By.TagName("span")).Click();
            }

            driver.FindElement(By.CssSelector("[data-e2e=\"voteSave\"]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("voting");
            GetGridCell(7, "ID", "Voting").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));

            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "grid count all before deleting");
            VerifyAreEqual("Answer 12", GetGridCell(0, "Name", "Answers").Text, "grid name before deleting");

            GetGridCell(0, "_serviceColumn", "Answers").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            //check admin 
            GoToAdmin("voting");
            GetGridCell(7, "ID", "Voting").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));

            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "grid count all after deleting");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "grid after deleting");

            //check client
            GoToClient();
            VerifyIsFalse(driver.PageSource.Contains("VoiceTheme8"), "voting name client deleted");
            VerifyIsFalse(driver.PageSource.Contains("Answer 12"), "voting answer 1 client deleted");

            VerifyFinally(testname);
        }


    }
}
