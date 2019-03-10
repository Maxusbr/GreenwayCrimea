using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.CMS.News
{
    [TestFixture]
    public class CMSNewsPageTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CMS);
            InitializeService.LoadData(
                "data\\Admin\\CMS\\News\\Settings.News.csv",
                "data\\Admin\\CMS\\News\\Settings.NewsCategory.csv"
                );

            Init();
        }
        
         

        [Test]
        public void Page()
        {
            testname = "CMSNewsPage";
            VerifyBegin(testname);

            GoToAdmin("news");
            VerifyAreEqual("title Test News 145", GetGridCell(0, "Title").Text, "page 1 line 1");
            VerifyAreEqual("title Test News 135", GetGridCell(9, "Title").Text, "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            VerifyAreEqual("title Test News 129", GetGridCell(0, "Title").Text, "page 2 line 1");
            VerifyAreEqual("title Test News 111", GetGridCell(9, "Title").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            VerifyAreEqual("title Test News 109", GetGridCell(0, "Title").Text, "page 3 line 1");
            VerifyAreEqual("title Test News 95", GetGridCell(9, "Title").Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            VerifyAreEqual("title Test News 89", GetGridCell(0, "Title").Text, "page 4 line 1");
            VerifyAreEqual("title Test News 71", GetGridCell(9, "Title").Text, "page 4 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            VerifyAreEqual("title Test News 69", GetGridCell(0, "Title").Text, "page 5 line 1");
            VerifyAreEqual("title Test News 55", GetGridCell(9, "Title").Text, "page 5 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            VerifyAreEqual("title Test News 49", GetGridCell(0, "Title").Text, "page 6 line 1");
            VerifyAreEqual("title Test News 31", GetGridCell(9, "Title").Text, "page 6 line 10");

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            VerifyAreEqual("title Test News 145", GetGridCell(0, "Title").Text, "page 1 line 1");
            VerifyAreEqual("title Test News 135", GetGridCell(9, "Title").Text, "page 1 line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void ToPrevious()
        {
            testname = "CMSNewsPageToPrevious";
            VerifyBegin(testname);

            GoToAdmin("news");
            VerifyAreEqual("title Test News 145", GetGridCell(0, "Title").Text, "page 1 line 1");
            VerifyAreEqual("title Test News 135", GetGridCell(9, "Title").Text, "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("title Test News 129", GetGridCell(0, "Title").Text, "page 2 line 1");
            VerifyAreEqual("title Test News 111", GetGridCell(9, "Title").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("title Test News 109", GetGridCell(0, "Title").Text, "page 3 line 1");
            VerifyAreEqual("title Test News 95", GetGridCell(9, "Title").Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            VerifyAreEqual("title Test News 129", GetGridCell(0, "Title").Text, "page 2 line 1");
            VerifyAreEqual("title Test News 111", GetGridCell(9, "Title").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("title Test News 145", GetGridCell(0, "Title").Text, "page 1 line 1");
            VerifyAreEqual("title Test News 135", GetGridCell(9, "Title").Text, "page 1 line 10");

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            VerifyAreEqual("title Test News 22", GetGridCell(0, "Title").Text, "last page line 1");
            VerifyAreEqual("title Test News 2", GetGridCell(9, "Title").Text, "last page line 10");

            VerifyFinally(testname);
        }
    }
}
