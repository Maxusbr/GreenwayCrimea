using System.Linq;
using AdvantShop.Shipping;

namespace AdvantShop.Web.Admin.Handlers.ShippingMethods
{
    public class ChangeShippingSorting
    {
        private readonly int _methodId;
        private readonly int? _prevId;
        private readonly int? _nextId;

        public ChangeShippingSorting(int methodId, int? prevId, int? nextId)
        {
            _methodId = methodId;
            _prevId = prevId;
            _nextId = nextId;
        }

        public bool Execute()
        {
            var method = ShippingMethodService.GetShippingMethod(_methodId);
            if (method == null)
                return false;

            ShippingMethod prevMethod = null, nextMethod = null;

            if (_prevId != null)
                prevMethod = ShippingMethodService.GetShippingMethod(_prevId.Value);

            if (_nextId != null)
                nextMethod = ShippingMethodService.GetShippingMethod(_nextId.Value);

            if (prevMethod == null && nextMethod == null)
                return false;

            if (prevMethod != null && nextMethod != null)
            {
                if (nextMethod.SortOrder - prevMethod.SortOrder > 1)
                {
                    method.SortOrder = prevMethod.SortOrder + 1;
                    ShippingMethodService.UpdateShippingMethod(method, false);
                }
                else
                {
                    UpdateSortOrderForAll(method, prevMethod, nextMethod);
                }
            }
            else
            {
                UpdateSortOrderForAll(method, prevMethod, nextMethod);
            }

            return true;
        }

        private void UpdateSortOrderForAll(ShippingMethod method, ShippingMethod prevMethod, ShippingMethod nextMethod)
        {
            var methods =
                ShippingMethodService.GetAllShippingMethods()
                    .Where(x => x.ShippingMethodId != method.ShippingMethodId)
                    .OrderBy(x => x.SortOrder)
                    .ToList();

            if (prevMethod != null)
            {
                var index = methods.FindIndex(x => x.ShippingMethodId == prevMethod.ShippingMethodId);
                methods.Insert(index + 1, method);
            }
            else if (nextMethod != null)
            {
                var index = methods.FindIndex(x => x.ShippingMethodId == nextMethod.ShippingMethodId);
                methods.Insert(index > 0 ? index - 1 : 0, method);
            }

            for (int i = 0; i < methods.Count; i++)
            {
                methods[i].SortOrder = i * 10 + 10;
                ShippingMethodService.UpdateShippingMethod(methods[i], false);
            }
        }
    }
}
