using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System;

namespace AdvantShop.SeleniumTest.Admin.Marketing.Certificates
{
    [TestFixture]
    public class CertificateAddTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Orders | ClearType.Catalog);
            InitializeService.LoadData(
            "data\\Admin\\Certificates\\AddCertificate\\Catalog.Product.csv",
            "data\\Admin\\Certificates\\AddCertificate\\Catalog.Photo.csv",
            "data\\Admin\\Certificates\\AddCertificate\\Catalog.Offer.csv",
            "data\\Admin\\Certificates\\AddCertificate\\Catalog.Category.csv",
            "data\\Admin\\Certificates\\AddCertificate\\Catalog.ProductCategories.csv",
            "data\\Admin\\Certificates\\AddCertificate\\Catalog.Brand.csv",
            "data\\Admin\\Certificates\\AddCertificate\\Catalog.Color.csv",
            "data\\Admin\\Certificates\\AddCertificate\\Catalog.Size.csv",
             "data\\Admin\\Certificates\\AddCertificate\\[Order].Certificate.csv",
            "data\\Admin\\Certificates\\AddCertificate\\[Order].OrderContact.csv",
            "data\\Admin\\Certificates\\AddCertificate\\[Order].OrderCurrency.csv",
             "data\\Admin\\Certificates\\AddCertificate\\[Order].OrderItems.csv",
             "data\\Admin\\Certificates\\AddCertificate\\[Order].OrderStatus.csv",
             "data\\Admin\\Certificates\\AddCertificate\\[Order].[Order].csv",
             "Data\\Admin\\Certificates\\AddCertificate\\[Order].OrderSource.csv"
           );
            Init();
            
        }

        [Test]
        public void AddCertificate()
        {
            testname = "CertificateAdd";
            VerifyBegin(testname);

            GoToAdmin("certificates");

            driver.FindElement(By.CssSelector("[data-e2e=\"AddCertificates\"]")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-content"));
            VerifyAreEqual("Добавить подарочный сертификат", driver.FindElement(By.TagName("h2")).Text, "modal h2");

           driver.FindElement(By.CssSelector("[data-e2e=\"CertCode\"]")).SendKeys("New Certificate");
            driver.FindElement(By.CssSelector("[data-e2e=\"CertFrom\"]")).SendKeys("New From me");
           driver.FindElement(By.CssSelector("[data-e2e=\"CertTo\"]")).SendKeys("New To Me");
           driver.FindElement(By.CssSelector("[data-e2e=\"CertSum\"]")).SendKeys("100000");
            driver.FindElement(By.CssSelector("[data-e2e=\"CertMailTo\"]")).SendKeys("testTo@mail.ru");
            driver.FindElement(By.CssSelector("[data-e2e=\"CertMailFrom\"]")).SendKeys("testFrom@mail.ru");

            /*driver.FindElement(By.CssSelector("[data-e2e=\"CertUsed\"]")).Click();
             driver.FindElement(By.CssSelector("[data-e2e=\"CertEnabled\"]")).Click();
             driver.FindElement(By.CssSelector("[data-e2e=\"CertPaid\"]")).Click();
             */
            SetCkText("It is a new certificate!", "editor1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"CertPayment\"]")))).SelectByText("Оплата наличными при получении заказа в пункте выдачи");

            driver.FindElement(By.CssSelector("[data-e2e=\"CertSave\"]")).Click();
            Thread.Sleep(2000);

            //check grid
            VerifyAreEqual("New Certificate", GetGridCell(0, "CertificateCode").Text, "grid CertificateCode");
            VerifyAreNotEqual("", GetGridCell(0, "OrderId").Text, "grid OrderId");
            VerifyAreEqual("", GetGridCell(0, "ApplyOrderNumber").Text, "grid ApplyOrderNumber");
            VerifyAreEqual("100 000 руб.", GetGridCell(0, "FullSum").Text, "grid FullSum");
            VerifyIsFalse(GetGridCell(0, "Paid").FindElement(By.CssSelector("input")).Selected, "grid Paid");
            VerifyIsTrue(GetGridCell(0, "Enable").FindElement(By.CssSelector("input")).Selected, "grid Enable");
            VerifyIsFalse(GetGridCell(0, "Used").FindElement(By.CssSelector(" input")).Selected, "grid Used");
            VerifyIsTrue(GetGridCell(0, "CreationDates").Text.Contains(System.DateTime.Today.ToString("dd.MM.yyyy")), "grid CreationDates");
            //check grid orders
            string orderId = GetGridCell(0, "OrderId").Text;
            GoToAdmin("orders");
            
            VerifyAreEqual("Новый", GetGridCell(0, "StatusName").Text, " Grid orders StatusName");
            VerifyAreEqual(orderId, GetGridCell(0, "Number").Text, " Grid orders Number");
            VerifyAreEqual("100 000 руб.", GetGridCell(0, "SumFormatted").Text, " Grid orders Sum");
            VerifyIsTrue(GetGridCell(0, "OrderDateFormatted").Text.Contains(System.DateTime.Today.ToString("dd.MM.yyyy")), "grid:"+ GetGridCell(0, "OrderDateFormatted").Text + "orders CreationDates: "+ (System.DateTime.Today.ToString("dd.MM.yyyy")));
            //check order
            GetGridCell(0, "Number").Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Сертификат", GetGridCell(0, "CustomName", "OrderCertificates").Text, " Grid in order CustomName");
            VerifyAreEqual("New Certificate", GetGridCell(0, "CertificateCode", "OrderCertificates").Text, " Grid in order CertificateCode");
            VerifyAreEqual("100000", GetGridCell(0, "Sum", "OrderCertificates").Text, " Grid in order Sum");
            //pay
            driver.FindElement(By.CssSelector(".switcher-state-label span")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);
            //check grid cert
            GoToAdmin("certificates");
            VerifyAreEqual("New Certificate", GetGridCell(0, "CertificateCode").Text, "grid CertificateCode");
            VerifyIsTrue(GetGridCell(0, "Paid").FindElement(By.CssSelector("input")).Selected, "grid Paid");
            VerifyIsTrue(GetGridCell(0, "Enable").FindElement(By.CssSelector("input")).Selected, "grid Enable");
            VerifyIsFalse(GetGridCell(0, "Used").FindElement(By.CssSelector(" input")).Selected, "grid Used");
            //cheking client
            ProductToCard("1");
            GoToClient("cart");
            VerifyAreEqual("Скидка: 10%", driver.FindElement(By.CssSelector(".price-discount")).Text, "client discount");
            VerifyAreEqual("900 руб.", driver.FindElement(By.CssSelector(".cart-full-result-price")).Text, "client result price");
            driver.FindElement(By.CssSelector(".col-xs-8 input")).SendKeys("New Certificate");
            driver.FindElement(By.CssSelector(".btn-action.btn-expander")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Сертификат:", driver.FindElements(By.CssSelector(".cart-full-summary-name"))[1].Text, "client coupon");
            VerifyAreEqual("100 000 руб.", driver.FindElements(By.CssSelector(".cart-full-summary-price span"))[0].Text, "client sum coupon");
            VerifyAreEqual("0 руб.", driver.FindElement(By.CssSelector(".cart-full-result-price")).Text, "client rezult");

            ProductToCard("1");
            GoToClient("cart");
            VerifyAreEqual("Сертификат:", driver.FindElements(By.CssSelector(".cart-full-summary-name"))[1].Text, "client 2 coupon");
            VerifyAreEqual("100 000 руб.", driver.FindElements(By.CssSelector(".cart-full-summary-price span"))[0].Text, "client sum discount");
           VerifyAreEqual("0 руб.", driver.FindElement(By.CssSelector(".cart-full-result-price")).Text, "client rezult sum");
            ScrollTo(By.TagName("footer"));

            driver.FindElement(By.CssSelector(".btn.btn-middle.btn-submit")).Click();
            Thread.Sleep(5000);
            WaitForElem(By.CssSelector(".breads"));
            WaitForElem(By.Id("rightCell"));

            ScrollTo(By.TagName("footer"));
            VerifyIsTrue(driver.FindElement(By.CssSelector(".checkout-result")).Text.Contains("0 руб."), "checkout rezult");
            driver.FindElements(By.CssSelector(".btn.btn-big.btn-submit"))[1].Click();
            Thread.Sleep(4000);
            //check used certificates
            GoToAdmin("certificates");
            GetGridFilter().SendKeys("New Certificate");
            DropFocus("h1");
            VerifyAreEqual("New Certificate", GetGridCell(0, "CertificateCode").Text, "grid CertificateCode");
            VerifyAreEqual(orderId, GetGridCell(0, "OrderId").Text, "grid OrderId");
            int applayId = Convert.ToInt32(orderId) + 1;
            VerifyAreEqual(applayId.ToString(), GetGridCell(0, "ApplyOrderNumber").Text, "grid ApplyOrderNumber");
            VerifyIsTrue(GetGridCell(0, "Paid").FindElement(By.CssSelector("input")).Selected, "grid Paid");
            VerifyIsTrue(GetGridCell(0, "Enable").FindElement(By.CssSelector("input")).Selected, "grid Enable");
            VerifyIsTrue(GetGridCell(0, "Used").FindElement(By.CssSelector(" input")).Selected, "grid Used");
            //cheking repeat used client
            ProductToCard("1");
            GoToClient("cart");
            VerifyAreEqual("Скидка: 10%", driver.FindElement(By.CssSelector(".price-discount")).Text, "client discount");
            VerifyAreEqual("900 руб.", driver.FindElement(By.CssSelector(".cart-full-result-price")).Text, "client result price");
            driver.FindElement(By.CssSelector(".col-xs-8 input")).SendKeys("New Certificate");
            driver.FindElement(By.CssSelector(".btn-action.btn-expander")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("900 руб.", driver.FindElement(By.CssSelector(".cart-full-result-price")).Text, "old price without coupon");
            ScrollTo(By.TagName("footer"));

            driver.FindElement(By.CssSelector(".btn.btn-middle.btn-submit")).Click();
            Thread.Sleep(5000);
            WaitForElem(By.CssSelector(".breads"));
            WaitForElem(By.Id("rightCell"));

            ScrollTo(By.TagName(".btn.btn-small.btn-action.btn-expander"));
            VerifyIsTrue(driver.FindElement(By.CssSelector(".checkout-result")).Text.Contains("900 руб."), "checkout rezult");
            driver.FindElement(By.CssSelector(".col-xs-8 input")).SendKeys("New Certificate");
            driver.FindElement(By.CssSelector(".btn.btn-small.btn-action.btn-expander")).Click();
            Thread.Sleep(1000);

            VerifyIsTrue(driver.FindElement(By.CssSelector(".checkout-result")).Text.Contains("900 руб."), "checkout afred cert rezult");
            driver.FindElements(By.CssSelector(".btn.btn-big.btn-submit"))[1].Click();
            Thread.Sleep(4000);


            VerifyFinally(testname);
        }      

        [Test]
        public void AddDisabledCertificate()
        {
            testname = "AddDisabledCertificate";
            VerifyBegin(testname);


            GoToAdmin("certificates");

            driver.FindElement(By.CssSelector("[data-e2e=\"AddCertificates\"]")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-content"));
            VerifyAreEqual("Добавить подарочный сертификат", driver.FindElement(By.TagName("h2")).Text, "modal h2");

            driver.FindElement(By.CssSelector("[data-e2e=\"CertCode\"]")).SendKeys("New Disabled Certificate");
            driver.FindElement(By.CssSelector("[data-e2e=\"CertFrom\"]")).SendKeys("New From Disabled me");
            driver.FindElement(By.CssSelector("[data-e2e=\"CertTo\"]")).SendKeys("New To Disabled  Me");
            driver.FindElement(By.CssSelector("[data-e2e=\"CertSum\"]")).SendKeys("100");
            driver.FindElement(By.CssSelector("[data-e2e=\"CertMailTo\"]")).SendKeys("testTo@mail.ru");
            driver.FindElement(By.CssSelector("[data-e2e=\"CertMailFrom\"]")).SendKeys("testFrom@mail.ru");
            driver.FindElement(By.CssSelector("[data-e2e=\"CertEnabled\"]")).Click();
            
            SetCkText("It is a new Disabled certificate!", "editor1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"CertPayment\"]")))).SelectByText("Оплата наличными при получении заказа в пункте выдачи");

            driver.FindElement(By.CssSelector("[data-e2e=\"CertSave\"]")).Click();
            Thread.Sleep(2000);

            //check grid
            VerifyAreEqual("New Disabled Certificate", GetGridCell(0, "CertificateCode").Text, "grid CertificateCode");
            VerifyAreNotEqual("", GetGridCell(0, "OrderId").Text, "grid OrderId");
            VerifyAreEqual("", GetGridCell(0, "ApplyOrderNumber").Text, "grid ApplyOrderNumber");
            VerifyAreEqual("100 руб.", GetGridCell(0, "FullSum").Text, "grid FullSum");
            VerifyIsFalse(GetGridCell(0, "Paid").FindElement(By.CssSelector("input")).Selected, "grid Paid");
            VerifyIsFalse(GetGridCell(0, "Enable").FindElement(By.CssSelector("input")).Selected, "grid Enable");
            VerifyIsFalse(GetGridCell(0, "Used").FindElement(By.CssSelector(" input")).Selected, "grid Used");
            VerifyIsTrue(GetGridCell(0, "CreationDates").Text.Contains(System.DateTime.Today.ToString("dd.MM.yyyy")), "grid CreationDates");
                    
            //cheking client
            ProductToCard("1");
            GoToClient("cart");
            VerifyAreEqual("Скидка: 10%", driver.FindElement(By.CssSelector(".price-discount")).Text, "client percent discount");
            VerifyAreEqual("900 руб.", driver.FindElement(By.CssSelector(".cart-full-result-price")).Text, "client sum discont");
            driver.FindElement(By.CssSelector(".col-xs-8 input")).SendKeys("New Disabled Certificate");
            driver.FindElement(By.CssSelector(".btn-action.btn-expander")).Click();
            Thread.Sleep(1000);
            // VerifyIsTrue(driver.PageSource.Contains("Невозможно применить купон"), "client not enabled coupon");
            VerifyAreEqual("900 руб.", driver.FindElement(By.CssSelector(".cart-full-result-price")).Text, "client old cart price");

            GoToAdmin("orders");

            VerifyAreEqual("Новый", GetGridCell(0, "StatusName").Text, " Grid orders StatusName");
             VerifyAreEqual("100 руб.", GetGridCell(0, "SumFormatted").Text, " Grid orders Sum");
            VerifyIsTrue(GetGridCell(0, "OrderDateFormatted").Text.Contains(System.DateTime.Today.ToString("dd.MM.yyyy")), "grid orders CreationDates");
            //check order
            GetGridCell(0, "Number").Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Сертификат", GetGridCell(0, "CustomName", "OrderCertificates").Text, " Grid in order CustomName");
            VerifyAreEqual("New Disabled Certificate", GetGridCell(0, "CertificateCode", "OrderCertificates").Text, " Grid in order CertificateCode");
            VerifyAreEqual("100", GetGridCell(0, "Sum", "OrderCertificates").Text, " Grid in order Sum");
            //pay
            driver.FindElement(By.CssSelector(".switcher-state-label span")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);
            GoToClient("cart");
            VerifyAreEqual("Скидка: 10%", driver.FindElement(By.CssSelector(".price-discount")).Text, "client percent discount");
            VerifyAreEqual("900 руб.", driver.FindElement(By.CssSelector(".cart-full-result-price")).Text, "client sum discont");
            driver.FindElement(By.CssSelector(".col-xs-8 input")).SendKeys("New Disabled Certificate");
            driver.FindElement(By.CssSelector(".btn-action.btn-expander")).Click();
            Thread.Sleep(1000);
            // VerifyIsTrue(driver.PageSource.Contains("Невозможно применить купон"), "client not enabled coupon");
            VerifyAreEqual("900 руб.", driver.FindElement(By.CssSelector(".cart-full-result-price")).Text, "client old cart price");
            
            driver.FindElement(By.CssSelector(".cart-full-body-item.cart-full-remove a")).Click();
            Thread.Sleep(1000);
            VerifyFinally(testname);
        }

        [Test]
        public void EditCertificate()
        {
            testname = "EditCertificate";
            VerifyBegin(testname);
            GoToAdmin("certificates");

            GetGridFilter().SendKeys("Certificate1");
            DropFocus("h1");
            GetGridCell(0, "CertificateCode").FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Редактировать подарочный сертификат", driver.FindElement(By.TagName("h2")).Text, "modal edit h2");
            //cheking modal win

            VerifyAreEqual("Certificate1", driver.FindElement(By.CssSelector("[data-e2e=\"CertCode\"]")).GetAttribute("value"), " modal cod");
            VerifyAreEqual("FromMe1", driver.FindElement(By.CssSelector("[data-e2e=\"CertFrom\"]")).GetAttribute("value"), "modal from ");
            VerifyAreEqual("ToMe1", driver.FindElement(By.CssSelector("[data-e2e=\"CertTo\"]")).GetAttribute("value"), " modal to");
            VerifyAreEqual("100", driver.FindElement(By.CssSelector("[data-e2e=\"CertSum\"]")).GetAttribute("value"), " modal sum");
            VerifyAreEqual("me@gmail.com", driver.FindElement(By.CssSelector("[data-e2e=\"CertMailTo\"]")).GetAttribute("value"), " modal mail.");

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"CertUsed\"]")).FindElement(By.CssSelector("input")).Selected, " modal used");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"CertEnabled\"]")).FindElement(By.CssSelector("input")).Selected, "modal enabled ");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"CertPaid\"]")).FindElement(By.CssSelector("input")).Selected, " modal paid");
            AssertCkText("gift1", "editor1"); 

            driver.FindElement(By.CssSelector("[data-e2e=\"CertFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"CertFrom\"]")).SendKeys("Meee");
            driver.FindElement(By.CssSelector("[data-e2e=\"CertTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"CertTo\"]")).SendKeys("Mee");
            driver.FindElement(By.CssSelector("[data-e2e=\"CertSum\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"CertSum\"]")).SendKeys("1000");
            driver.FindElement(By.CssSelector("[data-e2e=\"CertUsed\"]")).FindElement(By.TagName("span")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"CertEnabled\"]")).FindElement(By.TagName("span")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"CertPaid\"]")).FindElement(By.TagName("span")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("[data-e2e=\"CertSave\"]")).Click();
            Thread.Sleep(1000);
            //cheking grid
            Refresh();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Certificate1");
            DropFocus("h1");
            VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "grid CertificateCode");
            VerifyAreEqual("1 000 руб.", GetGridCell(0, "FullSum").Text, "grid FullSum");
            VerifyIsFalse(GetGridCell(0, "Paid").FindElement(By.CssSelector("input")).Selected, "grid Paid");
            VerifyIsFalse(GetGridCell(0, "Enable").FindElement(By.CssSelector("input")).Selected, "grid Enable");
            VerifyIsFalse(GetGridCell(0, "Used").FindElement(By.CssSelector(" input")).Selected, "grid Used");

            VerifyFinally(testname);
        }
        [Test]
        public void UseCertificate()
        {
            testname = "UseCertificate";
            VerifyBegin(testname);
            ProductToCard("1");
            GoToClient("cart");
            VerifyAreEqual("Скидка: 10%", driver.FindElement(By.CssSelector(".price-discount")).Text, "client discount");
            VerifyAreEqual("900 руб.", driver.FindElement(By.CssSelector(".cart-full-result-price")).Text, "client result price");
            driver.FindElement(By.CssSelector(".col-xs-8 input")).SendKeys("Certificate2");
            driver.FindElement(By.CssSelector(".btn-action.btn-expander")).Click();
            Thread.Sleep(1000);

            VerifyAreEqual("900 руб.", driver.FindElement(By.CssSelector(".cart-full-result-price")).Text, "client old cart price");
            GoToAdmin("certificates");
            GetGridFilter().SendKeys("Certificate2");
            DropFocus("h1");
            GetGridCell(0, "CertificateCode").FindElement(By.TagName("a")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"CertPaid\"]")).FindElement(By.TagName("span")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"CertSave\"]")).Click();
            Thread.Sleep(1000);

            GoToClient("cart");
            driver.FindElement(By.CssSelector(".col-xs-8 input")).SendKeys("Certificate2");
            driver.FindElement(By.CssSelector(".btn-action.btn-expander")).Click();
            Thread.Sleep(1000);

            VerifyAreEqual("Сертификат:", driver.FindElements(By.CssSelector(".cart-full-summary-name"))[1].Text, "client coupon");
            VerifyAreEqual("150 руб.", driver.FindElements(By.CssSelector(".cart-full-summary-price span"))[0].Text, "client sum coupon");
            VerifyAreEqual("750 руб.", driver.FindElement(By.CssSelector(".cart-full-result-price")).Text, "client rezult");

            driver.FindElement(By.CssSelector(".cart-full-summary-price a")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("900 руб.", driver.FindElement(By.CssSelector(".cart-full-result-price")).Text, "client rezult");

            ScrollTo(By.TagName("footer"));
            driver.FindElement(By.CssSelector(".btn.btn-middle.btn-submit")).Click();
            Thread.Sleep(5000);
            WaitForElem(By.CssSelector(".breads"));
            WaitForElem(By.Id("rightCell"));

            ScrollTo(By.TagName(".btn.btn-small.btn-action.btn-expander"));
            VerifyIsTrue(driver.FindElement(By.CssSelector(".checkout-result")).Text.Contains("900 руб."), "checkout rezult");
            driver.FindElement(By.CssSelector(".col-xs-8 input")).SendKeys("Certificate2");
            driver.FindElement(By.CssSelector(".btn.btn-small.btn-action.btn-expander")).Click();
            Thread.Sleep(1000);

            VerifyIsTrue(driver.FindElement(By.CssSelector(".checkout-result")).Text.Contains("750 руб."), "checkout afred cert rezult");
            driver.FindElements(By.CssSelector(".btn.btn-big.btn-submit"))[1].Click();
            Thread.Sleep(4000);

            VerifyFinally(testname);
        }
        public void ProductToCard(string id)
        {
            GoToClient("products/test-product" + id);
            ScrollTo(By.CssSelector(".rating"));
            driver.FindElement(By.CssSelector(".details-payment-inline a")).Click();
            Thread.Sleep(2000);

        }
    }
}
