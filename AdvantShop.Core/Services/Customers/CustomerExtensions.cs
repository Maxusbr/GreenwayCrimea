using AdvantShop.Orders;

namespace AdvantShop.Customers
{
    public static class CustomerExtensions
    {
        public static string GetFullName(this Customer customer)
        {
            var result = "";
            result += customer.LastName;

            if (!string.IsNullOrEmpty(customer.FirstName) && customer.LastName != customer.FirstName)
                result += (result != "" ? " " : "") + customer.FirstName;

            if (!string.IsNullOrEmpty(customer.Patronymic))
                result += (result != "" ? " " : "") + customer.Patronymic;

            return result;
        }

        public static string GetShortName(this Customer customer)
        {
            var result = "";
            result += customer.LastName;

            if (!string.IsNullOrEmpty(customer.FirstName) && customer.FirstName != customer.LastName)
                result += (result != "" ? " " : "") + customer.FirstName;
            
            return result;
        }

        public static string GetFullName(this OrderCustomer customer)
        {
            var result = "";
            result += customer.LastName;

            if (!string.IsNullOrEmpty(customer.FirstName) && customer.LastName != customer.FirstName)
                result += (result != "" ? " " : "") + customer.FirstName;

            if (!string.IsNullOrEmpty(customer.Patronymic))
                result += (result != "" ? " " : "") + customer.Patronymic;

            return result;
        }
    }
}
