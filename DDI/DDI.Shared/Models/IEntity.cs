using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models
{
    public interface IEntity : ICanTransmogrify
    {
        string DisplayName { get; }
        void AssignPrimaryKey();
    }
}
