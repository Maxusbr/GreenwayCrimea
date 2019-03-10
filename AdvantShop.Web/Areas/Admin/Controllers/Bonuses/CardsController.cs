using System;
using System.IO;
using System.Web;
using System.Text;
using System.Web.Mvc;

using AdvantShop.Core;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.Services.Bonuses.Service;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Cards;
using AdvantShop.Web.Admin.Models.Cards;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;
using AdvantShop.Core.Services.Bonuses.Model.Enums;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using CsvHelper.Configuration;


namespace AdvantShop.Web.Admin.Controllers.Bonuses
{
    [Auth(RoleAction.BonusSystem)]
    [SaasFeature(Saas.ESaasProperty.BonusSystem)]
    public class CardsController : BaseAdminController
    {

        #region List of cards

        public ActionResult Index()
        {
            SetMetaInformation(T("Admin.Cards.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.CardsCtrl);

            return View();
        }

        public JsonResult GetCards(CardFilterModel model)
        {
            return Json(new GetCardHandler(model).Execute());
        }

        #region Commands

        private void Command(CardFilterModel model, Func<Guid, CardFilterModel, bool> func)
        {
            if (model.SelectMode == SelectModeCommand.None && model.Ids != null)
            {
                foreach (var id in model.Ids)
                {
                    func(id, model);
                }
            }
            else
            {
                var handler = new GetCardHandler(model);
                var ids = handler.GetItemsIds("CardId");

                foreach (var id in ids)
                {
                    if (model.Ids == null || !model.Ids.Contains(id))
                        func(id, model);
                }
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteCards(CardFilterModel model)
        {
            Command(model, (id, c) =>
            {
                new DeleteCardHandler(id).Execute();
                return true;
            });
            return Json(true);
        }

        #endregion

        #endregion

        #region Add | Edit card

        [HttpPost, ValidateJsonAntiForgeryToken, ValidateAjax]
        public JsonResult Add(CardModel model)
        {
            return ProcessJsonResult(new AddUpdateCard(model));
        }

        public ActionResult Edit(Guid id)
        {
            var card = CardService.Get(id);
            if (card == null)
                return RedirectToAction("Index");

            var bdrule = CustomRuleService.Get(ERule.ChangeGrade);

            var model = new CardModel()
            {
                IsEditMode = true,
                CardId = card.CardId,
                CardNumber = card.CardNumber,
                Name = card.Customer.LastName + " " + card.Customer.FirstName,
                Blocked = card.Blocked,
                BonusAmount = card.BonusAmount,
                AdditionBonusAmount = AdditionBonusService.ActualSum(id),
                GradeId = card.GradeId,
                CreateOn = card.CreateOn,
                DisabledChangeGrade = bdrule != null && bdrule.Enabled && !card.ManualGrade,
                ManualGrade = card.ManualGrade
            };
            model.Grades = GradeService.GetAll();
            SetMetaInformation(T("Admin.Cards.Index.Title") + " - " + card.CardNumber);
            SetNgController(NgControllers.NgControllersTypes.CardsCtrl);

            return View("Edit", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(CardModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var handler = new AddUpdateCard(model);
                    var result = handler.Execute();
                    if (result != Guid.Empty)
                    {
                        ShowMessage(NotifyType.Success, T("Admin.ChangesSuccessfullySaved"));
                        return RedirectToAction("Edit", new { id = model.CardId });
                    }
                }
                catch (BlException e)
                {
                    ModelState.AddModelError(e.Property, e.Message);
                }
            }

            ShowErrorMessages();

            SetMetaInformation(T("Admin.Cards.Index.Title") + " - " + model.Name);
            SetNgController(NgControllers.NgControllersTypes.CardsCtrl);

            return View("Index", model);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteCard(Guid cardId)
        {
            return ProcessJsonResult(new DeleteCardHandler(cardId));
        }

        #endregion

        [HttpGet]
        public ActionResult _AdditionBonus(Guid cardId)
        {
            var bonuses = AdditionBonusService.Actual(cardId);
            return PartialView(bonuses);
        }

        [HttpGet]
        public ActionResult _LastPurchase(Guid cardId)
        {
            ViewBag.CardId = cardId;
            var ps = PurchaseService.GetLast(cardId);
            return PartialView(ps);
        }

        [HttpGet]
        public ActionResult _LastTransaction(Guid cardId)
        {
            ViewBag.CardId = cardId;
            var trs = TransactionService.GetLast(cardId);
            return PartialView(trs);
        }

        [HttpGet]
        public ActionResult _UserInfo(Guid cardId)
        {
            var customer = CustomerService.GetCustomer(cardId);
            return PartialView(customer);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddBonus(AddMainBonusModel model)
        {
            return ProcessJsonResult(new AddMainBonusHandler(model));
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddBonusMass(CardFilterModelAddBonus model)
        {
            return ProcessJsonResult(new AddMainBonusMassHandler(model));
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SubtractBonus(AddMainBonusModel model)
        {
            return ProcessJsonResult(new SubstractMainBonusHandler(model));
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        [ValidateAjax]
        public JsonResult AddAdditionBonus(AddAdditionalBonusModel model)
        {
            return ProcessJsonResult(new AddAditionBonusHandler(model));
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        [ValidateAjax]
        public JsonResult AddAdditionBonusMass(CardFilterModelAddAdditionBonus model)
        {
            return ProcessJsonResult(new AddAditionBonusMassHandler(model));
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SubtractAdditionBonus(SubctractAdditionalBonusModel model)
        {
            return ProcessJsonResult(new SubtractAditionBonusHandler(model));
        }

        public JsonResult GetAdditionBonus(Guid cardId)
        {
            var bonus = AdditionBonusService.Actual(cardId);
            return JsonOk(bonus);
        }

        public ActionResult AllPurchase(PurcharseFilterModel model)
        {
            ViewBag.CardId = model.CardId;
            var handler = new GetPurchaseHandler(model);
            var result = handler.Execute();
            return View(result);
        }

        public ActionResult AllTransaction(TransactionFilterModel model)
        {
            ViewBag.CardId = model.CardId;
            var handler = new GetTransactionHandler(model);
            var result = handler.Execute();
            return View(result);
        }

        public JsonResult Generate()
        {
            var newcard = BonusSystemService.GenerateCardNumber(0);
            return JsonOk(newcard);
        }

        public JsonResult ImportCards(HttpPostedFileBase file)
        {
            using (MemoryStream target = new MemoryStream())
            {
                using (Stream inputStream = file.InputStream)
                {
                    inputStream.CopyTo(target);
                    byte[] data = target.ToArray();
                    new ImportCardsHandler(data).Execute();
                    //thanks for bad code
                    return Json(new { Result = true });
                }
            }
        }

        public ActionResult HistoryPersent(PersenthistoryFilterModel model)
        {
            ViewBag.CardId = model.CardId;
            var handler = new GetHistoryPercentHandler(model);
            var result = handler.Execute();
            return View(result);
        }

        public ActionResult ExportCards()
        {
            try
            {
                var export = new ExportCardsHandler();
                var result = export.Execute();
                var fileName = string.Format("cards{0}.csv", DateTime.Now.ToString("yyyyMMddhhmmss"));
                return File(result, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
            return Json(false);
        }
    }
}
