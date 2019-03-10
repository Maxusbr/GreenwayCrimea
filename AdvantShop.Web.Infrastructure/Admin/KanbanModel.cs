using System.Collections.Generic;
using System.Linq;

namespace AdvantShop.Web.Infrastructure.Admin
{
    public class KanbanModel<TCard> where TCard : KanbanCardModel
    {
        public KanbanModel()
        {
            Columns = new List<KanbanColumnModel<TCard>>();
        }

        public string Name { get; set; }

        public List<KanbanColumnModel<TCard>> Columns { get; set; }

        public int NumberOfColumns { get { return Columns.Count; } }

        public int TotalCardsCount { get { return Columns.Sum(x => x.Cards.Count); } }
    }

    public class KanbanColumnModel<TCard> : KanbanColumnFilterModel where TCard : KanbanCardModel
    {
        public KanbanColumnModel()
        {
            Cards = new List<TCard>();
        }

        public string Name { get; set; }
        public string Class { get; set; }

        public List<TCard> Cards { get; set; }
    }

    public class KanbanCardModel
    {

    }
}