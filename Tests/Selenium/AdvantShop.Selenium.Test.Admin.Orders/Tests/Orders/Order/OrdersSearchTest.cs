using System;
using NUnit.Framework;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium;

namespace AdvantShop.SeleniumTest.Admin.Orders
{
    [TestFixture]
    public class OrdersSearchTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.Orders);
            InitializeService.LoadData(

             "Data\\Admin\\Orders\\OrderGrid\\Catalog.Product.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Offer.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Category.csv",
          "Data\\Admin\\Orders\\OrderGrid\\Catalog.ProductCategories.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Customers.CustomerGroup.csv",
           "data\\Admin\\Orders\\OrderGrid\\Customers.Country.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Region.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.City.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Customer.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Contact.csv",
              "data\\Admin\\Orders\\OrderGrid\\Customers.Managers.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCustomer.csv",
            "Data\\Admin\\Orders\\OrderGrid\\[Order].OrderSource.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderContact.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCurrency.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderItems.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderStatus.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].[Order].csv"
          );

            Init();
            GoToAdmin("orders");
        }

        [Test]
        public void ByNumberExist()
        {
            testname = "SearchByNumberExist";
            VerifyBegin(testname);

            GetGridFilterTab(0, "12");
            VerifyAreEqual("12", GetGridCell(0, "Number").Text, "order number");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void ByCustomer()
        {
            testname = "SearchByCustomer";
            VerifyBegin(testname);

            GetGridFilterTab(0, "FirstName4 LastName4");
            VerifyAreEqual("FirstName4 LastName4", GetGridCell(0, "BuyerName").Text, "order customer");
            VerifyAreEqual("99", GetGridCell(0, "Number").Text, "order number");
            VerifyAreEqual("Найдено записей: 14", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void ByNumberNotExist()
        {
            testname = "SearchByNumberNotExist";
            VerifyBegin(testname);

            GetGridFilterTab(0, "552");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "order number not exist");

            VerifyFinally(testname);
        }


        [Test]
        public void ByCustomerNotExist()
        {
            testname = "SearchByCustomerNotExist";
            VerifyBegin(testname);

            GetGridFilterTab(0, "FirstName2 LastName4");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "order customer not exist");

            VerifyFinally(testname);
        }

        [Test]
        public void MuchSymbols()
        {
            testname = "SearchMuchSymbols";
            VerifyBegin(testname);
            
            GetGridFilterTab(0, "1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search too much symbols");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void InvalidSymbols()
        {
            testname = "SearchInvalidSymbols";
            VerifyBegin(testname);
            
            GetGridFilterTab(0, "########@@@@@@@@&&&&&&&******,,,,..");

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search invalid symbols");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

    }
}