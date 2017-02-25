﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDSD.FluentNav.Metamodel
{
    public class View
    {
        public Type Type { get; private set; }
        public string Title { get; private set; }
        public MenuDefinition MenuDefinition { get; set; }

        private Dictionary<string, Transition> _transitions;

        public View(Type viewType, string title)
        {
            Type = viewType;
            Title = title;
            _transitions = new Dictionary<string, Transition>();
        }

        public void AddTransition(string eventId, Transition transition)
        {
            transition.SourceView = this;
            _transitions.Add(eventId, transition);
        }

        public Transition NextTransition(string eventId)
        {
            return _transitions[eventId];
        }

    }
}
