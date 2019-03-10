namespace AdvantShop.Web.Infrastructure.Handlers
{
    public interface ICommandHandler<TResult>
    {
        TResult Execute();
    }

    public interface ICommandHandler
    {
        void Execute();
    }
}
