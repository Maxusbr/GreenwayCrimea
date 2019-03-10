using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace AdvantShop.SeleniumTest.Admin.Export.ExportProducts
{
    [TestFixture]
    public class ExportProductsAddEditTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
            "data\\Admin\\Catalog\\Export\\Catalog.Product.csv",
             "data\\Admin\\Catalog\\Export\\Catalog.Photo.csv",
           "data\\Admin\\Catalog\\Export\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\Export\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\Export\\Catalog.ProductCategories.csv",
           "data\\Admin\\Catalog\\Export\\Catalog.Brand.csv",
             "data\\Admin\\Catalog\\Export\\Catalog.Property.csv",
                 "data\\Admin\\Catalog\\Export\\Catalog.PropertyValue.csv",
                 "data\\Admin\\Catalog\\Export\\Catalog.ProductPropertyValue.csv",
                "data\\Admin\\Catalog\\Export\\Catalog.PropertyGroup.csv",
                "Data\\Admin\\Catalog\\Export\\Catalog.Color.csv",
                "data\\Admin\\Catalog\\Export\\Catalog.Size.csv"
           );

            Init();
        }

        [Test]
        public void ExportProductzAddNew()
        {
            GoToAdmin("exportfeeds");

            Assert.IsFalse(driver.FindElement(By.CssSelector(".aside-menu")).Text.Contains("Новая выгрузка тест"));

            driver.FindElement(By.CssSelector("[data-e2e=\"AddExportFeed\"]")).Click();
            WaitForElem(By.CssSelector(".modal-header"));

            Assert.AreEqual("Новая выгрузка", driver.FindElement(By.TagName("h2")).Text);

            driver.FindElement(By.CssSelector("[data-e2e=\"ExportAddName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ExportAddName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ExportAddName\"]")).SendKeys("Новая выгрузка тест");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"ExportAddType\"]")))).SelectByText("Excel-файл (csv)");

            driver.FindElement(By.CssSelector("[data-e2e=\"ExportAddDesc\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"ExportAddDesc\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"ExportAddDesc\"]")).SendKeys("Описание тест");

            driver.FindElement(By.XPath("//button[contains(text(), 'Ок')]")).Click();
            Thread.Sleep(2000);

            //check 
            GoToAdmin("exportfeeds");

            Assert.IsTrue(driver.FindElement(By.CssSelector(".aside-menu")).Text.Contains("Новая выгрузка тест"));

            driver.FindElement(By.CssSelector(".aside-menu")).FindElement(By.XPath("//div[contains(text(), 'Новая выгрузка тест')]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//a[contains(text(), 'Параметры выгрузки')]")).Click();
            WaitForElem(By.Id("Name"));

            Assert.AreEqual("Новая выгрузка тест", driver.FindElement(By.Id("Name")).GetAttribute("value"));
            Assert.AreEqual("Описание тест", driver.FindElement(By.Id("Description")).GetAttribute("value"));
        }

        [Test]
        public void ExportProductsEdit()
        {
            GoToAdmin("exportfeeds");

            Assert.IsTrue(driver.FindElement(By.CssSelector(".aside-menu")).Text.Contains("Выгрузка каталога в Csv"));

            driver.FindElement(By.CssSelector(".aside-menu")).FindElement(By.XPath("//div[contains(text(), 'Выгрузка каталога в Csv')]")).Click();
            WaitForElem(By.XPath("//a[contains(text(), 'Параметры выгрузки')]"));
            driver.FindElement(By.XPath("//a[contains(text(), 'Параметры выгрузки')]")).Click();
            WaitForElem(By.Id("Name"));

            if (!driver.FindElement(By.Id("ExportFeedSettings_Active")).Selected)

            {
                Assert.IsFalse(driver.FindElement(By.Id("ExportFeedSettings_Interval")).Enabled);
                Assert.IsFalse(driver.FindElement(By.Id("ddlIntervalType")).Enabled);

                driver.FindElement(By.CssSelector("[data-e2e=\"exportCommonSettingsActive\"]")).Click();
                Assert.IsTrue(driver.FindElement(By.Id("ExportFeedSettings_Interval")).Enabled);
                Assert.IsTrue(driver.FindElement(By.Id("ddlIntervalType")).Enabled);

                driver.FindElement(By.Id("ExportFeedSettings_Interval")).Click();
                driver.FindElement(By.Id("ExportFeedSettings_Interval")).Clear();

                driver.FindElement(By.Id("ExportFeedSettings_Interval")).SendKeys("10");
                DropFocus("h1");

                (new SelectElement(driver.FindElement(By.Id("ddlIntervalType")))).SelectByText("В часах");
            }

            else
            {
                driver.FindElement(By.Id("ExportFeedSettings_Interval")).Click();
                driver.FindElement(By.Id("ExportFeedSettings_Interval")).Clear();

                driver.FindElement(By.Id("ExportFeedSettings_Interval")).SendKeys("10");
                DropFocus("h1");

                (new SelectElement(driver.FindElement(By.Id("ddlIntervalType")))).SelectByText("В часах");
            }

            driver.FindElement(By.Id("Name")).Click();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("Измененное название выгрузки тест");

            driver.FindElement(By.Id("Description")).Click();
            driver.FindElement(By.Id("Description")).Clear();
            driver.FindElement(By.Id("Description")).SendKeys("Измененное описание выгрузки тест");

            driver.FindElement(By.CssSelector("[data-e2e=\"ButtonSave\"]")).Click();
            Thread.Sleep(1000);
            
            //check 
            GoToAdmin("exportfeeds");

            Assert.IsFalse(driver.FindElement(By.CssSelector(".aside-menu")).Text.Contains("Выгрузка каталога в Csv"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".aside-menu")).Text.Contains("Измененное название выгрузки тест"));

            driver.FindElement(By.CssSelector(".aside-menu")).FindElement(By.XPath("//div[contains(text(), 'Измененное название выгрузки тест')]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("exportfeeds/index/2");
            Thread.Sleep(1000);

            driver.FindElement(By.XPath("//a[contains(text(), 'Параметры выгрузки')]")).Click();
            Thread.Sleep(2000);

            Assert.AreEqual("Измененное название выгрузки тест", driver.FindElement(By.Id("Name")).GetAttribute("value"));
            Assert.AreEqual("Измененное описание выгрузки тест", driver.FindElement(By.Id("Description")).GetAttribute("value"));

            IWebElement selectElem = driver.FindElement(By.Id("ddlIntervalType"));
            SelectElement select = new SelectElement(selectElem);
            Assert.IsTrue(select.SelectedOption.Text.Contains("В часах"));
        }

        [Test]
        public void ExportProductsDelete()
        {
            GoToAdmin("exportfeeds");

            Assert.IsTrue(driver.FindElement(By.CssSelector(".aside-menu")).Text.Contains("Выгрузка в Yandex.Market"));

            driver.FindElement(By.CssSelector(".aside-menu")).FindElement(By.XPath("//div[contains(text(), 'Выгрузка в Yandex.Market')]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("exportfeeds/index/3");
            Thread.Sleep(1000);

            driver.FindElement(By.XPath("//a[contains(text(), 'Удалить')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            //check 
            GoToAdmin("exportfeeds");

            Assert.IsFalse(driver.FindElement(By.CssSelector(".aside-menu")).Text.Contains("Выгрузка в Yandex.Market"));
        }
    }
}
