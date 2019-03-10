using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using AdvantShop.Selenium.Core.Infrastructure;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.CRM.Lead.Settings
{
    [TestFixture]
    public class CRMLeadSettingsTest : BaseSeleniumTest
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
                   "data\\Admin\\CRM\\Lead\\Customers.Departments.csv",
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
            GoToAdmin("settingscrm");
        }

         

        
        [Test]
        public void DealAdd()
        {
            testname = "LeadSettingsDealAdd";
            VerifyBegin(testname);
            
            ScrollTo(By.CssSelector("[data-e2e=\"DealStatusName\"]"));
            driver.FindElement(By.CssSelector("[data-e2e=\"DealStatusName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"DealStatusName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"DealStatusName\"]")).SendKeys("Deal Status Added");

            driver.FindElement(By.CssSelector("[data-e2e=\"DealStatusAdd\"]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("settingscrm");

            VerifyIsTrue(driver.FindElement(By.TagName("deal-statuses")).Text.Contains("Deal Status Added"), "deal status added");

            VerifyFinally(testname);
        }

        [Test]
        public void aDealDelete()
        {
            testname = "LeadSettingsDealDelete";
            VerifyBegin(testname);
            
            var dealStatuses = driver.FindElements(By.CssSelector("[data-e2e-crm=\"DealStatus\"]"));
            VerifyIsTrue(dealStatuses.Count == 6, "statuses before deleting"); 

            ScrollTo(By.CssSelector("[data-e2e-crm=\"DealStatusDelete\"][data-e2e-crm-deal-status-delete-id=\"1\"]"));
            driver.FindElement(By.CssSelector("[data-e2e-crm=\"DealStatusDelete\"][data-e2e-crm-deal-status-delete-id=\"2\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            GoToAdmin("settingscrm");

            var statusDeleted = driver.FindElements(By.CssSelector("[data-e2e-crm=\"DealStatus\"]"));
            VerifyIsTrue(statusDeleted.Count == 5, "statuses after deleting");

            VerifyFinally(testname);
        }
        
        [Test]
        public void DealEdit()
        {
            testname = "LeadSettingsDealEdit";
            VerifyBegin(testname);
            
            ScrollTo(By.CssSelector("[data-e2e-crm=\"DealStatus\"][data-e2e-crm-deal-status-id=\"5\"]"));
            VerifyIsTrue(driver.FindElement(By.TagName("deal-statuses")).Text.Contains("Сделка заключена"), "pre check deal status");

            driver.FindElement(By.CssSelector("[data-e2e-crm=\"DealStatus\"][data-e2e-crm-deal-status-id=\"5\"]")).Click();
            Thread.Sleep(2000);

            VerifyIsTrue(driver.FindElement(By.TagName("h2")).Text.Contains("Этап сделки"));

            driver.FindElement(By.CssSelector("[data-e2e=\"DealStatusName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"DealStatusName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"DealStatusName\"]")).SendKeys("Deal Status Edited");

            XPathContainsText("span", "Сохранить");
            Thread.Sleep(1000);

            GoToAdmin("settingscrm");

            VerifyIsTrue(driver.FindElement(By.TagName("deal-statuses")).Text.Contains("Deal Status Edited"), "deal status edited");
            VerifyIsFalse(driver.FindElement(By.TagName("deal-statuses")).Text.Contains("Сделка заключена"), "deal status old");

            VerifyFinally(testname);
        }

    }
}