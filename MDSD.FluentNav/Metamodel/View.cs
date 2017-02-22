using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDSD.FluentNav.Metamodel
{
    public class View<ViewType>
    {
        public ViewType Type { get; private set; }

        private Dictionary<ViewType, Transition<ViewType>> transitions = new Dictionary<ViewType, Transition<ViewType>>();

        public View(ViewType viewType)
        {
            Type = viewType;
        }

        // TODO add transitions
    }
}
