using System.Linq;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.Services.Bonuses.Service;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.Web.Infrastructure.Handlers;

namespace AdvantShop.Web.Admin.Handlers.Orders
{
    public class UpdateOrderTotal : AbstractCommandHandler<bool>
    {
        private readonly float? _useBonusesAmount;
        private readonly Order _order;

        public UpdateOrderTotal(Order order)
        {
            _order = order;
        }

        public UpdateOrderTotal(Order order, float? useBonusesAmount)
        {
            _order = order;
            _useBonusesAmount = useBonusesAmount;
        }

        public UpdateOrderTotal(int orderId, float? useBonusesAmount)
        {
            _order = OrderService.GetOrder(orderId);
            _useBonusesAmount = useBonusesAmount;
        }

        protected override bool Handle()
        {
            if (_order == null || _order.Payed)
                return false;
            
            if (BonusSystem.IsActive)
            {
                var bonusCard = _order.GetOrderBonusCard();

                if (bonusCard != null && !bonusCard.Blocked)
                { 
                    _order.BonusCardNumber = bonusCard.CardNumber;

                    var purchase = PurchaseService.GetByOrderId(_order.OrderID);
                

                    var totalPrice = _order.OrderItems.Sum(oi => oi.Price * oi.Amount);
                    var totalDiscount = _order.TotalDiscount;// + _order.BonusCost;

                    var productsPrice = totalPrice - totalDiscount;
                    var price = productsPrice -
                                _order.OrderItems.Where(x => !x.AccrueBonuses).Sum(x => (x.Price - x.Price / totalPrice * totalDiscount) * x.Amount);

                    var bonusCostChanged = false;
                    var bonusCost = _order.BonusCost;

                    if (_useBonusesAmount != null && BonusSystemService.CanChangeBonusAmount(_order, bonusCard, purchase))
                    {
                        var bonusTotalAmount = (float) (bonusCard.BonusesTotalAmount +
                                                        (purchase != null ? purchase.MainBonusAmount + purchase.AdditionBonusAmount : 0));

                        var amount =
                            _useBonusesAmount.Value > bonusTotalAmount
                                ? bonusTotalAmount
                                : _useBonusesAmount.Value;

                        _order.BonusCost = BonusSystemService.GetBonusCost(productsPrice + _order.ShippingCost, productsPrice, amount);

                        bonusCostChanged = bonusCost != _order.BonusCost;
                    }

                    var sumPrice = BonusSystem.BonusType == EBonusType.ByProductsCostWithShipping
                            ? price + _order.ShippingCost
                            : price;

                    if (sumPrice < 0)
                        sumPrice = 0;

                    if (purchase == null)
                    {
                        BonusSystemService.MakeBonusPurchase(_order.BonusCardNumber.Value, (decimal) totalPrice, (decimal) sumPrice, _order);
                    }
                    else if (purchase.PurchaseAmount != (decimal) sumPrice || bonusCostChanged)
                    {
                        BonusSystemService.UpdatePurchase(_order.BonusCardNumber.Value, (decimal) totalPrice, (decimal) sumPrice,  _order);
                    }
                }
            }


            var trackChanges = !_order.IsDraft;
            var changedBy = new OrderChangedBy(CustomerContext.CurrentCustomer);

            OrderService.UpdateOrderMain(_order, changedBy: changedBy, trackChanges: trackChanges);
            OrderService.RefreshTotal(_order, changedBy: changedBy, ignoreHistory: !trackChanges);

            return true;
        }
    }
}
