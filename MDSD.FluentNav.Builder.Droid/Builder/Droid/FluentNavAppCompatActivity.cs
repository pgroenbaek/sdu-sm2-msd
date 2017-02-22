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
    public abstract class FluentNavAppCompatActivity : AppCompatActivity, INavigationBuilder, IViewBuilder, IViewBuilderPlain,
        IViewBuilderMenuDrawer, IViewBuilderMenuTabbedSlider, ITransitionBuilder
    {
        private NavigationModel<Android.Support.V4.App.Fragment> NavModel { get; }

        public FluentNavAppCompatActivity()
        {
            NavModel = new NavigationModel<Android.Support.V4.App.Fragment>();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            BuildNavigation(this);
        }

        protected abstract void BuildNavigation(INavigationBuilder navigation);
        
        public INavigationBuilder TopView<T>(string title = null)
        {
            Type viewType = typeof(T);
            return this;
        }

        public IViewBuilderPlain Plain()
        {
            return this;
        }

        public IViewBuilderMenuDrawer DrawerMenu()
        {
            return this;
        }

        public IViewBuilderMenuTabbedSlider TabbedSlider()
        {
            return this;
        }

        public ITransitionBuilder OnClick(int resourceId)
        {
            return this;
        }

        public IViewBuilder SubView<T>(string title = null)
        {
            Type viewType = typeof(T);
            return this;
        }

        public IViewBuilderPlain NavigateTo<T>()
        {
            Type viewType = typeof(T);
            return this;
        }

        IViewBuilderMenuDrawer IViewBuilderMenuDrawer.Spacer(string name = null, object icon = null)
        {
            return this;
        }

        IViewBuilderMenuDrawer IViewBuilderMenuDrawer.Item<T>(string name, object icon)
        {
            Type viewType = typeof(T);
            return this;
        }

        IViewBuilderMenuTabbedSlider IViewBuilderMenuTabbedSlider.Item<T>(string name, object icon)
        {
            Type viewType = typeof(T);
            return this;
        }
    }
}
