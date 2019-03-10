using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.CMS;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Web.Admin.Handlers.Voting
{
    public class ChangeVotingSorting
    {
        private readonly int _answersId;
        private readonly int? _prevId;
        private readonly int? _nextId;
        private readonly int _themeId;

        public ChangeVotingSorting(int answersId, int themeId, int? prevId, int? nextId)
        {
            _answersId = answersId;
            _prevId = prevId;
            _nextId = nextId;
            _themeId = themeId;
        }
        public bool Execute()
        {
            var answers = VoiceService.GetAllAnswers(_themeId);
            var answer = answers.Where(x => x.AnswerId == _answersId).ToList().First();
            if (answer == null)
                return false;

            Answer prevAnswer = null, nextAnswer = null;

            if (_prevId != null)
                prevAnswer = answers.Where(x => x.AnswerId == _prevId).ToList().First();

            if (_nextId != null)
                nextAnswer = answers.Where(x => x.AnswerId == _nextId).ToList().First();

            if (prevAnswer == null && nextAnswer == null)
                return false;

            if (prevAnswer != null && nextAnswer != null)
            {
                if (nextAnswer.Sort - prevAnswer.Sort > 1)
                {
                    answer.Sort = prevAnswer.Sort + 1;
                    VoiceService.UpdateAnswer(answer);
                }
                else
                {
                    UpdateSortOrderForAll(answer, prevAnswer, nextAnswer);
                }
            }
            else
            {
                UpdateSortOrderForAll(answer, prevAnswer, nextAnswer);
            }

            return true;
        }

        private void UpdateSortOrderForAll(Answer answer, Answer prevAnswer, Answer nextAnswer)
        {
            var answers =
                VoiceService.GetAllAnswers(_themeId)
                    .Where(x => x.AnswerId != answer.AnswerId)
                    .OrderBy(x => x.Sort)
                    .ToList();

            if (prevAnswer != null)
            {
                var index = answers.FindIndex(x => x.AnswerId == prevAnswer.AnswerId);
                answers.Insert(index + 1, answer);
            }
            else if (nextAnswer != null)
            {
                var index = answers.FindIndex(x => x.AnswerId == nextAnswer.AnswerId);
                answers.Insert(index > 0 ? index - 1 : 0, answer);
            }

            for (int i = 0; i < answers.Count; i++)
            {
                answers[i].Sort = i * 10 + 10;
                VoiceService.UpdateAnswer(answers[i]);
            }
        }
    }
}
