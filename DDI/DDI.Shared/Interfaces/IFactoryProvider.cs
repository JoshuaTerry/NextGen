using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
