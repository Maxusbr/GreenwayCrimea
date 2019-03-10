using System;
using AdvantShop.CMS;

namespace AdvantShop.Web.Admin.Models.Voting
{
    public class VotingThemeFilterResultModel
    {
        public int ID { get; set; }
        public int PsyID { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public bool IsHaveNullVoice { get; set; }
        public bool IsClose { get; set; }
        private DateTime _dateadd { get; set; }
        public string DateAdded
        {
            get {
                return _dateadd.ToString("dd.MM.yyyy HH:mm");
            }
            set {
                DateTime date;
                DateTime.TryParse(value, out date);
                _dateadd = date;
            }
        }
        private DateTime _datemodyf { get; set; }
        public string DateModify
        {
            get
            {
                return _datemodyf.ToString("dd.MM.yyyy HH:mm");
            }
            set
            {
                DateTime date;
                DateTime.TryParse(value, out date);
                _datemodyf = date;
            }
        }
        public string CountAnswers { get; set; }
    }
}
