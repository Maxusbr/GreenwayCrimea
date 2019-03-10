using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Attachments;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using Lucene.Net.Store;
using Debug = AdvantShop.Diagnostics.Debug;

namespace AdvantShop.Core.Services.Helpers
{
    public static class FilesStorageService
    {
        private static readonly object ObjSync = new object();
        private static bool _isRun = false;

        private static bool IsRun
        {
            get
            {
                lock (ObjSync)
                    return _isRun;
            }
            set
            {
                lock (ObjSync)
                    _isRun = value;
            }
        }

        public static void RecalcAttachmentsSizeInBackground()
        {
            if (!IsRun)
                Task.Run(() => RecalcAttachmentsSize());
        }

        public static long RecalcAttachmentsSize()
        {
            IsRun = true;

            long length = 0;

            try
            {
                var folders = new List<string>()
                {
                    FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Big, ""),
                    FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Middle, ""),
                    FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Small, ""),

                    FoldersHelper.GetPathAbsolut(FolderType.UserFiles)
                };

                foreach (var folderType in AttachmentService.FolderTypes)
                {
                    folders.Add(FoldersHelper.GetPathAbsolut(folderType.Value));
                }

                var sw = new Stopwatch();
                sw.Start();

                foreach (var folder in folders)
                {
                    if (System.IO.Directory.Exists(folder))
                        length += FileHelpers.GetDirectorySize(folder);
                }

                length += FileHelpers.GetFilesInRootDirectory().Sum(x => x.Length);

                sw.Stop();

                SettingsMain.CurrentFilesStorageSize = length;
                SettingsMain.CurrentFilesStorageSwTime = sw.Elapsed.TotalMilliseconds;
                SettingsMain.CurrentFilesStorageLastUpdateTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            IsRun = false;

            return length;
        }

        public static void IncrementAttachmentsSize(string fileName)
        {
            SettingsMain.CurrentFilesStorageSize += FileHelpers.GetFileSize(fileName);
        }

        public static void IncrementAttachmentsSize(long size)
        {
            SettingsMain.CurrentFilesStorageSize += size;
        }

        public static void DecrementAttachmentsSize(string fileName)
        {
            SettingsMain.CurrentFilesStorageSize -= FileHelpers.GetFileSize(fileName);
        }

        public static void DecrementAttachmentsSize(long size)
        {
            SettingsMain.CurrentFilesStorageSize -= size;
        }
    }
}
