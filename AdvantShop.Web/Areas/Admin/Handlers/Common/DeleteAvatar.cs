using AdvantShop.Customers;
using AdvantShop.FilePath;
using AdvantShop.Helpers;

namespace AdvantShop.Web.Admin.Handlers.Common
{
    public class DeleteAvatar
    {
        private readonly Customer _customer;

        public DeleteAvatar(Customer customer)
        {
            _customer = customer;
        }

        public string Execute()
        {
            FileHelpers.DeleteFile(FoldersHelper.GetPathAbsolut(FolderType.Avatar, _customer.Avatar));

            _customer.Avatar = null;
            CustomerService.UpdateCustomer(_customer);

            return FoldersHelper.GetPath(FolderType.Avatar, _customer.Avatar, false); ;
        }
    }
}
