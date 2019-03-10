using AdvantShop.Localization;
using AdvantShop.Orders;
using AdvantShop.Web.Admin.Models.OrdersEdit;

namespace AdvantShop.Web.Admin.Handlers.Orders
{
    public class GetClientInfo
    {
        private readonly OrderModel _orderModel;

        public GetClientInfo(OrderModel orderModel)
        {
            _orderModel = orderModel;
        }

        public ClientInfoModel Execute()
        {
            var order = _orderModel.Order;

            var model = new ClientInfoModel()
            {
                CustomerGroup = order.GroupDiscountString,
            };

            if (_orderModel.Customer != null)
            {
                model.CustomerId = _orderModel.Customer.Id.ToString();
                model.RegistrationDate = Culture.ConvertDate(_orderModel.Customer.RegistrationDateTime);
                model.AdminCommentAboutCustomer = _orderModel.Customer.AdminComment;
                model.OrdersCount = OrderService.GetOrdersCountByCustomer(_orderModel.Customer.Id);
            }

            //if (_orderModel.BonusCard != null)
            //{
            //    model.Segment = _orderModel.BonusCard.CityName + ", " + (_orderModel.BonusCard.Gender ? "Женщины" : "Мужчины");
            //}

            return model;
        }
    }
}
