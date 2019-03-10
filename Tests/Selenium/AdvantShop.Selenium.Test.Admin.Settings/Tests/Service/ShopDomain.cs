using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using AdvantShop.Web.Infrastructure.Admin.Buttons;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Settings.Service
{
    [TestFixture]
    public class ShopDomain : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            Init();
        }

         


        [Test]
        public void BuyDomainPerson()
        {
            testname = "BuyDomainPerson";
            VerifyBegin(testname);
            GoToAdmin();

            driver.FindElement(By.XPath("//a[contains(text(), 'Привязать домен')]")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.Url.Contains("service/domains"), "url domain");
            VerifyIsTrue(driver.FindElements(By.TagName("iframe")).Count > 0, "iframe on page");

            driver.SwitchTo().Frame(0);
            Thread.Sleep(1000);
            VerifyAreEqual("Привязать домен", driver.FindElement(By.TagName("h1")).Text, " h1, shop domain");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".radio-label-custom")).Count > 0, "radio buttom on page");

            driver.FindElement(By.Name("ctl00$cphMain$btnGo")).Click();
            Thread.Sleep(2000);
           
            VerifyAreEqual("Регистрация домена", driver.FindElement(By.TagName("h1")).Text, " h1, reg domain");

            driver.FindElement(By.Id("searchDomain")).SendKeys("testNameShop");

            driver.FindElements(By.CssSelector(".adv-checkbox-emul"))[1].Click();
            driver.FindElements(By.CssSelector(".adv-checkbox-emul"))[2].Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("btnCheckDomain")).Click();
            Thread.Sleep(1000);



            driver.FindElement(By.CssSelector(".regdomain-item-reg a")).Click();
            Thread.Sleep(2000);

            VerifyAreEqual("Регистрация домена", driver.FindElement(By.TagName("h1")).Text, " h1, reg domain");
            VerifyAreEqual("Регистрация домена testNameShop.ru", driver.FindElement(By.TagName("h3")).Text, " h3, reg domain");

            driver.FindElement(By.Id("txtEmail")).SendKeys("test@mail.ru");
            driver.FindElement(By.Id("txtZip")).SendKeys("111111");
            driver.FindElement(By.Id("txtAddress")).SendKeys("Москва, ул. Тестовая, 6");
            driver.FindElement(By.Id("txtPhone")).SendKeys("+79990000000");
            driver.FindElement(By.Id("txtPerson")).SendKeys("Testov Test");

            driver.FindElement(By.Id("txtPersonR")).SendKeys("Тестов Тест");
            driver.FindElement(By.Id("txtPassportSeria")).SendKeys("1122");
            driver.FindElement(By.Id("txtPassportNumber")).SendKeys("777888");
            driver.FindElement(By.Id("txtPassportDate")).SendKeys("01.01.2000");
            driver.FindElement(By.Id("txtPassport")).SendKeys("выдан 1 о/м Москвы");
            driver.FindElement(By.Id("txtBirthDate")).SendKeys("01.01.1950");
            (new SelectElement(driver.FindElement(By.Id("ddlCountry")))).SelectByText("Россия");


            driver.FindElement(By.Id("btnRegDomain")).Click();
            Thread.Sleep(1000);
            //сделать проверки!!!
            VerifyFinally(testname);
        }

        [Test]
        public void BuyDomainOrg()
        {
            testname = "BuyDomainOrg";
            VerifyBegin(testname);
            GoToAdmin();

            driver.FindElement(By.XPath("//a[contains(text(), 'Привязать домен')]")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(driver.Url.Contains("service/domains"), "url domain");
            VerifyIsTrue(driver.FindElements(By.TagName("iframe")).Count > 0, "iframe on page");

            driver.SwitchTo().Frame(0);
            Thread.Sleep(1000);
            VerifyAreEqual("Привязать домен", driver.FindElement(By.TagName("h1")).Text, " h1, shop domain");
            VerifyIsTrue(driver.FindElements(By.CssSelector(".radio-label-custom")).Count > 0, "radio buttom on page");

            driver.FindElement(By.Name("ctl00$cphMain$btnGo")).Click();
            Thread.Sleep(2000);

            VerifyAreEqual("Регистрация домена", driver.FindElement(By.TagName("h1")).Text, " h1, reg domain");

            driver.FindElement(By.Id("searchDomain")).SendKeys("OrganizationTestNameShop");

            driver.FindElements(By.CssSelector(".adv-checkbox-emul"))[0].Click();
            driver.FindElements(By.CssSelector(".adv-checkbox-emul"))[1].Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("btnCheckDomain")).Click();
            Thread.Sleep(1000);



            driver.FindElement(By.CssSelector(".regdomain-item-reg a")).Click();
            Thread.Sleep(2000);

            VerifyAreEqual("Регистрация домена", driver.FindElement(By.TagName("h1")).Text, " h1, reg domain");
            VerifyAreEqual("Регистрация домена OrganizationTestNameShop.su", driver.FindElement(By.TagName("h3")).Text, " h3, reg domain");
            driver.FindElements(By.CssSelector(".adv-radio-label"))[1].Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("txtEmail")).SendKeys("test@mail.ru");
            driver.FindElement(By.Id("txtZip")).SendKeys("111111");
            driver.FindElement(By.Id("txtAddress")).SendKeys("Москва, ул. Тестовая, 6");
            driver.FindElement(By.Id("txtPhone")).SendKeys("+79990000000");
            driver.FindElement(By.Id("txtOrg")).SendKeys("TestNameOrganization");
            driver.FindElement(By.Id("txtOrgR")).SendKeys("Имя тестовой организации");
            driver.FindElement(By.Id("txtOrgInn")).SendKeys("7701107259");
            driver.FindElement(By.Id("txtOrgKpp")).SendKeys("632946014");
            (new SelectElement(driver.FindElement(By.Id("ddlCountry")))).SelectByText("Россия");
            driver.FindElement(By.Id("txtAddress")).SendKeys("Москва, ул. Тестовая, 6");



            driver.FindElement(By.Id("btnRegDomain")).Click();
            Thread.Sleep(1000);
            //сделать проверки!!!
            VerifyFinally(testname);
        }
    }
}
