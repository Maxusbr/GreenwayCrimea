using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.BonusSystem.Cards
{
    [TestFixture]
    public class BonusSystemCardSort : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.Bonuses);
            InitializeService.LoadData(

             "Data\\Admin\\Settings\\BonusSystem\\Grid\\Catalog.Product.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Grid\\Catalog.Offer.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Grid\\Catalog.Category.csv",
          "Data\\Admin\\Settings\\BonusSystem\\Grid\\Catalog.ProductCategories.csv",
               "Data\\Admin\\Settings\\BonusSystem\\Grid\\Bonus.Grade.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Grid\\Bonus.Card.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Grid\\Customers.CustomerGroup.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Grid\\Customers.Customer.csv"


                );
            Init();

            GoToAdmin("cards");
        }

         

        [Test]
        public void ByNum()
        {
            testname = "CardSortByNum";
            VerifyBegin(testname);

            GetGridCell(-1, "CardNumber").Click();
            WaitForAjax();
            VerifyAreEqual("530801", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "sort card num asc 1");
            VerifyAreEqual("530810", GetGridCell(9, "CardNumber").FindElement(By.TagName("a")).Text, "sort card num asc 10");

            GetGridCell(-1, "CardNumber").Click();
            WaitForAjax();
            VerifyAreEqual("530920", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "sort card num desc 1");
            VerifyAreEqual("530911", GetGridCell(9, "CardNumber").FindElement(By.TagName("a")).Text, "sort card num desc 10");

            VerifyFinally(testname);
        }

        [Test]
        public void ByFIO()
        {
            testname = "CardSortByFIO";
            VerifyBegin(testname);

            GetGridCell(-1, "FIO").Click();
            WaitForAjax();
            VerifyAreEqual("LastName1 FirstName1", GetGridCell(0, "FIO").Text, "sort card fio asc 1");
            VerifyAreEqual("LastName107 FirstName107", GetGridCell(9, "FIO").Text, "sort card fio asc 10");

            GetGridCell(-1, "FIO").Click();
            WaitForAjax();
            VerifyAreEqual("LastName99 FirstName99", GetGridCell(0, "FIO").Text, "sort card fio desc 1");
            VerifyAreEqual("LastName90 FirstName90", GetGridCell(9, "FIO").Text, "sort card fio desc 10");

            VerifyFinally(testname);
        }

        [Test]
        public void ByGrade()
        {
            testname = "CardSortByGrade";
            VerifyBegin(testname);

            GetGridCell(-1, "GradeName").Click();
            WaitForAjax();
            VerifyAreEqual("Бронзовый", GetGridCell(0, "GradeName").Text, "sort card grade asc 1");
            VerifyAreEqual("Бронзовый", GetGridCell(9, "GradeName").Text, "sort card grade asc 10");
            VerifyIsFalse(GetGridCell(0, "CardNumber").Text.Equals(GetGridCell(9, "CardNumber").Text), "sort card grade asc diff fields");

            GetGridCell(-1, "GradeName").Click();
            WaitForAjax();
            VerifyAreEqual("Серебряный", GetGridCell(0, "GradeName").Text, "sort card grade desc 1");
            VerifyAreEqual("Серебряный", GetGridCell(9, "GradeName").Text, "sort card grade desc 10");
            VerifyIsFalse(GetGridCell(0, "CardNumber").Text.Equals(GetGridCell(9, "CardNumber").Text), "sort card grade desc diff fields");

            VerifyFinally(testname);
        }

        [Test]
        public void ByPercent()
        {
            testname = "CardSortByPercent";
            VerifyBegin(testname);

            GetGridCell(-1, "GradePersent").Click();
            WaitForAjax();
            VerifyAreEqual("3", GetGridCell(0, "GradePersent").Text, "sort card percent asc 1");
            VerifyAreEqual("3", GetGridCell(9, "GradePersent").Text, "sort card percent asc 10");
            VerifyIsFalse(GetGridCell(0, "CardNumber").Text.Equals(GetGridCell(9, "CardNumber").Text), "sort card percent asc diff fields");

            GetGridCell(-1, "GradePersent").Click();
            WaitForAjax();
            VerifyAreEqual("30", GetGridCell(0, "GradePersent").Text, "sort card percent desc 1");
            VerifyAreEqual("30", GetGridCell(9, "GradePersent").Text, "sort card percent desc 10");
            VerifyIsFalse(GetGridCell(0, "CardNumber").Text.Equals(GetGridCell(9, "CardNumber").Text), "sort card percent desc diff fields");

            VerifyFinally(testname);
        }

        [Test]
        public void ByCreatedDate()
        {
            testname = "CardSortByCreatedDate";
            VerifyBegin(testname);

            GetGridCell(-1, "CreatedFormatted").Click();
            WaitForAjax();
            VerifyAreEqual("22.12.2016 15:40", GetGridCell(0, "CreatedFormatted").Text, "sort card created date asc 1");
            VerifyAreEqual("31.12.2016 15:40", GetGridCell(9, "CreatedFormatted").Text, "sort card created date asc 10");

            GetGridCell(-1, "CreatedFormatted").Click();
            WaitForAjax();
            VerifyAreEqual("20.04.2017 15:40", GetGridCell(0, "CreatedFormatted").Text, "sort card created date desc 1");
            VerifyAreEqual("11.04.2017 15:40", GetGridCell(9, "CreatedFormatted").Text, "sort card created date desc 10");

            VerifyFinally(testname);
        }
    }
}
