namespace AdvantShop.Core.Common.Attributes
{
    public interface IAttribute<T>
    {
        T Value { get; }
    }
}