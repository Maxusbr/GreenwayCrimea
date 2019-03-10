using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using AdvantShop.Selenium.Core.Infrastructure;
using System.Collections.ObjectModel;

namespace AdvantShop.Selenium.Test.Admin.Settings.Tests.ShopSettings
{
    [TestFixture]
    public class SettingsShopTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();      
            Init();
            GoToAdmin("settingssystem");
            if (driver.FindElement(By.Id("EnableInplace")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"EnableInplace\"]")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.CssSelector("[data-e2e=\"BtnSaveSettings\"]")).Click();
                Thread.Sleep(2000);
            }
        }
        [Test]
        public void ShopInfo()
        {
            testname = "ShopInfo";
            VerifyBegin(testname);
            GoToAdmin("settings");

            (new SelectElement(driver.FindElement(By.Id("CountryId")))).SelectByText("Беларусь");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"RegionSelect\"]")))).SelectByText("Брестская область");
            driver.FindElement(By.Id("City")).Clear();
            driver.FindElement(By.Id("City")).SendKeys("Брест");
            driver.FindElement(By.Id("Phone")).Clear();
            driver.FindElement(By.Id("Phone")).SendKeys("999999");

            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);
            GoToAdmin("settings");
            IWebElement selectElem1 = driver.FindElement(By.Id("CountryId"));
            SelectElement select3 = new SelectElement(selectElem1);
            VerifyIsTrue(select3.SelectedOption.Text.Contains("Беларусь"), "country");

            selectElem1 = driver.FindElement(By.CssSelector("[data-e2e=\"RegionSelect\"]"));
            select3 = new SelectElement(selectElem1);
            VerifyIsTrue(select3.SelectedOption.Text.Contains("Брестская область"), "region");

            VerifyAreEqual("Брест", driver.FindElement(By.Id("City")).GetAttribute("value"), "city");
            VerifyAreEqual("999999", driver.FindElement(By.Id("Phone")).GetAttribute("value"), "phone");

            GoToClient();
            VerifyAreEqual("999999", driver.FindElement(By.CssSelector(".site-head-phone")).Text, "phone client");

            VerifyFinally(testname);
        }
        [Test]
        public void ShopFavicon()
        {
            testname = "ShopFavicon";
            VerifyBegin(testname);
            GoToAdmin("settings");

            driver.FindElement(By.XPath("(//input[@type='file'])[3]")).Clear();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("(//input[@type='file'])[3]")).SendKeys(GetPicturePath("big.png"));
            Thread.Sleep(2000);
            VerifyIsFalse(driver.FindElements(By.CssSelector(".picture-uploader-img"))[1].GetAttribute("src").Contains("nophoto"), "add img admin");
            Refresh();
            VerifyIsFalse(driver.FindElements(By.CssSelector(".picture-uploader-img"))[1].GetAttribute("src").Contains("nophoto"), "add img admin 2");

            driver.FindElements(By.CssSelector("[data-e2e=\"imgDel\"]"))[1].Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.FindElements(By.CssSelector(".picture-uploader-img"))[1].GetAttribute("src").Contains("nophoto"), "del img admin");

            driver.FindElements(By.CssSelector("[data-e2e=\"imgByHref\"]"))[1].Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"imgByHrefLinkText\"]")).SendKeys("https://hsto.org/storage/habraeffect/8e/de/8ede5c77f2055b9374613f69b39c8e1c.png");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"imgByHrefBtnSave\"]")).Click();
            Thread.Sleep(2000);

            VerifyIsFalse(driver.FindElements(By.CssSelector(".picture-uploader-img"))[1].GetAttribute("src").Contains("nophoto"), "add img  by href admin");

            driver.FindElements(By.CssSelector("[data-e2e=\"imgDel\"]"))[1].Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.FindElements(By.CssSelector(".picture-uploader-img"))[1].GetAttribute("src").Contains("nophoto"), "del img admin");

            VerifyFinally(testname);
        }
        [Test]
        public void ShopLogo()
        {
            testname = "ShopLogo";
            VerifyBegin(testname);
            GoToAdmin("settings");

            driver.FindElement(By.XPath("(//input[@type='file'])")).Clear();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("(//input[@type='file'])")).SendKeys(GetPicturePath("brandpic.png"));
            Thread.Sleep(2000);
            VerifyIsFalse(driver.FindElement(By.CssSelector(".picture-uploader-img")).GetAttribute("src").Contains("nophoto"), "add img admin");

            GoToClient();
            VerifyIsTrue(driver.FindElements(By.Id("logo")).Count==1, "add img client");

            GoToAdmin("settings");
            driver.FindElement(By.CssSelector("[data-e2e=\"imgDel\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);            
            VerifyIsTrue(driver.FindElement(By.CssSelector(".picture-uploader-img")).GetAttribute("src").Contains("nophoto"), "del img admin");

            GoToClient();
            VerifyIsTrue(driver.FindElements(By.Id("logo")).Count == 0, "del img client");

            GoToAdmin("settings");

            driver.FindElement(By.CssSelector("[data-e2e=\"imgByHref\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"imgByHrefLinkText\"]")).SendKeys("http://paporotnik.com.ua/images/works/logotip-dlya-reklamnogo-internet-servisa-kartinka_1_mobile_2015-04-22-00-19-13.jpg");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"imgByHrefBtnSave\"]")).Click();
            Thread.Sleep(2000);

            VerifyIsFalse(driver.FindElement(By.CssSelector(".picture-uploader-img")).GetAttribute("src").Contains("nophoto"), "add img admin 2");

            GoToClient();
            VerifyIsTrue(driver.FindElements(By.Id("logo")).Count == 1, "add img client 2");

            VerifyFinally(testname);
        }
       
      
        [Test]
        public void ShopLogoCreate()
        {
            testname = "ShopLogoCreate";
            VerifyBegin(testname);
            GoToAdmin("settings");

            driver.FindElement(By.CssSelector("[data-e2e=\"btnLogo\"]")).Click();
            Thread.Sleep(2000);

            Thread.Sleep(2000);
            ReadOnlyCollection<String> windowHandles = driver.WindowHandles;
            String AdminColors = windowHandles[windowHandles.Count - 1];
            driver.SwitchTo().Window(AdminColors);

            VerifyIsTrue(driver.Url.EndsWith("/?logoGeneratorEditOnPageLoad=true"), " url");
            VerifyIsTrue(driver.FindElement(By.Id("logoMainModal")).Displayed, " modal");

            VerifyFinally(testname);
        }
    }
}
