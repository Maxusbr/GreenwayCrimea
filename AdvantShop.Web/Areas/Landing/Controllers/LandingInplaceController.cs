using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Diagnostics;
using AdvantShop.App.Landing.Domain;
using AdvantShop.App.Landing.Filters;
using AdvantShop.App.Landing.Handlers.Inplace;
using AdvantShop.App.Landing.Handlers.Pictures;
using AdvantShop.App.Landing.Models.Catalogs;
using AdvantShop.App.Landing.Models.Inplace;
using Newtonsoft.Json;

namespace AdvantShop.App.Landing.Controllers
{
    [AuthLp]
    public partial class LandingInplaceController : LandingBaseController
    {
        private readonly LpService _lpService;
        private readonly LpBlockService _lpBlockService;

        public LandingInplaceController()
        {
            _lpService = new LpService();
            _lpBlockService = new LpBlockService();
        }

        #region Blocks

        /// <summary>
        /// Получаем список блоков для лендинга
        /// </summary>
        public JsonResult GetBlocks(int landingPageId)
        {
            var model = new GetBlocks(landingPageId).Execute();
            return Json(model);
        }

        /// <summary>
        /// Добавление блока
        /// </summary>
        /// <returns>Json(new { result = true })</returns>
        public JsonResult AddBlock(AddBlockModel model)
        {
            var result = new AddBlock(model, this).Execute();
            return Json(result);
        }

        /// <summary>
        /// Сохранение настроек блока
        /// </summary>
        /// <returns>Json(new { result = false })</returns>
        public JsonResult SaveBlockSettings(int blockId, string settings)
        {
            var result = new SaveBlockSettings(blockId, settings).Execute();
            return Json(result);
        }

        /// <summary>
        /// Сохранение сортировки
        /// </summary>
        /// <returns>Json(new { result = false })</returns>
        public JsonResult SaveBlockSortOrder(int blockId, bool top)
        {
            var block = _lpBlockService.Get(blockId);
            if (block == null)
                return Json(new { result = false });

            var blocks = _lpBlockService.GetList(block.LandingId);
            var index = blocks.FindIndex(x => x.Id == block.Id);
            var swapIndex = index + (top ? -1 : 1);

            for (int i = 0; i < blocks.Count; i++)
            {
                var sorting = i;

                if (i == swapIndex)
                    sorting = index;
                else if (i == index)
                    sorting = swapIndex;

                blocks[i].SortOrder = sorting * 100;

                _lpBlockService.Update(blocks[i]);
            }

            return Json(new { result = true });
        }

        /// <summary>
        /// Удаление блока
        /// </summary>
        /// <returns>Json(new { result = false })</returns>
        public JsonResult RemoveBlock(int blockId)
        {
            var block = _lpBlockService.Get(blockId);
            if (block == null)
                return Json(new { result = false });

            _lpBlockService.Delete(block.Id);

            var removePictures = new RemoveBlockPicturesHandler(block.LandingId, block.Id).Execute();

            return Json(new { result = true });
        }

        #endregion

        #region SubBlocks

        /// <summary>
        /// Обновление контента подблока
        /// </summary>
        /// <returns>Json(new { result = false })</returns>
        public JsonResult UpdateSubBlockContent(int subBlockId, string content)
        {
            var subBlock = _lpBlockService.GetSubBlock(subBlockId);
            if (subBlock == null)
                return Json(new { result = false });

            subBlock.ContentHtml = content;

            _lpBlockService.UpdateSubBlock(subBlock);

            return Json(new { result = true });
        }

