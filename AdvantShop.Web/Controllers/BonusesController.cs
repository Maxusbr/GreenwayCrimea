using System.Collections.Generic;
using System.Web.Mvc;
using AdvantShop.CMS;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.Services.Bonuses.Model;
using AdvantShop.Customers;
using AdvantShop.Models.BonusSystemModule;
using AdvantShop.Orders;
using AdvantShop.Web.Infrastructure.Extensions;

namespace AdvantShop.Controllers
{
    public partial class BonusesController : BaseClientController
    {
        #region Help methods

        //private string IsValidCardData(string firstName, string lastName, long? phone, string birthDay = null)
        //{
        //    var isValid = firstName.IsNotEmpty() && lastName.IsNotEmpty();

        //    if (birthDay != null)
        //        isValid &= birthDay.IsNotEmpty();

        //    if (!isValid)
        //        return T("Bonuses.ErrorRequired");

        //    if (phone == null || phone == 0)
        //        return T("Bonuses.ErrorPhone");

        //    return string.Empty;
        //}

        //private void UpdateCustomer(long cardNumber, bool isCheckout)
        //{
        //    var customer = CustomerContext.CurrentCustomer;

        //    if (customer.RegistredUser)
        //    {
        //        customer.BonusCardNumber = cardNumber;
        //        CustomerService.UpdateCustomer(customer);
        //    }
        //    else if (!isCheckout)
        //    {
        //        Session["bonuscard"] = cardNumber;
        //    }
        //    //else save in checkout data other ajax request
        //}

        #endregion

        public ActionResult GetBonusCard()
        {
            if (!BonusSystem.IsActive)
                return Error404();
            
            var breadCrumbs = new List<BreadCrumbs>()
            {
                new BreadCrumbs(T("MainPage"), Url.AbsoluteRouteUrl("Home")),
                new BreadCrumbs(T("Module.BonusSystem.GetBonusCardTitle"), Url.AbsoluteRouteUrl("GetBonusCardRoute"))
            };

            var model = new GetBonusCardViewModel
            {
                BreadCrumbs = breadCrumbs,
                BonusTextBlock = BonusSystem.BonusTextBlock,
                BonusRightTextBlock = BonusSystem.BonusRightTextBlock,
                Grades = BonusSystem.IsActive && BonusSystem.BonusShowGrades
                        ? BonusSystemService.GetGrades()
                        : null
            };

            SetMetaInformation(T("Module.BonusSystem.GetBonusCardTitle"));

            return View(model);
        }

        public JsonResult BonusJson()
        {
            var customer = CustomerContext.CurrentCustomer;

            var bonusCard = BonusSystemService.GetCard(customer.Id);

            if (bonusCard == null)
            {
                var checkoutData = OrderConfirmationService.Get(customer.Id);
                if (checkoutData != null && checkoutData.User.BonusCardId != null)
                {
                    bonusCard = BonusSystemService.GetCard(checkoutData.User.BonusCardId.Value);
                }
            }

            if (bonusCard == null && Session["bonuscard"] != null)
            {
                bonusCard = BonusSystemService.GetCard((long)Session["bonuscard"]);
            }
            
            return Json(bonusCard != null
                ? new
                {
                    bonus = new
                    {
                        CardNumber = bonusCard.CardNumber,
                        BonusAmount = bonusCard.BonusesTotalAmount,
                        BonusPercent = bonusCard.Grade.BonusPercent,
                        Blocked = bonusCard.Blocked
                    },
                    bonusText =
                        string.Format("{0} ({1} {2} {3})", T("Bonuses.ByBonusCard"), T("Bonuses.YourBonuses"),
                            bonusCard.BonusesTotalAmount.ToString("F1"),
                            Strings.Numerals((float)bonusCard.BonusesTotalAmount, T("Bonuses.Bonuses0"),
                                T("Bonuses.Bonuses1"), T("Bonuses.Bonuses2"), T("Bonuses.Bonuses5")))
                }
                : null);
        }

