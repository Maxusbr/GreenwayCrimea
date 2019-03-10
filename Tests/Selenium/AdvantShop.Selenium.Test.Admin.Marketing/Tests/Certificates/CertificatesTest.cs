using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.SeleniumTest.Admin.Marketing.Certificates
{
    [TestFixture]
    public class CertificatesTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Orders|ClearType.Catalog);
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
        public void CertificatesSearchNotexist()
        {
            testname = "SearchNotExist";
            VerifyBegin(testname);
            //search not exist product
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Certificate222");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search not exist discount price range");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);

        }

        [Test]
        public void CertificatesSearchMuch()
        {
            testname = "SearchMuch";
            VerifyBegin(testname);
            //search too much symbols
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("1111111111222222222223333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search not exist discount price range");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void CertificatesSearchInvalid()
        {
            testname = "SearchInvalid";
            VerifyBegin(testname);
            //search invalid symbols
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search not exist discount price range");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void SearchExist()
        {
            testname = "SearchExist";
            VerifyBegin(testname);
            
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Certificate2");
            VerifyAreEqual("Certificate2", GetGridCell(0, "CertificateCode").Text, "find value");
            VerifyAreEqual("Найдено записей: 2", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");
            VerifyFinally(testname);
        }

        [Test]
        public void xCertificatesRedirect()
        {
            testname = "CertificatesRedirect";
            VerifyBegin(testname);
            
            GoToAdmin("certificates");
            VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "1 value");
            GetGridCell(0, "OrderId").Click();
            Thread.Sleep(3000);
            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Заказ № 6"), " go to orderId");

            GoBack();
            GetGridCell(0, "ApplyOrderNumber").Click();
            Thread.Sleep(3000);
            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Заказ № 7"), " go to ApplyOrder");

            GoBack();
            GetGridCell(0, "CertificateCode").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);
            VerifyIsTrue(driver.FindElement(By.TagName("h2")).Text.Contains("Редактировать подарочный сертификат"), " modal h2 ");
            VerifyAreEqual("Certificate1", driver.FindElement(By.CssSelector("[data-e2e=\"CertCode\"]")).GetAttribute("value"), " modal cod");
            VerifyAreEqual("FromMe1", driver.FindElement(By.CssSelector("[data-e2e=\"CertFrom\"]")).GetAttribute("value"), "modal from ");
            VerifyAreEqual("ToMe1", driver.FindElement(By.CssSelector("[data-e2e=\"CertTo\"]")).GetAttribute("value"), " modal to");
            VerifyAreEqual("100", driver.FindElement(By.CssSelector("[data-e2e=\"CertSum\"]")).GetAttribute("value"), " modal sum");
            VerifyAreEqual("me@gmail.com", driver.FindElement(By.CssSelector("[data-e2e=\"CertMailTo\"]")).GetAttribute("value"), " modal mail.");

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"CertUsed\"]")).FindElement(By.CssSelector("input")).Selected, " modal used");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"CertEnabled\"]")).FindElement(By.CssSelector("input")).Selected, "modal enabled ");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"CertPaid\"]")).FindElement(By.CssSelector("input")).Selected, " modal paid");
            AssertCkText("gift1", "editor1");
            VerifyFinally(testname);
        }
    }
}
