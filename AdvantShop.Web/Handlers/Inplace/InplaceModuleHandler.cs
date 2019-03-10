using System;
using System.Linq;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;

namespace AdvantShop.Handlers.Inplace
{
    public class InplaceModuleHandler
    {
        public object Execute(int id, string type, string field, string content)
        {
            foreach (var cls in AttachedModules.GetModules<IInplaceEditor>())
            {
                var classInstance = (IInplaceEditor)Activator.CreateInstance(cls, null);
                classInstance.Edit(id, type, field, content);
            }
            return true;
        }
    }
}