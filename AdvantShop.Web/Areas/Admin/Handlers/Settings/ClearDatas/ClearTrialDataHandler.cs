using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Core.SQL;
using AdvantShop.Customers;
using AdvantShop.News;
using AdvantShop.Orders;
using AdvantShop.Payment;
using AdvantShop.Shipping;
using AdvantShop.Trial;

namespace AdvantShop.Web.Admin.Handlers.Settings.ClearDatas
{
    public class ClearTrialDataHandler
    {
        public void Execute()
        {
            foreach (var categoryId in CategoryService.GetAllCategoryIDs(true))
            {
                CategoryService.DeleteCategoryAndPhotos(categoryId);
            }

            foreach (var productId in ProductService.GetAllProductIDs(true))
            {
                ProductService.DeleteProduct(productId, false);
            }

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

            foreach (var brandId in BrandService.GetAllBrandIDs(true))
            {
                BrandService.DeleteBrand(brandId);
            }

            foreach (var tag in TagService.GetAllTags())
            {
                TagService.Delete(tag.Id);
            }

            foreach (var paymentId in PaymentService.GetAllPaymentMethodIDs())
            {
                PaymentService.DeletePaymentMethod(paymentId);
            }

            foreach (var shippingId in ShippingMethodService.GetAllShippingMethodIds())
            {
                ShippingMethodService.DeleteShippingMethod(shippingId);
            }

            foreach (var news in NewsService.GetNews())
            {
                NewsService.DeleteNews(news.NewsId);
            }

            foreach (var newsCategory in NewsService.GetNewsCategories())
            {
                NewsService.DeleteNewsCategory(newsCategory.NewsCategoryId);
            }

            foreach (var order in OrderService.GetAllOrders())
            {
                OrderService.DeleteOrder(order.OrderID);
            }

            SQLDataAccess2.ExecuteNonQuery("delete FROM [Order].[StatusHistory]");

            OrderService.ResetOrderID(1);

            foreach (var lead in LeadService.GetAllLeads())
            {
                LeadService.DeleteLead(lead.Id);
            }

            foreach (var task in ManagerTaskService.GeAllTasks())
            {
                ManagerTaskService.DeleteManagerTask(task.TaskId);
            }

            foreach (var call in CallService.GetAllCalls())
            {
                CallService.DeleteCall(call.Id);
            }

            foreach (var coupon in CouponService.GetAllCoupons())
            {
                CouponService.DeleteCoupon(coupon.CouponID);
            }

            foreach (var manager in ManagerService.GetManagersList(false))
            {
                if (!CustomerService.GetCustomer(manager.CustomerId).IsAdmin)
                {
                    ManagerService.DeleteManager(manager.ManagerId);
                }
            }

            foreach (var customer in CustomerService.GetCustomers())
            {
                if (!customer.IsAdmin)
                {
                    CustomerService.DeleteCustomer(customer.Id);
                }
            }

            CategoryService.RecalculateProductsCountManual();
            CategoryService.SetCategoryHierarchicallyEnabled(0);
            CacheManager.Clean();

            TrialService.TrackEvent(TrialEvents.DeleteTestData, string.Empty);
            TrialService.TrackEvent(ETrackEvent.Trial_ClearData);
        }
    }
}
