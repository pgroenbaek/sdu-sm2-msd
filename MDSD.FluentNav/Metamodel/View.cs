using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDSD.FluentNav.Metamodel
{
    public class View
    {
        public Type Type { get; private set; }
        public string Title { get; set; }
        public Menu MenuDefinition { get; set; }
        
        internal Dictionary<string, List<Transition>> _transitions;

        public View(Type viewType, string title)
        {
            Type = viewType;
            Title = title;
            _transitions = new Dictionary<string, List<Transition>>();
        }

        public void AddTransition(string eventId, Transition transition)
        {
            transition.SourceView = this;
            if (!_transitions.ContainsKey(eventId))
            {
                _transitions[eventId] = new List<Transition>();
            }
            _transitions[eventId].Add(transition);
        }
        
        internal Transition NextTransition(string eventId)
        {
            if (_transitions.ContainsKey(eventId))
            {
                // Evaluate from first to last condition.
                for (int i = 0; i < _transitions[eventId].Count; i++)
                {
                    // Return the transition where the first null or true was encountered.
                    // The condition being null means either "no condition" or it corresponds to the final "else".
                    Transition t = _transitions[eventId][i];
                    
                    if (t.Conditional == null)
                    {
                        return t;
                    }
                    
                    if(t.Conditional.Invoke())
                    {
                        return t;
                    }
                }
            }
            return null;
        }

    }
}
