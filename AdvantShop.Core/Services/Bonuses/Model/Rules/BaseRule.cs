using System;
using AdvantShop.Core.Services.Bonuses.Model.Enums;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Diagnostics;
using Newtonsoft.Json;
using Quartz;

namespace AdvantShop.Core.Services.Bonuses.Model.Rules
{
    public class BaseRule:IJob
    {
        public static BaseRule Get(CustomRule model)
        {
            switch (model.RuleType)
            {
                case ERule.BirthDay: return creator<BirthDayRule>(model.Params);
                case ERule.CancellationsBonus: return creator<CancellationsBonusRule>(model.Params);
                case ERule.NewCard: return creator<NewCardRule>(model.Params);
                case ERule.ChangeGrade: return creator<ChangeGradeRule>(model.Params);
                case ERule.CleanExpiredBonus: return creator<CleanExpiredBonusRule>(model.Params);
                default:
                    throw new BlException(LocalizationService.GetResource("AdvantShop.Core.Services.Bonuses.Model.Rules.WrongType") + model.RuleType);
            }
        }

        private static BaseRule creator<T>(string p) where T : BaseRule, new()
        {
            return string.IsNullOrWhiteSpace(p) || p == "null" ? new T() : JsonConvert.DeserializeObject<T>(p);
        }

        public static string Set(BaseRule model)
        {
            return JsonConvert.SerializeObject(model);
        }
       
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Process(context);
            }
            catch (Exception e)
            {
                Debug.Log.Error(e);
            }
        }

        public virtual void Process(IJobExecutionContext contex)
        {
            
        }
    }
}
