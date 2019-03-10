using System;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Bonuses.Model;
using AdvantShop.Core.Services.Bonuses.Service;
using AdvantShop.Diagnostics;
using AdvantShop.Web.Admin.Models.Grades;
using AdvantShop.Web.Infrastructure.Handlers;

namespace AdvantShop.Web.Admin.Handlers.Grades
{
    public class AddUpdateGrade : AbstractCommandHandler<int>
    {
        private readonly GradeModel _model;
        private bool _editMode;

        public AddUpdateGrade(GradeModel model, bool editMode)
        {
            _model = model;
            _editMode = editMode;
        }

        protected override void Validate()
        {
            
        }

        protected override int Handle()
        {
            var grade = new Grade()
            {
                Name = _model.Name.DefaultOrEmpty(),
                SortOrder = _model.SortOrder,
                BonusPercent = _model.BonusPercent,
                PurchaseBarrier = _model.PurchaseBarrier
            };

            try
            {
                if (_editMode)
                {
                    grade.Id = _model.Id;
                    GradeService.Update(grade);
                }
                else
                {
                    grade.Id = GradeService.Add(grade);
                }

                return grade.Id;
            }
            catch (Exception ex)
            {
                Debug.Log.Error("AddUpdate grade", ex);
            }

            return 0;
        }
    }
}
