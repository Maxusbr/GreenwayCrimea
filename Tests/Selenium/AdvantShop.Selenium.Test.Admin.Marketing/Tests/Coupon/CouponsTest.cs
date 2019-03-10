using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Marketing.Coupons
{
    [TestFixture]
    public class CouponsTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
            "data\\Admin\\Coupons\\Catalog.Product.csv",
            "data\\Admin\\Coupons\\Catalog.Photo.csv",
            "data\\Admin\\Coupons\\Catalog.Offer.csv",
            "data\\Admin\\Coupons\\Catalog.Category.csv",
            "data\\Admin\\Coupons\\Catalog.ProductCategories.csv",
            "data\\Admin\\Coupons\\Catalog.Brand.csv",
            "data\\Admin\\Coupons\\Catalog.Color.csv",
            "data\\Admin\\Coupons\\Catalog.Size.csv",
             "data\\Admin\\Coupons\\Catalog.Coupon.csv",
            "data\\Admin\\Coupons\\Catalog.CouponCategories.csv",
            "data\\Admin\\Coupons\\Catalog.CouponProducts.csv"
           );

            Init();
            GoToAdmin("coupons");
        }


        [Test]
        public void CouponSearch()
        {
            testname = "CouponSearch";
            VerifyBegin(testname);
            GetGridFilter().SendKeys("test2");
            VerifyAreEqual("test2", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), " find value");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");
            VerifyFinally(testname);
        }
        [Test]
        public void CouponSearchNotExist()
        {
            testname = "CouponSearchNotExist";
            VerifyBegin(testname);
            //search not exist product
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("test111");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search not exist discount price range");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }
        [Test]
        public void CouponSearchMuch()
        {
            testname = "CouponSearchMuch";
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
        public void CouponSearchInvalid()
        {
            testname = "CouponSearchInvalid";
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
        public void xCouponInplace()
        {
            testname = "xCouponInplace";
            VerifyBegin(testname);
            GoToAdmin("coupons");
           VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "grid Code");
            GetGridCell(0, "Code").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "Code").FindElement(By.TagName("input")).SendKeys("edit1");
           VerifyAreEqual("edit1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "grid edit Code");

            VerifyAreEqual("100", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"), "grid  Value");
            GetGridCell(0, "Value").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "Value").FindElement(By.TagName("input")).SendKeys("1000");
           VerifyAreEqual("1000", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"), "grid edit Value");

            VerifyAreEqual("0", GetGridCell(0, "MinimalOrderPrice").FindElement(By.TagName("input")).GetAttribute("value"), "grid Price ");
            GetGridCell(0, "MinimalOrderPrice").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "MinimalOrderPrice").FindElement(By.TagName("input")).SendKeys("10");
           VerifyAreEqual("10", GetGridCell(0, "MinimalOrderPrice").FindElement(By.TagName("input")).GetAttribute("value"), "grid edit Price");

            VerifyIsTrue(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected, "grid Enabled ");
            GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();
            Thread.Sleep(2000);
           VerifyIsFalse(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected, "grid edit Enabled ");
            Refresh();
           VerifyAreEqual("edit1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "Code ");
            VerifyAreEqual("1000", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"), "Value ");
            VerifyAreEqual("10", GetGridCell(0, "MinimalOrderPrice").FindElement(By.TagName("input")).GetAttribute("value"), " Price");
            VerifyIsFalse(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected, " Enabled");
            VerifyFinally(testname);
        }
       
    }
}
