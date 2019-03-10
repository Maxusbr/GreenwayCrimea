using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Bonuses.Model;
using AdvantShop.Core.Services.Bonuses.Model.Enums;
using AdvantShop.Core.Services.Bonuses.Model.Rules;
using AdvantShop.Core.Services.Bonuses.Service;
using AdvantShop.Core.Services.Bonuses.Sms;
using AdvantShop.Core.Services.Bonuses.Sms.Template;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Customers;
using AdvantShop.Orders;
using Transaction = AdvantShop.Core.Services.Bonuses.Model.Transaction;

namespace AdvantShop.Core.Services.Bonuses
{
    public class BonusSystemService
    {
        private const string BonusFirstPercentCacheKey = "BonusSystem.BonusFirstPercent";
        private const string BonusGradesCacheKey = "BonusSystem.BonusGrades";

        #region Bonus card api methods

        public static Card GetCard(long? cardId)
        {
            if (cardId == null)
                return null;

            return GetCard(cardId.Value);
        }

        public static Card GetCard(long cardnumber)
        {
            return CardService.Get(cardnumber);
        }

        public static Card GetCardByPhone(string phone)
        {
            if (phone.IsNullOrEmpty())
                return null;

            var card = CardService.GetByPhone(phone);

            return card;
        }

        public static Card GetCard(Guid? customerId)
        {
            if (customerId == null || customerId == Guid.Empty)
                return null;

            var card = CardService.Get(customerId.Value);

            return card;
        }

        public static long AddCard(Card card)
        {
            card.CardNumber = GenerateCardNumber(card.CardNumber);
            card.GradeId = BonusSystem.DefaultGrade;
            card.CreateOn = DateTime.Now;
            CardService.Add(card);           
            return card.CardNumber;
        }

        public static long GenerateCardNumber(long cardNumber)
        {
            if (cardNumber != 0) return cardNumber;
            var count = 0;
            var from = BonusSystem.CardFrom;
            var to = BonusSystem.CardTo;

            cardNumber = GetRandom(from, to);
            while (CardService.Get(cardNumber) != null)
            {
                cardNumber = GetRandom(from, to);
                count++;
                if (count == 50)
                {
                    throw new BlException(LocalizationService.GetResource("Admin.Cards.AddUpdateCard.Error.CanNotGenerate"), "CardNumber");
                }
            }
            return cardNumber;
        }


        public static bool MakeBonusPurchase(long cardNumber, Order order)
        {
            var totalPrice = order.OrderItems.Sum(x => x.Price * x.Amount);
            var totalDiscount = order.TotalDiscount;

            var productsPrice = totalPrice - totalDiscount;
            var price = productsPrice -
                        order.OrderItems.Where(x => !x.AccrueBonuses)
                            .Sum(x => (x.Price - x.Price / totalPrice * totalDiscount) * x.Amount);

            var sumPrice = BonusSystem.BonusType == EBonusType.ByProductsCostWithShipping
                    ? price + order.ShippingCost
                    : price;

            if (sumPrice < 0)
                return false;

            return MakeBonusPurchase(cardNumber, (decimal)price, (decimal)sumPrice, order);
        }

        public static bool MakeBonusPurchase(long cardNumber, ShoppingCart cart, float shippingPrice, Order order)
        {
            var totalPrice = cart.TotalPrice;
            var totalDiscount = cart.TotalDiscount;

            var productsPrice = totalPrice - totalDiscount;
            var price = productsPrice -
                        cart.Where(x => !x.Offer.Product.AccrueBonuses)
                            .Sum(x => (x.PriceWithDiscount - x.PriceWithDiscount / totalPrice * totalDiscount) * x.Amount);


            var sumPrice = BonusSystem.BonusType == EBonusType.ByProductsCostWithShipping
                    ? price + shippingPrice
                    : price;

            if (sumPrice < 0)
                return false;
            
            return MakeBonusPurchase(cardNumber, (decimal)price, (decimal)sumPrice, order);
        }


