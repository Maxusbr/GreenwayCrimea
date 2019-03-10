using System;

namespace AdvantShop.Web.Admin.Models.Voting
{
    public  class VotingAnswerFilterResultModel
    {
        public int ID { get; set; }
        public int ThemeId { get; set; }
        public string Name { get; set; }
        public int CountVoice { get; set; }
        public int SortOrder { get; set; }
        public bool IsVisible { get; set; }
        private DateTime _dateadd { get; set; }
        public string DateAdded {
            get
            {
                return _dateadd.ToString("dd.MM.yyyy HH:mm");
            }
            set
            {
                DateTime date;
                DateTime.TryParse(value, out date);
                _dateadd = date;
            }
        }
        private DateTime _datemodyfy { get; set; }
        public string DateModify {
            get
            {
                return _datemodyfy.ToString("dd.MM.yyyy HH:mm");
            }
            set
            {
                DateTime date;
                DateTime.TryParse(value, out date);
                _datemodyfy = date;
            }
        }
    }
}
