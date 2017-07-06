namespace DDI.Shared
{
    public interface ITestRepository<T> : IRepository<T> where T : class
    {
        string ModifiedPropertyList { get; set; }
        void Clear();
    }
}
