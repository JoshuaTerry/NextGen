namespace DDI.Shared.Interfaces
{
    /// <summary>
    /// Interface for a provider class that can provide a reference to an IFactory instance.
    /// </summary>
    public interface IFactoryProvider
    {
        IFactory GetFactory();
    }
}
