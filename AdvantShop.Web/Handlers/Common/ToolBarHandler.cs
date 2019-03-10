using System;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.ViewModel.Common;

namespace AdvantShop.Handlers.Common
{
    public class ToolBarHandler
    {
        public ToolBarBottomViewModel Get()
        {
            var model = new ToolBarBottomViewModel();

            foreach (var module in AttachedModules.GetModules<IShoppingCart>())
            {
                var moduleObject = (IShoppingCart)Activator.CreateInstance(module, null);
                model.ShowConfirmButton &= moduleObject.ShowConfirmButtons;
            }

            return model;
        }
    }
}