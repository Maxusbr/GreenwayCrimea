using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace AdvantShop.SeleniumTest.Admin.CMS.NewsCategory
{
    [TestFixture]
    public class CMSNewsCategoryPageTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CMS);
            InitializeService.LoadData(
                "data\\Admin\\CMS\\NewsCategory\\Settings.News.csv",
                "data\\Admin\\CMS\\NewsCategory\\Settings.NewsCategory.csv"
           );

            Init();
        }

        [Test]
        public void Page()
        {
            testname = "CMSNewsCategoryPage";
            VerifyBegin(testname);

            GoToAdmin("newscategory");
            VerifyAreEqual("NewsCategory1", GetGridCell(0, "Name").Text, "page 1 line 1");
            VerifyAreEqual("NewsCategory10", GetGridCell(9, "Name").Text, "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            Thread.Sleep(4000);
            VerifyAreEqual("NewsCategory11", GetGridCell(0, "Name").Text, "page 2 line 1");
            VerifyAreEqual("NewsCategory20", GetGridCell(9, "Name").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            Thread.Sleep(4000);
            VerifyAreEqual("NewsCategory21", GetGridCell(0, "Name").Text, "page 3 line 1");
            VerifyAreEqual("NewsCategory30", GetGridCell(9, "Name").Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            Thread.Sleep(4000);
            VerifyAreEqual("NewsCategory31", GetGridCell(0, "Name").Text, "page 4 line 1");
            VerifyAreEqual("NewsCategory40", GetGridCell(9, "Name").Text, "page 4 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(4000);
            VerifyAreEqual("NewsCategory41", GetGridCell(0, "Name").Text, "page 5 line 1");
            VerifyAreEqual("NewsCategory50", GetGridCell(9, "Name").Text, "page 5 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(4000);
            VerifyAreEqual("NewsCategory51", GetGridCell(0, "Name").Text, "page 6 line 1");
            VerifyAreEqual("NewsCategory60", GetGridCell(9, "Name").Text, "page 6 line 10");

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            Thread.Sleep(4000);
            VerifyAreEqual("NewsCategory1", GetGridCell(0, "Name").Text, "page 1 line 1");
            VerifyAreEqual("NewsCategory10", GetGridCell(9, "Name").Text, "page 1 line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void PageToPrevious()
        {
            testname = "CMSNewsCategoryPageToPrevious";
            VerifyBegin(testname);

            GoToAdmin("newscategory");
            VerifyAreEqual("NewsCategory1", GetGridCell(0, "Name").Text, "page 1 line 1");
            VerifyAreEqual("NewsCategory10", GetGridCell(9, "Name").Text, "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(4000);
            VerifyAreEqual("NewsCategory11", GetGridCell(0, "Name").Text, "page 2 line 1");
            VerifyAreEqual("NewsCategory20", GetGridCell(9, "Name").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(4000);
            VerifyAreEqual("NewsCategory21", GetGridCell(0, "Name").Text, "page 3 line 1");
            VerifyAreEqual("NewsCategory30", GetGridCell(9, "Name").Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(4000);
            VerifyAreEqual("NewsCategory11", GetGridCell(0, "Name").Text, "page 2 line 1");
            VerifyAreEqual("NewsCategory20", GetGridCell(9, "Name").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(4000);
            VerifyAreEqual("NewsCategory1", GetGridCell(0, "Name").Text, "page 1 line 1");
            VerifyAreEqual("NewsCategory10", GetGridCell(9, "Name").Text, "page 1 line 10");

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            Thread.Sleep(4000);
            VerifyAreEqual("NewsCategory101", GetGridCell(0, "Name").Text, "last page line 1");
            VerifyAreEqual("NewsCategory102", GetGridCell(1, "Name").Text, "last page line 10");

            VerifyFinally(testname);

        }
    }
}
