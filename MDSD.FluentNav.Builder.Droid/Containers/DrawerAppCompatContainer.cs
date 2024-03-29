using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MDSD.FluentNav.Metamodel;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Content.Res;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder.Droid.Containers
{
    public class DrawerAppCompatContainer : Android.Support.V4.App.Fragment, Android.Views.ViewGroup.IOnHierarchyChangeListener
    {
        private FluentNavAppCompatActivity _parentActivity;
        private ActionBarDrawerToggle _drawerToggle;
        private DrawerLayout _drawerLayout;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _parentActivity = (FluentNavAppCompatActivity) Activity;
        }

        public override Android.Views.View OnCreateView(LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            Android.Views.ViewGroup rootView = (Android.Views.ViewGroup) inflater.Inflate(Resource.Layout.container_drawer, container, false);
            rootView.FindViewById<FrameLayout>(Resource.Id.activity_fluentnav_contentframe).SetOnHierarchyChangeListener(this);

            _parentActivity.SupportFragmentManager
                .BeginTransaction()
                .Replace(Resource.Id.activity_fluentnav_drawerframe, new MenuFragment())
                .DisallowAddToBackStack()
                .Commit();

            return rootView;
        }

        public override void OnViewCreated(Android.Views.View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            BuildDrawerMenu(_parentActivity.GetAppliedMenuDefinition());
        }

        private void BuildDrawerMenu(Metamodel.Menu menuDef)
        {
            _drawerLayout = _parentActivity.FindViewById<DrawerLayout>(Resource.Id.activity_fluentnav_drawerlayout);
            _drawerToggle = new ActionBarDrawerToggle(
                    _parentActivity,
                    _drawerLayout,
                    _parentActivity.GetToolbar(),
                    Resource.String.drawer_open,
                    Resource.String.drawer_close
                );
            _drawerLayout.DrawerOpened += (object sender, DrawerLayout.DrawerOpenedEventArgs e) => { };
            _drawerLayout.AddDrawerListener(_drawerToggle);
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            if (_parentActivity.GetToolbar() != null)
            {
                _drawerToggle.OnConfigurationChanged(newConfig);
            }
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            if (_parentActivity.GetToolbar() != null)
            {
                _drawerToggle.SyncState();
            }
        }

        // Add click listeners to buttons, when child views are added. Could be expanded to things other than buttons.
        public void OnChildViewAdded(Android.Views.View parent, Android.Views.View child)
        {
            for (int i = 0; i < ((Android.Views.ViewGroup)child).ChildCount; i++)
            {
                Android.Views.View childView = ((Android.Views.ViewGroup)child).GetChildAt(i);
                if (childView is Button)
                {
                    Button b = (Button)childView;
                    b.Click += (btnSender, btnEvent) =>
                    {
                        _parentActivity.HandleEvent(Convert.ToString(b.Id));
                    };
                }
            }
        }

        public void OnChildViewRemoved(Android.Views.View parent, Android.Views.View child)
        {

        }
    }

    class MenuFragment : Android.Support.V4.App.Fragment, NavigationView.IOnNavigationItemSelectedListener
    {
        private FluentNavAppCompatActivity _parentActivity;
        private NavigationView _navigationView;
        private DrawerLayout _drawerLayout;
        private Metamodel.Menu _menuDef;
        private Metamodel.View _appliedView;
        private IMenuItem _previousMenuItem;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _parentActivity = (FluentNavAppCompatActivity)Activity;
        }

        public override Android.Views.View OnCreateView(LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            Android.Views.View view = inflater.Inflate(Resource.Layout.container_drawer_menu, null, false);

            _menuDef = _parentActivity.GetAppliedMenuDefinition();
            _appliedView = _parentActivity.GetAppliedView();

            _navigationView = view.FindViewById<NavigationView>(Resource.Id.activity_fluentnav_navigationview);
            _drawerLayout = _parentActivity.FindViewById<DrawerLayout>(Resource.Id.activity_fluentnav_drawerlayout);
            _navigationView.InflateMenu(Resource.Menu.menu_empty);
            if(_appliedView is Metamodel.ViewGroup)
            {
                int itemCounter = 0;
                foreach (Metamodel.View v in ((Metamodel.ViewGroup) _appliedView).SubViews)
                {
                    _navigationView.Menu.Add(0, itemCounter, itemCounter + 1, v.Title);
                }
            }
            _navigationView.SetNavigationItemSelectedListener(this);
            _navigationView.Menu.GetItem(0).SetChecked(true);

            return view;
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            item.SetCheckable(true);
            item.SetChecked(true);
            _previousMenuItem?.SetChecked(false);
            _previousMenuItem = item;

            int position = item.ItemId + item.GroupId;
            string eventId = (string) _menuDef.MenuAttributes["itemClick" + item.ItemId];
            Navigate(eventId);

            return true;
        }

        private async Task Navigate(string eventId)
        {
            _drawerLayout.CloseDrawers();
            await Task.Delay(TimeSpan.FromMilliseconds(250));

            _parentActivity.HandleEvent(eventId);
        }
    }
}