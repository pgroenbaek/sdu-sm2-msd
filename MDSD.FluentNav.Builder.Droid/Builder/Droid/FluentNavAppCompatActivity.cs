using Android.App;
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
        IViewBuilderDrawerMenu, IViewBuilderTabbedSlider, ITransitionBuilder
    {
        private NavigationModel NavModel { get; }

        public FluentNavAppCompatActivity()
        {
            NavModel = new NavigationModel();
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

        public IViewBuilderDrawerMenu DrawerMenu()
        {
            return this;
        }

        public IViewBuilderTabbedSlider TabbedSlider()
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

        public IViewBuilderDrawerMenu Spacer(string name = null, object icon = null)
        {
            return this;
        }

        public IViewBuilderDrawerMenu Item<T>(string name = null, object icon = null)
        {
            Type viewType = typeof(T);
            return this;
        }

        public IViewBuilderTabbedSlider Item<T>(string name, object icon)
        {
            Type viewType = typeof(T);
            return this;
        }
    }
}
