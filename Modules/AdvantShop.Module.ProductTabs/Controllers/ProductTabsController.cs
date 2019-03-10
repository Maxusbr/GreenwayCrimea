using System.Collections.Generic;
using System.Web.Mvc;
using AdvantShop.Module.ProductTabs.Domain;
using AdvantShop.Module.ProductTabs.Models;
using AdvantShop.Web.Infrastructure.Controllers;

namespace AdvantShop.Module.ProductTabs.Controllers
{
    public class ProductTabsController : ModuleController
    {
        public ActionResult AdminProductTab(int productId)
        {
            var model = new AdminProductTabsModel() {Tabs = new List<AdminProductTabModel>()};

            foreach (var tab in ProductTabsRepository.GetProductTabTitles())
            {
                var tabBody = ProductTabsRepository.GetProductTabBody(tab.TabTitleId, productId);

                var item = new AdminProductTabModel()
                {
                    ProductId = productId,
                    Active = tab.Active,
                    SortOrder = tab.SortOrder,

                    TabTitleId = tab.TabTitleId,
                    Title = tab.Title,

                    TabBodyId = tabBody != null ? tabBody.TabBodyId : 0,
                    Body = tabBody != null ? tabBody.Body : null
                };

                model.Tabs.Add(item);
            }

            return PartialView("~/Modules/ProductTabs/Views/AdminProductTab/Tab.cshtml", model);
        }
        
        [HttpPost]
        public ActionResult AdminProductTab(int productId, List<AdminProductTabModel> tabs)
        {
            if (tabs == null || tabs.Count == 0)
                return new EmptyResult();

            foreach (var tab in tabs)
            {
                var tabTitle = ProductTabsRepository.GetProductTabTitle(tab.TabTitleId);
                if (tabTitle == null)
                    continue;

                var body = tab.Body;
                
                if (string.IsNullOrEmpty(body))
                    ProductTabsRepository.DeleteProductTabBody(tab.ProductId, tabTitle.TabTitleId);
                else
                    ProductTabsRepository.AddUpdateProductTabBody(new TabBody
                    {
                        Body = body,
                        ProductId = tab.ProductId,
                        TabTitleId = tabTitle.TabTitleId
                    });
            }

            return AdminProductTab(productId);
        }
    }
}
