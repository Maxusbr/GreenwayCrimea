using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using AdvantShop.Selenium.Core.Infrastructure;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.CRM.Leads
{
    [TestFixture]
    public class CRMLeadSortTest : BaseSeleniumTest
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
        public void LeadSortId()
        {
            testname = "CRMLeadSortId";
            VerifyBegin(testname);

            GetGridCell(-1, "Id").Click();
            WaitForAjax();
            VerifyAreEqual("1", GetGridCell(0, "Id").FindElement(By.TagName("a")).Text, "sort lead Id 1 asc");
            VerifyAreEqual("10", GetGridCell(9, "Id").FindElement(By.TagName("a")).Text, "sort lead Id 10 asc");

            GetGridCell(-1, "Id").Click();
            WaitForAjax();
            VerifyAreEqual("120", GetGridCell(0, "Id").FindElement(By.TagName("a")).Text, "sort lead Id 1 desc");
            VerifyAreEqual("111", GetGridCell(9, "Id").FindElement(By.TagName("a")).Text, "sort lead Id 10 desc");

            VerifyFinally(testname);
        }

        [Test]
        public void LeadSortDealStatusName()
        {
            testname = "CRMLeadSortDealStatusName";
            VerifyBegin(testname);

            GetGridCell(-1, "DealStatusName").Click();
            WaitForAjax();
            VerifyAreEqual("", GetGridCell(0, "DealStatusName").Text, "sort DealStatusName 1 asc");
            VerifyAreEqual("", GetGridCell(9, "DealStatusName").Text, "sort DealStatusName 10 asc");

            string ascLine1 = GetGridCell(0, "Id").FindElement(By.TagName("a")).Text;
            string ascLine10 = GetGridCell(9, "Id").FindElement(By.TagName("a")).Text;

            VerifyIsFalse(ascLine1.Equals(ascLine10), "asc different leads");

            GetGridCell(-1, "DealStatusName").Click();
            WaitForAjax();
            VerifyAreEqual("Созвон с клиентом", GetGridCell(0, "DealStatusName").Text, "sort DealStatusName 1 desc");
            VerifyAreEqual("Созвон с клиентом", GetGridCell(9, "DealStatusName").Text, "sort DealStatusName 10 desc");

            string descLine1 = GetGridCell(0, "Id").FindElement(By.TagName("a")).Text;
            string descLine10 = GetGridCell(9, "Id").FindElement(By.TagName("a")).Text;

            VerifyIsFalse(descLine1.Equals(descLine10), "desc different leads");

            VerifyFinally(testname);
        }

        [Test]
        public void LeadSortContact()
        {
            testname = "CRMLeadSortContact";
            VerifyBegin(testname);

            GetGridCell(-1, "FullName").Click();
            WaitForAjax();
            VerifyAreEqual("LastName1 FirstName1 Patron1", GetGridCell(0, "FullName").Text, "sort FullName 1 asc");
            VerifyAreEqual("LastName10 FirstName10 Patron10", GetGridCell(9, "FullName").Text, "sort FullName 10 asc");

            GetGridCell(-1, "FullName").Click();
            WaitForAjax();
            VerifyAreEqual("LastName FirstName Patronymic", GetGridCell(0, "FullName").Text, "sort FullName 1 desc");
            VerifyAreEqual("LastName FirstName Patronymic", GetGridCell(9, "FullName").Text, "sort FullName 10 asc");

            string descLine1 = GetGridCell(0, "Id").FindElement(By.TagName("a")).Text;
            string descLine10 = GetGridCell(9, "Id").FindElement(By.TagName("a")).Text;

            VerifyIsFalse(descLine1.Equals(descLine10), "desc different leads");

            VerifyFinally(testname);
        }


        [Test]
        public void LeadSortSortManager()
        {
            testname = "CRMLeadSortSortManager";
            VerifyBegin(testname);

            GetGridCell(-1, "ManagerName").Click();
            WaitForAjax();
            VerifyAreEqual("", GetGridCell(0, "ManagerName").Text, "sort ManagerName 1 asc");
            VerifyAreEqual("", GetGridCell(9, "ManagerName").Text, "sort ManagerName 10 asc");

            string ascLine1 = GetGridCell(0, "Id").FindElement(By.TagName("a")).Text;
            string ascLine10 = GetGridCell(9, "Id").FindElement(By.TagName("a")).Text;

            VerifyIsFalse(ascLine1.Equals(ascLine10), "asc different leads");

            GetGridCell(-1, "ManagerName").Click();
            WaitForAjax();
            VerifyAreEqual("test testov", GetGridCell(0, "ManagerName").Text, "sort ManagerName 1 desc");
            VerifyAreEqual("test testov", GetGridCell(9, "ManagerName").Text, "sort ManagerName 10 asc");

            string descLine1 = GetGridCell(0, "Id").FindElement(By.TagName("a")).Text;
            string descLine10 = GetGridCell(9, "Id").FindElement(By.TagName("a")).Text;

            VerifyIsFalse(descLine1.Equals(descLine10), "desc different leads");

            VerifyFinally(testname);
        }

        [Test]
        public void LeadSortProductsCount()
        {
            testname = "CRMLeadSortProductsCount";
            VerifyBegin(testname);

            GetGridCell(-1, "ProductsCount").Click();
            WaitForAjax();
            VerifyAreEqual("1", GetGridCell(0, "ProductsCount").Text, "sort ProductsCount 1 asc");
            VerifyAreEqual("5", GetGridCell(9, "ProductsCount").Text, "sort ProductsCount 10 asc");

            GetGridCell(-1, "ProductsCount").Click();
            WaitForAjax();
            VerifyAreEqual("100", GetGridCell(0, "ProductsCount").Text, "sort ProductsCount 1 desc");
            VerifyAreEqual("91", GetGridCell(9, "ProductsCount").Text, "sort ProductsCount 10 desc");

            VerifyFinally(testname);
        }

        [Test]
        public void LeadSortSum()
        {
            testname = "CRMLeadSortSum";
            VerifyBegin(testname);

            GetGridCell(-1, "Sum").Click();
            WaitForAjax();
            VerifyAreEqual("1", GetGridCell(0, "Sum").Text, "sort Sum 1 asc");
            VerifyAreEqual("10", GetGridCell(9, "Sum").Text, "sort Sum 10 asc");

            GetGridCell(-1, "Sum").Click();
            WaitForAjax();
            VerifyAreEqual("120", GetGridCell(0, "Sum").Text, "sort Sum 1 desc");
            VerifyAreEqual("111", GetGridCell(9, "Sum").Text, "sort Sum 10 desc");

            VerifyFinally(testname);
        }

    }
}