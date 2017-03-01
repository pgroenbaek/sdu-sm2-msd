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
    public abstract class FluentNavAppCompatActivity : AppCompatActivity
    {
        public enum MenuType // TODO, Move this into specific builders..
        {
            Plain,
            Drawer,
            TabbedSlider
        }

        private Android.Support.V7.Widget.Toolbar _toolbar;
        private MenuDefinition _appliedMenuDef;
        private NavigationModel _navModel;

        private GenericFluentNavBuilder<Android.Support.V4.App.Fragment> _fluentNavBuilder = new GenericFluentNavBuilder<Android.Support.V4.App.Fragment>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_fluentnav);
            _toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.activity_fluentnav_toolbar);
            SetSupportActionBar(_toolbar);

            if (savedInstanceState == null)
            {
                BuildNavigation(_fluentNavBuilder);
                _navModel = _fluentNavBuilder.FetchBuiltModel();
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

    }
}
