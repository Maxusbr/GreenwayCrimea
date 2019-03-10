using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.Selenium.Test.Admin.Marketing.Tests.Coupon
{
    [TestFixture]
    public class CouponSortTest : BaseSeleniumTest
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
        public void CouponSortCode()
        {
            testname = "CouponSortCode";
            VerifyBegin(testname);

            VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "grid Code");
            VerifyAreEqual("100", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"), "grid Value");
            VerifyAreEqual("0", GetGridCell(0, "MinimalOrderPrice").FindElement(By.TagName("input")).GetAttribute("value"), "grid MinimalOrderPrice");
            VerifyAreEqual("Фиксированный", GetGridCell(0, "TypeFormatted").Text, "grid TypeFormatted");
            VerifyAreEqual("Бессрочно", GetGridCell(0, "ExpirationDateFormatted").Text, "grid ExpirationDate");
            VerifyAreEqual("0 / 0", GetGridCell(0, "ActualUses").Text, "grid ActualUses");
            VerifyAreEqual("21.11.2016 11:10:00", GetGridCell(0, "AddingDateFormatted").Text, "grid AddingDate");

            GetGridCell(-1, "Code").Click();
            WaitForAjax();
            VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort Code ASC  value1");
            VerifyAreEqual("test6", GetGridCell(5, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort Code DESC  value5");

            GetGridCell(-1, "Code").Click();
            WaitForAjax();
            VerifyAreEqual("test6", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort Code ASC  value1");
            VerifyAreEqual("test1", GetGridCell(5, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort Code DESC  value5");
            VerifyFinally(testname);
        }
        [Test]
        public void CouponSortValue()
        {
            testname = "CouponSortValue";
            VerifyBegin(testname);
            GetGridCell(-1, "Value").Click();
            WaitForAjax();
            VerifyAreEqual("test3", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort Value ASC  value1");
            VerifyAreEqual("test6", GetGridCell(5, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort Value DESC  value5");

            GetGridCell(-1, "Value").Click();
            WaitForAjax();
            VerifyAreEqual("test2", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort Value ASC  value1");
            VerifyAreEqual("test3", GetGridCell(5, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort Value DESC  value5");
            VerifyFinally(testname);
        }
        [Test]
        public void CouponSortMinimalOrderPrice()
        {
            testname = "CouponSortMinimalOrderPrice";
            VerifyBegin(testname);
            GetGridCell(-1, "MinimalOrderPrice").Click();
            WaitForAjax();
            VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort MinimalOrderPrice ASC  value1");
            VerifyAreEqual("test4", GetGridCell(5, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort MinimalOrderPrice DESC  value5");

            GetGridCell(-1, "MinimalOrderPrice").Click();
            WaitForAjax();
            VerifyAreEqual("test4", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort MinimalOrderPrice ASC  value1");
            VerifyAreEqual("test3", GetGridCell(5, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort MinimalOrderPrice DESC  value5");
            VerifyFinally(testname);
        }
        [Test]
        public void CouponSortTypeFormatted()
        {
            testname = "CouponSortTypeFormatted";
            VerifyBegin(testname);
            GetGridCell(-1, "TypeFormatted").Click();
            WaitForAjax();
            VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort TypeFormatted ASC  value1");
            VerifyAreEqual("test4", GetGridCell(5, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort TypeFormatted DESC  value5");

            GetGridCell(-1, "TypeFormatted").Click();
            WaitForAjax();
            VerifyAreEqual("test3", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort TypeFormatted ASC  value1");
            VerifyAreEqual("test2", GetGridCell(5, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort TypeFormatted DESC  value5");
            VerifyFinally(testname);
        }
        [Test]
        public void CouponSortDate()
        {
            testname = "CouponSortDate";
            VerifyBegin(testname);
            GetGridCell(-1, "ExpirationDateFormatted").Click();
            WaitForAjax();
            VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort ExpirationDateFormatted ASC  value1");
            VerifyAreEqual("test6", GetGridCell(5, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort ExpirationDateFormatted DESC  value5");

            GetGridCell(-1, "ExpirationDateFormatted").Click();
            WaitForAjax();
            VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort ExpirationDateFormatted ASC  value1");
            VerifyAreEqual("test6", GetGridCell(5, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort ExpirationDateFormatted DESC  value5");
            VerifyFinally(testname);
        }
        [Test]
        public void CouponSortActualUses()
        {
            testname = "CouponSortActualUses";
            VerifyBegin(testname);
            GetGridCell(-1, "ActualUses").Click();
            WaitForAjax();
            VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort ActualUses ASC  value1");
            VerifyAreEqual("test6", GetGridCell(5, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort ActualUses DESC  value5");

            GetGridCell(-1, "ActualUses").Click();
            WaitForAjax();
            VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort ActualUses ASC  value1");
            VerifyAreEqual("test6", GetGridCell(5, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort ActualUses DESC  value5");
            VerifyFinally(testname);
        }
        [Test]
        public void CouponSortAddingDate()
        {
            testname = "CouponSortAddingDate";
            VerifyBegin(testname);
            GetGridCell(-1, "AddingDateFormatted").Click();
            WaitForAjax();
            VerifyAreEqual("test6", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort AddingDateFormatted ASC  value1");
            VerifyAreEqual("test1", GetGridCell(5, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort AddingDateFormatted DESC  value1");

            GetGridCell(-1, "AddingDateFormatted").Click();
            WaitForAjax();
            VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort AddingDateFormatted ASC  value1");
            VerifyAreEqual("test6", GetGridCell(5, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort AddingDateFormatted DESC  value5");
            VerifyFinally(testname);
        }
        [Test]
        public void CouponSortEnabled()
        {
            testname = "CouponSortEnabled";
            VerifyBegin(testname);
            GetGridCell(-1, "Enabled").Click();
            WaitForAjax();
            VerifyAreEqual("test5", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort Enabled ASC  value1");
            VerifyAreEqual("test4", GetGridCell(5, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort Enabled DESC  value5");

            GetGridCell(-1, "Enabled").Click();
            WaitForAjax();
            VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort Enabled ASC  value1");
            VerifyAreEqual("test5", GetGridCell(5, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "sort Enabled DESC  value5");

            //back default
            GetGridCell(-1, "Code").Click();
            WaitForAjax();
            VerifyFinally(testname);
        }
    }
}
