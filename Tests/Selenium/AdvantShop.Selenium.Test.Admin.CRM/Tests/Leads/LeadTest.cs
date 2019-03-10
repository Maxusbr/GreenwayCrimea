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
    public class CRMLeadTest : BaseSeleniumTest
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
        public void aLeadGrid()
        {
            testname = "CRMLeadGrid";
            VerifyBegin(testname);

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Лиды"), "h1 lead grid");
            
            VerifyAreEqual("120", GetGridCell(0, "Id").FindElement(By.TagName("a")).Text, "lead Id");
            VerifyAreEqual("Новый", GetGridCell(0, "DealStatusName").Text, "DealStatusName");
            VerifyAreEqual("LastName FirstName Patronymic", GetGridCell(0, "FullName").Text, "FullName");
            VerifyAreEqual("test testov", GetGridCell(0, "ManagerName").Text, "ManagerName");
            VerifyAreEqual("20", GetGridCell(0, "ProductsCount").Text, "ProductsCount");
            VerifyAreEqual("120", GetGridCell(0, "Sum").Text, "Sum");

            VerifyAreEqual("Найдено записей: 120", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all leads");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".ui-grid-custom-footer")).Text.Contains("Сумма сделок: 7260 руб."), "count all sum");

            VerifyFinally(testname);
        }
        
        [Test]
        public void LeadGoToEditById()
        {
            testname = "CRMLeadGoToEditById";
            VerifyBegin(testname);

            GetGridCell(0, "Id").FindElement(By.TagName("a")).Click();
            Thread.Sleep(4000);

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Лид № 120"), "h1 lead edit");
            VerifyIsTrue(driver.Url.Contains("edit"), "url lead edit");

            GoToAdmin("leads");

            VerifyFinally(testname);
        }


        [Test]
        public void LeadGoToEditByServiceCol()
        {
            testname = "CRMLeadGoToEditByServiceCol";
            VerifyBegin(testname);

            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Лид № 120"), "h1 lead edit page");
            VerifyIsTrue(driver.Url.Contains("edit"), "url lead edit");

            driver.FindElement(By.LinkText("Список лидов")).Click();
            Thread.Sleep(4000);

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Лиды"), "h1 lead grid");

            VerifyFinally(testname);
        }
        
        [Test]
        public void LeadzSelectDelete()
        {
            testname = "CRMLeadSelectDelete";
            VerifyBegin(testname);

            //check delete cancel 
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("120", GetGridCell(0, "Id").FindElement(By.TagName("a")).Text, "1 grid canсel delete");

            //check delete
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            VerifyAreNotEqual("120", GetGridCell(0, "Id").FindElement(By.TagName("a")).Text, "1 grid delete");

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
            VerifyAreEqual("116", GetGridCell(0, "Id").FindElement(By.TagName("a")).Text, "selected 2 grid delete");
            VerifyAreEqual("115", GetGridCell(1, "Id").FindElement(By.TagName("a")).Text, "selected 3 grid delete");
            VerifyAreEqual("114", GetGridCell(2, "Id").FindElement(By.TagName("a")).Text, "selected 4 grid delete");

            //check select all on page
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 1 grid");
            VerifyIsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 10 grid");

            //check delete all on page
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyAreEqual("106", GetGridCell(0, "Id").FindElement(By.TagName("a")).Text, "selected all on page 2 grid delete");
            VerifyAreEqual("97", GetGridCell(9, "Id").FindElement(By.TagName("a")).Text, "selected all on page 10 grid delete");

            //check select all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            VerifyAreEqual("106", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count all selected after deleting");

            //check deselect all 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            VerifyIsTrue(!GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 1 grid");
            VerifyIsTrue(!GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 10 grid");

            //check delete all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            GoToAdmin("leads");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete all");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all after deleting");

            VerifyFinally(testname);
        }
    }
}