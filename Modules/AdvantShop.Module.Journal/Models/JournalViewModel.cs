namespace AdvantShop.Module.Journal.Models
{
    public class JournalViewModel
    {
        public string ColorScheme { get; set; }

        public JournalProductViewModel Products { get; set; }

        public string ViewMode { get; set; }

        public bool DisplayHead { get; set; }

        public bool DisplayBottom { get; set; }

        public string HeadBlock { get; set; }

        public bool IsLeft { get; set; }

        public int Page { get; set; }

        public string CategoryName { get; set; }
    }
}
