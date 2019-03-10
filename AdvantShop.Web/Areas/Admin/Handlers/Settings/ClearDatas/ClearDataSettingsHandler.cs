using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.CMS;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.CMS;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Core.SQL;
using AdvantShop.Customers;
using AdvantShop.FilePath;
using AdvantShop.News;
using AdvantShop.Orders;
using AdvantShop.Payment;
using AdvantShop.Shipping;
using AdvantShop.Trial;
using AdvantShop.Web.Admin.ViewModels.Settings;

namespace AdvantShop.Web.Admin.Handlers.Settings.ClearDatas
{
    public class ClearDataSettingsHandler
    {
        private readonly ClearDataViewModel _model;

        public ClearDataSettingsHandler(ClearDataViewModel model)
        {
            _model = model;
        }

        public void Execute()
        {
            if (!TrialService.IsTrialEnabled) return;
            
            if (_model.DeleteCategoties)
            {
                foreach (var categoryId in CategoryService.GetAllCategoryIDs(true))
                {
                    CategoryService.DeleteCategoryAndPhotos(categoryId);
                }
            }

            if (_model.DeleteProducts)
            {
                foreach (var productId in ProductService.GetAllProductIDs(true))
                {
                    ProductService.DeleteProduct(productId, false);
                }
            }

            if (_model.DeleteProperty)
            {
                foreach (var property in PropertyService.GetAllProperties())
                {
                    PropertyService.DeleteProperty(property.PropertyId);
                }

                foreach (var group in PropertyGroupService.GetList())
                {
                    PropertyGroupService.Delete(group.PropertyGroupId);
                }

                foreach (var color in ColorService.GetAllColors())
                {
                    ColorService.DeleteColor(color.ColorId);
                }

                foreach (var size in SizeService.GetAllSizes())
                {
                    SizeService.DeleteSize(size.SizeId);
                }

                foreach (var tag in TagService.GetAllTags())
                {
                    TagService.Delete(tag.Id);
                }
            }

            if (_model.DeleteBrands)
            {
                foreach (var brandId in BrandService.GetAllBrandIDs(true))
                {
                    BrandService.DeleteBrand(brandId);
                }
            }

            if (_model.DeletePayments)
            {
                foreach (var paymentId in PaymentService.GetAllPaymentMethodIDs())
                {
                    PaymentService.DeletePaymentMethod(paymentId);
                }
            }

            if (_model.DeleteShippings)
            {
                foreach (var shippingId in ShippingMethodService.GetAllShippingMethodIds())
                {
                    ShippingMethodService.DeleteShippingMethod(shippingId);
                }
            }

            if (_model.DeleteNews)
            {
                foreach (var news in NewsService.GetNews())
                {
                    NewsService.DeleteNews(news.NewsId);
                }

                foreach (var newsCategory in NewsService.GetNewsCategories())
                {
                    NewsService.DeleteNewsCategory(newsCategory.NewsCategoryId);
                }
            }

            if (_model.DeleteOrder)
            {
                foreach (var order in OrderService.GetAllOrders())
                {
                    OrderService.DeleteOrder(order.OrderID);
                }
                
                SQLDataAccess2.ExecuteNonQuery("delete FROM [Order].[StatusHistory]");

                OrderService.ResetOrderID(1);

                foreach (var coupon in CouponService.GetAllCoupons())
                {
                    CouponService.DeleteCoupon(coupon.CouponID);
                }
            }
            if (_model.DeleteCrm)
            {
                foreach (var lead in LeadService.GetAllLeads())
                {
                    LeadService.DeleteLead(lead.Id);
                }

                foreach (var call in CallService.GetAllCalls())
                {
                    CallService.DeleteCall(call.Id);
                }
            }

            if (_model.DeleteTasks)
            {
                foreach (var task in ManagerTaskService.GeAllTasks())
                {
                    ManagerTaskService.DeleteManagerTask(task.TaskId);
                }
                foreach (var task in TaskService.GetAllTasks())
                {
                    TaskService.DeleteTask(task.Id);
                }

                foreach (var task in TaskGroupService.GetAllTaskGroups())
                {
                    TaskGroupService.DeleteTaskGroup(task.Id);
                }

            }
            
            if (_model.DeleteUsers)
            {
                foreach (var manager in ManagerService.GetManagersList(false))
                {
                    if (!CustomerService.GetCustomer(manager.CustomerId).IsAdmin)
                    {
                        ManagerService.DeleteManager(manager.ManagerId);
                    }
                }
            }

            if (_model.DeleteCustomers)
            {
                foreach (var customer in CustomerService.GetCustomers())
                {
                    if (!customer.IsAdmin)
                    {
                        var m = ManagerService.GetManager(customer.Id);
                        if (m != null)
                        {
                            var tsk = TaskService.GetTaskByAssignedManagerId(m.ManagerId);
                            foreach (var t in tsk)
                            {
                                t.AssignedManagerId = null;
                                TaskService.UpdateTask(t);
                            }

                            var t1sk = TaskService.GetTaskByAppointedManagerId(m.ManagerId);
                            foreach (var t in t1sk)
                            {
                                t.AppointedManagerId = null;
                                TaskService.UpdateTask(t);
                            }
                        }
                     
                        CustomerService.DeleteCustomer(customer.Id);
                    }
                }
            }

            if (_model.DeleteCarosel)
            {
                foreach (var carousel in CarouselService.GetAllCarousels())
                {
                    CarouselService.DeleteCarousel(carousel.CarouselId);
                }
            }

            if (_model.DeleteSubscription)
            {

            }
            if (_model.DeleteMenu)
            {
                DeleteMenus(EMenuType.Bottom);
                DeleteMenus(EMenuType.Mobile);
                DeleteMenus(EMenuType.Top);
            }

            if (_model.DeletePage)
            {
                foreach (var page in StaticPageService.GetAllStaticPages())
                {
                    StaticPageService.DeleteStaticPage(page.ID);
                }
            }

            CategoryService.RecalculateProductsCountManual();
            CategoryService.SetCategoryHierarchicallyEnabled(0);
            CacheManager.Clean();

            TrialService.TrackEvent(TrialEvents.DeleteTestData, string.Empty);
            TrialService.TrackEvent(ETrackEvent.Trial_ClearData);
        }

        private static void DeleteMenus(EMenuType type)
        {
            foreach (var menuItem in MenuService.GetChildMenuItemsByParentId(0, type))
            {
                foreach (var menuChild in MenuService.GetChildMenuItemsByParentId(0, type))
                {
                    MenuService.DeleteMenuItemIconById(menuChild.MenuItemID, FoldersHelper.GetPathAbsolut(FolderType.MenuIcons, menuChild.MenuItemIcon));
                    MenuService.DeleteMenuItemById(menuChild.MenuItemID);
                }
                MenuService.DeleteMenuItemIconById(menuItem.MenuItemID, FoldersHelper.GetPathAbsolut(FolderType.MenuIcons, menuItem.MenuItemIcon));
                MenuService.DeleteMenuItemById(menuItem.MenuItemID);
            }
        }
    }
}
