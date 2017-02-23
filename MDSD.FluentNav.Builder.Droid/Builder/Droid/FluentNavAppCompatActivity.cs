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

        public enum MenuTypes {
            DrawerMenu,
            TabbedSlider
        }

        private NavigationModel<MenuTypes> navModel;
        private bool modelIsBuilt = false;

        private Type currentView;
        private List<Transition<MenuTypes>> currentTransitions;
        private 

        public FluentNavAppCompatActivity()
        {
            navModel = new NavigationModel<MenuTypes>();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            if (savedInstanceState == null)
            {
                BuildNavigation(this);
                modelIsBuilt = true;
            }
        }


        // Instantiate metamodel + verify correctness of metamodel
        // Send click events + nav events to metamodel
        // Change state within metamodel accordingly
        // Observe currentview. Make changes to activity accordingly.

        protected abstract void BuildNavigation(INavigationBuilder<Android.Support.V4.App.Fragment> navigation);


        public IViewBuilder<Android.Support.V4.App.Fragment> TopView<T>(string title = null) where T : Android.Support.V4.App.Fragment
        {
            if(modelIsBuilt)
            {
                throw new InvalidOperationException("Cannot build a new navigation model, after it is already built.");
            }

            return this;
        }

        public IViewBuilderMenuDrawer<Android.Support.V4.App.Fragment> Spacer(string name = null, object icon = null)
        {

            return this;
        }

        public IViewBuilderMenuDrawer<Android.Support.V4.App.Fragment> Item<T>(string name = null, object icon = null) where T : Android.Support.V4.App.Fragment
        {

            return this;
        }
        
        public IViewBuilderPlain<Android.Support.V4.App.Fragment> Plain()
        {

            return this;
        }

        public IViewBuilderMenuDrawer<Android.Support.V4.App.Fragment> DrawerMenu()
        {

            return this;
        }

        public IViewBuilderMenuTabbedSlider<Android.Support.V4.App.Fragment> TabbedSlider()
        {

            return this;
        }

        public ITransitionBuilder<Android.Support.V4.App.Fragment> OnClick(int resourceId)
        {

            return this;
        }

        public IViewBuilder<Android.Support.V4.App.Fragment> SubView<T>(string title = null) where T : Android.Support.V4.App.Fragment
        {

            return this;
        }

        IViewBuilderMenuTabbedSlider<Android.Support.V4.App.Fragment> IViewBuilderMenuTabbedSlider<Android.Support.V4.App.Fragment>.Item<T>(string name, object icon)
        {

            return this;
        }

        public IViewBuilderPlain<Android.Support.V4.App.Fragment> NavigateTo<T>() where T : Android.Support.V4.App.Fragment
        {

            return this;
        }
        

        private void FlushView()
        {

        }

        private void FlushTransition()
        {

        }
    }
}
