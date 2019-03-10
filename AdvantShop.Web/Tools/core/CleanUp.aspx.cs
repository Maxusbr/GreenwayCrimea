//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.SQL;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.CMS;

namespace Tools.core
{
    public partial class CleanUp : Page
    {
        protected void btnCleanUpPictureFolder_Click(object sender, EventArgs e)
        {
            var fileNames = new List<string>();

            foreach (var photo in PhotoService.GetNamePhotos(0, PhotoType.Product, true))
            {
                fileNames.Add(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Big, photo));
                fileNames.Add(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Middle, photo));
                fileNames.Add(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Small, photo));
                fileNames.Add(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.XSmall, photo));
            }

            foreach (string photo in PhotoService.GetNamePhotos(0, PhotoType.CategoryBig, true))
            {
                fileNames.Add(FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Big, photo));
            }

            foreach (string photo in PhotoService.GetNamePhotos(0, PhotoType.CategorySmall, true))
            {
                fileNames.Add(FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Small, photo));
            }

            foreach (string photo in PhotoService.GetNamePhotos(0, PhotoType.Color, true))
            {
                fileNames.Add(FoldersHelper.GetImageColorPathAbsolut(ColorImageType.Catalog, photo));
                fileNames.Add(FoldersHelper.GetImageColorPathAbsolut(ColorImageType.Details, photo));
            }

            foreach (string photo in PhotoService.GetNamePhotos(0, PhotoType.Brand, true))
            {
                fileNames.Add(FoldersHelper.GetPathAbsolut(FolderType.BrandLogo, photo));
            }
            foreach (string photo in PhotoService.GetNamePhotos(0, PhotoType.Carousel, true))
            {
                fileNames.Add(FoldersHelper.GetPathAbsolut(FolderType.Carousel, photo));
            }

            //foreach (string photo in PhotoService.GetNamePhotos(0, PhotoType.MenuIcon, true))
            //{
            //    fileNames.Add(FoldersHelper.GetPathAbsolut(FolderType.MenuIcons, photo));
            //}

            foreach (var photo in MenuService.GetMenuIcons())
            {
                fileNames.Add(FoldersHelper.GetPathAbsolut(FolderType.MenuIcons, photo));
            }

            foreach (string photo in PhotoService.GetNamePhotos(0, PhotoType.News, true))
            {
                fileNames.Add(FoldersHelper.GetPathAbsolut(FolderType.News, photo));
            }

            //Add Logo image in exceptions
            fileNames.Add(SettingsMain.LogoImageName);

            var files = new List<string>();
            files.AddRange(Directory.GetFiles(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Big, string.Empty)));
            files.AddRange(Directory.GetFiles(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Middle, string.Empty)));
            files.AddRange(Directory.GetFiles(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Small, string.Empty)));
            files.AddRange(Directory.GetFiles(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.XSmall, string.Empty)));

            // CategoryImageType ------------------------

            files.AddRange(Directory.GetFiles(FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Big, string.Empty)));

            if (Directory.Exists(FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Small, string.Empty)))
            {
                files.AddRange(Directory.GetFiles(FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Small, string.Empty)));
            }

            // ColorImageType Check exist ----------------

            if (Directory.Exists(FoldersHelper.GetImageColorPathAbsolut(ColorImageType.Catalog, string.Empty)))
            {
                files.AddRange(Directory.GetFiles(FoldersHelper.GetImageColorPathAbsolut(ColorImageType.Catalog, string.Empty)));
            }
            if (Directory.Exists(FoldersHelper.GetImageColorPathAbsolut(ColorImageType.Details, string.Empty)))
            {
                files.AddRange(Directory.GetFiles(FoldersHelper.GetImageColorPathAbsolut(ColorImageType.Details, string.Empty)));
            }

            //FolderType--------------------------------

            files.AddRange(Directory.GetFiles(FoldersHelper.GetPathAbsolut(FolderType.BrandLogo, string.Empty)));
            files.AddRange(Directory.GetFiles(FoldersHelper.GetPathAbsolut(FolderType.Carousel, string.Empty)));
            files.AddRange(Directory.GetFiles(FoldersHelper.GetPathAbsolut(FolderType.MenuIcons, string.Empty)));
            files.AddRange(Directory.GetFiles(FoldersHelper.GetPathAbsolut(FolderType.News, string.Empty)));


            var deleted = new List<string>();

            foreach (string file in files)
            {
                if (!fileNames.Contains(file) || (file.Trim().Length == 0))
                {
                    if (chboxDeleteFiles.Checked)
                    {
                        File.Delete(file);
                    }
                    deleted.Add(file);
                }
            }

            if (!chboxDeleteFiles.Checked)
            {
                lCompleted.Text = @"Analysis successfully completed";
            }

            lCompleted.Visible = true;

            var res = new StringBuilder();

            foreach (string del in deleted)
            {
                res.Append(del);
                res.Append("<br />");
            }

            if (deleted.Count > 0)
            {
                if (!chboxDeleteFiles.Checked)
                {
                    lResultHeader.Text = @"Files to delete";
                }
                lResultHeader.Visible = true;
                lResult.Text = res.ToString();
            }
            else
            {
                lResultHeader.Text = @"No unnecessary files";
                lResultHeader.Visible = true;
            }
        }

        protected void btnCleanUpBD_Click(object sender, EventArgs e)
        {
            var res = new StringBuilder();

            try
            {
                foreach (var photoName in PhotoService.GetNamePhotos(0, PhotoType.Product, true))
                {
                    res.Append(CheckExistingFile(PhotoType.Product, photoName, FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Big, photoName)));
                    res.Append(CheckExistingFile(PhotoType.Product, photoName, FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Middle, photoName)));
                    res.Append(CheckExistingFile(PhotoType.Product, photoName, FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Small, photoName)));
                    res.Append(CheckExistingFile(PhotoType.Product, photoName, FoldersHelper.GetImageProductPathAbsolut(ProductImageType.XSmall, photoName)));
                }

                foreach (var photoName in PhotoService.GetNamePhotos(0, PhotoType.CategoryBig, true))
                {
                    res.Append(CheckExistingFile(PhotoType.CategoryBig, photoName, FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Big, photoName)));
                }

                foreach (var photoName in PhotoService.GetNamePhotos(0, PhotoType.CategorySmall, true))
                {
                    res.Append(CheckExistingFile(PhotoType.CategorySmall,photoName, FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Small, photoName)));
                }


                foreach (var photoName in PhotoService.GetNamePhotos(0, PhotoType.Color, true))
                {
                    res.Append(CheckExistingFile(PhotoType.Color, photoName, FoldersHelper.GetImageColorPathAbsolut(ColorImageType.Catalog, photoName)));
                }


                foreach (var photoName in PhotoService.GetNamePhotos(0, PhotoType.Color, true))
                {
                    res.Append(CheckExistingFile(PhotoType.Color, photoName, FoldersHelper.GetImageColorPathAbsolut(ColorImageType.Details, photoName)));
                }

                foreach (var photoName in PhotoService.GetNamePhotos(0, PhotoType.Brand, true))
                {
                    res.Append(CheckExistingFile(PhotoType.Brand, photoName, FoldersHelper.GetPathAbsolut(FolderType.BrandLogo, photoName)));
                }
                foreach (var photoName in PhotoService.GetNamePhotos(0, PhotoType.Carousel, true))
                {
                    res.Append(CheckExistingFile(PhotoType.Carousel, photoName, FoldersHelper.GetPathAbsolut(FolderType.Carousel, photoName)));
                }
                foreach (var photoName in PhotoService.GetNamePhotos(0, PhotoType.MenuIcon, true))
                {
                    res.Append(CheckExistingFile(PhotoType.MenuIcon, photoName, FoldersHelper.GetPathAbsolut(FolderType.MenuIcons, photoName)));
                }
                foreach (var photoName in PhotoService.GetNamePhotos(0, PhotoType.News, true))
                {
                    res.Append(CheckExistingFile(PhotoType.News, photoName, FoldersHelper.GetPathAbsolut(FolderType.News, photoName)));
                }
                foreach (var photoName in PhotoService.GetNamePhotos(0, PhotoType.Payment, true))
                {
                    res.Append(CheckExistingFile(PhotoType.Payment, photoName, FoldersHelper.GetPathAbsolut(FolderType.PaymentLogo, photoName)));
                }
                foreach (var photoName in PhotoService.GetNamePhotos(0, PhotoType.Shipping, true))
                {
                    res.Append(CheckExistingFile(PhotoType.Shipping, photoName, FoldersHelper.GetPathAbsolut(FolderType.ShippingLogo, photoName)));
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            if (!chboxMakeNull.Checked)
            {
                lDBCleanupCompleted.Text = @"Analysis successfully completed";
            }

            lDBCleanupCompleted.Visible = true;

            lDBResult.Text = string.IsNullOrEmpty(res.ToString()) ? @"No items to correct" : res.ToString();
        }

        private string CheckExistingFile(PhotoType type, string photoName, string fullPath)
        {
            if (!File.Exists(fullPath))
            {
                if (chboxMakeNull.Checked)
                {
                    PhotoService.DeletePhotoWithPath(type, photoName);
                    return string.Format("File {0} deleted<br />", fullPath);
                }

                return string.Format("File {0} will be deleted<br />", fullPath);
            }

            return "";
        }


        protected void GetDeletedFolderSize(object sender, EventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/pictures_deleted/"));
            lblFolderSize.Text = Directory.Exists(di.FullName)
                                     ? di.EnumerateFiles("*", SearchOption.AllDirectories).Sum(fi => fi.Length)/1024/1024 + "MB."
                                     : "Folder not found";
        }

        protected void CleanDeletedFolder(object sender, EventArgs e)
        {
            AdvantShop.Helpers.FileHelpers.DeleteDirectory(Server.MapPath("~/pictures_deleted/"));
        }
    }
}