//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace AdvantShop.CMS
{
    public class Answer
    {
        public int AnswerId { get; set; }
        public int FkidTheme { get; set; }
        public string Name { get; set; }
        public int CountVoice { get; set; }
        public int Sort { get; set; }
        public bool IsVisible { get; set; }
        public int Percent { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateModify { get; set; }
    }

    public class VoiceTheme
    {
        public int VoiceThemeId { get; set; }
        public int PsyId { get; set; }
        public int CountVoice { get; set; }
        public string Name { get; set; }
        public bool IsHaveNullVoice { get; set; }
        public bool IsDefault { get; set; }
        public bool IsClose { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateModify { get; set; }

        private List<Answer> _answers;
        public List<Answer> Answers
        {
            get
            {
                if (_answers == null)
                {
                     _answers = VoiceService.GetAllAnswers(VoiceThemeId).Where(x => x.IsVisible).OrderBy(x => x.Sort).ToList();
                    int sum = _answers.Sum(a => a.CountVoice);
                    if (sum == 0) 
                        _answers.ForEach(a => a.Percent = 0);
                    else 
                        _answers.ForEach(a => a.Percent = (int)Math.Round((decimal)a.CountVoice * 100 / sum));
                }

                return _answers;
            }
        }
    }
}