using MDSD.FluentNav.Metamodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder
{
    public class GenericFluentNavBuilder<TBaseView> : INavigationBuilder<TBaseView>, IViewBuilder<TBaseView>,
        IViewBuilderPlain<TBaseView>, IViewBuilderMenuDrawer<TBaseView>, IViewBuilderMenuTabbedSlider<TBaseView>,
        ITransitionBuilder<TBaseView>, ITransitionBuilderConditional<TBaseView>
    {
        public enum MenuType // TODO, Move this into specific builders..
        {
            Plain,
            Drawer,
            TabbedSlider
        }

        private NavigationModel _navModel;
        
        private int _currentMenuDefPosition = 0;
        private string _currentEvent = null;
        private Type _firstViewType = null;
        private Type _currentViewType = null;
        private MenuDefinition _currentMenuDef = null;
        private Dictionary<string, Type> _currentTransitionsTo = new Dictionary<string, Type>();
        private Dictionary<Type, View> _allViews = new Dictionary<Type, View>();
        
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

        public IViewBuilder<TBaseView> TopView<TView>(string title = null) where TView : TBaseView
        {
            _navModel = new NavigationModel(); // Make sure to overwrite model, if an attempt is made to redefine it within the BuildNavigation() impl.
            _firstViewType = typeof(TView);
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

        public IViewBuilder<TBaseView> View<TView>(string title = null) where TView : TBaseView
        {
            if (_currentMenuDef != null && !_currentMenuDef.MenuType.Equals(MenuType.Plain.ToString()))
            {
                FlushMenuTransitions();
            }

            _currentViewType = typeof(T);
            if (!_allViews.ContainsKey(_currentViewType))
            {
                _allViews.Add(_currentViewType, new View(_currentViewType, title));
            }
            else
            {
                _allViews[_currentViewType].Title = title;
            }
            return this;
        }

        public IViewBuilderMenuDrawer<TBaseView> DrawerSpacer(string name = null)
        {
            if (_currentMenuDef == null || _currentViewType == null)
            {
                return this;
            }

            if (!_currentMenuDef.FeaturesAtPosition.ContainsKey(_currentMenuDefPosition))
            {
                _currentMenuDef.FeaturesAtPosition[_currentMenuDefPosition] = new Dictionary<string, object>();
            }
            _currentMenuDef.FeaturesAtPosition[_currentMenuDefPosition].Add("name", name);
            _currentMenuDef.FeaturesAtPosition[_currentMenuDefPosition].Add("type", "spacer");

            NextMenuItem();
            return this;
        }

        public IViewBuilderMenuDrawer<TBaseView> DrawerItem<TView>(string name = null, object icon = null) where TView : TBaseView
        {
            if (_currentMenuDef == null || _currentViewType == null)
            {
                return this;
            }

            Type itemViewType = typeof(TView);
            if (!_allViews.ContainsKey(itemViewType))
            {
                _allViews.Add(itemViewType, new Metamodel.View(itemViewType, null));
            }
            if (!_currentMenuDef.FeaturesAtPosition.ContainsKey(_currentMenuDefPosition))
            {
                _currentMenuDef.FeaturesAtPosition[_currentMenuDefPosition] = new Dictionary<string, object>();
            }
            string eventId = Convert.ToString(_currentMenuDefPosition);
            _currentMenuDef.FeaturesAtPosition[_currentMenuDefPosition].Add("eventId", eventId);
            _currentMenuDef.FeaturesAtPosition[_currentMenuDefPosition].Add("type", "item");
            _currentMenuDef.FeaturesAtPosition[_currentMenuDefPosition].Add("name", name);
            _currentMenuDef.FeaturesAtPosition[_currentMenuDefPosition].Add("icon", icon);

            _currentTransitionsTo.Add(eventId, itemViewType);

            NextMenuItem();
            return this;
        }

        public IViewBuilderMenuTabbedSlider<TBaseView> TabbedItem<TView>(string name = null, object icon = null) where TView : TBaseView
        {
            if (_currentMenuDef == null || _currentViewType == null)
            {
                return this;
            }

            Type itemViewType = typeof(TView);
            if (!_allViews.ContainsKey(itemViewType))
            {
                _allViews.Add(itemViewType, new View(itemViewType, null));
            }
            if (!_currentMenuDef.FeaturesAtPosition.ContainsKey(_currentMenuDefPosition))
            {
                _currentMenuDef.FeaturesAtPosition[_currentMenuDefPosition] = new Dictionary<string, object>();
            }
            string eventId = Convert.ToString(_currentMenuDefPosition);
            _currentMenuDef.FeaturesAtPosition[_currentMenuDefPosition].Add("eventId", eventId);
            _currentMenuDef.FeaturesAtPosition[_currentMenuDefPosition].Add("type", "item");
            _currentMenuDef.FeaturesAtPosition[_currentMenuDefPosition].Add("name", name);
            _currentMenuDef.FeaturesAtPosition[_currentMenuDefPosition].Add("icon", icon);

            _currentTransitionsTo.Add(eventId, itemViewType);

            NextMenuItem();
            return this;
        }

        public IViewBuilderPlain<TBaseView> Content()
        {
            _currentMenuDef = new MenuDefinition(MenuType.Plain.ToString());
            return this;
        }

        public IViewBuilderMenuDrawer<TBaseView> DrawerMenu()
        {
            _currentMenuDef = new MenuDefinition(MenuType.Drawer.ToString());
            return this;
        }

        public IViewBuilderMenuTabbedSlider<TBaseView> TabbedSlider()
        {
            _currentMenuDef = new MenuDefinition(MenuType.TabbedSlider.ToString());
            return this;
        }

        public ITransitionBuilder<TBaseView> OnClick(int resourceId)
        {
            _currentEvent = Convert.ToString(resourceId);
            return this;
        }

        public IViewBuilderPlain<TBaseView> NavigateTo<TView>() where TView : TBaseView
        {
            if (_currentEvent != null)
            {
                Type targetViewType = typeof(TView);
                FlushTransition(_currentViewType, _currentEvent, new Transition(targetViewType));
                _currentEvent = null;
            }
            return this;
        }

        public ITransitionBuilderConditional<TBaseView> NavigateToIf<TView>(Func<bool> booleanExpression) where TView : TBaseView
        {
            if (_currentEvent != null)
            {
                Type targetViewType = typeof(TView);
                FlushTransition(_currentViewType, _currentEvent, new Transition(targetViewType, booleanExpression));
            }
            return this;
        }

        public ITransitionBuilderConditional<TBaseView> ElseIfNavigateTo<TView>(Func<bool> booleanExpression) where TView : TBaseView
        {
            if (_currentEvent != null)
            {
                Type targetViewType = typeof(TView);
                FlushTransition(_currentViewType, _currentEvent, new Transition(targetViewType, booleanExpression));
            }
            return this;
        }

        public IViewBuilderPlain<TBaseView> ElseNavigateTo<TView>() where TView : TBaseView
        {
            if (_currentEvent != null)
            {
                Type targetViewType = typeof(TView);
                FlushTransition(_currentViewType, _currentEvent, new Transition(targetViewType));
                _currentEvent = null;
            }
            return this;
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
                if (view.MenuDefinition == null)
                {
                    view.MenuDefinition = new MenuDefinition(MenuType.Plain.ToString());
                }
                _navModel.AddView(_allViews[viewType]);
            }
        }

        private void NextMenuItem()
        {
            _currentMenuDefPosition += 1;
        }
    }
}