        /// <summary>
        /// Списание бонусов
        /// </summary>
        /// <param name="cardNumber">Номер карты</param>
        /// <param name="purchaseFullAmount"></param>
        /// <param name="purchaseAmount">Сумма, из которой расчитывались бонусы</param>
        /// <param name="order">Заказ</param>
        public static bool MakeBonusPurchase(long cardNumber, decimal purchaseFullAmount, decimal purchaseAmount, Order order)
        {
            // Сколько бонусов списать
            var bonusAmount = (decimal) order.BonusCost;

            using (var scope = new TransactionScope())
            {
                //Нельзя списать бонусов больше чем сумма продажи
                if (Math.Round((float)purchaseFullAmount, 2) < Math.Round((float)bonusAmount, 2))
                    return false;

                var card = CardService.Get(cardNumber);
                //Этот номер карты не существует
                if (card == null)
                    return false;

                //Карта заблокирована
                if (card.Blocked)
                    return false;

                var p = PurchaseService.GetByOrderId(order.OrderID);
                //Продажа с таким номером заказа уже существует
                if (p != null)
                    return false;
                
                //Нельзя списать больше чем имеется бонусов
                var tmpSum = AdditionBonusService.ActualSum(card.CardId);
                var balance = card.BonusAmount + tmpSum;
                if (Math.Round((float)balance, 2) < Math.Round((float)bonusAmount, 2))
                    return false;

                var comment = "Заказ № " + (BonusSystem.UseOrderId ? order.OrderID.ToString() : order.Number) + " в магазине " +
                              SettingsMain.SiteUrlPlain;

                var purchase = new Purchase
                {
                    CardId = card.CardId,
                    CreateOn = DateTime.Now,
                    CreateOnCut = DateTime.Now,
                    PurchaseAmount = purchaseAmount,
                    PurchaseFullAmount = purchaseFullAmount,
                    CashAmount = 0,
                    MainBonusAmount = 0,
                    AdditionBonusAmount = 0,
                    NewBonusAmount = 0,
                    MainBonusBalance = card.BonusAmount,
                    AdditionBonusBalance = tmpSum,
                    Comment = comment,
                    Status = EPuchaseState.Hold,
                    OrderId = order.OrderID
                };
                purchase.Id = PurchaseService.Add(purchase);

                var addBonuses = AdditionBonusService.Actual(card.CardId).OrderBy(x => x.EndDate);
                var tempBonusAmount = bonusAmount;
                foreach (var addbonus in addBonuses)
                {
                    decimal substractsum;
                    if (tempBonusAmount <= 0) break;
                    if (addbonus.Amount <= tempBonusAmount)
                    {
                        substractsum = addbonus.Amount;
                        tempBonusAmount -= substractsum;
                        addbonus.Status = EAdditionBonusStatus.Remove;
                        AdditionBonusService.Update(addbonus);
                    }
                    else
                    {
                        substractsum = tempBonusAmount;
                        addbonus.Amount -= substractsum;
                        tempBonusAmount = 0;
                        addbonus.Status = EAdditionBonusStatus.Substract;
                        AdditionBonusService.Update(addbonus);
                    }
                    tmpSum -= substractsum;
                    purchase.AdditionBonusAmount += substractsum;

                    var transLog = Transaction.Factory(card.CardId, substractsum, comment,
                        EOperationType.SubtractAdditionBonus, tmpSum, purchase.Id, addbonus.Id);
                    TransactionService.Create(transLog);
                }

                if (tempBonusAmount > 0)
                {
                    decimal substractsum;
                    if (card.BonusAmount <= tempBonusAmount)
                    {
                        substractsum = card.BonusAmount;
                        card.BonusAmount = 0;
                    }
                    else
                    {
                        substractsum = tempBonusAmount;
                        card.BonusAmount -= substractsum;
                    }
                    CardService.Update(card);

                    purchase.MainBonusAmount = substractsum;
                    tempBonusAmount -= substractsum;

                    var purchaseId = purchase.Id;
                    var transLog = Transaction.Factory(card.CardId, substractsum, comment, EOperationType.SubtractMainBonus, card.BonusAmount, purchaseId);
                    TransactionService.Create(transLog);
                }

                purchase.MainBonusBalance = card.BonusAmount;
                purchase.AdditionBonusBalance = tmpSum;

                purchase.CashAmount = purchaseAmount - bonusAmount + tempBonusAmount;
                if (purchase.CashAmount < 0)
                    purchase.CashAmount = 0;

                var newBonusAmount = card.Grade.BonusPercent * purchase.CashAmount / 100;
                purchase.NewBonusAmount = newBonusAmount;

                PurchaseService.Update(purchase);
                scope.Complete();
            }
            return true;
        }

        /// <summary>
        /// Можно ли менять кол-во бонусов у заказа?
        /// </summary>
        public static bool CanChangeBonusAmount(Order order, Card card, Purchase purchase)
        {
            // Если заказ не оплачен и есть бонусная карта и у продажи статус != завершена

            return !order.Payed &&
                   card != null && !card.Blocked && card.BonusesTotalAmount > 0 &&
                   (purchase == null || purchase.Status != EPuchaseState.Complete);
        }

