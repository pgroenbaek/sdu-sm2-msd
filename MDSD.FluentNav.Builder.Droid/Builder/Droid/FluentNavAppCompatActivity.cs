using Android.Support.V7.App;
using MDSD.FluentNav.Builder;
using MDSD.FluentNav.Metamodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.OS;

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

        private NavigationModel navModel;
        private bool modelIsBuilt = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            if (savedInstanceState == null)
            {
                BuildNavigation(this);
                modelIsBuilt = true;
            }
        }

        protected abstract void BuildNavigation(INavigationBuilder<Android.Support.V4.App.Fragment> navigation);






        // TODO's
        // Instantiate metamodel + verify correctness of metamodel
        // Send click events + nav events to metamodel
        // Change state within metamodel accordingly
        // Observe currentview. Make changes to activity accordingly.

        ///////////////////////////////////////
        ///////////////////////////////////////
        ///// Fluent builder-stuff below //////
        ///////////////////////////////////////
        ///////////////////////////////////////

        private int currentMenuDefPosition = 0;
        private string currentEvent = null;
        private View currentView = null;
        private MenuDefinition currentMenuDef = null;

        public IViewBuilder<Android.Support.V4.App.Fragment> TopView<T>(string title = null) where T : Android.Support.V4.App.Fragment
        {
            if(modelIsBuilt)
            {
                throw new InvalidOperationException("Cannot build a new navigation model after it is already built.");
            }
            navModel = new NavigationModel(); // Make sure to overwrite model, if an attempt has been made to redefine it within the BuildNavigation() impl.
            currentView = new View(typeof(T));
            return this;
        }

        public IViewBuilder<Android.Support.V4.App.Fragment> SubView<T>(string title = null) where T : Android.Support.V4.App.Fragment
        {
            FlushView();
            currentView = new View(typeof(T));
            return this;
        }

        public IViewBuilderMenuDrawer<Android.Support.V4.App.Fragment> DrawerSpacer(string name = null)
        {
            if (currentMenuDef == null || currentView == null)
            {
                return this;
            }

            if (currentMenuDef.FeaturesAtPosition[currentMenuDefPosition] == null)
            {
                currentMenuDef.FeaturesAtPosition[currentMenuDefPosition] = new Dictionary<string, object>();
            }
            currentMenuDef.FeaturesAtPosition[currentMenuDefPosition].Add("name", name);
            
            NextMenuItem();
            return this;
        }

        public IViewBuilderMenuDrawer<Android.Support.V4.App.Fragment> DrawerItem<T>(string name = null, object icon = null) where T : Android.Support.V4.App.Fragment
        {
            if (currentMenuDef == null || currentView == null)
            {
                return this;
            }

            if (currentMenuDef.FeaturesAtPosition[currentMenuDefPosition] == null)
            {
                currentMenuDef.FeaturesAtPosition[currentMenuDefPosition] = new Dictionary<string, object>();
            }
            string eventId = Convert.ToString("m" + currentMenuDefPosition);
            currentMenuDef.FeaturesAtPosition[currentMenuDefPosition].Add("eventId", eventId);
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

            if (currentMenuDef.FeaturesAtPosition[currentMenuDefPosition] == null)
            {
                currentMenuDef.FeaturesAtPosition[currentMenuDefPosition] = new Dictionary<string, object>();
            }
            string eventId = Convert.ToString("m" + currentMenuDefPosition);
            currentMenuDef.FeaturesAtPosition[currentMenuDefPosition].Add("eventId", eventId);
            currentMenuDef.FeaturesAtPosition[currentMenuDefPosition].Add("name", name);
            currentMenuDef.FeaturesAtPosition[currentMenuDefPosition].Add("icon", icon);

            Type itemViewType = typeof(T);
            currentView.AddTransition(eventId, new Transition(itemViewType));

            NextMenuItem();
            return this;
        }

        public IViewBuilderPlain<Android.Support.V4.App.Fragment> Plain()
        {
            currentMenuDef = new MenuDefinition("Plain");
            return this;
        }

        public IViewBuilderMenuDrawer<Android.Support.V4.App.Fragment> DrawerMenu()
        {
            currentMenuDef = new MenuDefinition("DrawerMenu");
            return this;
        }

        public IViewBuilderMenuTabbedSlider<Android.Support.V4.App.Fragment> TabbedSlider()
        {
            currentMenuDef = new MenuDefinition("TabbedSlider");
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
            navModel.AddView(currentView);

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
