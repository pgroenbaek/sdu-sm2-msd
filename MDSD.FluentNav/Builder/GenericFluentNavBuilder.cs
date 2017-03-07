using MDSD.FluentNav.Builder.Interfaces;
using MDSD.FluentNav.Metamodel;
using System;
using System.Collections.Generic;

namespace MDSD.FluentNav.Builder
{
    public class GenericFluentNavBuilder<TBaseView> : INavigationBuilder<TBaseView>,
        IViewGroupBuilder<TBaseView>, IContentBuilder<TBaseView>,
        ITransitionBuilder<TBaseView>, ITransitionConditionalBuilder<TBaseView>,
        IMenuBuilder<TBaseView>, IViewBuilder<TBaseView>
    {
        private NavigationModel _navModel;
        
        private string _currentEvent = null;
        private View _currentView = null;
        private Stack<ViewGroup> _currentViewGroup = new Stack<ViewGroup>();

        public GenericFluentNavBuilder()
        {
            _navModel = new NavigationModel();
        }
 
        public NavigationModel Build()
        {
            if (_currentViewGroup.Count > 0)
            {
                throw new InvalidOperationException("Unbalanced viewgroups. Is an EndViewGroup() missing somewhere?");
            }
            _navModel.Initialize();
            return _navModel;
        }
        
        public IViewBuilder<TBaseView> View<TView>(string title = null) where TView : TBaseView
        {
            FlushView();
            _currentView = new View(typeof(TView), title);
            return this;
        }

        public IViewGroupBuilder<TBaseView> BeginViewGroup()
        {
            FlushView();
            _currentViewGroup.Push(new ViewGroup());
            return this;
        }

        public IMenuBuilder<TBaseView> Menu()
        {
            if(_currentView != null)
            {
                _currentView.MenuDefinition = new Menu();
            }
            return this;
        }

        public IContentBuilder<TBaseView> Content()
        {
            return this;
        }

        public IViewBuilder<TBaseView> EndViewGroup()
        {
            FlushViewGroup();
            return this;
        }

        public ITransitionBuilder<TBaseView> OnClick(int resourceId)
        {
            _currentEvent = Convert.ToString(resourceId);
            return this;
        }

        public IContentBuilder<TBaseView> NavigateTo<TView>() where TView : TBaseView
        {
            if (_currentEvent != null && _currentView != null)
            {
                Type targetViewType = typeof(TView);
                FlushTransition(_currentEvent, new Transition(targetViewType, _currentView));
                _currentEvent = null;
            }
            return this;
        }

        public ITransitionConditionalBuilder<TBaseView> NavigateToIf<TView>(Func<bool> condition) where TView : TBaseView
        {
            if (_currentEvent != null && _currentView != null)
            {
                Type targetViewType = typeof(TView);
                FlushTransition(_currentEvent, new Transition(targetViewType, _currentView, condition));
            }
            return this;
        }

        public ITransitionConditionalBuilder<TBaseView> ElseIfNavigateTo<TView>(Func<bool> condition) where TView : TBaseView
        {
            if (_currentEvent != null && _currentView != null)
            {
                Type targetViewType = typeof(TView);
                FlushTransition(_currentEvent, new Transition(targetViewType, _currentView, condition));
            }
            return this;
        }

        public IContentBuilder<TBaseView> ElseNavigateTo<TView>() where TView : TBaseView
        {
            if (_currentEvent != null && _currentView != null)
            {
                Type targetViewType = typeof(TView);
                FlushTransition(_currentEvent, new Transition(targetViewType, _currentView));
                _currentEvent = null;
            }
            return this;
        }

        public IMenuBuilder<TBaseView> Type(string type)
        {
            if (_currentView != null && _currentView.MenuDefinition != null)
            {
                _currentView.MenuDefinition.MenuType = type;
            }
            return this;
        }

        public IMenuBuilder<TBaseView> Attribute(string key, object attribute)
        {
            if (_currentView != null && _currentView.MenuDefinition != null)
            {
                _currentView.MenuDefinition.MenuAttributes.Add(key, attribute);
            }
            return this;
        }

        public void FlushTransition(string eventId, Transition transition)
        {
            if (_currentView == null)
            {
                return;
            }

            if (!_currentView.Transitions.ContainsKey(eventId))
            {
                _currentView.Transitions[eventId] = new List<Transition>();
            }
            _currentView.Transitions[eventId].Add(transition);

            _currentEvent = null;
        }

        private void FlushView()
        {
            if (_currentView == null)
            {
                return;
            }

            if(_currentViewGroup.Count == 0)
            {
                _navModel.AllViews.Add(_currentView);
            }
            else if(_currentViewGroup.Count > 0)
            {
                _currentViewGroup.Peek().SubViews.Add(_currentView);
            }

            _currentView = null;
        }

        private void FlushViewGroup()
        {
            if (_currentViewGroup.Count == 1)
            {
                ViewGroup viewGroup = _currentViewGroup.Pop();

                if (viewGroup.MenuDefinition != null)
                {
                    // Build navigation within menu
                    foreach (View viewFrom in viewGroup.SubViews)
                    {
                        int count = 0;
                        foreach (View viewTo in viewGroup.SubViews)
                        {
                            Type toViewType = viewTo.Type;
                            string eventId = "m-" + toViewType.ToString();
                            viewGroup.MenuDefinition.MenuAttributes.Add("itemClick" + count, eventId);
                            viewFrom.Transitions.Add(eventId, new List<Transition>());
                            viewFrom.Transitions[eventId].Add(new Transition(toViewType, viewFrom));
                            count++;
                        }
                    }
                }

                _navModel.AllViews.Add(viewGroup);
            }
            else if (_currentViewGroup.Count > 1)
            {
                ViewGroup viewGroup = _currentViewGroup.Pop();
                _currentViewGroup.Peek().SubViews.Add(viewGroup);
            }
        }
        
    }
}
