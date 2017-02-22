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
    public abstract class FluentNavAppCompatActivity : AppCompatActivity, INavigationBuilder<Android.Support.V4.App.Fragment>, IViewBuilder<Android.Support.V4.App.Fragment>, IViewBuilderPlain<Android.Support.V4.App.Fragment>,
        IViewBuilderMenuDrawer<Android.Support.V4.App.Fragment>, IViewBuilderMenuTabbedSlider<Android.Support.V4.App.Fragment>, ITransitionBuilder<Android.Support.V4.App.Fragment>
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

        protected abstract void BuildNavigation(INavigationBuilder<Android.Support.V4.App.Fragment> navigation);
        

        public IViewBuilderMenuDrawer<Android.Support.V4.App.Fragment> Spacer(string name = null, object icon = null)
        {
            throw new NotImplementedException();
        }

        public IViewBuilderMenuDrawer<Android.Support.V4.App.Fragment> Item<T>(string name = null, object icon = null) where T : Android.Support.V4.App.Fragment
        {
            throw new NotImplementedException();
        }

        public IViewBuilder<Android.Support.V4.App.Fragment> TopView<T>(string title = null) where T : Android.Support.V4.App.Fragment
        {
            throw new NotImplementedException();
        }

        public IViewBuilderPlain<Android.Support.V4.App.Fragment> Plain()
        {
            throw new NotImplementedException();
        }

        public IViewBuilderMenuDrawer<Android.Support.V4.App.Fragment> DrawerMenu()
        {
            throw new NotImplementedException();
        }

        public IViewBuilderMenuTabbedSlider<Android.Support.V4.App.Fragment> TabbedSlider()
        {
            throw new NotImplementedException();
        }

        public ITransitionBuilder<Android.Support.V4.App.Fragment> OnClick(int resourceId)
        {
            throw new NotImplementedException();
        }

        public IViewBuilder<Android.Support.V4.App.Fragment> SubView<T>(string title = null) where T : Android.Support.V4.App.Fragment
        {
            throw new NotImplementedException();
        }

        IViewBuilderMenuTabbedSlider<Android.Support.V4.App.Fragment> IViewBuilderMenuTabbedSlider<Android.Support.V4.App.Fragment>.Item<T>(string name, object icon)
        {
            throw new NotImplementedException();
        }

        public IViewBuilderPlain<Android.Support.V4.App.Fragment> NavigateTo<T>() where T : Android.Support.V4.App.Fragment
        {
            throw new NotImplementedException();
        }
    }
}
