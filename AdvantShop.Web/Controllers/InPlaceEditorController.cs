using AdvantShop.Configuration;
using AdvantShop.Core.Services.InplaceEditor;
using AdvantShop.Customers;
using AdvantShop.Handlers.Inplace;
using AdvantShop.SEO;
using AdvantShop.Web.Infrastructure.Filters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace AdvantShop.Controllers
{
    public partial class InplaceEditorController : BaseClientController
    {
        [HttpPost]
        [InPlace(RoleKey = RoleAction.Cms)]
        public JsonResult StaticPage(int id, string content, StaticPageInplaceField field)
        {
            var model = new InplaceStaticPageHandler().Execute(id, content, field);
            return Json(model);
        }

        [HttpPost]
        [InPlace(RoleKey = RoleAction.Cms)]
        public JsonResult News(int id, string content, NewsInplaceField field)
        {
            var model = new InplaceNewsHandler().Execute(id, content, field);
            return Json(model);
        }

        [HttpPost]
        [InPlace(RoleKey = RoleAction.Cms)]
        public JsonResult StaticBlock(int id, string content, StaticBlockInplaceField field)
        {
            var model = new InplaceStaticBlockHandler().Execute(id, content, field);
            return Json(model);
        }

		[HttpPost]
		[InPlace(RoleKey = RoleAction.Catalog)]
		public JsonResult Tag(int id, string content, TagInplaceField field)
		{
			var model = new InplaceTagHandler().Execute(id, content, field);
			return Json(model);
		}

        [HttpPost]
        [InPlace(RoleKey = RoleAction.Catalog)]
        public JsonResult Category(int id, string content, CategoryInplaceField field)
        {
            var model = new InplaceCategoryHandler().Execute(id, content, field);
            return Json(model);
        }

        [HttpPost]
        [InPlace(RoleKey = RoleAction.Catalog)]
        public JsonResult Review(int id, string content, ReviewInplaceField field)
        {
            var model = new InplaceReviewHandler().Execute(id, content, field);
            return Json(model);
        }

        [HttpPost]
        [InPlace(RoleKey = RoleAction.Catalog)]
        public JsonResult Product(int id, string content, ProductInplaceField field)
        {
            var model = new InplaceProductHandler().Execute(id, content, field);
            return Json(model);
        }

        [HttpPost]
        [InPlace(RoleKey = RoleAction.Catalog)]
        public JsonResult Offer(int id, string content, OfferInplaceField field)
        {
            var model = new InplaceOfferHandler().Execute(id, content, field);
            return Json(model);
        }

        [HttpPost]
        [InPlace(RoleKey = RoleAction.Catalog)]
        public JsonResult PropertyAdd(int productId, string name, string value, int propertyId = 0, int propertyValueId = 0)
        {
            return Json(new InplacePropertyHandler().AddProperty(productId, name, value, propertyId, propertyValueId));
        }

        [HttpPost]
        [InPlace(RoleKey = RoleAction.Catalog)]
        public JsonResult PropertyUpdate(int productId, int id, string content)
        {
            return Json(new InplacePropertyHandler().UpdateProperty(productId, id, content));
        }

        [HttpPost]
        [InPlace(RoleKey = RoleAction.Catalog)]
        public JsonResult PropertyDelete(int productId, int id)
        {
            return Json(new InplacePropertyHandler().DeleteProperty(productId, id));
        }

        [HttpPost]
        [InPlace(RoleKey = RoleAction.Catalog)]
        public JsonResult Brand(int id, string content, BrandInplaceField field)
        {
            var model = new InplaceBrandHandler().Execute(id, content, field);
            return Json(model);
        }

        [HttpPost]
        [InPlace]
        public JsonResult Image(List<HttpPostedFileBase> file, string data)
        {
            var dataTemp = new
            {
                field = ImageInplaceField.Logo,
                command = ImageInplaceCommands.Add,
                colorId = 0,
                additionalData = string.Empty,
                id = 0,
                objId = 0
            };

            var fields = JsonConvert.DeserializeAnonymousType(data, dataTemp);

            var model = new InplaceImageHandler().Execute(file, fields.field, fields.command, fields.colorId, fields.additionalData, fields.id, fields.objId);
            return Json(model);
        }

        [HttpPost]
        [InPlace(RoleKey = RoleAction.Settings)]
        public JsonResult Meta(int id, string name, MetaType metaType, string title, string h1, string metaKeywords, string metaDescription)
        {
            var model = new InplaceMetaHandler().Execute(id, name, metaType, title, h1, metaKeywords, metaDescription);
            return Json(model);
        }

        [HttpPost]
        [InPlace]
        public JsonResult SetEnable(bool isEnabled)
        {
            SettingsMain.EnableInplace = isEnabled;
            return Json(true);
        }

        [HttpGet]
        [InPlace]
        public JsonResult GetMeta(int id, MetaType metaType)
        {
            var model = new GetMetaHandler().Execute(id, metaType);
            return Json(model);
        }


        [HttpPost]
        [InPlace]
        public JsonResult Module(int id, string type, string field, string content)
        {
            var model = new InplaceModuleHandler().Execute(id, type, field, content);
            return Json(model);
        }


    }
}