using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using AdvantShop.Selenium.Core.Infrastructure;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.Customers.CustomerSegments
{
    [TestFixture]
    public class CustomerSegmentsAddEditTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers | ClearType.Orders | ClearType.Catalog);
            InitializeService.LoadData(
               "data\\Admin\\CustomerSegments\\Customers.CustomerGroup.csv",
                  "data\\Admin\\CustomerSegments\\Customers.Country.csv",
            "data\\Admin\\CustomerSegments\\Customers.Region.csv",
            "data\\Admin\\CustomerSegments\\Customers.City.csv",
            "data\\Admin\\CustomerSegments\\Customers.Customer.csv",
            "data\\Admin\\CustomerSegments\\Customers.Contact.csv",
                       "data\\Admin\\CustomerSegments\\Customers.Departments.csv",
           "data\\Admin\\CustomerSegments\\Customers.Managers.csv",
               "data\\Admin\\CustomerSegments\\Customers.CustomerField.csv",
               "data\\Admin\\CustomerSegments\\Customers.CustomerFieldValue.csv",
               "data\\Admin\\CustomerSegments\\Customers.CustomerFieldValuesMap.csv",
             "data\\Admin\\CustomerSegments\\Catalog.Product.csv",
           "data\\Admin\\CustomerSegments\\Catalog.Offer.csv",
           "data\\Admin\\CustomerSegments\\Catalog.Category.csv",
           "data\\Admin\\CustomerSegments\\Catalog.ProductCategories.csv",
            "data\\Admin\\CustomerSegments\\[Order].OrderContact.csv",
              "Data\\Admin\\CustomerSegments\\[Order].OrderSource.csv",
            "data\\Admin\\CustomerSegments\\[Order].OrderCurrency.csv",
             "data\\Admin\\CustomerSegments\\[Order].OrderItems.csv",
             "data\\Admin\\CustomerSegments\\[Order].OrderStatus.csv",
                 "data\\Admin\\CustomerSegments\\[Order].PaymentMethod.csv",
            "data\\Admin\\CustomerSegments\\[Order].ShippingMethod.csv",
               "data\\Admin\\CustomerSegments\\[Order].[Order].csv",
               "data\\Admin\\CustomerSegments\\[Order].OrderCustomer.csv"
           //    "data\\Admin\\CustomerSegments\\Customers.CustomerSegment.csv",
           //  "data\\Admin\\CustomerSegments\\Customers.CustomerSegment_Customer.csv"

           );

            Init();
        }

        [Test]
        public void CustomersSegmentOrderSumAdd()
        {
            testname = "CustomersSegmentOrderSumAdd";
            VerifyBegin(testname);

            GoToAdmin("customersegments/add");

            driver.FindElement(By.Id("Name")).Click();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("New Segment Orders Sum");

            driver.FindElement(By.Id("SegmentFilter_OrdersSumFrom")).Click();
            driver.FindElement(By.Id("SegmentFilter_OrdersSumFrom")).Clear();
            driver.FindElement(By.Id("SegmentFilter_OrdersSumFrom")).SendKeys("150");

            driver.FindElement(By.Id("SegmentFilter_OrdersSumTo")).Click();
            driver.FindElement(By.Id("SegmentFilter_OrdersSumTo")).Clear();
            driver.FindElement(By.Id("SegmentFilter_OrdersSumTo")).SendKeys("200");

            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);

            string url = driver.Url;

            //check segments grid
            GoToAdmin("customersegments");
            VerifyAreEqual("New Segment Orders Sum", GetGridCell(0, "Name").Text, "segment grid name");
            VerifyAreEqual("7", GetGridCell(0, "CustomersCount").Text, "segment grid customers count");

            //check segment's edit
            driver.Navigate().GoToUrl(url);

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Сегмент \"New Segment Orders Sum\""), "segment added h1");

            VerifyAreEqual("150", driver.FindElement(By.Id("SegmentFilter_OrdersSumFrom")).GetAttribute("value"), "segment filter orders sum from");
            VerifyAreEqual("200", driver.FindElement(By.Id("SegmentFilter_OrdersSumTo")).GetAttribute("value"), "segment filter orders sum to");

            VerifyAreEqual("Найдено записей: 7", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "segment filter orders sum customers count");
            VerifyAreEqual("FirstName37 LastName37", GetGridCell(0, "Name").Text, "segment customer name");

            //check customer's edit
            GoToAdmin("customers/edit/2c8fb106-8f07-499b-b06f-51b43076f337");

            VerifyIsTrue(driver.PageSource.Contains("New Segment Orders Sum"), "segment filter in customer edit");

            VerifyFinally(testname);
        }

        [Test]
        public void CustomersSegmentOrderPaidSumAdd()
        {
            testname = "CustomersSegmentOrderPaidSumAdd";
            VerifyBegin(testname);

            GoToAdmin("customersegments/add");

            driver.FindElement(By.Id("Name")).Click();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("New Segment Orders Paid Sum");

            driver.FindElement(By.Id("SegmentFilter_OrdersPaidSumFrom")).Click();
            driver.FindElement(By.Id("SegmentFilter_OrdersPaidSumFrom")).Clear();
            driver.FindElement(By.Id("SegmentFilter_OrdersPaidSumFrom")).SendKeys("70");

            driver.FindElement(By.Id("SegmentFilter_OrdersPaidSumTo")).Click();
            driver.FindElement(By.Id("SegmentFilter_OrdersPaidSumTo")).Clear();
            driver.FindElement(By.Id("SegmentFilter_OrdersPaidSumTo")).SendKeys("200");

            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);

            string url = driver.Url;

            //check segments grid
            GoToAdmin("customersegments");
            VerifyAreEqual("New Segment Orders Paid Sum", GetGridCell(0, "Name").Text, "segment grid name");
            VerifyAreEqual("15", GetGridCell(0, "CustomersCount").Text, "segment grid customers count");

            //check segment's edit
            driver.Navigate().GoToUrl(url);

            VerifyAreEqual("70", driver.FindElement(By.Id("SegmentFilter_OrdersPaidSumFrom")).GetAttribute("value"), "segment filter orders paid sum from");
            VerifyAreEqual("200", driver.FindElement(By.Id("SegmentFilter_OrdersPaidSumTo")).GetAttribute("value"), "segment filter orders paid sum to");

            VerifyAreEqual("Найдено записей: 15", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "segment filter orders paid sum customers count");
            VerifyAreEqual("FirstName50 LastName50", GetGridCell(0, "Name").Text, "segment customer name");

            //check customer's edit
            GoToAdmin("customers/edit/2c8fb106-8f07-499b-b06f-51b43076f344");
            
            VerifyIsTrue(driver.PageSource.Contains("New Segment Orders Paid Sum"), "segment filter in customer edit");

            VerifyFinally(testname);
        }

        [Test]
        public void CustomersSegmentOrdersCountAdd()
        {
            testname = "CustomersSegmentOrdersCountAdd";
            VerifyBegin(testname);

            GoToAdmin("customersegments/add");

            driver.FindElement(By.Id("Name")).Click();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("New Segment Orders Count");

            driver.FindElement(By.Id("SegmentFilter_OrdersCountFrom")).Click();
            driver.FindElement(By.Id("SegmentFilter_OrdersCountFrom")).Clear();
            driver.FindElement(By.Id("SegmentFilter_OrdersCountFrom")).SendKeys("1");

            driver.FindElement(By.Id("SegmentFilter_OrdersCountTo")).Click();
            driver.FindElement(By.Id("SegmentFilter_OrdersCountTo")).Clear();
            driver.FindElement(By.Id("SegmentFilter_OrdersCountTo")).SendKeys("2");

            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);

            string url = driver.Url;

            //check segments grid
            GoToAdmin("customersegments");
            VerifyAreEqual("New Segment Orders Count", GetGridCell(0, "Name").Text, "segment grid name");
            VerifyAreEqual("37", GetGridCell(0, "CustomersCount").Text, "segment grid customers count");

            //check segment's edit
            driver.Navigate().GoToUrl(url);

            VerifyAreEqual("1", driver.FindElement(By.Id("SegmentFilter_OrdersCountFrom")).GetAttribute("value"), "segment filter orders count from");
            VerifyAreEqual("2", driver.FindElement(By.Id("SegmentFilter_OrdersCountTo")).GetAttribute("value"), "segment filter orders count to");

            VerifyAreEqual("Найдено записей: 37", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "segment filter orders count customers count");
            VerifyAreEqual("FirstName50 LastName50", GetGridCell(0, "Name").Text, "segment customer name");

            //check customer's edit
            GoToAdmin("customers/edit/2c8fb106-8f07-499b-b06f-51b43076f340");

            VerifyIsTrue(driver.PageSource.Contains("New Segment Orders Count"), "segment filter in customer edit");

            VerifyFinally(testname);
        }

        [Test]
        public void CustomersSegmentCityAdd()
        {
            testname = "CustomersSegmentCityAdd";
            VerifyBegin(testname);

            GoToAdmin("customersegments/add");

            driver.FindElement(By.Id("Name")).Click();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("New Segment City");
            
            driver.FindElement(By.CssSelector("[data-e2e=\"customerSegmentCities\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerSegmentCities\"]")).SendKeys("Москва");
            WaitForElem(By.CssSelector("span.ui-select-choices-row-inner"));
            driver.FindElement(By.CssSelector("span.ui-select-choices-row-inner")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);

            string url = driver.Url;

            //check segments grid
            GoToAdmin("customersegments");
            VerifyAreEqual("New Segment City", GetGridCell(0, "Name").Text, "segment grid name");
            VerifyAreEqual("28", GetGridCell(0, "CustomersCount").Text, "segment grid customers count");

            //check segment's edit
            driver.Navigate().GoToUrl(url);

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"customerSegmentCities\"]")).Text.Contains("Москва"), "segment filter city");

            VerifyAreEqual("Найдено записей: 28", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "segment filter city");
            VerifyAreEqual("FirstName47 LastName47", GetGridCell(0, "Name").Text, "segment customer name");

            //check customer's edit
            GoToAdmin("customers/edit/2c8fb106-8f07-499b-b06f-51b43076f344");

            VerifyIsTrue(driver.PageSource.Contains("New Segment City"), "segment filter in customer edit");

            VerifyFinally(testname);
        }


        [Test]
        public void CustomersSegmentCountryAdd()
        {
            testname = "CustomersSegmentCountryAdd";
            VerifyBegin(testname);

            GoToAdmin("customersegments/add");

            driver.FindElement(By.Id("Name")).Click();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("New Segment Country");

            driver.FindElement(By.CssSelector("[data-e2e=\"customerSegmentCountries\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerSegmentCountries\"]")).SendKeys("Украина");
            WaitForElem(By.CssSelector("span.ui-select-choices-row-inner"));
            driver.FindElement(By.CssSelector("span.ui-select-choices-row-inner")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);

            string url = driver.Url;

            //check segments grid
            GoToAdmin("customersegments");
            VerifyAreEqual("New Segment Country", GetGridCell(0, "Name").Text, "segment grid name");
            VerifyAreEqual("6", GetGridCell(0, "CustomersCount").Text, "segment grid customers count");

            //check segment's edit
            driver.Navigate().GoToUrl(url);

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"customerSegmentCountries\"]")).Text.Contains("Украина"), "segment filter country");

            VerifyAreEqual("Найдено записей: 6", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "segment filter country");
            VerifyAreEqual("FirstName41 LastName41", GetGridCell(0, "Name").Text, "segment customer name");

            //check customer's edit
            GoToAdmin("customers/edit/2c8fb106-8f07-499b-b06f-51b43076f338");

            VerifyIsTrue(driver.PageSource.Contains("New Segment Country"), "segment filter in customer edit");

            VerifyFinally(testname);
        }

        [Test]
        public void CustomersSegmentLastOrderDateAdd()
        {
            testname = "CustomersSegmentLastOrderDateAdd";
            VerifyBegin(testname);

            GoToAdmin("customersegments/add");

            driver.FindElement(By.Id("Name")).Click();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("New Segment Last Order Date");

            driver.FindElement(By.Id("SegmentFilter_LastOrderDateFrom")).Click();
            driver.FindElement(By.Id("SegmentFilter_LastOrderDateFrom")).Clear();
            driver.FindElement(By.Id("SegmentFilter_LastOrderDateFrom")).SendKeys("01.09.2016");

            driver.FindElement(By.Id("SegmentFilter_LastOrderDateTo")).Click();
            driver.FindElement(By.Id("SegmentFilter_LastOrderDateTo")).Clear();
            driver.FindElement(By.Id("SegmentFilter_LastOrderDateTo")).SendKeys("10.09.2016");

            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);

            string url = driver.Url;

            //check segments grid
            GoToAdmin("customersegments");
            VerifyAreEqual("New Segment Last Order Date", GetGridCell(0, "Name").Text, "segment grid name");
            VerifyAreEqual("13", GetGridCell(0, "CustomersCount").Text, "segment grid customers count");

            //check segment's edit
            driver.Navigate().GoToUrl(url);

            VerifyAreEqual("01.09.2016", driver.FindElement(By.Id("SegmentFilter_LastOrderDateFrom")).GetAttribute("value"), "segment filter last order date from");
            VerifyAreEqual("10.09.2016", driver.FindElement(By.Id("SegmentFilter_LastOrderDateTo")).GetAttribute("value"), "segment filter last order date to");

            VerifyAreEqual("Найдено записей: 13", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "segment filter last order date customers count");
            VerifyAreEqual("FirstName50 LastName50", GetGridCell(0, "Name").Text, "segment customer name");

            //check customer's edit
            GoToAdmin("customers/edit/2c8fb106-8f07-499b-b06f-51b43076f340");

            VerifyIsTrue(driver.PageSource.Contains("New Segment Last Order Date"), "segment filter in customer edit");

            VerifyFinally(testname);
        }

        [Test]
        public void CustomersSegmentSegmentAverageCheckAdd()
        {
            testname = "CustomersSegmentAverageCheckAdd";
            VerifyBegin(testname);

            GoToAdmin("customersegments/add");

            driver.FindElement(By.Id("Name")).Click();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("New Segment Average Check");

            driver.FindElement(By.Id("SegmentFilter_AverageCheckFrom")).Click();
            driver.FindElement(By.Id("SegmentFilter_AverageCheckFrom")).Clear();
            driver.FindElement(By.Id("SegmentFilter_AverageCheckFrom")).SendKeys("50");

            driver.FindElement(By.Id("SegmentFilter_AverageCheckTo")).Click();
            driver.FindElement(By.Id("SegmentFilter_AverageCheckTo")).Clear();
            driver.FindElement(By.Id("SegmentFilter_AverageCheckTo")).SendKeys("60");

            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);

            string url = driver.Url;

            //check segments grid
            GoToAdmin("customersegments");
            VerifyAreEqual("New Segment Average Check", GetGridCell(0, "Name").Text, "segment grid name");
            VerifyAreEqual("13", GetGridCell(0, "CustomersCount").Text, "segment grid customers count");

            //check segment's edit
            driver.Navigate().GoToUrl(url);

            VerifyAreEqual("50", driver.FindElement(By.Id("SegmentFilter_AverageCheckFrom")).GetAttribute("value"), "segment filter average check from");
            VerifyAreEqual("60", driver.FindElement(By.Id("SegmentFilter_AverageCheckTo")).GetAttribute("value"), "segment filter average check to");

            VerifyAreEqual("Найдено записей: 13", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "segment filter average check customers count");
            VerifyAreEqual("FirstName34 LastName34", GetGridCell(0, "Name").Text, "segment customer name");

            //check customer's edit
            GoToAdmin("customers/edit/2c8fb106-8f07-499b-b06f-51b43076f315");

            VerifyIsTrue(driver.PageSource.Contains("New Segment Average Check"), "segment filter in customer edit");

            VerifyFinally(testname);
        }

        [Test]
        public void CustomersSegmentCategoriesAdd()
        {
            testname = "CustomersSegmentCategoriesAdd";
            VerifyBegin(testname);

            GoToAdmin("customersegments/add");

            driver.FindElement(By.Id("Name")).Click();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("New Segment Categories");

            driver.FindElement(By.CssSelector("[data-e2e=\"customerSegmentCategories\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerSegmentCategories\"]")).SendKeys("TestCategory5");
            WaitForElem(By.CssSelector("span.ui-select-choices-row-inner"));
            driver.FindElement(By.CssSelector("span.ui-select-choices-row-inner")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);

            string url = driver.Url;

            //check segments grid
            GoToAdmin("customersegments");
            VerifyAreEqual("New Segment Categories", GetGridCell(0, "Name").Text, "segment grid name");
            VerifyAreEqual("20", GetGridCell(0, "CustomersCount").Text, "segment grid customers count");

            //check segment's edit
            driver.Navigate().GoToUrl(url);

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"customerSegmentCategories\"]")).Text.Contains("TestCategory5"), "segment filter categories");

            VerifyAreEqual("Найдено записей: 20", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "segment filter categories");
            VerifyAreEqual("FirstName37 LastName37", GetGridCell(0, "Name").Text, "segment customer name");

            //check customer's edit
            GoToAdmin("customers/edit/2c8fb106-8f07-499b-b06f-51b43076f318");

            VerifyIsTrue(driver.PageSource.Contains("New Segment Categories"), "segment filter in customer edit");

            VerifyFinally(testname);
        }
    }
}