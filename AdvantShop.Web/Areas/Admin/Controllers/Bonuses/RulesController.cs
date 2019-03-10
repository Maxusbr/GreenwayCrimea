using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Core;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.Bonuses.Model.Enums;
using AdvantShop.Core.Services.Bonuses.Service;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Models.Rules;
using AdvantShop.Web.Infrastructure.Admin.ModelBinders;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Bonuses
{
    [Auth(RoleAction.BonusSystem)]
    [SaasFeature(Saas.ESaasProperty.BonusSystem)]
    public class RulesController : BaseAdminController
    {
        public ActionResult Index()
        {
            SetMetaInformation(T("Admin.Rules.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.RulesCtrl);
            return View();
        }

        public JsonResult GetRules()
        {
            var result = CustomRuleService.GetAll()
                                          .Select(x => new
                                          {
                                              RuleType = x.RuleType.ToString(),
                                              RuleTypeStr = x.RuleType.Localize(),
                                              x.Name,
                                              x.Enabled
                                          })
                                          .ToList();
            return Json(new
            {
                TotalItemsCount = result.Count,
                DataItems = result
            });
        }

        public JsonResult GetRuleTypes()
        {
            var result = EnumExtensions.ToDictionary<ERule>().Where(x => x.Key != ERule.BirthDay).Select(x => new { Value = x.Value, Text = x.Key.Localize() }).ToList();
            return JsonOk(result);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteRuleMass(List<ERule> ids, string selectMode)
        {
            if (selectMode == "all")
            {
                var t = CustomRuleService.GetAll();
                foreach (var id in t)
                {
                    CustomRuleService.Delete(id.RuleType);
                }
            }

            if (ids == null) return Json(false);
            foreach (var id in ids)
            {
                CustomRuleService.Delete(id);
            }
            return Json(true);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteRule(ERule id)
        {
            CustomRuleService.Delete(id);
            return Json(true);
        }


        [HttpPost, ValidateJsonAntiForgeryToken, ValidateAjax]
        public ActionResult Create(ERule ruleType)
        {
            return ProcessJsonResult(new AddRule(ruleType));
        }

        public ActionResult Edit(ERule id)
        {
            var rule = CustomRuleService.Get(id);
            if (rule == null)
                return RedirectToAction("Index");

            var model = RuleModel.Get(rule);

            SetMetaInformation(T("Admin.Rules.Index.Title") + " - " + rule.RuleType);
            SetNgController(NgControllers.NgControllersTypes.RulesCtrl);

            return View("Edit", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit([ModelBinder(typeof(ModelTypeBinder))] RuleModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var handler = new UpdateRule(model);
                    var result = handler.Execute();
                    if (result)
                    {
                        ShowMessage(NotifyType.Success, T("Admin.ChangesSuccessfullySaved"));
                        return RedirectToAction("Edit", new { id = model.RuleType });
                    }
                }
                catch (BlException e)
                {
                    ModelState.AddModelError(e.Property, e.Message);
                }
            }

            ShowErrorMessages();

            SetMetaInformation(T("Admin.Rules.Index.Title") + " - " + model.Name);
            SetNgController(NgControllers.NgControllersTypes.RulesCtrl);

            return View("Index");
        }
    }
}
