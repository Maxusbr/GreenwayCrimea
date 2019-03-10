using AdvantShop.Core.Common.Extensions;
using System;

namespace AdvantShop.Web.Admin.Models.Files
{
    public class FilesModel
    {
        public string Id { get; set; }

        public string FileName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public string Link { get; set; }


        public string DateCreatedString { get { return DateCreated.ToString("dd.MM.yyyy HH:mm:ss"); } }
        public string DateModifiedString { get { return DateModified.ToString("dd.MM.yyyy HH:mm:ss"); } }

        public long FileSize { get; set; }
        public string FileSizeString
        {
            get
            {

                string[] sizes = { "B", "KB", "MB", "GB" };
                double len = FileSize;
                int order = 0;
                while (len >= 1024 && ++order < sizes.Length)
                {
                    len = len / 1024;
                }
                return string.Format("{0:0.##} {1}", len, sizes[order]);
            }
        }
    }
}