        /// <summary>
        /// Запрос смс кода по номеру карты
        /// </summary>
        /// <param name="cardNumber">Номер карты</param>
        /// <returns></returns>
        //public static int GetSmsCode(long cardNumber)
        //{
        //    var card = CardService.Get(cardNumber);
        //    var smsCode = GenerateDigit(6);
        //    if (card != null)
        //    {
        //        var customer = CustomerService.GetCustomer(card.CardId);
        //        if (customer != null && customer.StandardPhone.HasValue)
        //            SmsService.Process(customer.StandardPhone.Value, ESmsType.OnSmsCode, new OnSmsCodeTempalte { Code = smsCode });
        //    }
        //    return smsCode;
        //}

        public static int GenerateDigit(int size)
        {
            var rnd = new Random();
            return rnd.Next(1, (int)Math.Pow(10, size));
        }

        /// <summary>
        /// Проверка занят ли телефон
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsPhoneExist(string phone)
        {
            var temp = CustomerService.GetCustomersByPhone(phone);

            return temp.Any();
        }

        /// <summary>
        /// Подтверждаем, что заказ оплачен
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="orderNumber"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static bool Confirm(long? cardNumber, string orderNumber, int orderId)
        {
            var p = PurchaseService.GetByOrderId(orderId);
            if (p == null) return false;
            var card = CardService.Get(p.CardId);
            using (TransactionScope scope = new TransactionScope())
            {
                if (p.Status != EPuchaseState.Hold) return false;
                p.Status = EPuchaseState.Complete;
                card.BonusAmount += p.NewBonusAmount;
                PurchaseService.Update(p);
                CardService.Update(card);
                var tranLog = Transaction.Factory(p.CardId, p.NewBonusAmount, p.Comment, EOperationType.AddMainBonus, card.BonusAmount, p.Id);
                TransactionService.Create(tranLog);
                scope.Complete();
            }
            new ChangeGradeRule().Execute(p.CardId);
            var addBonuses = AdditionBonusService.ActualSum(p.CardId);
            var customer = CustomerService.GetCustomer(card.CardId);
            if (customer != null && customer.StandardPhone.HasValue)
            {
                SmsService.Process(customer.StandardPhone.Value, ESmsType.OnPurchase, new OnPurchaseTempalte
                {
                    CompanyName = SettingsMain.ShopName,
                    PurchaseFull = p.PurchaseFullAmount,
                    Purchase = p.PurchaseAmount,
                    UsedBonus = (p.AdditionBonusAmount + p.MainBonusAmount),
                    AddBonus = p.NewBonusAmount,
                    Balance = (card.BonusAmount + addBonuses)
                });
            }
            return true;
        }

        /// <summary>
        /// Получить продажу
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="orderNumber"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static Purchase GetPurchase(long? cardNumber, string orderNumber, int orderId)
        {
            var p = PurchaseService.GetByOrderId(orderId);
            return p;
        }

        /// <summary>
        /// Отмена продажи
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="orderNumber"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static void CancelPurchase(long? cardNumber, string orderNumber, int orderId)
        {
            var p = PurchaseService.Get(orderId);
            //Продажа не найдена
            if (p == null) return;
            //Отмена продажи возможна только в статусе ожидание
            if (p.Status != EPuchaseState.Hold) return;
            PurchaseService.RollBack(p);
        }

        /// <summary>
        /// Обновление продажи при редатировании заказа
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="fullsum"></param>
        /// <param name="sum"></param>
        public static void UpdatePurchase(long cardNumber, decimal fullsum, decimal sum, Order order)
        {
            var purchase = PurchaseService.GetByOrderId(order.OrderID);
            PurchaseService.RollBack(purchase);
            MakeBonusPurchase(cardNumber, fullsum, sum, order);
        }

        /// <summary>
        /// Процент бонусов по умолчанию
        /// </summary>
        /// <returns></returns>
        public static decimal GetBonusDefaultPercent()
        {
            if (CacheManager.Contains(BonusFirstPercentCacheKey))
                return CacheManager.Get<decimal>(BonusFirstPercentCacheKey);

            var grade = GradeService.Get(BonusSystem.DefaultGrade);

            var percent = grade.BonusPercent;

            CacheManager.Insert(BonusFirstPercentCacheKey, percent);
            return percent;
        }

        /// <summary>
        /// Список грейдов компании
        /// </summary>
        public static List<Grade> GetGrades()
        {
            if (CacheManager.Contains(BonusGradesCacheKey))
                return CacheManager.Get<List<Grade>>(BonusGradesCacheKey);

            var grades = GradeService.GetAll();

            CacheManager.Insert(BonusGradesCacheKey, grades, 2);
            return grades;
        }


