using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Conversion
{
    public interface IDataConversion
    {
        void Execute(ConversionArgs args);
    }
}
