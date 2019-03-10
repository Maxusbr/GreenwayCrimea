using System.Collections.Generic;
using AdvantShop.Core.Services.MyAccount;

namespace AdvantShop.Core.Modules.Interfaces
{
    public interface IMyAccountTabs : IModule
    {
        IList<MyAccountTab> GetMyAccountTabs();
    }
}