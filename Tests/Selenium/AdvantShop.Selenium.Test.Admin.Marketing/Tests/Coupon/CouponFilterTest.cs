using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.Selenium.Test.Admin.Marketing.Tests.Coupon
{
    [TestFixture]
    public class CouponFilterTest : BaseSeleniumTest
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
        public void CouponFilterCode()
        {
            testname = "CouponFilterCode";
            VerifyBegin(testname);
            //Код
            Functions.GridFilterSet(driver, baseURL, "Code");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("test2");
            DropFocus("h1");
            Refresh();
           VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 1, " count row");
           VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count elem");
            VerifyAreEqual("test2", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "value elem ");
            Functions.GridFilterClose(driver, baseURL, "Code");
            VerifyAreEqual("Найдено записей: 6", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all elem");
            VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "close value elem 1 ");
            VerifyAreEqual("test6", GetGridCell(5, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "close value elem 5 ");

            VerifyFinally(testname);
        }
        [Test]
        public void CouponFilterTypeFormatted()
        {
            testname = "CouponFilterTypeFormatted";
            VerifyBegin(testname);
            //Тип 
            Functions.GridFilterSelectDropFocus(driver, baseURL, filterName: "TypeFormatted", filterItem: "Фиксированный", tag: "h1");
           VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 4, "count row ");
            VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "value elem 1 ");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Процентный");
            DropFocus("h1");
           VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 2, "count row ");
            VerifyAreEqual("test3", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "value elem 2 ");
            Functions.GridFilterClose(driver, baseURL, "TypeFormatted");
           VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "close value elem 1 ");
            VerifyAreEqual("test6", GetGridCell(5, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "close value elem 5 ");

            VerifyFinally(testname);
        }
        [Test]
        public void CouponFilterValue()
        {
            testname = "CouponFilterValue";
            VerifyBegin(testname);
            //Значение
            Functions.GridFilterSet(driver, baseURL, "Value");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("30");
            DropFocus("h1");
           VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 1, "count row ");
            VerifyAreEqual("test3", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "value elem  ");
            Functions.GridFilterClose(driver, baseURL, "Value");
           VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "close value elem 1 ");
            VerifyAreEqual("test6", GetGridCell(5, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "close value elem 5 ");

            VerifyFinally(testname);
        }
        [Test]
        public void CouponFilterEnabled()
        {
            testname = "CouponFilterEnabled";
            VerifyBegin(testname);
            //Активность
            Functions.GridFilterSelectDropFocus(driver, baseURL, filterName: "Enabled", filterItem: "Активные", tag: "h1");
           VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 5, "count row ");
            VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "value elem  ");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Неактивные");
            DropFocus("h1");
           VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 1, "count row ");
            VerifyAreEqual("test5", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "value elem 2 ");
            Functions.GridFilterClose(driver, baseURL, "Enabled");
           VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "close value elem 1");
            VerifyAreEqual("test6", GetGridCell(5, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "close value elem 5");

            VerifyFinally(testname);
        }
        [Test]
        public void CouponFilterPrice()
        {
            testname = "CouponFilterPrice";
            VerifyBegin(testname);
            //Min summ
            Functions.GridFilterSet(driver, baseURL, "MinimalOrderPrice");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("100");
            DropFocus("h1");
           VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 1, "count row ");
            VerifyAreEqual("test4", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "value elem  ");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("300");
            DropFocus("h1");
           VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no count elem");

            Functions.GridFilterClose(driver, baseURL, "MinimalOrderPrice");
           VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "close value elem 1");
            VerifyAreEqual("test6", GetGridCell(5, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "close value elem 5");
            VerifyFinally(testname);
            
        }

    }
}
