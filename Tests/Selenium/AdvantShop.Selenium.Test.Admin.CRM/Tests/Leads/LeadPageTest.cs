using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.CRM.Leads
{
    [TestFixture]
    public class CRMLeadPageTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CRM | ClearType.Customers | ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\CRM\\Lead\\Catalog.Product.csv",
           "data\\Admin\\CRM\\Lead\\Catalog.Offer.csv",
           "data\\Admin\\CRM\\Lead\\Catalog.Category.csv",
           "data\\Admin\\CRM\\Lead\\Catalog.ProductCategories.csv",
         "data\\Admin\\CRM\\Lead\\Customers.Customer.csv",
           "data\\Admin\\CRM\\Lead\\Customers.CustomerGroup.csv",
               "data\\Admin\\CRM\\Lead\\Customers.CustomerField.csv",
               "data\\Admin\\CRM\\Lead\\Customers.CustomerFieldValue.csv",
               "data\\Admin\\CRM\\Lead\\Customers.CustomerFieldValuesMap.csv",
               "data\\Admin\\CRM\\Lead\\Customers.Managers.csv",
                "data\\Admin\\CRM\\Lead\\CRM.DealStatus.csv",
               //    "data\\Admin\\CRM\\Lead\\CRM.BizProcessRule.csv",
               "data\\Admin\\CRM\\Lead\\[Order].OrderSource.csv",
                "data\\Admin\\CRM\\Lead\\[Order].LeadCurrency.csv",
                 "data\\Admin\\CRM\\Lead\\[Order].LeadEvent.csv",
                    "data\\Admin\\CRM\\Lead\\[Order].LeadItem.csv",
                "data\\Admin\\CRM\\Lead\\[Order].Lead.csv",
             "data\\Admin\\CRM\\Lead\\Customers.TaskGroup.csv",
            "data\\Admin\\CRM\\Lead\\Customers.Task.csv"
           );

            Init();
            GoToAdmin("leads");
        }


         

        [Test]
        public void LeadPage()
        {
            testname = "LeadPage";
            VerifyBegin(testname);

            VerifyAreEqual("120", GetGridCell(0, "Id").FindElement(By.TagName("a")).Text, "page 1 line 1");
            VerifyAreEqual("111", GetGridCell(9, "Id").FindElement(By.TagName("a")).Text, "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            VerifyAreEqual("110", GetGridCell(0, "Id").FindElement(By.TagName("a")).Text, "page 2 line 1");
            VerifyAreEqual("101", GetGridCell(9, "Id").FindElement(By.TagName("a")).Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            VerifyAreEqual("100", GetGridCell(0, "Id").FindElement(By.TagName("a")).Text, "page 3 line 1");
            VerifyAreEqual("91", GetGridCell(9, "Id").FindElement(By.TagName("a")).Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            VerifyAreEqual("90", GetGridCell(0, "Id").FindElement(By.TagName("a")).Text, "page 4 line 1");
            VerifyAreEqual("81", GetGridCell(9, "Id").FindElement(By.TagName("a")).Text, "page 4 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            VerifyAreEqual("80", GetGridCell(0, "Id").FindElement(By.TagName("a")).Text, "page 5 line 1");
            VerifyAreEqual("71", GetGridCell(9, "Id").FindElement(By.TagName("a")).Text, "page 5 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            VerifyAreEqual("70", GetGridCell(0, "Id").FindElement(By.TagName("a")).Text, "page 6 line 1");
            VerifyAreEqual("61", GetGridCell(9, "Id").FindElement(By.TagName("a")).Text, "page 6 line 10");

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            VerifyAreEqual("120", GetGridCell(0, "Id").FindElement(By.TagName("a")).Text, "page 1 line 1");
            VerifyAreEqual("111", GetGridCell(9, "Id").FindElement(By.TagName("a")).Text, "page 1 line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void LeadPageToPrevious()
        {
            testname = "LeadPageToPrevious";
            VerifyBegin(testname);

            VerifyAreEqual("120", GetGridCell(0, "Id").FindElement(By.TagName("a")).Text, "page 1 line 1");
            VerifyAreEqual("111", GetGridCell(9, "Id").FindElement(By.TagName("a")).Text, "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("110", GetGridCell(0, "Id").FindElement(By.TagName("a")).Text, "page 2 line 1");
            VerifyAreEqual("101", GetGridCell(9, "Id").FindElement(By.TagName("a")).Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("100", GetGridCell(0, "Id").FindElement(By.TagName("a")).Text, "page 3 line 1");
            VerifyAreEqual("91", GetGridCell(9, "Id").FindElement(By.TagName("a")).Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            VerifyAreEqual("110", GetGridCell(0, "Id").FindElement(By.TagName("a")).Text, "page 2 line 1");
            VerifyAreEqual("101", GetGridCell(9, "Id").FindElement(By.TagName("a")).Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            VerifyAreEqual("120", GetGridCell(0, "Id").FindElement(By.TagName("a")).Text, "page 1 line 1");
            VerifyAreEqual("111", GetGridCell(9, "Id").FindElement(By.TagName("a")).Text, "page 1 line 10");

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            VerifyAreEqual("10", GetGridCell(0, "Id").FindElement(By.TagName("a")).Text, "last page line 1");
            VerifyAreEqual("1", GetGridCell(9, "Id").FindElement(By.TagName("a")).Text, "last page line 10");

            VerifyFinally(testname);
        }
    }
}