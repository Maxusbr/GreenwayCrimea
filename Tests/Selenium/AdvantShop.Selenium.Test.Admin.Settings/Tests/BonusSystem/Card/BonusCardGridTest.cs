using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.BonusSystem.Cards
{
    [TestFixture]
    public class BonusSystemCardGrid : BaseSeleniumTest
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
        public void CardGrid()
        {
            testname = "CardGrid";
            VerifyBegin(testname);

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Карты"), "h1 card grid");

            VerifyAreEqual("530801", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "Card Number");
            VerifyAreEqual("LastName1 FirstName1", GetGridCell(0, "FIO").Text, "FIO"); 
            VerifyAreEqual("Гостевой", GetGridCell(0, "GradeName").Text, "GradeName");
            VerifyAreEqual("3", GetGridCell(0, "GradePersent").Text, "Grade Percent");
            VerifyAreEqual("20.04.2017 15:40", GetGridCell(0, "CreatedFormatted").Text, "card date");

            VerifyAreEqual("Найдено записей: 120", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all cards");

            VerifyFinally(testname);
        }

        [Test]
        public void CardGoToEditByCardNum()
        {
            testname = "CardGoToEditByCardNum";
            VerifyBegin(testname);

            GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Click();
            Thread.Sleep(4000);

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Карта LastName1 FirstName1"), "h1 card edit");
            VerifyIsTrue(driver.Url.Contains("edit"), "url card edit");

            GoToAdmin("cards");

            VerifyFinally(testname);
        }


        [Test]
        public void CardGoToEditByServiceCol()
        {
            testname = "CardGoToEditByServiceCol";
            VerifyBegin(testname);

            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Карта LastName1 FirstName1"), "h1 card edit");
            VerifyIsTrue(driver.Url.Contains("edit"), "url lead edit");

            driver.FindElement(By.LinkText("Все карты")).Click();
            Thread.Sleep(3000);

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Карты"), "h1 card grid");

            VerifyFinally(testname);
        }

        [Test]
        public void CardzSelectDelete()
        {
            testname = "CardsSelectDelete";
            VerifyBegin(testname);

            //check delete cancel 
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("530801", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "1 grid canсel delete");

            //check delete
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            VerifyAreNotEqual("530801", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "1 grid delete");

            //check select 
            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 2 grid");
            VerifyIsTrue(GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 3 grid");
            VerifyIsTrue(GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 4 grid");
            VerifyAreEqual("3", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count selected");

            //check delete selected items
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyAreEqual("530805", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "selected 2 grid delete");
            VerifyAreEqual("530806", GetGridCell(1, "CardNumber").FindElement(By.TagName("a")).Text, "selected 3 grid delete");
            VerifyAreEqual("530807", GetGridCell(2, "CardNumber").FindElement(By.TagName("a")).Text, "selected 4 grid delete");

            //check select all on page
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 1 grid");
            VerifyIsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 10 grid");

            //check delete all on page
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyAreEqual("530815", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "selected all on page 2 grid delete");
            VerifyAreEqual("530824", GetGridCell(9, "CardNumber").FindElement(By.TagName("a")).Text, "selected all on page 10 grid delete");

            //check select all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("106", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count all selected after deleting");

            //check deselect all 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(!GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 1 grid");
            VerifyIsTrue(!GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 10 grid");

            //check delete all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            GoToAdmin("cards");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete all");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all after deleting");

            VerifyFinally(testname);
        }
    }
}
