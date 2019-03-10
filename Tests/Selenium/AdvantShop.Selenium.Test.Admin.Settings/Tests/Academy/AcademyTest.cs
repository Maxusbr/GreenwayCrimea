using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.SeleniumTest.Admin.Settings.Academy
{
    [TestFixture]
    public class AcademyTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            Init();
        }
         

        [Test]
        public void OpenAcademy()
        {
            GoToAdmin();
            testname = "OpenAcademy";
            VerifyBegin(testname);

            driver.FindElement(By.XPath("//span[contains(text(), 'Обучение')]")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.Url.Contains("service/academy"), "url academy");
            VerifyIsTrue(driver.FindElements(By.TagName("iframe")).Count > 0, "iframe on page");
            VerifyIsTrue(driver.FindElement(By.TagName("iframe")).GetAttribute("src").ToString().Contains("advantshop.net/shop/Academy.aspx"), "src page");


            var iframe = driver.FindElement(By.TagName("iframe"));
            driver.SwitchTo().Frame(iframe);
            Thread.Sleep(1000);

            VerifyIsTrue(driver.FindElements(By.CssSelector(".header-alt.academy-title.js-academy-title")).Count > 0, "no wrap menu ");
           // VerifyAreEqual("Дизайн", driver.FindElement(By.TagName("h2")).Text, " lesson title");

            VerifyFinally(testname);
        }
    }
}
