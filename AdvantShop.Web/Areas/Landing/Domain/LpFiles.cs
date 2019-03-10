
namespace AdvantShop.App.Landing.Domain
{
    public class LpFiles
    {
        public const string UploadPictureFolder = "~/pictures/landing/";
        public const string UploadPictureFolderLanding = "~/pictures/landing/{0}/";                // lpId
        public const string UploadPictureFolderLandingBlock = "~/pictures/landing/{0}/{1}/";       // lpId, blockId
        public const string UploadPictureFolderLandingBlockRelative = "pictures/landing/{0}/{1}/"; // lpId, blockId

        public const string LpStaticPath = "~/areas/landing/";
    }
}
