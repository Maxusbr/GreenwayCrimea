using System;
using System.Drawing;
using System.IO;
using System.Linq;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;

namespace AdvantShop.Web.Admin.Handlers.Common
{
    public class UploadAvatarCropped
    {
        private readonly Customer _customer;
        private readonly string _fileName;
        private readonly string _base64String;

        public UploadAvatarCropped(Customer customer, string fileName, string base64String)
        {
            _customer = customer;
            _fileName = fileName;
            _base64String = base64String;
        }

        public string Execute()
        {
            if (!FileHelpers.CheckFileExtension(_fileName, EAdvantShopFileTypes.Image)) 
                return null;
            
            FileHelpers.DeleteFile(FoldersHelper.GetPathAbsolut(FolderType.Avatar, _customer.Avatar));

            var newFileName = _customer.Id + Path.GetExtension(_fileName.Split('?')[0]);
            var newFilePath = FoldersHelper.GetPathAbsolut(FolderType.Avatar, newFileName);

            try
            {
                var base64 = _base64String.Split(new[] { "base64," }, StringSplitOptions.None)[1];
                var bytes = Convert.FromBase64String(base64);

                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    using (Image image = Image.FromStream(ms, true))
                    {
                        FileHelpers.SaveResizePhotoFile(newFilePath, 270, 270, image, 100);
                    }
                }

                _customer.Avatar = newFileName;
                CustomerService.UpdateCustomer(_customer);

                // delete temp fileif exists
                var filePath = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, _fileName.Split('/').LastOrDefault());
                FileHelpers.DeleteFile(filePath);
            }
            catch (Exception ex)
            {
                Debug.Log.Error("UploadAvatarCropped", ex);
                return null;
            }

            return FoldersHelper.GetPath(FolderType.Avatar, _customer.Avatar, false);
        }
    }
}
