namespace DDI.Shared
{
    /// <summary>
    /// Defines the required functionality for a class to act as a "Repository" by accessing and
    /// returning entities from a data store.
    /// </summary>
    public interface ICachedRepository<T> : IRepository<T> where T : class
    {

        #region Public Methods

        void InvalidateCache();

        #endregion

    }
}
