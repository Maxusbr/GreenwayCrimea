//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Drawing;

namespace AdvantShop.Core.Modules.Interfaces
{
    public interface IProcessPhoto : IModule
    {
        Image DoProcessPhoto(Image photo);
    }
}
