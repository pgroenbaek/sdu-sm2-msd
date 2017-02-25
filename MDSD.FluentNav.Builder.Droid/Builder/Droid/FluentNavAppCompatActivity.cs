using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_fluentnav);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.activity_fluentnav_toolbar);
            SetSupportActionBar(toolbar);

            if (savedInstanceState == null)
            {
                BuildNavigation(this);
                FlushView(); // Make sure to add the last specified view also.
                _navModel.Initialize();
                ApplyView(_navModel.CurrentView);
            }
        }

        /// <summary>
        ///   Override with a definition of how views should be bound together.
        /// </summary>
        /// <param name="navigation">Builder interface</param>
        protected abstract void BuildNavigation(INavigationBuilder<Android.Support.V4.App.Fragment> navigation);



        //////////////////////////////////////
        //////////////////////////////////////
        ///// Using the built metamodel //////
        //////////////////////////////////////
        //////////////////////////////////////

        // TODO's
        // Send click events + nav events to metamodel
        // Observe currentview. Make changes to activity accordingly.

        
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            Console.WriteLine("BAAAAAACK");
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            return base.OnOptionsItemSelected(item);
        }

        private void ApplyView(Metamodel.View nextView)
        {
            Type nextViewType = nextView.Type;
            MenuDefinition nextMenuDef = nextView.MenuDefinition;

            MenuType menuType;
            Enum.TryParse(nextMenuDef.MenuType, out menuType);

            int containerLayoutId = 0;
            switch(menuType)
            {
                case MenuType.Plain:
                    containerLayoutId = Resource.Layout.container_plain;
                    break;

                case MenuType.Drawer:
                    containerLayoutId = Resource.Layout.container_drawer;
                    break;

                case MenuType.TabbedSlider:

                    break;

                default:
                    return;
            }

            RelativeLayout containerFrame = FindViewById<RelativeLayout>(Resource.Id.activity_fluentnav_containerframe);
            Android.Views.View containerView = LayoutInflater.From(this).Inflate(containerLayoutId, null, false);
            containerFrame.RemoveAllViewsInLayout();
            containerFrame.AddView(containerView);

            if(menuType == MenuType.Plain)
            {
                SetUpNavigationEnabled(true);
            }
            else if(menuType == MenuType.Drawer)
            {
                SetUpNavigationEnabled(false);
                BuildDrawerMenu(nextMenuDef);
            }
            else if(menuType == MenuType.TabbedSlider)
            {
                SetUpNavigationEnabled(true);
            }

            SupportFragmentManager
                .BeginTransaction()
                .Replace(Resource.Id.activity_fluentnav_contentframe, (Android.Support.V4.App.Fragment)Activator.CreateInstance(nextViewType))
                .AddToBackStack(nextViewType.ToString())
                .Commit();
        }
        
        private void BuildDrawerMenu(MenuDefinition menuDef)
        {
            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.activity_fluentnav_navigationview);
            navigationView.InflateMenu(Resource.Menu.menu_empty);

            int spacerCounter = 0;
            for (int i = 0; i < menuDef.FeaturesAtPosition.Count; i++)
            {
                if(menuDef.FeaturesAtPosition[i] == null)
                {
                    continue;
                }

                Dictionary<string, object> positionDef = menuDef.FeaturesAtPosition[i];
                if(positionDef["type"].Equals("item"))
                {
                    string title = (string) positionDef["name"];
                    navigationView.Menu.Add(spacerCounter, i, i + 1, title);
                }
                else if(positionDef["type"].Equals("spacer"))
                {
                    string title = (string)positionDef["name"];
                    spacerCounter += 1;
                    navigationView.Menu.Add(spacerCounter, i, i + 1, title);
                }
            }
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
        ///// Fluent builder-stuff below //////
        ///////////////////////////////////////
        ///////////////////////////////////////

        private int currentMenuDefPosition = 0;
        private string currentEvent = null;
        private Metamodel.View currentView = null;
        private MenuDefinition currentMenuDef = null;

        public IViewBuilder<Android.Support.V4.App.Fragment> TopView<T>(string title = null) where T : Android.Support.V4.App.Fragment
        {
            _navModel = new NavigationModel(); // Make sure to overwrite model, if an attempt has been made to redefine it within the BuildNavigation() impl.
            currentView = new Metamodel.View(typeof(T));
            return this;
        }

        public IViewBuilder<Android.Support.V4.App.Fragment> View<T>(string title = null) where T : Android.Support.V4.App.Fragment
        {
            FlushView();
            currentView = new Metamodel.View(typeof(T));
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
