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

        public enum MenuType {
            Plain,
            DrawerMenu,
            TabbedSlider
        }

        private NavigationModel<MenuType> navModel;
        private bool modelIsBuilt = false;

        private Type currentView = null;
        private int currentMenuItemPosition = 0;
        private MenuType? currentMenuType = null;
        private Dictionary<string, Type> currentTransitions = new Dictionary<string, Type>();
        private Dictionary<int, Dictionary<string, object>> currentMenuFeatures = new Dictionary<int, Dictionary<string, object>>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            if (savedInstanceState == null)
            {
                BuildNavigation(this);
                modelIsBuilt = true;
            }
        }

        // TODO's
        // Instantiate metamodel + verify correctness of metamodel
        // Send click events + nav events to metamodel
        // Change state within metamodel accordingly
        // Observe currentview. Make changes to activity accordingly.

        protected abstract void BuildNavigation(INavigationBuilder<Android.Support.V4.App.Fragment> navigation);


        public IViewBuilder<Android.Support.V4.App.Fragment> TopView<T>(string title = null) where T : Android.Support.V4.App.Fragment
        {
            if(modelIsBuilt)
            {
                throw new InvalidOperationException("Cannot build a new navigation model after it is already built.");
            }
            navModel = new NavigationModel<MenuType>(); // Overwrite model if an attempt is made to redefine it within the BuildNavigation() impl.
            currentView = typeof(T);
            return this;
        }

        public IViewBuilder<Android.Support.V4.App.Fragment> SubView<T>(string title = null) where T : Android.Support.V4.App.Fragment
        {
            FlushView();
            currentView = typeof(T);
            return this;
        }

        public IViewBuilderMenuDrawer<Android.Support.V4.App.Fragment> Spacer(string name = null)
        {
            if(currentMenuFeatures[currentMenuItemPosition] == null)
            {
                currentMenuFeatures[currentMenuItemPosition] = new Dictionary<string, object>();
            }
            currentMenuFeatures[currentMenuItemPosition].Add("transitionId", currentMenuItemPosition.ToString());
            currentMenuFeatures[currentMenuItemPosition].Add("name", name);

            NextMenuItem();
            return this;
        }

        IViewBuilderMenuDrawer<Android.Support.V4.App.Fragment> IViewBuilderMenuDrawer<Android.Support.V4.App.Fragment>.Item<T>(string name, object icon)
        {
            if (currentMenuFeatures[currentMenuItemPosition] == null)
            {
                currentMenuFeatures[currentMenuItemPosition] = new Dictionary<string, object>();
            }
            Type viewType = typeof(T);
            currentMenuFeatures[currentMenuItemPosition].Add("transitionId", currentMenuItemPosition.ToString());
            currentMenuFeatures[currentMenuItemPosition].Add("name", name);
            currentMenuFeatures[currentMenuItemPosition].Add("icon", icon);

            currentTransitions.Add(currentMenuItemPosition.ToString(), viewType);
            
            NextMenuItem();
            return this;
        }

        IViewBuilderMenuTabbedSlider<Android.Support.V4.App.Fragment> IViewBuilderMenuTabbedSlider<Android.Support.V4.App.Fragment>.Item<T>(string name, object icon)
        {
            if (currentMenuFeatures[currentMenuItemPosition] == null)
            {
                currentMenuFeatures[currentMenuItemPosition] = new Dictionary<string, object>();
            }
            Type viewType = typeof(T);
            currentMenuFeatures[currentMenuItemPosition].Add("transitionId", currentMenuItemPosition.ToString());
            currentMenuFeatures[currentMenuItemPosition].Add("name", name);
            currentMenuFeatures[currentMenuItemPosition].Add("icon", icon);

            currentTransitions.Add(currentMenuItemPosition.ToString(), viewType);
            
            NextMenuItem();
            return this;
        }

        public IViewBuilderPlain<Android.Support.V4.App.Fragment> Plain()
        {
            currentMenuType = MenuType.Plain;
            return this;
        }

        public IViewBuilderMenuDrawer<Android.Support.V4.App.Fragment> DrawerMenu()
        {
            currentMenuType = MenuType.DrawerMenu;
            return this;
        }

        public IViewBuilderMenuTabbedSlider<Android.Support.V4.App.Fragment> TabbedSlider()
        {
            currentMenuType = MenuType.TabbedSlider;
            return this;
        }

        public ITransitionBuilder<Android.Support.V4.App.Fragment> OnClick(int resourceId)
        {

            return this;
        }

        public IViewBuilderPlain<Android.Support.V4.App.Fragment> NavigateTo<T>() where T : Android.Support.V4.App.Fragment
        {
            Type viewType = typeof(T);

            return this;
        }
        

        private void FlushView()
        {
            if(currentView == null)
            {
                throw new InvalidOperationException("Cannot flush view to model when no view was defined.");
            }

            navModel.AddView(currentView, currentTransitions, currentMenuFeatures);

            currentView = null;
            currentTransitions = new Dictionary<string, Type>();
            currentMenuFeatures = new Dictionary<int, Dictionary<string, object>>();
            currentMenuType = null;
            currentMenuItemPosition = 0;
        }

        private void NextMenuItem()
        {
            currentMenuItemPosition += 1;
        }
    }
}
