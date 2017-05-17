namespace DDI.Shared.Models
{
    public interface IEntity : ICanTransmogrify
    {
        string DisplayName { get; }
        void AssignPrimaryKey();
    }
}
