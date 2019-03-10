using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.Customers
{
    [TestFixture]
    public class CustomersTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers | ClearType.Orders | ClearType.Catalog);
            InitializeService.LoadData(
               "data\\Admin\\Customers\\CustomerGrid\\Customers.CustomerGroup.csv",
                  "data\\Admin\\Customers\\CustomerGrid\\Customers.Country.csv",
            "data\\Admin\\Customers\\CustomerGrid\\Customers.Region.csv",
            "data\\Admin\\Customers\\CustomerGrid\\Customers.City.csv",
            "data\\Admin\\Customers\\CustomerGrid\\Customers.Customer.csv",
            "data\\Admin\\Customers\\CustomerGrid\\Customers.Contact.csv",
                       "data\\Admin\\Customers\\CustomerGrid\\Customers.Departments.csv",
           "data\\Admin\\Customers\\CustomerGrid\\Customers.Managers.csv",
               "data\\Admin\\Customers\\CustomerGrid\\Customers.CustomerField.csv",
               "data\\Admin\\Customers\\CustomerGrid\\Customers.CustomerFieldValue.csv",
               "data\\Admin\\Customers\\CustomerGrid\\Customers.CustomerFieldValuesMap.csv",
             "data\\Admin\\Customers\\CustomerGrid\\Catalog.Product.csv",
           "data\\Admin\\Customers\\CustomerGrid\\Catalog.Offer.csv",
           "data\\Admin\\Customers\\CustomerGrid\\Catalog.Category.csv",
           "data\\Admin\\Customers\\CustomerGrid\\Catalog.ProductCategories.csv",
            "data\\Admin\\Customers\\CustomerGrid\\[Order].OrderContact.csv",
              "Data\\Admin\\Customers\\CustomerGrid\\[Order].OrderSource.csv",
            "data\\Admin\\Customers\\CustomerGrid\\[Order].OrderCurrency.csv",
             "data\\Admin\\Customers\\CustomerGrid\\[Order].OrderItems.csv",
             "data\\Admin\\Customers\\CustomerGrid\\[Order].OrderStatus.csv",
                 "data\\Admin\\Customers\\CustomerGrid\\[Order].PaymentMethod.csv",
            "data\\Admin\\Customers\\CustomerGrid\\[Order].ShippingMethod.csv",
               "data\\Admin\\Customers\\CustomerGrid\\[Order].[Order].csv",

               "data\\Admin\\Customers\\CustomerGrid\\[Order].OrderCustomer.csv"

           );

            Init();
            GoToAdmin("customers");
        }



        [Test]
        public void aCustomerGrid()
        {
            testname = "aCustomerGrid";
            VerifyBegin(testname);

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Покупатели"), "h1 customers grid");

            VerifyAreEqual("FirstName120 LastName120", GetGridCell(0, "Name").FindElement(By.TagName("a")).Text, "customers Name grid");
            VerifyAreEqual("120", GetGridCell(0, "Phone").Text, "customers Phone grid");
            VerifyAreEqual("test@mail.ru120", GetGridCell(0, "Email").Text, "customers Email grid");
            VerifyAreEqual("0", GetGridCell(0, "OrdersCount").Text, "customers OrdersCount grid");
            VerifyAreEqual("", GetGridCell(0, "LastOrderNumber").Text, "customers LastOrderNumber grid");
            VerifyAreEqual("0", GetGridCell(0, "OrdersSum").Text, "customers OrdersSum grid");
            VerifyAreEqual("15.08.2017 15:37", GetGridCell(0, "RegistrationDateTimeFormatted").Text, "customers reg date grid");
            VerifyAreEqual("ManagerName2 ManagerLastName2", GetGridCell(0, "ManagerName").Text, "customers ManagerName grid");

            VerifyAreEqual("Найдено записей: 120", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all customers");

            VerifyFinally(testname);
        }

        [Test]
        public void CustomerGoToEditByName()
        {
            testname = "CustomerGoToEditByName";
            VerifyBegin(testname);

            GetGridCell(0, "Name").FindElement(By.TagName("a")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.Id("Customer_LastName"));

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("LastName120 FirstName120"), "customer edit");
            VerifyIsTrue(driver.Url.Contains("edit"), "url customer edit");

            GoToAdmin("customers");

            VerifyFinally(testname);
        }

        [Test]
        public void CustomerGoToLastOrder()
        {
            testname = "CustomerGoToLastOrder";
            VerifyBegin(testname);

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("FirstName5 LastName5");
            Blur();

            VerifyAreEqual("FirstName5 LastName5", GetGridCell(0, "Name").FindElement(By.TagName("a")).Text, "customer with last order");
            GetGridCell(0, "LastOrderNumber").FindElement(By.TagName("a")).Click();
            Thread.Sleep(5000);
            WaitForElem(By.Id("Order_OrderCustomer_LastName"));

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Заказ № 30"), "customer last order edit");
            VerifyIsTrue(driver.Url.Contains("edit/30"), "url customer last order edit");

            GoToAdmin("customers");

            VerifyFinally(testname);
        }

        [Test]
        public void CustomerGoToEditByServiceCol()
        {
            testname = "CustomerGoToEditByServiceCol";
            VerifyBegin(testname);

            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.Id("Customer_LastName"));

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("LastName120 FirstName120"), "customer edit");
            VerifyIsTrue(driver.Url.Contains("edit"), "url customer edit");

            GoToAdmin("customers");

            VerifyFinally(testname);
        }

        [Test]
        public void CustomerzSelectDelete()
        {
            testname = "CustomersSelectDelete";
            VerifyBegin(testname);

            //check delete cancel 
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("FirstName120 LastName120", GetGridCell(0, "Name").FindElement(By.TagName("a")).Text, "1 grid canсel delete");

            //check delete
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            VerifyAreNotEqual("FirstName120 LastName120", GetGridCell(0, "Name").FindElement(By.TagName("a")).Text, "1 grid delete");

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
            VerifyAreEqual("FirstName116 LastName116", GetGridCell(0, "Name").FindElement(By.TagName("a")).Text, "selected 2 grid delete");
            VerifyAreEqual("FirstName115 LastName115", GetGridCell(1, "Name").FindElement(By.TagName("a")).Text, "selected 3 grid delete");
            VerifyAreEqual("FirstName114 LastName114", GetGridCell(2, "Name").FindElement(By.TagName("a")).Text, "selected 4 grid delete");

            //check select all on page
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 1 grid");
            VerifyIsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 10 grid");

            //check delete all on page
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyAreEqual("FirstName106 LastName106", GetGridCell(0, "Name").FindElement(By.TagName("a")).Text, "selected all on page 2 grid delete");
            VerifyAreEqual("FirstName97 LastName97", GetGridCell(9, "Name").FindElement(By.TagName("a")).Text, "selected all on page 10 grid delete");

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

            GoToAdmin("customers");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete all");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all after deleting");

            VerifyFinally(testname);
        }
    }
}