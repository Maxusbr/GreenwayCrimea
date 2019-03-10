using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.Customers
{
    [TestFixture]
    public class CustomersSortTest : BaseSeleniumTest
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
        public void SortByCustomer()
        {
            testname = "SortByCustomer";
            VerifyBegin(testname);

            GetGridCell(-1, "Name").Click();
            WaitForAjax();
            VerifyAreEqual("FirstName1 LastName1", GetGridCell(0, "Name").FindElement(By.TagName("a")).Text, "sort by customer asc line 1");
            VerifyAreEqual("FirstName107 LastName107", GetGridCell(9, "Name").FindElement(By.TagName("a")).Text, "sort by customer asc line 10");

            GetGridCell(-1, "Name").Click();
            WaitForAjax();
            VerifyAreEqual("FirstName99 LastName99", GetGridCell(0, "Name").FindElement(By.TagName("a")).Text, "sort by customer desc line 1");
            VerifyAreEqual("FirstName90 LastName90", GetGridCell(9, "Name").FindElement(By.TagName("a")).Text, "sort by customer desc line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void SortByPhone()
        {
            testname = "SortByPhone";
            VerifyBegin(testname);

            GetGridCell(-1, "Phone").Click();
            WaitForAjax();
            VerifyAreEqual("1", GetGridCell(0, "Phone").Text, "sort by Phone asc line 1");
            VerifyAreEqual("107", GetGridCell(9, "Phone").Text, "sort by Phone asc line 10");

            GetGridCell(-1, "Phone").Click();
            WaitForAjax();
            VerifyAreEqual("99", GetGridCell(0, "Phone").Text, "sort by Phone desc line 1");
            VerifyAreEqual("90", GetGridCell(9, "Phone").Text, "sort by Phone desc line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void SortByMail()
        {
            testname = "SortByMail";
            VerifyBegin(testname);

            GetGridCell(-1, "Email").Click();
            WaitForAjax();
            VerifyAreEqual("test@mail.ru1", GetGridCell(0, "Email").Text, "sort by Email asc line 1");
            VerifyAreEqual("test@mail.ru107", GetGridCell(9, "Email").Text, "sort by Email asc line 10");

            GetGridCell(-1, "Email").Click();
            WaitForAjax();
            VerifyAreEqual("test@mail.ru99", GetGridCell(0, "Email").Text, "sort by Email desc line 1");
            VerifyAreEqual("test@mail.ru90", GetGridCell(9, "Email").Text, "sort by Email desc line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void SortByOrdersCount()
        {
            testname = "SortByOrdersCount";
            VerifyBegin(testname);

            GetGridCell(-1, "OrdersCount").Click();
            WaitForAjax();
            VerifyAreEqual("0", GetGridCell(0, "OrdersCount").Text, "sort by OrdersCount asc line 1");
            VerifyAreEqual("0", GetGridCell(9, "OrdersCount").Text, "sort by OrdersCount asc line 10");
            VerifyIsFalse(GetGridCell(0, "Name").FindElement(By.TagName("a")).Text.Equals(GetGridCell(9, "Name").FindElement(By.TagName("a")).Text), "diff customers asc");

            GetGridCell(-1, "OrdersCount").Click();
            WaitForAjax();
            VerifyAreEqual("12", GetGridCell(0, "OrdersCount").Text, "sort by OrdersCount desc line 1");
            VerifyAreEqual("FirstName5 LastName5", GetGridCell(0, "Name").FindElement(By.TagName("a")).Text, "sort by OrdersCount customer name");
            VerifyAreEqual("1", GetGridCell(1, "OrdersCount").Text, "sort by OrdersCount desc line 2");
            VerifyAreEqual("1", GetGridCell(2, "OrdersCount").Text, "sort by OrdersCount desc line 3");
            VerifyAreEqual("1", GetGridCell(3, "OrdersCount").Text, "sort by OrdersCount desc line 4");
            VerifyAreEqual("1", GetGridCell(4, "OrdersCount").Text, "sort by OrdersCount desc line 5");
            VerifyAreEqual("0", GetGridCell(9, "OrdersCount").Text, "sort by OrdersCount desc line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void SortByLastOrderNumber()
        {
            testname = "SortByLastOrderNumber";
            VerifyBegin(testname);

            GetGridCell(-1, "LastOrderNumber").Click();
            WaitForAjax();
            VerifyAreEqual("", GetGridCell(0, "LastOrderNumber").Text, "sort by LastOrderNumber asc line 1");
            VerifyAreEqual("", GetGridCell(9, "LastOrderNumber").Text, "sort by LastOrderNumber asc line 10");
            VerifyIsFalse(GetGridCell(0, "Name").FindElement(By.TagName("a")).Text.Equals(GetGridCell(9, "Name").FindElement(By.TagName("a")).Text), "diff customers asc");

            GetGridCell(-1, "LastOrderNumber").Click();
            WaitForAjax();
            VerifyAreEqual("# 30", GetGridCell(0, "LastOrderNumber").FindElement(By.TagName("a")).Text, "sort by LastOrderNumber desc line 1");
            VerifyAreEqual("FirstName5 LastName5", GetGridCell(0, "Name").FindElement(By.TagName("a")).Text, "sort by LastOrderNumber customer name");
            VerifyAreEqual("# 19", GetGridCell(1, "LastOrderNumber").FindElement(By.TagName("a")).Text, "sort by LastOrderNumber desc line 2");
            VerifyAreEqual("# 18", GetGridCell(2, "LastOrderNumber").FindElement(By.TagName("a")).Text, "sort by LastOrderNumber desc line 3");
            VerifyAreEqual("# 17", GetGridCell(3, "LastOrderNumber").FindElement(By.TagName("a")).Text, "sort by LastOrderNumber desc line 4");
            VerifyAreEqual("# 16", GetGridCell(4, "LastOrderNumber").FindElement(By.TagName("a")).Text, "sort by LastOrderNumber desc line 5");
            VerifyAreEqual("", GetGridCell(9, "LastOrderNumber").Text, "sort by LastOrderNumber desc line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void SortByOrdersSum()
        {
            testname = "SortByOrdersSum";
            VerifyBegin(testname);

            GetGridCell(-1, "OrdersSum").Click();
            WaitForAjax();
            VerifyAreEqual("0", GetGridCell(0, "OrdersSum").Text, "sort by OrdersCount asc line 1");
            VerifyAreEqual("0", GetGridCell(9, "OrdersSum").Text, "sort by OrdersCount asc line 10");
            VerifyIsFalse(GetGridCell(0, "Name").FindElement(By.TagName("a")).Text.Equals(GetGridCell(9, "Name").FindElement(By.TagName("a")).Text), "diff customers asc");

            GetGridCell(-1, "OrdersSum").Click();
            WaitForAjax();
            VerifyAreEqual("290", GetGridCell(0, "OrdersSum").Text, "sort by OrdersCount desc line 1");
            VerifyAreEqual("FirstName5 LastName5", GetGridCell(0, "Name").FindElement(By.TagName("a")).Text, "sort by OrdersCount customer name");
            VerifyAreEqual("19", GetGridCell(1, "OrdersSum").Text, "sort by OrdersCount desc line 2");
            VerifyAreEqual("18", GetGridCell(2, "OrdersSum").Text, "sort by OrdersCount desc line 3");
            VerifyAreEqual("17", GetGridCell(3, "OrdersSum").Text, "sort by OrdersCount desc line 4");
            VerifyAreEqual("16", GetGridCell(4, "OrdersSum").Text, "sort by OrdersCount desc line 5");
            VerifyAreEqual("0", GetGridCell(9, "OrdersSum").Text, "sort by OrdersCount desc line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void SortByRegDate()
        {
            testname = "SortByRegDate";
            VerifyBegin(testname);

            GetGridCell(-1, "RegistrationDateTimeFormatted").Click();
            WaitForAjax();
            VerifyAreEqual("19.04.2017 15:37", GetGridCell(0, "RegistrationDateTimeFormatted").Text, "sort by RegDate asc line 1");
            VerifyAreEqual("28.04.2017 15:37", GetGridCell(9, "RegistrationDateTimeFormatted").Text, "sort by RegDate asc line 10");

            GetGridCell(-1, "RegistrationDateTimeFormatted").Click();
            WaitForAjax();
            VerifyAreEqual("15.08.2017 15:37", GetGridCell(0, "RegistrationDateTimeFormatted").Text, "sort by RegDate desc line 1");
            VerifyAreEqual("06.08.2017 15:37", GetGridCell(9, "RegistrationDateTimeFormatted").Text, "sort by RegDate desc line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void SortByManager()
        {
            testname = "SortByManager";
            VerifyBegin(testname);

            GetGridCell(-1, "ManagerName").Click();
            WaitForAjax();
            VerifyAreEqual("", GetGridCell(0, "ManagerName").Text, "sort by ManagerName asc line 1");
            VerifyAreEqual("", GetGridCell(9, "ManagerName").Text, "sort by ManagerName asc line 10");
            VerifyIsFalse(GetGridCell(0, "Name").FindElement(By.TagName("a")).Text.Equals(GetGridCell(9, "Name").FindElement(By.TagName("a")).Text), "diff customers asc");

            GetGridCell(-1, "ManagerName").Click();
            WaitForAjax();
            VerifyAreEqual("ManagerName2 ManagerLastName2", GetGridCell(0, "ManagerName").Text, "sort by ManagerName desc line 1");
            VerifyAreEqual("ManagerName2 ManagerLastName2", GetGridCell(9, "ManagerName").Text, "sort by ManagerName desc line 10");
            VerifyIsFalse(GetGridCell(0, "Name").FindElement(By.TagName("a")).Text.Equals(GetGridCell(9, "Name").FindElement(By.TagName("a")).Text), "diff customers desc");

            VerifyFinally(testname);
        }
    }
}