        /// <summary>
        /// Обновление настроек подблока
        /// </summary>
        /// <returns>Json(new { result = false })</returns>
        public JsonResult UpdateSubBlockSettings(int subBlockId, string settings)
        {
            var subBlock = _lpBlockService.GetSubBlock(subBlockId);
            if (subBlock == null)
                return Json(new { result = false });

            try
            {
                var settingsOld = JsonConvert.DeserializeObject<Dictionary<string, object>>(subBlock.Settings);
                var settingsNew = JsonConvert.DeserializeObject<Dictionary<string, object>>(settings);

                if (settingsNew == null)
                    return Json(new { result = false });

                if (settingsOld == null)
                    settingsOld = new Dictionary<string, object>();

                foreach (var key in settingsNew.Keys)
                {
                    if (settingsOld.ContainsKey(key))
                    {
                        settingsOld[key] = settingsNew[key];
                    }
                    else
                    {
                        settingsOld.Add(key, settingsNew[key]);
                    }
                }

                subBlock.Settings = JsonConvert.SerializeObject(settingsOld);

                _lpBlockService.UpdateSubBlock(subBlock);

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return Json(new { result = false });
        }

        #endregion

        #region Pictures

        [HttpPost]
        public JsonResult UploadPicture(int lpId, int blockId)
        {
            var lpblock = _lpBlockService.Get(blockId);
            if (lpblock == null || lpblock.LandingId != lpId)
                return Json(new UploadPictureResult() {Error = "wrong lp block"});

            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0 && file.ContentLength < 10000000)
                {
                    var result = new UploadPictureHandler(lpId, blockId, file).Execute();

                    return Json(result);
                }
            }

            return Json(new UploadPictureResult()
            {
                Error = "Не корректное изображение. Изображение должно быть формата .jpg, .jpeg, .png, .bmp или .gif и не больше 10 мб"
            });
        }

        [HttpPost]
        public JsonResult RemovePicture(int lpId, int blockId, string picture)
        {
            var lpblock = _lpBlockService.Get(blockId);
            if (lpblock == null || lpblock.LandingId != lpId)
            {
                return Json(new UploadPictureResult() {Error = "wrong lp block"});
            }

            if (string.IsNullOrEmpty(picture))
                return Json(new UploadPictureResult() {Error = "Not found picture"});

            var result = new RemovePictureHandler(picture).Execute();

            return Json(new UploadPictureResult() {Result = result});
        }

        [HttpPost]
        public JsonResult UpdatePicture(int lpId, int blockId, string picture)
        {
            var lpblock = _lpBlockService.Get(blockId);
            if (lpblock == null || lpblock.LandingId != lpId)
            {
                return Json(new UploadPictureResult() {Error = "wrong lp block"});
            }

            if (string.IsNullOrEmpty(picture))
                return Json(new UploadPictureResult() {Error = "Not found picture"});

            var result = new RemovePictureHandler(picture).Execute();

            if (result == false)
                return Json(new UploadPictureResult() {Error = "Error on delete picture"});

            return UploadPicture(lpId, blockId);
        }


        [HttpPost]
        public JsonResult ProcessPictute(string command, int lpId, int blockId, string picture = null)
        {
            var result = new JsonResult();

            switch (command)
            {
                case "add":
                    result = UploadPicture(lpId, blockId);
                    break;
                case "update":
                    result = UpdatePicture(lpId, blockId, picture);
                    break;
                case "delete":
                    result = RemovePicture(lpId, blockId, picture);
                    break;
                default:
                    result = Json(new UploadPictureResult(){Error = "wrong action for process picture"});
                    break;
            }

            return result;
        }
        #endregion

        #region Catalog Tree view

        public JsonResult CategoriesTree(int categoryId = 0)
        {
            var categories =
                CategoryService.GetChildCategoriesByCategoryId(categoryId, true).Select(x => new CategoryLpModel()
                {
                    id = x.CategoryId.ToString(),
                    parent = x.ParentCategoryId.ToString(),
                    text = x.Name.ToString(),
                    children = x.HasChild
                });

            return Json(categories);
        }

        #endregion

        #region Settings

        public JsonResult GetSettings(int landingId)
        {
            var result = new GetSettings(landingId).Execute();
            return Json(result);
        }

        #endregion
    }
}
