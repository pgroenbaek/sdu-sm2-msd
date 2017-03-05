using MDSD.FluentNav.Validator;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Metamodel
{
    public class NavigationModel
    {
        public View CurrentView { get; private set; }
        public bool IsModelBuilt { get; private set; }
        
        internal Dictionary<Type, View> _views;
        internal Stack<Transition> _transitionStack;

        public NavigationModel()
        {
            IsModelBuilt = false;
            _views = new Dictionary<Type, View>();
            _transitionStack = new Stack<Transition>();
        }

        public void AddView(View view)
        {
            if(IsModelBuilt)
            {
                return;
            }

            if(_views.Count == 0)
            {
                CurrentView = view;
            }
            _views.Add(view.Type, view);
        }

        public void Initialize()
        {
            IsModelBuilt = true;
            NavigationModelValidator.Validate(this);
        }

        // Returns true if transition stack is empty.
        public bool HandleBackPressed()
        {
            if(!IsModelBuilt)
            {
                throw new InvalidOperationException("Cannot use the model before it has been built. Call 'Initialize()' before attempting to do so.");
            }

            if (_transitionStack.Count > 0)
            {
                CurrentView = _transitionStack.Peek().SourceView;
                _transitionStack.Pop();
                return false;
            }
            return true;
        }

        public Transition HandleEvent(string eventId)
        {
            if (!IsModelBuilt)
            {
                throw new InvalidOperationException("Cannot use the model before it has been built. Call 'Initialize()' before attempting to do so.");
            }

            if (CurrentView == null)
            {
                return null;
            }

            Transition nextTransition = CurrentView.NextTransition(eventId);
            if(nextTransition != null && _views.ContainsKey(nextTransition.TargetView))
            {
                CurrentView = _views[nextTransition.TargetView];
                _transitionStack.Push(nextTransition);
            }
            return nextTransition;
        }
    }
}
