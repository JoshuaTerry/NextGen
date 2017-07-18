using System.ComponentModel.DataAnnotations;

namespace DDI.Shared.Models
{
    public interface IEntity : ICanTransmogrify
    {
        string DisplayName { get; }

        [Timestamp]
        byte[] RowVersion { get; set; }
        void AssignPrimaryKey();
    }
}
