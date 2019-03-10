using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.Selenium.Test.Admin.Marketing.Tests.Certificates
{
    [TestFixture]
    public class CertificatesFilterTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Orders | ClearType.Catalog);
            InitializeService.LoadData(
            "data\\Admin\\Certificates\\Catalog.Product.csv",
            "data\\Admin\\Certificates\\Catalog.Photo.csv",
            "data\\Admin\\Certificates\\Catalog.Offer.csv",
            "data\\Admin\\Certificates\\Catalog.Category.csv",
            "data\\Admin\\Certificates\\Catalog.ProductCategories.csv",
            "data\\Admin\\Certificates\\Catalog.Brand.csv",
            "data\\Admin\\Certificates\\Catalog.Color.csv",
            "data\\Admin\\Certificates\\Catalog.Size.csv",
             "data\\Admin\\Certificates\\[Order].Certificate.csv",
            "data\\Admin\\Certificates\\[Order].OrderContact.csv",
            "data\\Admin\\Certificates\\[Order].OrderCurrency.csv",
             "data\\Admin\\Certificates\\[Order].OrderItems.csv",
             "data\\Admin\\Certificates\\[Order].OrderStatus.csv",
             "data\\Admin\\Certificates\\[Order].[Order].csv",
             "Data\\Admin\\Certificates\\[Order].OrderSource.csv"
           );
            Init();
            GoToAdmin("certificates");
        }

        [Test]
        public void CertificatesFilterCode()
        {
            testname = "CertificatesFilterCode";
            VerifyBegin(testname);
            //Код
            Functions.GridFilterSet(driver, baseURL, "CertificateCode");
            DropFocus("h1");

            //search by not exist 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("50000");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("111111111122222222222222222222222222222");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "too much symbols");

            //search by exist
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("Certificate2");
            DropFocus("h1");

            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 2, "filter CertificateCode row");
            VerifyAreEqual("Найдено записей: 2", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter CertificateCode count");
            VerifyAreEqual("Certificate2", GetGridCell(0, "CertificateCode").Text, "filter CertificateCode value");

            //check go to edit and back 
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-modal-trigger")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("Редактировать подарочный сертификат", driver.FindElement(By.TagName("h2")).Text, "pop up h2");
            XPathContainsText("button", "Отмена");

            VerifyAreEqual("Найдено записей: 2", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter CertificateCode return");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"CertificateCode\"]")).Displayed);


            Functions.GridFilterClose(driver, baseURL, "CertificateCode");
            VerifyAreEqual("Найдено записей: 20", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter CertificateCode exit");
            VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "filter CertificateCode exit");
            VerifyAreEqual("Certificate6", GetGridCell(5, "CertificateCode").Text, "filter CertificateCode exit");

            VerifyFinally(testname);
        }
        [Test]
        public void CertificatesFilterOrder()
        {
            testname = "CertificatesFilterOrder";
            VerifyBegin(testname);
            //OrderId 
            Functions.GridFilterSet(driver, baseURL, "OrderId");
            DropFocus("h1");
            //search by not exist 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("50000");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("111111111122222222222222222222222222222");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "too much symbols");

            //search by exist
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("6");
            DropFocus("h1");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter OrderId count");

            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 1, "filter OrderId row");
            VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "filter OrderId value");
            Functions.GridFilterClose(driver, baseURL, "OrderId");
           VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "filter OrderId exit 1");
            VerifyAreEqual("Certificate6", GetGridCell(5, "CertificateCode").Text, "filter OrderId exit 5");

            VerifyFinally(testname);
        }
        [Test]
        public void CertificatesFilterApplyOrder()
        {
            testname = "CertificatesFilterApplyOrder";
            VerifyBegin(testname);
            //ApplyOrderNumber
            Functions.GridFilterSet(driver, baseURL, "ApplyOrderNumber");
            DropFocus("h1");
            //search by not exist 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("50000");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("111111111122222222222222222222222222222");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "too much symbols");

            //search by exist
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("7");
            DropFocus("h1");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter ApplyOrderNumber count");

            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 1, "filter ApplyOrderNumber row");
            VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "filter ApplyOrderNumber value");
            Functions.GridFilterClose(driver, baseURL, "ApplyOrderNumber");
           VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "filter ApplyOrderNumber exit 1");
            VerifyAreEqual("Certificate6", GetGridCell(5, "CertificateCode").Text, "filter ApplyOrderNumber exit 5");

            VerifyFinally(testname);
        }
        [Test]
        public void CertificatesFilterSum()
        {
            testname = "CertificatesFilterSum";
            VerifyBegin(testname);
            //FullSum
            Functions.GridFilterSet(driver, baseURL, "FullSum");
            DropFocus("h1");
            //search by not exist 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("50000");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("111111111122222222222222222222222222222");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "too much symbols");

            //search by exist
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("200");
            DropFocus("h1");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter FullSum count");

            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 1, "filter FullSum row");
            VerifyAreEqual("Certificate3", GetGridCell(0, "CertificateCode").Text, "filter FullSum value");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("3000");
            DropFocus("h1");
           VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter FullSum no find");

            Functions.GridFilterClose(driver, baseURL, "FullSum");
           VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "filter FullSum exit 1");
            VerifyAreEqual("Certificate6", GetGridCell(5, "CertificateCode").Text, "filter FullSum exit 5");

            VerifyFinally(testname);
        }
        [Test]
        public void CertificatesFilterPaid()
        {
            testname = "CertificatesFilterPaid";
            VerifyBegin(testname);
            //Paid
            Functions.GridFilterSelectDropFocus(driver, baseURL, filterName: "Paid", filterItem: "Да", tag: "h1");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter Paid count");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 1, "filter Paid row");
            VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "filter Paid value");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Нет");
            DropFocus("h1");
            VerifyAreEqual("Найдено записей: 19", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter Paid 2 count");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 10, "filter Paid 2 row");
            VerifyAreEqual("Certificate2", GetGridCell(0, "CertificateCode").Text, "filter Paid 2 value");
            Functions.GridFilterClose(driver, baseURL, "Paid");
           VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "filter Paid exit 1");
            VerifyAreEqual("Certificate6", GetGridCell(5, "CertificateCode").Text, "filter Paid exit 5");

            VerifyFinally(testname);
        }
        [Test]
        public void CertificatesFilterEnable()
        {
            testname = "CertificatesFilterEnable";
            VerifyBegin(testname);
            //Enable
            Functions.GridFilterSelectDropFocus(driver, baseURL, filterName: "Enable", filterItem: "Да", tag: "h1");
            VerifyAreEqual("Найдено записей: 10", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter Enable count");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 10, "filter Enable row");
            VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "filter Enable value");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Нет");
            DropFocus("h1");
            VerifyAreEqual("Найдено записей: 10", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter Enable count");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 10, "filter Enable 2 row");
            VerifyAreEqual("Certificate11", GetGridCell(0, "CertificateCode").Text, "filter Enable 2 value");
            Functions.GridFilterClose(driver, baseURL, "Enable");
           VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "filter Enable exit 1 ");
            VerifyAreEqual("Certificate6", GetGridCell(5, "CertificateCode").Text, "filter Enable exit 5");

            VerifyFinally(testname);
        }
        [Test]
        public void CertificatesFilterCreate()
        {
            testname = "CertificatesFilterCreate";
            VerifyBegin(testname);
            //CreationDates
            Functions.GridFilterSet(driver, baseURL, "CreationDates");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("20.03.2013 00:00");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("01.01.2013 00:00");
            DropFocus("h1");
            VerifyAreEqual("Найдено записей: 3", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter CreationDates count");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 3, "filter CreationDates row");
            VerifyAreEqual("Certificate18", GetGridCell(0, "CertificateCode").Text, "filter CreationDates value");

            Functions.GridFilterClose(driver, baseURL, "CreationDates");
           VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "filter CreationDates exit1 ");
            VerifyAreEqual("Certificate6", GetGridCell(5, "CertificateCode").Text, "filter CreationDates exit1");

            VerifyFinally(testname);
        }
        [Test]
        public void CertificatesFilterUsed()
        {
            testname = "CertificatesFilterUsed";
            VerifyBegin(testname);
            //Used
            Functions.GridFilterSelectDropFocus(driver, baseURL, filterName: "Used", filterItem: "Нет", tag: "h1");
            VerifyAreEqual("Найдено записей: 19", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter Used count");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 10, "filter Used row");
            VerifyAreEqual("Certificate2", GetGridCell(0, "CertificateCode").Text, "filter Used value");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Да");
            DropFocus("h1");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter Used  2 count");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 1, "filter Used 2 row");
            VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "filter Used 2 value");
            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, "Used");
            VerifyAreEqual("Найдено записей: 19", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "no filter count");
            VerifyAreEqual("Certificate2", GetGridCell(0, "CertificateCode").Text, "filter Used exit1");
            VerifyAreEqual("Certificate7", GetGridCell(5, "CertificateCode").Text, "filter Used exit2");

            VerifyFinally(testname);
        }

    }
}
