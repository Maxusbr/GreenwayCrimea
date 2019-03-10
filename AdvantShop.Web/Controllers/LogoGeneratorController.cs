using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Design;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Models.Common;
using AdvantShop.Trial;
using AdvantShop.Web.Infrastructure.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdvantShop.Controllers
{
    public class LogoGeneratorController : Controller
    {
        [AdminAuth]
        public JsonResult GetData()
        {
            return Json(new
            {
                logo = new
                {
                    text = SettingsMain.ShopName,
                    style = new
                    {
                        color = DesignService.GetDesigns(eDesign.Color).FirstOrDefault(x => x.Name == SettingsDesign.ColorScheme).Color,
                        fontFamily = "Lobster",
                        fontSize = 48,
                        textAlign = "center"
                    },
                    font = new
                    {
                        link = "https://fonts.googleapis.com/css?family=Lobster&amp;subset=cyrillic",
                        fontFamily = "Lobster",
                        languages = new List<string>() {
                            "latin",
                            "cyrillic"
                        }
                    }
                },
                slogan = new
                {
                    text = "Слоган интернет-магазина",
                    style = new
                    {
                        fontFamily = "Old Standard TT",
                        fontSize = 14,
                        textAlign = "center",
                        marginTop = 0,
                        marginBottom = 0
                    },
                    font = new
                    {
                        link = "https://fonts.googleapis.com/css?family=Old+Standard+TT&amp;subset=cyrillic",
                        fontFamily = "Old Standard TT",
                        languages = new List<string>() {
                            "latin",
                            "cyrillic"
                        }
                    },
                    marginValue = 0
                }
            }, JsonRequestBehavior.AllowGet);
        }

        [AdminAuth]
        [HttpPost]
        public JsonResult SaveLogo(string dataUrl, string fileExtension, string fontFamilyLogo)
        {

            try
            {
                if (FileHelpers.CheckFileExtension(fileExtension, EAdvantShopFileTypes.Image))
                {
                    var base64 = dataUrl.Split(new[] { "base64," }, StringSplitOptions.None)[1];
                    var bytes = Convert.FromBase64String(base64);

                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        using (Image image = Image.FromStream(ms, true))
                        {
                            FileHelpers.CreateDirectory(FoldersHelper.GetPathAbsolut(FolderType.ImageTemp));
                            FileHelpers.DeleteFile(FoldersHelper.GetPathAbsolut(FolderType.Pictures, SettingsMain.LogoImageName));

                            var newFile = "logo_generated".FileNamePlusDate() + FileHelpers.GetExtension(fileExtension);
                            var newFilePath = FoldersHelper.GetPathAbsolut(FolderType.Pictures, newFile);

                            SettingsMain.LogoImageName = newFile;

                            FileHelpers.SaveResizePhotoFile(newFilePath, 500, 500, image, 100);
                        }
                    }

                    TrialService.TrackEvent(TrialEvents.GenerateLogo, fontFamilyLogo);

                    return Json(new LogoModel());
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error("SaveLogoGenerator", ex);
                return null;
            }
        }
    }
}