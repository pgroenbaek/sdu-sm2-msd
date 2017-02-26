using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using MDSD.FluentNav.Builder.Droid.Builder.Droid.Containers;
using MDSD.FluentNav.Metamodel;
using System;
using System.Collections.Generic;

namespace MDSD.FluentNav.Builder.Droid
{
    public abstract class FluentNavAppCompatActivity : AppCompatActivity, INavigationBuilder<Android.Support.V4.App.Fragment>, IViewBuilder<Android.Support.V4.App.Fragment>,
        IViewBuilderPlain<Android.Support.V4.App.Fragment>, IViewBuilderMenuDrawer<Android.Support.V4.App.Fragment>,
        IViewBuilderMenuTabbedSlider<Android.Support.V4.App.Fragment>, ITransitionBuilder<Android.Support.V4.App.Fragment>
    {
        public enum MenuType
        {
            Plain,
            Drawer,
            TabbedSlider
        }

        private NavigationModel _navModel;
        private Android.Support.V7.Widget.Toolbar _toolbar;
        private MenuDefinition _appliedMenuDef;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_fluentnav);
            _toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.activity_fluentnav_toolbar);
            SetSupportActionBar(_toolbar);

            if (savedInstanceState == null)
            {
                BuildNavigation(this);
                FlushAllViews();
                _navModel.Initialize();
                ApplyView(_navModel.CurrentView);
            }
        }

        internal Android.Support.V7.Widget.Toolbar GetToolbar()
        {
            return _toolbar;
        }

        internal MenuDefinition GetAppliedMenuDefinition()
        {
            return _appliedMenuDef;
        }

        /// <summary>
        ///   Override with a definition of how views should be bound together.
        /// </summary>
        /// <param name="navigation">Builder interface</param>
        protected abstract void BuildNavigation(INavigationBuilder<Android.Support.V4.App.Fragment> navigation);





        //////////////////////////////////////
        //////////////////////////////////////
        ///   Using the built metamodel    ///
        //////////////////////////////////////
        //////////////////////////////////////


        // TODO's
        // - Implement TabbedSlider
        // - Make proper example work
        // - Make simple validator + test
        // - Introduce state (only on button-clicks)
        // - Write README.md

        
        public override void OnBackPressed()
        {
            bool isEmpty = _navModel.HandleBackPressed();
            if(isEmpty)
            {
                Finish();
                return;
            }
            ApplyView(_navModel.CurrentView);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Console.WriteLine(item);
            return base.OnOptionsItemSelected(item);
        }

        public void HandleEvent(string eventId)
        {
            _navModel.HandleEvent(eventId);
            ApplyView(_navModel.CurrentView);
        }
        
        private void ApplyView(Metamodel.View view)
        {
            Type contentType = view.Type;
            _appliedMenuDef = view.MenuDefinition;
            
            // Set basic attributes.
            SupportActionBar.Title = view.Title;

            // Configure style of menu.
            MenuType menuType;
            Enum.TryParse<MenuType>(_appliedMenuDef.MenuType, out menuType);
            Android.Support.V4.App.Fragment container = null;
            switch(menuType)
            {
                case MenuType.Plain:
                    container = new PlainAppCompatContainer();
                    break;

                case MenuType.Drawer:
                    container = new DrawerAppCompatContainer();
                    break;

                case MenuType.TabbedSlider:
                    container = new TabbedSliderAppCompatContainer();
                    break;

                default:
                    return;
            }
            SupportFragmentManager
                .BeginTransaction()
                .Replace(Resource.Id.activity_fluentnav_containerframe, container)
                .DisallowAddToBackStack()
                .Commit();

            // Instantiate content fragment.
            Android.Support.V4.App.Fragment contentFragment = (Android.Support.V4.App.Fragment)Activator.CreateInstance(contentType);
            
            // Set the content frame to the specified type of fragment.
            SupportFragmentManager
                .BeginTransaction()
                .Replace(Resource.Id.activity_fluentnav_contentframe, contentFragment)
                .DisallowAddToBackStack()
                .Commit();
        }





        ///////////////////////////////////////
        ///////////////////////////////////////
        ///   Fluent builder-stuff below    ///
        ///////////////////////////////////////
        ///////////////////////////////////////

        private int currentMenuDefPosition = 0;
        private string currentEvent = null;
        private Type firstViewType = null;
        private Type currentViewType = null;
        private MenuDefinition currentMenuDef = null;
        private Dictionary<string, Type> currentTransitionsTo = new Dictionary<string, Type>();
        private Dictionary<Type, Metamodel.View> allViews = new Dictionary<Type, Metamodel.View>();

        public IViewBuilder<Android.Support.V4.App.Fragment> TopView<T>(string title = null) where T : Android.Support.V4.App.Fragment
        {
            _navModel = new NavigationModel(); // Make sure to overwrite model, if an attempt is made to redefine it within the BuildNavigation() impl.
            firstViewType = typeof(T);
            currentViewType = firstViewType;
            if (!allViews.ContainsKey(firstViewType))
            {
                allViews.Add(firstViewType, new Metamodel.View(firstViewType, title));
            }
            else
            {
                allViews[firstViewType].Title = title;
            }
            return this;
        }

        public IViewBuilder<Android.Support.V4.App.Fragment> View<T>(string title = null) where T : Android.Support.V4.App.Fragment
        {
            if (currentMenuDef != null && !currentMenuDef.MenuType.Equals(MenuType.Plain.ToString()))
            {
                FlushMenuTransitions();
            }

            currentViewType = typeof(T);
            if (!allViews.ContainsKey(currentViewType))
            {
                allViews.Add(currentViewType, new Metamodel.View(currentViewType, title));
            }
            else
            {
                allViews[currentViewType].Title = title;
            }
            return this;
        }

        public IViewBuilderMenuDrawer<Android.Support.V4.App.Fragment> DrawerSpacer(string name = null)
        {
            if (currentMenuDef == null || currentViewType == null)
            {
                return this;
            }

            if (!currentMenuDef.FeaturesAtPosition.ContainsKey(currentMenuDefPosition))
            {
                currentMenuDef.FeaturesAtPosition[currentMenuDefPosition] = new Dictionary<string, object>();
            }
            currentMenuDef.FeaturesAtPosition[currentMenuDefPosition].Add("name", name);
            currentMenuDef.FeaturesAtPosition[currentMenuDefPosition].Add("type", "spacer");

            NextMenuItem();
            return this;
        }

        public IViewBuilderMenuDrawer<Android.Support.V4.App.Fragment> DrawerItem<T>(string name = null, object icon = null) where T : Android.Support.V4.App.Fragment
        {
            if (currentMenuDef == null || currentViewType == null)
            {
                return this;
            }

            Type itemViewType = typeof(T);
            if (!allViews.ContainsKey(itemViewType))
            {
                allViews.Add(itemViewType, new Metamodel.View(itemViewType, null));
            }
            if (!currentMenuDef.FeaturesAtPosition.ContainsKey(currentMenuDefPosition))
            {
                currentMenuDef.FeaturesAtPosition[currentMenuDefPosition] = new Dictionary<string, object>();
            }
            string eventId = Convert.ToString(currentMenuDefPosition);
            currentMenuDef.FeaturesAtPosition[currentMenuDefPosition].Add("eventId", eventId);
            currentMenuDef.FeaturesAtPosition[currentMenuDefPosition].Add("type", "item");
            currentMenuDef.FeaturesAtPosition[currentMenuDefPosition].Add("name", name);
            currentMenuDef.FeaturesAtPosition[currentMenuDefPosition].Add("icon", icon);
            
            currentTransitionsTo.Add(eventId, itemViewType);

            NextMenuItem();
            return this;
        }

        public IViewBuilderMenuTabbedSlider<Android.Support.V4.App.Fragment> TabbedItem<T>(string name = null, object icon = null) where T : Android.Support.V4.App.Fragment
        {
            if (currentMenuDef == null || currentViewType == null)
            {
                return this;
            }
            
            Type itemViewType = typeof(T);
            if (!allViews.ContainsKey(itemViewType))
            {
                allViews.Add(itemViewType, new Metamodel.View(itemViewType, null));
            }
            if (!currentMenuDef.FeaturesAtPosition.ContainsKey(currentMenuDefPosition))
            {
                currentMenuDef.FeaturesAtPosition[currentMenuDefPosition] = new Dictionary<string, object>();
            }
            string eventId = Convert.ToString(currentMenuDefPosition);
            currentMenuDef.FeaturesAtPosition[currentMenuDefPosition].Add("eventId", eventId);
            currentMenuDef.FeaturesAtPosition[currentMenuDefPosition].Add("type", "item");
            currentMenuDef.FeaturesAtPosition[currentMenuDefPosition].Add("name", name);
            currentMenuDef.FeaturesAtPosition[currentMenuDefPosition].Add("icon", icon);
            
            currentTransitionsTo.Add(eventId, itemViewType);

            NextMenuItem();
            return this;
        }

        public IViewBuilderPlain<Android.Support.V4.App.Fragment> Plain()
        {
            currentMenuDef = new MenuDefinition(MenuType.Plain.ToString());
            return this;
        }

        public IViewBuilderMenuDrawer<Android.Support.V4.App.Fragment> DrawerMenu()
        {
            currentMenuDef = new MenuDefinition(MenuType.Drawer.ToString());
            return this;
        }

        public IViewBuilderMenuTabbedSlider<Android.Support.V4.App.Fragment> TabbedSlider()
        {
            currentMenuDef = new MenuDefinition(MenuType.TabbedSlider.ToString());
            return this;
        }

        public ITransitionBuilder<Android.Support.V4.App.Fragment> OnClick(int resourceId)
        {
            currentEvent = Convert.ToString(resourceId);
            return this;
        }

        public IViewBuilderPlain<Android.Support.V4.App.Fragment> NavigateTo<T>() where T : Android.Support.V4.App.Fragment
        {
            if (currentEvent != null) {
                Type targetViewType = typeof(T);
                FlushTransition(currentViewType, currentEvent, new Transition(targetViewType));
                currentEvent = null;
            }
            return this;
        }
        
        private void FlushMenuTransitions()
        {
            foreach (string menuItemFrom in currentTransitionsTo.Keys)
            {
                Type fromViewType = currentTransitionsTo[menuItemFrom];
                allViews[fromViewType].MenuDefinition = currentMenuDef;
                foreach (string menuItemTo in currentTransitionsTo.Keys)
                {
                    Type toViewType = currentTransitionsTo[menuItemTo];
                    if (allViews.ContainsKey(fromViewType))
                    {
                        allViews[fromViewType].AddTransition(menuItemTo, new Transition(toViewType));
                    }
                }
            }

            currentTransitionsTo.Clear();
            currentMenuDef = null;
            currentEvent = null;
            currentMenuDefPosition = 0;
        }

        private void FlushTransition(Type sourceViewType, string eventId, Transition t)
        {
            if (!allViews.ContainsKey(sourceViewType))
            {
                allViews.Add(sourceViewType, new Metamodel.View(sourceViewType, null));
            }
            allViews[sourceViewType].AddTransition(eventId, t);
        }

        private void FlushAllViews()
        {
            // Add all views, the view that was specified as topview is added as the first view.
            _navModel.AddView(allViews[firstViewType]);
            foreach(Type viewType in allViews.Keys)
            {
                if(viewType == firstViewType)
                {
                    continue;
                }
                Metamodel.View view = allViews[viewType];
                if(view.MenuDefinition == null) 
                {
                    view.MenuDefinition = new MenuDefinition(MenuType.Plain.ToString());
                }
                _navModel.AddView(allViews[viewType]);
            }
        }

        private void NextMenuItem()
        {
            currentMenuDefPosition += 1;
        }
    }
}