        [HttpPost]
        public JsonResult CreateBonusCard()
        {
            var customer = CustomerContext.CurrentCustomer;
            if (!customer.RegistredUser)
                return Json(new { result = false, error = "Пользователь не зарегистрирован" });

            var bonusCard = BonusSystemService.GetCard(customer.Id);
            if (bonusCard != null)
                return Json(new { result = false, error = "Бонусная карта уже зарегестрирована" });

            customer.BonusCardNumber = BonusSystemService.AddCard(new Card { CardId = customer.Id });
            CustomerService.UpdateCustomer(customer);

            return Json(new {result = true});
        }

        ///// <summary>
        ///// Confirm card by card number (phone)
        ///// </summary>
        ///// <returns>sms code or error</returns>
        //public JsonResult ConfirmCard(long? cardnumber = null, string phone = null)
        //{
        //    var phoneParsed = StringHelper.ConvertToStandardPhone(phone, true);

        //    if (cardnumber == null && (phone.IsNullOrEmpty() || phoneParsed == null))
        //        return Json(new { error = T("Bonuses.CardNotExist") });

        //    var customer = CustomerContext.CurrentCustomer;
        //    if (customer.RegistredUser && BonusSystemService.GetCard(customer.Id) != null)
        //    {
        //        return Json(new { error = T("Bonuses.CardNotExist") });
        //    }

        //    if (cardnumber != null)
        //    {
        //        //var bonusCodeResponse = BonusSystemService.GetSmsCode((long)cardnumber);
        //        //Session["BonusSmsCode"] = bonusCodeResponse + "_" + cardnumber;
        //        return Json(new { error = "" });
        //    }

        //    if (phoneParsed != null)
        //    {
        //        var cardByPhone = BonusSystemService.GetCardByPhone(phoneParsed.ToString());
        //        if (cardByPhone != null)
        //        {
        //            //var bonusCodeByPhoneResponse = BonusSystemService.GetSmsCode(phoneParsed.ToString());
        //            //Session["BonusSmsCode"] = bonusCodeByPhoneResponse + "_" + cardByPhone.CardNumber;
        //            return Json(new { error = "" });
        //        }
        //    }

        //    return Json(new { error = T("Bonuses.CardNotFound") });
        //}

        //public JsonResult ConfirmNewCard(string phone, bool skipCheck = false)
        //{
        //    var phoneLong = StringHelper.ConvertToStandardPhone(phone, true);

        //    if (phone.IsNullOrEmpty() || phoneLong == null)
        //        return Json(new { error = T("Bonuses.WrongPhoneNumber") });

        //    var customer = CustomerContext.CurrentCustomer;
        //    if (customer.RegistredUser && BonusSystemService.GetCard(customer.Id) != null)
        //    {
        //        return Json(new { error = T("Bonuses.PhoneNumberExists") });
        //    }

        //    if (!skipCheck)
        //    {
        //        var isExistPhone = BonusSystemService.IsPhoneExist(phoneLong.ToString());
        //        if (isExistPhone)
        //        {
        //            return Json(new { error = T("Bonuses.PhoneNumberExists") });
        //        }
        //    }
        //    //todo add blexception
        //    //var bonusCodeResponse = BonusSystemService.GetSmsCode(phoneLong.ToString());
        //    //Session["BonusSmsCode"] = bonusCodeResponse;
        //    return Json(new { error = "" });
        //}

        ///// <summary>
        ///// Confirm sms code
        ///// </summary>
        ///// <param name="code"></param>
        ///// <param name="isCheckout"></param>
        ///// <returns></returns>
        //public JsonResult ConfirmCode(string code, bool isCheckout)
        //{
        //    if (code.IsNullOrEmpty() || Session["BonusSmsCode"] == null)
        //    {
        //        return Json(new { error = T("Bonuses.WrongCode") });
        //    }

        //    var bonusSmsCode = Session["BonusSmsCode"].ToString().Split(new[] { "_" }, StringSplitOptions.None);

        //    if (bonusSmsCode[0] != code)
        //        return Json(new { error = T("Bonuses.WrongCode") });


        //    Card bonusCard = null;

        //    if (bonusSmsCode.Length == 2)
        //    {
        //        var cardNumber = bonusSmsCode[1].TryParseLong();
        //        bonusCard = BonusSystemService.GetCard(cardNumber);

        //        UpdateCustomer(cardNumber, isCheckout);
        //    }

