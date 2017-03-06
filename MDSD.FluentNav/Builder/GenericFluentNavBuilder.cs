using MDSD.FluentNav.Builder.Interfaces;
using MDSD.FluentNav.Metamodel;
using System;
using System.Collections.Generic;

namespace MDSD.FluentNav.Builder
{
    public class GenericFluentNavBuilder<TBaseView> : INavigationBuilder<TBaseView>,
        IViewGroupBuilder<TBaseView>, IContentBuilder<TBaseView>,
        ITransitionBuilder<TBaseView>, ITransitionConditionalBuilder<TBaseView>,
        IMenuBuilder<TBaseView>
    {
        private NavigationModel _navModel;

        private int _currentMenuDefPosition = 0;
        private string _currentEvent = null;
        private Type _firstViewType = null;
        private Type _currentViewType = null;
        private Menu _currentMenuDef = null;
        private Dictionary<string, Type> _currentTransitionsTo = new Dictionary<string, Type>();
        private Dictionary<Type, View> _allViews = new Dictionary<Type, View>();        
                   
        // TODOlist:
        //
        // - Adjust metamodel
        // - Adjust builder
        // - Adjust syntax
        // - Make sure the example works.

        public GenericFluentNavBuilder()
        {
            _navModel = new NavigationModel();
        }
 
        public NavigationModel FetchBuiltModel()
        {
            FlushAllViews();
            _navModel.Initialize();
            return _navModel;
        }

        public IViewGroupBuilder<TBaseView> View<TView>(string title = null) where TView : TBaseView
        {
            if(_firstViewType == null)
            {
                _navModel = new NavigationModel();
                _firstViewType = typeof(TView);
            }
            _currentViewType = _firstViewType;
            if (!_allViews.ContainsKey(_firstViewType))
            {
                _allViews.Add(_firstViewType, new View(_firstViewType, title));
            }
            else
            {
                _allViews[_firstViewType].Title = title;
            }
            return this;
        }

        /*public IViewBuilder<TBaseView, TMenuTypeSelector> View<TView>(string title = null) where TView : TBaseView
        {
            if (_currentMenuDef != null && !_currentMenuDef.MenuType.Equals(MenuType.Plain.ToString()))
            {
                FlushMenuTransitions();
            }

            _currentViewType = typeof(TView);
            if (!_allViews.ContainsKey(_currentViewType))
            {
                _allViews.Add(_currentViewType, new View(_currentViewType, title));
            }
            else
            {
                _allViews[_currentViewType].Title = title;
            }
            return this;
        }*/

        public IContentBuilder<TBaseView> Content()
        {
            //_currentMenuDef = new MenuDefinition(MenuType.Plain.ToString());
            return this;
        }

        public ITransitionBuilder<TBaseView> OnClick(int resourceId)
        {
            _currentEvent = Convert.ToString(resourceId);
            return this;
        }

        public IContentBuilder<TBaseView> NavigateTo<TView>() where TView : TBaseView
        {
            if (_currentEvent != null)
            {
                Type targetViewType = typeof(TView);
                FlushTransition(_currentViewType, _currentEvent, new Transition(targetViewType));
                _currentEvent = null;
            }
            return this;
        }

        public ITransitionConditionalBuilder<TBaseView> NavigateToIf<TView>(Func<bool> booleanExpression) where TView : TBaseView
        {
            if (_currentEvent != null)
            {
                Type targetViewType = typeof(TView);
                FlushTransition(_currentViewType, _currentEvent, new Transition(targetViewType, booleanExpression));
            }
            return this;
        }

        public ITransitionConditionalBuilder<TBaseView> ElseIfNavigateTo<TView>(Func<bool> booleanExpression) where TView : TBaseView
        {
            if (_currentEvent != null)
            {
                Type targetViewType = typeof(TView);
                FlushTransition(_currentViewType, _currentEvent, new Transition(targetViewType, booleanExpression));
            }
            return this;
        }

        public IContentBuilder<TBaseView> ElseNavigateTo<TView>() where TView : TBaseView
        {
            if (_currentEvent != null)
            {
                Type targetViewType = typeof(TView);
                FlushTransition(_currentViewType, _currentEvent, new Transition(targetViewType));
                _currentEvent = null;
            }
            return this;
        }

        public IMenuBuilder<TBaseView> Menu()
        {
            return this;
        }

        public IMenuBuilder<TBaseView> Type(string type)
        {
            throw new NotImplementedException();
        }

        public IMenuBuilder<TBaseView> Attribute(string key, object attribute)
        {
            throw new NotImplementedException();
        }

        private void FlushMenuTransitions()
        {
            foreach (string menuItemFrom in _currentTransitionsTo.Keys)
            {
                Type fromViewType = _currentTransitionsTo[menuItemFrom];
                _allViews[fromViewType].MenuDefinition = _currentMenuDef;
                foreach (string menuItemTo in _currentTransitionsTo.Keys)
                {
                    Type toViewType = _currentTransitionsTo[menuItemTo];
                    if (_allViews.ContainsKey(fromViewType))
                    {
                        _allViews[fromViewType].AddTransition(menuItemTo, new Transition(toViewType));
                    }
                }
            }

            _currentTransitionsTo.Clear();
            _currentMenuDef = null;
            _currentEvent = null;
            _currentMenuDefPosition = 0;
        }

        private void FlushTransition(Type sourceViewType, string eventId, Transition t)
        {
            if (!_allViews.ContainsKey(sourceViewType))
            {
                _allViews.Add(sourceViewType, new View(sourceViewType, null));
            }
            _allViews[sourceViewType].AddTransition(eventId, t);
        }

        private void FlushAllViews()
        {
            // Add all views, the view that was specified as topview is added as the first view.
            _navModel.AddView(_allViews[_firstViewType]);
            foreach (Type viewType in _allViews.Keys)
            {
                if (viewType == _firstViewType)
                {
                    continue;
                }
                View view = _allViews[viewType];
                _navModel.AddView(_allViews[viewType]);
            }
        }

        private void NextMenuItem()
        {
            _currentMenuDefPosition += 1;
        }
    }
}
