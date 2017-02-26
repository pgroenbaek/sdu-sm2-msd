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
                FlushView(); // Make sure to add the last specified view also.
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
        // - Implement Drawer
        // - Implement TabbedSlider
        // - Make proper example work
        // - Make simple validator + test
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

        public void HandleEvent(string eventId)
        {
            _navModel.HandleEvent(eventId);
            ApplyView(_navModel.CurrentView);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Console.WriteLine(item);
            return base.OnOptionsItemSelected(item);
        }

        private void ApplyView(Metamodel.View view)
        {
            Type contentType = view.Type;
            _appliedMenuDef = view.MenuDefinition;

            // Set basic attributes.
            SupportActionBar.Title = view.Title;

            // Configure style of menu.
            MenuType menuType;
            Enum.TryParse(_appliedMenuDef.MenuType, out menuType);
            Android.Support.V4.App.Fragment container = null;
            switch(menuType)
            {
                case MenuType.Plain:
                    container = new PlainAppCompatContainer();
                    SetUpNavigationEnabled(true);
                    break;

                case MenuType.Drawer:
                    container = new DrawerAppCompatContainer();
                    SetUpNavigationEnabled(false);
                    break;

                case MenuType.TabbedSlider:
                    container = new TabbedSliderAppCompatContainer();
                    SetUpNavigationEnabled(true);
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

        private void SetUpNavigationEnabled(bool upNavigationEnabled)
        {
            if(SupportActionBar != null)
            {
                SupportActionBar.SetDisplayHomeAsUpEnabled(!upNavigationEnabled);
                SupportActionBar.SetDisplayShowHomeEnabled(upNavigationEnabled);
            }
        }


        ///////////////////////////////////////
        ///////////////////////////////////////
        ///   Fluent builder-stuff below    ///
        ///////////////////////////////////////
        ///////////////////////////////////////

        private int currentMenuDefPosition = 0;
        private string currentEvent = null;
        private Metamodel.View currentView = null;
        private MenuDefinition currentMenuDef = null;

        public IViewBuilder<Android.Support.V4.App.Fragment> TopView<T>(string title = null) where T : Android.Support.V4.App.Fragment
        {
            _navModel = new NavigationModel(); // Make sure to overwrite model, if an attempt has been made to redefine it within the BuildNavigation() impl.
            currentView = new Metamodel.View(typeof(T), title);
            return this;
        }

        public IViewBuilder<Android.Support.V4.App.Fragment> View<T>(string title = null) where T : Android.Support.V4.App.Fragment
        {
            FlushView();
            currentView = new Metamodel.View(typeof(T), title);
            return this;
        }

        public IViewBuilderMenuDrawer<Android.Support.V4.App.Fragment> DrawerSpacer(string name = null)
        {
            if (currentMenuDef == null || currentView == null)
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
            if (currentMenuDef == null || currentView == null)
            {
                return this;
            }

            if (!currentMenuDef.FeaturesAtPosition.ContainsKey(currentMenuDefPosition))
            {
                currentMenuDef.FeaturesAtPosition[currentMenuDefPosition] = new Dictionary<string, object>();
            }
            string eventId = Convert.ToString("m" + currentMenuDefPosition);
            currentMenuDef.FeaturesAtPosition[currentMenuDefPosition].Add("eventId", eventId);
            currentMenuDef.FeaturesAtPosition[currentMenuDefPosition].Add("type", "item");
            currentMenuDef.FeaturesAtPosition[currentMenuDefPosition].Add("name", name);
            currentMenuDef.FeaturesAtPosition[currentMenuDefPosition].Add("icon", icon);

            Type itemViewType = typeof(T);
            currentView.AddTransition(eventId, new Transition(itemViewType));
            
            NextMenuItem();
            return this;
        }

        public IViewBuilderMenuTabbedSlider<Android.Support.V4.App.Fragment> TabbedItem<T>(string name = null, object icon = null) where T : Android.Support.V4.App.Fragment
        {
            if (currentMenuDef == null || currentView == null)
            {
                return this;
            }

            if (!currentMenuDef.FeaturesAtPosition.ContainsKey(currentMenuDefPosition))
            {
                currentMenuDef.FeaturesAtPosition[currentMenuDefPosition] = new Dictionary<string, object>();
            }
            string eventId = Convert.ToString("m" + currentMenuDefPosition);
            currentMenuDef.FeaturesAtPosition[currentMenuDefPosition].Add("eventId", eventId);
            currentMenuDef.FeaturesAtPosition[currentMenuDefPosition].Add("type", "item");
            currentMenuDef.FeaturesAtPosition[currentMenuDefPosition].Add("name", name);
            currentMenuDef.FeaturesAtPosition[currentMenuDefPosition].Add("icon", icon);

            Type itemViewType = typeof(T);
            currentView.AddTransition(eventId, new Transition(itemViewType));

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
                currentView.AddTransition(currentEvent, new Transition(targetViewType));
                currentEvent = null;
            }
            return this;
        }
        
        private void FlushView()
        {
            if(currentView == null)
            {
                return;
            }
            currentView.MenuDefinition = currentMenuDef;

            _navModel.AddView(currentView);

            currentView = null;
            currentMenuDef = null;
            currentEvent = null;
            currentMenuDefPosition = 0;
        }

        private void NextMenuItem()
        {
            currentMenuDefPosition += 1;
        }
    }
}