        //    Session["BonusSmsCode"] = null;
        //    return Json(new
        //    {
        //        success = "true",
        //        error = "",
        //        bonus = bonusCard,
        //        bonusText =
        //                string.Format("{0} ({1} {2} {3})", T("Bonuses.ByBonusCard"), T("Bonuses.YourBonuses"),
        //                    bonusCard.BonusAmount,
        //                    Strings.Numerals((float)bonusCard.BonusAmount, T("Bonuses.Bonuses0"), T("Bonuses.Bonuses1"), T("Bonuses.Bonuses2"), T("Bonuses.Bonuses5")))
        //    });
        //}

        ///// <summary>
        ///// Confirm sms code and create new bonus card
        ///// </summary>
        ///// <returns></returns>
        //public JsonResult ConfirmCodeForNewCard(string code, bool isCheckout)
        //string firstName, string lastName, string secondName, int gender, string birthDay,string phone, string email, string city)
        //{
        //    if (code.IsNullOrEmpty() || Session["BonusSmsCode"] == null)
        //        return Json(new { error = T("Bonuses.WrongCode") });

        //    var bonusSmsCode = Session["BonusSmsCode"].ToString().Split(new[] { "_" }, StringSplitOptions.None);

        //    if (bonusSmsCode[0] != code)
        //        return Json(new { error = T("Bonuses.WrongCode") });

        //    var cardId = CustomerContext.CurrentCustomer.Id;
        //    var bonusCard = BonusSystemService.GetCard(cardId);
        //    if (bonusCard == null)
        //    {
        //        bonusCard = new Card { CardId = CustomerContext.CurrentCustomer.Id };
        //        try
        //        {
        //            var cardResponse = BonusSystemService.AddCard(bonusCard);
        //        }
        //        catch (BlException e)
        //        {
        //            return Json(new { error = e.Message });
        //        }
        //    }
        //    UpdateCustomer(bonusCard.CardNumber, isCheckout);
        //    Session["BonusSmsCode"] = null;
        //    return Json(new
        //    {
        //        success = "true",
        //        error = "",
        //        bonus = bonusCard,
        //        bonusText =
        //            string.Format("{0} ({1} {2} {3})", T("Bonuses.ByBonusCard"), T("Bonuses.YourBonuses"),
        //                bonusCard.BonusAmount,
        //                Strings.Numerals((float)bonusCard.BonusAmount, T("Bonuses.Bonuses0"), T("Bonuses.Bonuses1"), T("Bonuses.Bonuses2"), T("Bonuses.Bonuses5")))

        //    });
        //}

        // TODO: add localization!
        //public JsonResult UpdateCard(Card bonus)
        //{
        //    var customer = CustomerContext.CurrentCustomer;

        //    if (!customer.RegistredUser || customer.BonusCardNumber == null)
        //        return Json(new { error = T("Bonuses.CheckData") });

        //    bonus.FirstName = bonus.FirstName.Trim();
        //    bonus.LastName = bonus.LastName.IsNotEmpty() ? bonus.LastName.Trim() : "-";
        //    bonus.SecondName = bonus.SecondName.IsNotEmpty() ? bonus.SecondName.Trim() : "-";

        //    long? phoneParsed = null;

        //    if (bonus.CellPhone.IsNotEmpty())
        //        phoneParsed = StringHelper.ConvertToStandardPhone(bonus.CellPhone, true);

        //    bonus.CellPhone = phoneParsed.ToString();

        //    var error = IsValidCardData(bonus.FirstName, bonus.LastName, phoneParsed);
        //    if (!string.IsNullOrEmpty(error))
        //        return Json(new { error });

        //    var cardResponse = BonusSystemService.UpdateCard(bonus);

        //    if (cardResponse != null && cardResponse.Status == 200 && cardResponse.Data.CardNumber != 0)
        //    {
        //        return Json(new { error = "" });
        //    }
        //    else if (cardResponse != null && cardResponse.Message.IsNotEmpty())
        //    {
        //        return Json(new { error = cardResponse.Message });
        //    }
        //    else
        //    {
        //        return Json(new { error = T("Bonuses.CheckData") });
        //    }
        //}
    }
}