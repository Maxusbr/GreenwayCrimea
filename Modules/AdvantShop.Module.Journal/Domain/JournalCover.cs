using System.Collections.Generic;

namespace AdvantShop.Module.Journal.Domain
{
    enum JournalCoverType
    {
        None = 0,
        Yellow = 1,
        Red = 2,
        Blue = 3,
        Green = 4,
        Gray = 5,
        BlueDark = 6
    }

    enum JournalViewMode
    {
        Tile = 0,
        List = 1
    }

    public class JournalExport
    {
        public JournalExport()
        {
            CategoryIds = new List<int>();
        }

        public List<int> CategoryIds { get; set; }
    }
}
