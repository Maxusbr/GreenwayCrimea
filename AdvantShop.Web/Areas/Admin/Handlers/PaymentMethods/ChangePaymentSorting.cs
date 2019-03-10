using System.Linq;
using AdvantShop.Payment;

namespace AdvantShop.Web.Admin.Handlers.PaymentMethods
{
    public class ChangePaymentSorting
    {
        private readonly int _methodId;
        private readonly int? _prevId;
        private readonly int? _nextId;

        public ChangePaymentSorting(int methodId, int? prevId, int? nextId)
        {
            _methodId = methodId;
            _prevId = prevId;
            _nextId = nextId;
        }

        public bool Execute()
        {
            var method = PaymentService.GetPaymentMethod(_methodId);
            if (method == null)
                return false;

            PaymentMethod prevMethod = null, nextMethod = null;

            if (_prevId != null)
                prevMethod = PaymentService.GetPaymentMethod(_prevId.Value);

            if (_nextId != null)
                nextMethod = PaymentService.GetPaymentMethod(_nextId.Value);

            if (prevMethod == null && nextMethod == null)
                return false;

            if (prevMethod != null && nextMethod != null)
            {
                if (nextMethod.SortOrder - prevMethod.SortOrder > 1)
                {
                    method.SortOrder = prevMethod.SortOrder + 1;
                    PaymentService.UpdatePaymentMethod(method, false);
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

        private void UpdateSortOrderForAll(PaymentMethod method, PaymentMethod prevMethod, PaymentMethod nextMethod)
        {
            var methods =
                PaymentService.GetAllPaymentMethods(false)
                    .Where(x => x.PaymentMethodId != method.PaymentMethodId)
                    .OrderBy(x => x.SortOrder)
                    .ToList();

            if (prevMethod != null)
            {
                var index = methods.FindIndex(x => x.PaymentMethodId == prevMethod.PaymentMethodId);
                methods.Insert(index + 1, method);
            }
            else if (nextMethod != null)
            {
                var index = methods.FindIndex(x => x.PaymentMethodId == nextMethod.PaymentMethodId);
                methods.Insert(index > 0 ? index - 1 : 0, method);
            }

            for (int i = 0; i < methods.Count; i++)
            {
                methods[i].SortOrder = i * 10 + 10;
                PaymentService.UpdatePaymentMethod(methods[i], false);
            }
        }
    }
}
