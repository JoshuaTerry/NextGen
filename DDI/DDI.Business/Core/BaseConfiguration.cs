using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Business.Core
{
    public abstract class BaseConfiguration
    {

        public virtual void LoadProperty(string name, string value)
        {
        }

        public virtual string SaveProperty(string name)
        {
            return null;
        }

    }
}
