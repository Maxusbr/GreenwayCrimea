namespace AdvantShop.Core.Models
{
    public class Pager
    {
        public Pager()
        {
            CurrentPage = 1;
            VisiblePages = 5;
            BlockPages = 2;
            DisplayPrevNext = true;
            DisplayArrows = false;
        }

        public int TotalItemsCount { get; set; }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

        public int VisiblePages { get; set; }

        public int BlockPages { get; set; }

        public bool DisplayShowAll { get; set; }

        public bool DisplayPrevNext { get; set; }

        public bool DisplayArrows { get; set; }
    }
}
