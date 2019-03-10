using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.CustomersGroup
{
    [TestFixture]
    public class CustomersGroupPageTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.ClearData(ClearType.Customers);
            InitializeService.LoadData(
           "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupPage\\Catalog.Product.csv",
           "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupPage\\Catalog.Offer.csv",
           "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupPage\\Catalog.Category.csv",
           "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupPage\\Catalog.ProductCategories.csv",
           "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupPage\\Customers.Customer.csv",
           "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupPage\\Customers.CustomerGroup.csv",
           "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupPage\\Customers.Departments.csv",
           "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupPage\\Customers.Managers.csv",
            "data\\Admin\\Customers\\CustomersGroup\\CustomersGroupPage\\Customers.ManagerTask.csv"
           );
             
            Init();
            GoToAdmin("customergroups");
        }

         

        [Test]
        public void CustomersGroupPage()
        {
            testname = "CustomersGroupPage";
            VerifyBegin(testname);
            
            VerifyAreEqual("CustomerGroup101", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 1");
            VerifyAreEqual("CustomerGroup110", GetGridCell(9, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            VerifyAreEqual("CustomerGroup111", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"), "page 2 line 1");
            VerifyAreEqual("CustomerGroup120", GetGridCell(9, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"), "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            VerifyAreEqual("CustomerGroup191", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"), "page 3 line 1");
            VerifyAreEqual("CustomerGroup200", GetGridCell(9, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"), "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            VerifyAreEqual("CustomerGroup201", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"), "page 4 line 1");
            VerifyAreEqual("CustomerGroup210", GetGridCell(9, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"), "page 4 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            VerifyAreEqual("CustomerGroup211", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"), "page 5 line 1");
            VerifyAreEqual("CustomerGroup220", GetGridCell(9, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"), "page 5 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            VerifyAreEqual("CustomerGroup221", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"), "page 6 line 1");
            VerifyAreEqual("CustomerGroup230", GetGridCell(9, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"), "page 6 line 10");

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            VerifyAreEqual("CustomerGroup101", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 1");
            VerifyAreEqual("CustomerGroup110", GetGridCell(9, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 10");

            VerifyFinally(testname);
        }
       
        [Test]
        public void CustomersGroupPageToPrevious()
        {
            testname = "CustomersGroupPageToPrevious";
            VerifyBegin(testname);
            
            VerifyAreEqual("CustomerGroup101", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 1");
            VerifyAreEqual("CustomerGroup110", GetGridCell(9, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("CustomerGroup111", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"), "page 2 line 1");
            VerifyAreEqual("CustomerGroup120", GetGridCell(9, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"), "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("CustomerGroup191", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"), "page 3 line 1");
            VerifyAreEqual("CustomerGroup200", GetGridCell(9, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"), "page 3 line 10");
            
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            VerifyAreEqual("CustomerGroup111", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"), "page 2 line 1");
            VerifyAreEqual("CustomerGroup120", GetGridCell(9, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"), "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            VerifyAreEqual("CustomerGroup101", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 1");
            VerifyAreEqual("CustomerGroup110", GetGridCell(9, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 10");

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            VerifyAreEqual("CustomerGroup181", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"), "last page line 1");
            VerifyAreEqual("CustomerGroup190", GetGridCell(9, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"), "last page line 10");

            VerifyFinally(testname);
        }
    }
}