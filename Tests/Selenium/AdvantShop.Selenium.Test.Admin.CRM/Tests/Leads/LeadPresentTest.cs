using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.CRM.Leads
{
    [TestFixture]
    public class CRMLeadPresentTest : BaseSeleniumTest
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
        public void LeadPresent10()
        {
            testname = "LeadPresent10";
            VerifyBegin(testname);

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("10");
            VerifyAreEqual("120", GetGridCell(0, "Id").FindElement(By.TagName("a")).Text, "present line 1");
            VerifyAreEqual("111", GetGridCell(9, "Id").FindElement(By.TagName("a")).Text, "present line 10");
                    
            VerifyFinally(testname);
        }

        [Test]
        public void LeadPresent20()
        {
            testname = "LeadPresent20";
            VerifyBegin(testname);

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("20");
            VerifyAreEqual("120", GetGridCell(0, "Id").FindElement(By.TagName("a")).Text, "present line 1");
            VerifyAreEqual("101", GetGridCell(19, "Id").FindElement(By.TagName("a")).Text, "present line 20");

            VerifyFinally(testname);
        }

        [Test]
        public void LeadPresent50()
        {
            testname = "LeadPresent50";
            VerifyBegin(testname);
            
            ScrollToElements(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("50");
            VerifyAreEqual("120", GetGridCell(0, "Id").FindElement(By.TagName("a")).Text, "present line 1");
            VerifyAreEqual("71", GetGridCell(49, "Id").FindElement(By.TagName("a")).Text, "present line 50");

            VerifyFinally(testname);
        }

        [Test]
        public void LeadPresent100()
        {
            testname = "LeadPresent100";
            VerifyBegin(testname);

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("100");
            VerifyAreEqual("120", GetGridCell(0, "Id").FindElement(By.TagName("a")).Text, "present line 1");
            VerifyAreEqual("21", GetGridCell(99, "Id").FindElement(By.TagName("a")).Text, "present line 100");

            VerifyFinally(testname);
        }
    }
}