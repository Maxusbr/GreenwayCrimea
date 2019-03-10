using AdvantShop.Security;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Module.RussianPostPrintBlank.Models;
using AdvantShop.Module.RussianPostPrintBlank.Service;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace AdvantShop.Module.RussianPostPrintBlank.Controllers
{
    //[Module(Type = "RussianPostPrintBlank")]
    public class RPPBTemplatesController : ModuleAdminController
    {
        public JsonResult GetAvailableTemplateTypes()
        {
            var types = new List<FormItem>();
            //types.Add(new FormItem { Name = "Не выбран", Type = "-1" });
            types.Add(new FormItem { Name = "Адресный ярлык Ф.7", Type = "F7" });
            types.Add(new FormItem { Name = "Опись вложения Ф.107", Type = "F107" });
            types.Add(new FormItem { Name = "Наложенный платеж Ф.112", Type = "F112" });

            return Json(types, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTemplateType(string templateType)
        {
            if (string.IsNullOrEmpty(templateType) || templateType == "-1")
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            switch(templateType)
            {
                case "F7":
                    var f7Model = new F7ModelView();
                    f7Model.TemplateName = string.Empty;
                    f7Model.F7 = new F7Model();

                    return Json(new
                    {
                        Type = FormType.F7,
                        TemplateName = f7Model.TemplateName,
                        Content = f7Model.F7
                    }, JsonRequestBehavior.AllowGet);

                case "F107":
                    var f107Model = new F107ModelView();
                    f107Model.TemplateName = string.Empty;
                    f107Model.F107 = new F107Model();

                    return Json(new
                    {
                        Type = FormType.F107,
                        TemplateName = f107Model.TemplateName,
                        Content = f107Model.F107
                    }, JsonRequestBehavior.AllowGet);

                case "F112":
                    var f112Model = new F112ModelView();
                    f112Model.TemplateName = string.Empty;
                    f112Model.F112 = new F112Model();

                    return Json(new
                    {
                        Type = FormType.F112,
                        TemplateName = f112Model.TemplateName,
                        Content = f112Model.F112
                    }, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddTemplate(string templateType, string name, string content)
        {
            if (string.IsNullOrEmpty(templateType) || content.Length <= 0)
            {
                return Json(false);
            }

            var type = FormType.None;
            Enum.TryParse<FormType>(templateType, out type);
            
            var newTemplate = new Template
            {
                Name = name,
                Content = content,
                Type = type
            };

            TemplatesService.AddTemplate(newTemplate);

            return Json(true);
        }

        [HttpPost]
        public JsonResult EditTemplate(int templateId, string name, string content)
        {
            if (templateId <= 0)
            {
                return Json(false);
            }

            var template = TemplatesService.GetTemplate(templateId);
            if (template == null)
            {
                return Json(false);
            }

            template.Name = name;
            template.Content = content;

            TemplatesService.UpdateTemplate(template);

            return Json(true);
        }

        [HttpPost]
        public JsonResult DeleteTemplate(int templateId)
        {
            if (templateId <= 0)
            {
                return Json(false);
            }

            TemplatesService.DeleteTemplate(templateId);

            return Json(true);
        }

        [HttpGet]
        public JsonResult TemplatesList()
        {
            var templates = TemplatesService.GetTemplates();
            return Json(templates, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public JsonResult GetTemplate(int? templateId)
        {
            if (!templateId.HasValue || templateId.Value < 0)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            var template = TemplatesService.GetTemplate(templateId.Value);

            switch (template.Type)
            {
                case FormType.F7:
                    var f7Model = new F7ModelView();
                    f7Model.TemplateId = templateId.Value;
                    f7Model.TemplateName = template.Name;
                    f7Model.F7 = JsonConvert.DeserializeObject<F7Model>(template.Content);

                    return Json(new
                    {
                        Type = template.Type,
                        TemplateId = f7Model.TemplateId,
                        TemplateName = f7Model.TemplateName,
                        Content = f7Model.F7
                    }, JsonRequestBehavior.AllowGet);

                case FormType.F107:
                    var f107Model = new F107ModelView();
                    f107Model.TemplateId = templateId.Value;
                    f107Model.TemplateName = template.Name;
                    f107Model.F107 = JsonConvert.DeserializeObject<F107Model>(template.Content);

                    return Json(new
                    {
                        Type = template.Type,
                        TemplateId = f107Model.TemplateId,
                        TemplateName = f107Model.TemplateName,
                        Content = f107Model.F107
                    }, JsonRequestBehavior.AllowGet);

                case FormType.F112:
                    var f112Model = new F112ModelView();
                    f112Model.TemplateId = templateId.Value;
                    f112Model.TemplateName = template.Name;
                    f112Model.F112 = JsonConvert.DeserializeObject<F112Model>(template.Content);

                    return Json(new
                    {
                        Type = template.Type,
                        TemplateId = f112Model.TemplateId,
                        TemplateName = f112Model.TemplateName,
                        Content = f112Model.F112
                    }, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }
    }
}
