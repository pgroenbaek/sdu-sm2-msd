using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Metamodel
{
    public class MenuDefinition<EMenuTypes>
    {
        public EMenuTypes MenuTypes { get; private set; }
        public Dictionary<int, Dictionary<string, object>> FeaturesAtPosition { get; private set; }

        public MenuDefinition()
        {

        }
        
    }
}
