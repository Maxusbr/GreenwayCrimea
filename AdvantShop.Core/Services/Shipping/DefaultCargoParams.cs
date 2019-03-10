namespace AdvantShop.Shipping
{
    public class DefaultWeightParams
    {
        public const string DefaultWeight = "DefaultWeight";
    }

    public class DefaultCargoParams : DefaultWeightParams
    {
        public const string DefaultLength = "DefaultLength";
        public const string DefaultWidth = "DefaultWidth";
        public const string DefaultHeight = "DefaultHeight";
    }
}