using AdvantShop.Configuration;
using AdvantShop.Customers;

namespace AdvantShop.Core.Services.InplaceEditor
{
    public enum InplaceType
    {
        StaticPage = 0,
        StaticBlock = 1,
        NewsItem = 2,
        Category = 3,
        Product = 4,
        Phone = 5,
        Offer = 6,
        Property = 7,
        Brand = 8,
        Image = 9,
        Meta = 10,
        Review = 11
    }

    public enum StaticPageInplaceField
    {
        PageText = 1
    }

    public enum StaticBlockInplaceField
    {
        Content = 0
    }

    public enum NewsInplaceField
    {
        TextAnnotation = 1,
        TextToPublication = 2
    }

    public enum CategoryInplaceField
    {
        Description = 1,
        BriefDescription = 2
    }

    public enum ReviewInplaceField
    {
        Message = 1,
        Name = 2
    }

    public enum ProductInplaceField
    {
        ArtNo = 1,
        BriefDescription = 2,
        Description = 3,
        Unit = 4,
        Weight = 5
    }

    public enum PhoneInplaceField
    {
        Number = 0
    }

    public enum OfferInplaceField
    {
        Price = 0,
        DiscountAbs = 1,
        DiscountPercent = 2,
        Amount = 3,
        ArtNo = 4
    }

    public enum PropertyInplaceField
    {
        Name = 0,
        Value = 1
    }

    public enum BrandInplaceField
    {
        Description = 1,
        BriefDescription = 2
    }

    public enum ImageInplaceField
    {
        Logo = 0,
        Brand = 1,
        News = 2,
        Carousel = 3,
        CategoryBig = 4,
        CategorySmall = 5,
        Product = 6,
        Review = 7
    }

    public enum ImageInplaceCommands
    {
        Add = 0,
        Update = 1,
        Delete = 2
    }

    public enum MetaInplaceField
    {
        Name = 0,
        H1 = 1,
        MetaKeywords = 2,
        MetaDescription = 3,
        Title = 4
    }

	public enum TagInplaceField
	{
		Description = 1,
		BriefDescription = 2
	}

    public class InplaceEditorService
    {
        public static bool CanUseInplace(RoleAction role)
        {
            return SettingsMain.EnableInplace &&
                   (CustomerContext.CurrentCustomer.IsAdmin ||
                    CustomerContext.CurrentCustomer.IsVirtual ||
                    (CustomerContext.CurrentCustomer.IsModerator && CustomerContext.CurrentCustomer.HasRoleAction(role)));
        }

        public static bool DisplayInplace()
        {
            return  (CustomerContext.CurrentCustomer.IsAdmin ||
                    CustomerContext.CurrentCustomer.IsVirtual ||
                    CustomerContext.CurrentCustomer.IsModerator);
        }

        public static string PrepareContent(string content){
            var result = string.Empty;

            if (!string.IsNullOrEmpty(content))
            {
                result = content.Replace("<script>", "<script type=\"inplace\">").Replace("type=\"text/javascript\"", "type=\"inplace\"");
            }

            return result;
        }
    }
}