using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.Customers
{
    [TestFixture]
    public class CustomersSearchTest : BaseSeleniumTest
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
        }
        
        [Test]
        public void SearchExist()
        {
            testname = "SearchExist";
            VerifyBegin(testname);

            GoToAdmin("customers");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("FirstName111 LastName111");
            Blur();

            VerifyAreEqual("FirstName111 LastName111", GetGridCell(0, "Name").FindElement(By.TagName("a")).Text, "search exist customer");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }


        [Test]
        public void SearchNotExist()
        {
            testname = "SearchNotExist";
            VerifyBegin(testname);

            GoToAdmin("customers");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Unknown Unknown");
            Blur();

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search not exist");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }
        
        [Test]
        public void SearchMuchSymbols()
        {
            testname = "SearchMuchSymbols";
            VerifyBegin(testname);

            GoToAdmin("customers");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            Blur();

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search too much symbols");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void SearchInvalidSymbols()
        {
            testname = "SearchInvalidSymbols";
            VerifyBegin(testname);

            GoToAdmin("customers");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            Blur();

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search invalid symbols");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }
    }
}