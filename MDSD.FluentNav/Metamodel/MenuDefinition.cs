using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Metamodel
{
    public class MenuDefinition
    {
        public string MenuType { get; private set; }
        public Dictionary<int, Dictionary<string, object>> FeaturesAtPosition { get; private set; }

        public MenuDefinition(string menuType)
        {
            MenuType = menuType;
            FeaturesAtPosition = new Dictionary<int, Dictionary<string, object>>();
        }
    }
}
