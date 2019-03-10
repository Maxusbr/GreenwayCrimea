//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Core.Modules.Interfaces
{
    public interface IInplaceEditor : IModule
    {
        void Edit(int id, string type, string field, string content);
    }
}
