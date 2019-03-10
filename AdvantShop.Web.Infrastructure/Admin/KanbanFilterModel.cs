using System.Collections.Generic;

namespace AdvantShop.Web.Infrastructure.Admin
{
    public class KanbanFilterModel<TColumn> where TColumn : KanbanColumnFilterModel
    {
        public KanbanFilterModel()
        {
            Columns = new List<TColumn>();
        }

        public List<TColumn> Columns { get; set; }

        public string ColumnId { get; set; }

        public string Search { get; set; }
    }

    public class KanbanColumnFilterModel
    {
        public KanbanColumnFilterModel()
        {
            Page = 1;
            CardsPerColumn = 10;
        }

        public KanbanColumnFilterModel(string id) : this()
        {
            Id = id;
        }

        public string Id { get; set; }

        private int _page = 1;
        public int Page
        {
            get { return _page; }
            set { _page = value > 0 && value < int.MaxValue ? value : 1; }
        }
        public int TotalCardsCount { get; set; }
        public int TotalPagesCount { get; set; }
        public int CardsPerColumn { get; set; }
    }
}