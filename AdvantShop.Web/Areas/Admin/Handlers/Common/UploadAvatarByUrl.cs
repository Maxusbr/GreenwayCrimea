using System;
using System.IO;
using System.Linq;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;

namespace AdvantShop.Web.Admin.Handlers.Common
{
    public class UploadAvatarByUrl
    {
        private readonly Customer _customer;
        private readonly string _url;

        public UploadAvatarByUrl(Customer customer, string url)
        {
            _customer = customer;
            _url = url;
        }

        public string Execute()
        {
            if (string.IsNullOrWhiteSpace(_url))
                return null;
            
            try
            {
                if (_url.Contains("http://") || _url.Contains("https://"))
                {
                    var uri = new Uri(_url);
                    var photoname = uri.PathAndQuery.Trim('/').Replace("/", "-");
                    photoname = Path.GetInvalidFileNameChars().Aggregate(photoname, (current, c) => current.Replace(c.ToString(), ""));
                    
                    photoname = Guid.NewGuid().ToString() + Path.GetExtension(photoname);

                    var filePath = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photoname);
                    
                    if (FileHelpers.DownloadRemoteImageFile(_url, filePath))
                    {
                        return FoldersHelper.GetPath(FolderType.ImageTemp, photoname, false);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error("UploadAvatarByUrl", ex);
            }

            return null;
        }
    }
}
