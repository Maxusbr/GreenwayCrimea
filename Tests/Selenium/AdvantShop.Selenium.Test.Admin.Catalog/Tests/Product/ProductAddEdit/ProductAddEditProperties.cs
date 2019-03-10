using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Catalog.ProductAddEdit.Properties
{
    [TestFixture]
    public class ProductAddEditProperties : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.ProductCategories.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Brand.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Tag.csv",
           "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Property.csv",
                 "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.PropertyValue.csv",
                 "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.ProductPropertyValue.csv",
                 "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.PropertyGroup.csv",
                 "Data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Color.csv",
                 "data\\Admin\\Catalog\\ProductAddEdit\\Catalog.Size.csv"
           );

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
        public void ProductEditAddProperty()
        {
            GoToAdmin("product/edit/1");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'Свойства товара')]")).Click();
            Thread.Sleep(3000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"selectProperty\"]")))).SelectByText("Property2");
            Thread.Sleep(2000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"selectPropertyValue\"]")))).SelectByText("PropertyValue12");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"propertyAddSelected\"]")).Click();
            Thread.Sleep(2000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"selectProperty\"]")))).SelectByText("Property2");
            Thread.Sleep(2000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"selectPropertyValue\"]")))).SelectByText("PropertyValue13");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"propertyAddSelected\"]")).Click();
            Thread.Sleep(2000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"selectProperty\"]")))).SelectByText("Property3");
            Thread.Sleep(2000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"selectPropertyValue\"]")))).SelectByText("PropertyValue22");
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("[data-e2e=\"propertyAddSelected\"]")).Click();
            Thread.Sleep(2000);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"selectProperty\"]")))).SelectByText("Property10");
            Thread.Sleep(2000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"selectPropertyValue\"]")))).SelectByText("PropertyValue55");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"propertyAddSelected\"]")).Click();
            Thread.Sleep(2000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"selectProperty\"]")))).SelectByText("Property11");
            Thread.Sleep(2000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"selectPropertyValue\"]")))).SelectByText("PropertyValue60");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"propertyAddSelected\"]")).Click();
            Thread.Sleep(2000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"selectProperty\"]")))).SelectByText("Property20");
            Thread.Sleep(2000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"selectPropertyValue\"]")))).SelectByText("PropertyValue80");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"propertyAddSelected\"]")).Click();
            Thread.Sleep(3000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".properties-group-name.cs-t-5")).Text.Contains("PropertyGroup1"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-group-name.cs-t-5"))[1].Text.Contains("PropertyGroup2"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-group-name.cs-t-5"))[2].Text.Contains("PropertyGroup3"));

            Assert.IsTrue(driver.FindElement(By.CssSelector(".properties-item-name")).Text.Contains("Property1"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-item-name"))[1].Text.Contains("Property2"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-item-name"))[2].Text.Contains("Property3"));

            Assert.IsTrue(driver.FindElement(By.CssSelector(".properties-item-value")).Text.Contains("PropertyValue2"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-item-value"))[1].Text.Contains("PropertyValue12"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-item-value"))[1].Text.Contains("PropertyValue13"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-item-value"))[2].Text.Contains("PropertyValue22"));
            
            GoToClient("products/test-product1#?tab=tabOptions");
            Thread.Sleep(3000);
            ScrollTo(By.Id("tabDescription"));

            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-group-name.cs-t-5"))[0].Text.Contains("PropertyGroup1"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-group-name.cs-t-5"))[1].Text.Contains("PropertyGroup2"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-group-name.cs-t-5"))[2].Text.Contains("PropertyGroup3"));
           
            ScrollTo(By.Id("tabDescription"));
            Assert.AreEqual("Property1",driver.FindElements(By.CssSelector(".properties-item-name"))[0].Text);
            Assert.AreEqual("Property2", driver.FindElements(By.CssSelector(".properties-item-name"))[1].Text);
           // Assert.AreEqual("Property2", driver.FindElements(By.CssSelector(".properties-item-name"))[2].Text);
            Assert.AreEqual("Property3", driver.FindElements(By.CssSelector(".properties-item-name"))[2].Text);
            Assert.AreEqual("Property10", driver.FindElements(By.CssSelector(".properties-item-name"))[3].Text);
            Assert.AreEqual("Property11", driver.FindElements(By.CssSelector(".properties-item-name"))[4].Text);
            Assert.AreEqual("Property20", driver.FindElements(By.CssSelector(".properties-item-name"))[5].Text);

            Assert.AreEqual("PropertyValue2", driver.FindElements(By.CssSelector(".properties-item-value"))[0].Text);
            Assert.AreEqual("PropertyValue12, PropertyValue13", driver.FindElements(By.CssSelector(".properties-item-value"))[1].Text);
          //  Assert.AreEqual("PropertyValue13", driver.FindElements(By.CssSelector(".properties-item-value"))[2].Text);
            Assert.AreEqual("PropertyValue22", driver.FindElements(By.CssSelector(".properties-item-value"))[2].Text);
            Assert.AreEqual("PropertyValue55", driver.FindElements(By.CssSelector(".properties-item-value"))[3].Text);
            Assert.AreEqual("PropertyValue60", driver.FindElements(By.CssSelector(".properties-item-value"))[4].Text);
            Assert.AreEqual("PropertyValue80", driver.FindElements(By.CssSelector(".properties-item-value"))[5].Text);

        }

        [Test]
        public void ProductEditAddNewProperty()
        {
            GoToAdmin("product/edit/5");
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//div[contains(text(), 'Свойства товара')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector("[data-e2e=\"propertyAdd\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"inputPropertyAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"inputPropertyAdd\"]")).SendKeys("Новое свойство");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"inputPropertyValueAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"inputPropertyValueAdd\"]")).SendKeys("Новое значение свойства");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"propertyBtnAdd\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.PageSource.Contains("Новое значение свойства"));
            Assert.IsTrue(driver.PageSource.Contains("Новое свойство"));

            Assert.IsTrue(driver.FindElement(By.CssSelector(".properties-item-name")).Text.Contains("Property1"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-item-name"))[1].Text.Contains("Новое свойство"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".properties-item-value")).Text.Contains("PropertyValue6"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-item-value"))[1].Text.Contains("Новое значение свойства"));

            GoToAdmin("properties");
            Assert.IsTrue(driver.PageSource.Contains("Новое свойство"));
           
            GoToClient("products/test-product5#?tab=tabOptions");
            Thread.Sleep(3000);
            ScrollTo(By.Id("tabDescription"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-group-name.cs-t-5"))[0].Text.Contains("PropertyGroup1"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-group-name.cs-t-5"))[1].Text.Contains("Прочее"));
                      
            Assert.AreEqual("Property1",driver.FindElements(By.CssSelector(".properties-item-name"))[0].Text);
            Assert.AreEqual("Новое свойство",driver.FindElements(By.CssSelector(".properties-item-name"))[1].Text);

            Assert.AreEqual("PropertyValue6",driver.FindElements(By.CssSelector(".properties-item-value"))[0].Text);
            Assert.AreEqual("Новое значение свойства",driver.FindElements(By.CssSelector(".properties-item-value"))[1].Text);
        }
        
        [Test]
        public void ProductEditAddNewPropertyValue()
        {
            GoToAdmin("product/edit/7");
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//div[contains(text(), 'Свойства товара')]")).Click();
            Thread.Sleep(3000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"selectProperty\"]")))).SelectByText("Property100");
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"propertyValueAdd\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"inputPropertyValueAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"inputPropertyValueAdd\"]")).SendKeys("Новое значение свойства2");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"propertyBtnAdd\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".properties-group-name.cs-t-5")).Text.Contains("PropertyGroup1"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-group-name.cs-t-5"))[1].Text.Contains("Прочее"));

            Assert.IsTrue(driver.FindElement(By.CssSelector(".properties-item-name")).Text.Contains("Property1"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-item-name"))[1].Text.Contains("Property100"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".properties-item-value")).Text.Contains("PropertyValue8"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-item-value"))[1].Text.Contains("Новое значение свойства2"));

            GoToAdmin("propertyValues?propertyId=100");
            Assert.AreEqual("Новое значение свойства2", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"));

            GoToClient("products/test-product7#?tab=tabOptions");
            Thread.Sleep(3000);
            ScrollTo(By.Id("tabDescription"));

            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-group-name.cs-t-5"))[0].Text.Contains("PropertyGroup1"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-group-name.cs-t-5"))[1].Text.Contains("Прочее"));

            ScrollTo(By.Id("tabDescription"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-item-name"))[0].GetAttribute("innerText").Contains("Property1"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-item-name"))[1].GetAttribute("innerText").Contains("Property100"));

            Assert.AreEqual("PropertyValue8", driver.FindElements(By.CssSelector(".properties-item-value"))[0].Text);
            Assert.AreEqual("Новое значение свойства2", driver.FindElements(By.CssSelector(".properties-item-value"))[1].Text);
        }

        [Test]
        public void ProductEditAddDelProperty()
        {
            GoToAdmin("product/edit/6");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[contains(text(), 'Свойства товара')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector("[data-e2e=\"propertyAdd\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"inputPropertyAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"inputPropertyAdd\"]")).SendKeys("Новое свойство0");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"inputPropertyValueAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"inputPropertyValueAdd\"]")).SendKeys("Новое значение свойства0");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"propertyBtnAdd\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"propertyAdd\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"inputPropertyAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"inputPropertyAdd\"]")).SendKeys("Новое свойство01");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"inputPropertyValueAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"inputPropertyValueAdd\"]")).SendKeys("Новое значение свойства01");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"propertyBtnAdd\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"propertyAdd\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"inputPropertyAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"inputPropertyAdd\"]")).SendKeys("Новое свойство02");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"inputPropertyValueAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"inputPropertyValueAdd\"]")).SendKeys("Новое значение свойства02");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"propertyBtnAdd\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"propertyAdd\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"inputPropertyAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"inputPropertyAdd\"]")).SendKeys("Новое свойство03");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"inputPropertyValueAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"inputPropertyValueAdd\"]")).SendKeys("Новое значение свойства03");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"propertyBtnAdd\"]")).Click();
            Thread.Sleep(4000);

            Assert.IsTrue(driver.FindElement(By.CssSelector(".properties-item-name")).Text.Contains("Property1"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-item-name"))[1].Text.Contains("Новое свойство0"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-item-name"))[2].Text.Contains("Новое свойство01"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-item-name"))[3].Text.Contains("Новое свойство02"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-item-name"))[4].Text.Contains("Новое свойство03"));

            Assert.IsTrue(driver.FindElement(By.CssSelector(".properties-item-value")).Text.Contains("PropertyValue7"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-item-value"))[1].Text.Contains("Новое значение свойства0"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-item-value"))[2].Text.Contains("Новое значение свойства01"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-item-value"))[3].Text.Contains("Новое значение свойства02"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-item-value"))[4].Text.Contains("Новое значение свойства03"));

            driver.FindElements(By.CssSelector(".close.ui-select-match-close"))[3].Click();
            Thread.Sleep(3000);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-item-value"))[3].Text.Contains("Новое значение свойства03"));

            GoToAdmin("properties");
            Thread.Sleep(1000);
            Assert.AreEqual("Новое свойство0", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Новое свойство01", GetGridCell(1, "Name").Text);
            Assert.AreEqual("Новое свойство02", GetGridCell(2, "Name").Text);
            Assert.AreEqual("Новое свойство03", GetGridCell(3, "Name").Text);

            GetGridCell(0, "GroupName").Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Новое значение свойства0", GetGridCell(0, "Value").FindElement(By.TagName("input")).GetAttribute("value"));

            GoToClient("products/test-product6#?tab=tabOptions");
            Thread.Sleep(3000);
            ScrollTo(By.Id("tabDescription"));

            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-group-name.cs-t-5"))[0].Text.Contains("PropertyGroup1"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-group-name.cs-t-5"))[1].Text.Contains("Прочее"));

            ScrollTo(By.Id("tabDescription"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-item-name"))[0].GetAttribute("innerText").Contains("Property1"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-item-name"))[1].GetAttribute("innerText").Contains("Новое свойство0"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-item-name"))[2].GetAttribute("innerText").Contains("Новое свойство01"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-item-name"))[3].GetAttribute("innerText").Contains("Новое свойство03"));
            
            Assert.AreEqual("PropertyValue7", driver.FindElements(By.CssSelector(".properties-item-value"))[0].Text);
            Assert.AreEqual("Новое значение свойства0", driver.FindElements(By.CssSelector(".properties-item-value"))[1].Text);
            Assert.AreEqual("Новое значение свойства01",driver.FindElements(By.CssSelector(".properties-item-value"))[2].Text);
            Assert.AreEqual("Новое значение свойства03", driver.FindElements(By.CssSelector(".properties-item-value"))[3].Text);
            
        }

        [Test]
        public void ProductEditDelAllProperty()
        {
            GoToAdmin("product/edit/4");
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//div[contains(text(), 'Свойства товара')]")).Click();
            Thread.Sleep(3000);

            driver.FindElement(By.CssSelector("[data-e2e=\"propertyAdd\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"inputPropertyAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"inputPropertyAdd\"]")).SendKeys("Новое свойство1");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"inputPropertyValueAdd\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"inputPropertyValueAdd\"]")).SendKeys("Новое значение свойства1");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"propertyBtnAdd\"]")).Click();
            Thread.Sleep(2000);
            
            Assert.IsTrue(driver.FindElement(By.CssSelector(".properties-item-name")).Text.Contains("Property1"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-item-name"))[1].Text.Contains("Новое свойство1"));
           
            Assert.IsTrue(driver.FindElement(By.CssSelector(".properties-item-value")).Text.Contains("PropertyValue5"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".properties-item-value"))[1].Text.Contains("Новое значение свойства1"));
           
            driver.FindElements(By.CssSelector(".close.ui-select-match-close"))[0].Click();
            Thread.Sleep(3000);
            driver.FindElements(By.CssSelector(".close.ui-select-match-close"))[0].Click();
            Thread.Sleep(3000);
            Assert.AreEqual(0, driver.FindElements(By.CssSelector(".properties-item-name")).Count);
            Assert.AreEqual(0, driver.FindElements(By.CssSelector(".properties-item-value")).Count);

            GoToClient("products/test-product4#?tab=tabOptions");
            ScrollTo(By.Id("tabDescription"));
            Assert.IsFalse(driver.PageSource.Contains("PropertyGroup1"));
            Assert.IsFalse(driver.PageSource.Contains("Прочее"));
            Assert.IsFalse(driver.PageSource.Contains("Новое значение свойства1"));
            Assert.IsFalse(driver.PageSource.Contains("Новое свойство1"));
            Assert.IsFalse(driver.PageSource.Contains("Property1"));
            Assert.IsFalse(driver.PageSource.Contains("PropertyValue5"));
        }
 
    }
}
