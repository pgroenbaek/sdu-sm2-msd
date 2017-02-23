using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDSD.FluentNav.Metamodel
{
    public class View<EMenuTypes>
    {
        public Type Type { get; private set; }
        public MenuDefinition<EMenuTypes> MenuDefinition { get; private set; }

        private Dictionary<Type, Transition<EMenuTypes>> transitions = new Dictionary<Type, Transition<EMenuTypes>>();

        public View(Type viewType)
        {
            Type = viewType;
        }

        // TODO add transitions
    }
}
