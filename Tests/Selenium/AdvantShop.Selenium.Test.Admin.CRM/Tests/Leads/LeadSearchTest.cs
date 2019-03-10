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
    public class CRMLeadSearchTest : BaseSeleniumTest
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
        }
        
         

        [Test]
        public void LeadSearchExistById()
        {
            testname = "LeadSearchExistById";
            VerifyBegin(testname);

            GoToAdmin("leads");

            GetGridFilterTab(0, "111");
            //DropFocus("h1");

            VerifyAreEqual("111", GetGridCell(0, "Id").FindElement(By.TagName("a")).Text, "search exist lead id");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }


        [Test]
        public void LeadSearchExistByContact()
        {
            testname = "LeadSearchExistByContact";
            VerifyBegin(testname);

            GoToAdmin("leads");

            GetGridFilterTab(0, "Patronymic");
            //DropFocus("h1");

            VerifyAreEqual("LastName FirstName Patronymic", GetGridCell(0, "FullName").Text, "search exist fullname");
            VerifyAreEqual("Найдено записей: 21", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void LeadSearchNotExistId()
        {
            testname = "LeadSearchNotExistId";
            VerifyBegin(testname);

            GoToAdmin("leads");
            
            GetGridFilterTab(0, "3333333333");
            //DropFocus("h1");

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search not exist lead id");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void LeadSearchMuchSymbols()
        {
            testname = "LeadSearchMuchSymbols";
            VerifyBegin(testname);

            GoToAdmin("leads");
            
            GetGridFilterTab(0, "1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            //DropFocus("h1");

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search too much symbols");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void LeadSearchInvalidSymbols()
        {
            testname = "LeadSearchInvalidSymbols";
            VerifyBegin(testname);

            GoToAdmin("leads");
            
            GetGridFilterTab(0, "########@@@@@@@@&&&&&&&******,,,,..");
            //DropFocus("h1");

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search invalid symbols");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

    }
}