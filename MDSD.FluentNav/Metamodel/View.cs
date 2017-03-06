using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDSD.FluentNav.Metamodel
{
    public class View
    {
        public Type Type { get; private set; }
        public string Title { get; private set; }
        public Menu MenuDefinition { get; set; } = null;

        public Dictionary<string, List<Transition>> Transitions;

        public View(Type viewType, string title)
        {
            Type = viewType;
            Title = title;
            Transitions = new Dictionary<string, List<Transition>>();
        }
    }
}
