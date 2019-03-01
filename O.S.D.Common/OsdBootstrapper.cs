using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleInjector;

namespace O.S.D.Common
{
    public abstract class OsdBootstrapper
    {

        public abstract void Register(Container maincontainer);

        public abstract void Initialize(Container maincontainer);
    }
}
