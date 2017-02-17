using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Models;

namespace DDI.Business
{
    public interface IEntityLogic
    {
        void Validate(IEntity entity);
        void UpdateSearchDocument(IEntity entity);
    }
}
