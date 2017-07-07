namespace DDI.Shared
{
    public interface IBusinessLogic
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
