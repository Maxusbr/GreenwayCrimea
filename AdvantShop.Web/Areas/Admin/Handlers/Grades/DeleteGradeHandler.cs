using AdvantShop.Core;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.Services.Bonuses.Service;
using AdvantShop.Web.Infrastructure.Handlers;

namespace AdvantShop.Web.Admin.Handlers.Grades
{
    public class DeleteGradeHandler:AbstractCommandHandler<bool>
    {
        private readonly int _gradeId;
        public DeleteGradeHandler(int gradeId)
        {
            _gradeId = gradeId;
        }
        protected override void Validate()
        {
            var isUse = GradeService.IsUsed(_gradeId);
            if (isUse)
                throw new BlException(T("Admin.Grades.DeleteGradeHandler.Error.GradeIsUsed"),"");

            if (_gradeId == BonusSystem.DefaultGrade)
                throw new BlException(T("Admin.Grades.DeleteGradeHandler.Error.ItDefault"), "");
        }

        protected override bool Handle()
        {
            GradeService.Delete(_gradeId);
            return true;
        }
    }
}
