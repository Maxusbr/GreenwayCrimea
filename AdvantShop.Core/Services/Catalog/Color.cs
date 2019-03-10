namespace AdvantShop.Catalog
{
    public class Color
    {
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public string ColorCode { get; set; }
        public int SortOrder { get; set; }

        private bool _loadedPicture;

        private ColorPhoto _picture;
        public ColorPhoto IconFileName
        {
            get
            {
                if (_loadedPicture)
                    return _picture;

                _loadedPicture = true;

                return  _picture ?? (_picture = PhotoService.GetPhotoByObjId<ColorPhoto>(ColorId, PhotoType.Color));
            }
            set
            {
                _picture = value;
            }
        }
        public override string ToString()
        {
            return ColorName;
        }
    }
}