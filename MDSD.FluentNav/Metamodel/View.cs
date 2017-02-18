using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDSD.FluentNav.Metamodel
{
    public class View
    {
        public Type Type { get; private set; }

        private Dictionary<Type, Transition> transitions = new Dictionary<Type, Transition>();

        public View(Type viewType)
        {
            Type = viewType;
        }

        // TODO add transitions
    }
}