        #endregion

        /// <summary>
        /// Расчет стоимости бонуса
        /// </summary>
        /// <param name="totaOrderlPrice">Стоимость товаров со скидками и доставкой</param>
        /// <param name="productsPrice">Стоимость товаров со скидками </param>
        /// <param name="bonusAmount">Бонусы</param>
        /// <returns></returns>
        public static float GetBonusCost(float totaOrderlPrice, float productsPrice, float bonusAmount)
        {
            var sumPrice = BonusSystem.BonusType == EBonusType.ByProductsCostWithShipping
                    ? totaOrderlPrice
                    : productsPrice;

            var bonusPrice = sumPrice > bonusAmount ? bonusAmount : sumPrice;

            if (BonusSystem.MaxOrderPercent == 100 || (bonusPrice * 100 / sumPrice) <= BonusSystem.MaxOrderPercent)
                return bonusPrice;

            return (sumPrice * BonusSystem.MaxOrderPercent / 100).RoundPrice();
        }

        /// <summary>
        /// Расчет стоимости бонусов, которые будут начислены на карту
        /// </summary>
        /// <param name="priceWithShippingAndDiscount"></param>
        /// <param name="priceWhitDiscount"></param>
        /// <param name="bonusPercent"></param>
        /// <returns></returns>
        public static float GetBonusPlus(float priceWithShippingAndDiscount, float priceWhitDiscount, decimal bonusPercent)
        {
            if (bonusPercent == 0)
                return 0;

            var price = BonusSystem.BonusType == EBonusType.ByProductsCostWithShipping
                    ? priceWithShippingAndDiscount
                    : priceWhitDiscount;

            return (price * (float)bonusPercent / 100);
        }

        public static BonusCost GetBonusCost(ShoppingCart cart, float shippingPrice = 0, bool useBonuses = false, bool wantBonusCard = true)
        {
            var bonusCard = GetCard(CustomerContext.CustomerId);
            return GetBonusCost(bonusCard, cart, shippingPrice, useBonuses, wantBonusCard);
        }

        /// <summary>
        /// Расчет bonusCost и bonusPlus (стоимость и сколько будет зачислено)
        /// </summary>
        public static BonusCost GetBonusCost(Card bonusCard, ShoppingCart cart, float shippingPrice = 0, bool useBonuses = false, bool wantBonusCard = true)
        {
            if (bonusCard != null && bonusCard.Blocked)
                return new BonusCost(0, 0);

            var cartTotalPrice = cart.TotalPrice;
            var cartTotalDiscount = cart.TotalDiscount;

            // Сколько боусов списать (bonusCost) расчитывается из цены товаров со скидкой (priceWithDiscount)
            // Сколько бонусов начислить (bonusPlus) расчитывается из товаров, у которых включено начисление бонусов (price)

            var priceWithDiscount = cartTotalPrice - cartTotalDiscount;
            var price = priceWithDiscount -
                        cart.Where(x => !x.Offer.Product.AccrueBonuses)
                            .Sum(x => (x.PriceWithDiscount - x.PriceWithDiscount/cartTotalPrice*cartTotalDiscount)*x.Amount);

            float bonusPlus = 0;
            float bonusCost = 0;
            
            if (bonusCard != null)
            {
                if (useBonuses && bonusCard.BonusesTotalAmount > 0)
                {                                                                                                                                                                              
                    bonusCost = GetBonusCost(priceWithDiscount + shippingPrice, priceWithDiscount, (float)bonusCard.BonusesTotalAmount);
                    price -= bonusCost;
                }

                bonusPlus = GetBonusPlus(price + shippingPrice, price, bonusCard.Grade.BonusPercent);
            }
            else if (wantBonusCard)
            {
                bonusPlus =
                    //BonusSystem.BonusesForNewCard +
                    GetBonusPlus(price + shippingPrice, price, BonusSystem.BonusFirstPercent);
            }

            return new BonusCost(bonusCost, bonusPlus);
        }


        private static readonly Random Rnd = new Random();
        private static long GetRandom(long min, long max)
        {
            var randomLong = min + (long)(Rnd.NextDouble() * (max - min));
            return randomLong;
        }
    }

    public class BonusCost
    {
        public BonusCost(float bonusPrice, float bonusPlus)
        {
            BonusPrice = bonusPrice > 0 ? bonusPrice : 0;
            BonusPlus = bonusPlus > 0 ? bonusPlus : 0;
        }

        public float BonusPrice { get; private set; }
        public float BonusPlus { get; private set; }
    }
}