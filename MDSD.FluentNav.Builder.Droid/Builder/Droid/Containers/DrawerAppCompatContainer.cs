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

namespace MDSD.FluentNav.Builder.Droid.Builder.Droid.Containers
{
    public class DrawerAppCompatContainer : Android.Support.V4.App.Fragment, ViewGroup.IOnHierarchyChangeListener
    {
        private FluentNavAppCompatActivity _parentActivity;
        private ActionBarDrawerToggle _drawerToggle;
        private DrawerLayout _drawerLayout;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _parentActivity = (FluentNavAppCompatActivity) Activity;
        }

        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            ViewGroup rootView = (ViewGroup) inflater.Inflate(Resource.Layout.container_drawer, container, false);
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

        private void BuildDrawerMenu(MenuDefinition menuDef)
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
            for (int i = 0; i < ((ViewGroup)child).ChildCount; i++)
            {
                Android.Views.View childView = ((ViewGroup)child).GetChildAt(i);
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
        private MenuDefinition _menuDef;
        private IMenuItem _previousMenuItem;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _parentActivity = (FluentNavAppCompatActivity)Activity;
        }

        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            Android.Views.View view = inflater.Inflate(Resource.Layout.container_drawer_menu, null, false);

            _menuDef = _parentActivity.GetAppliedMenuDefinition();

            _navigationView = view.FindViewById<NavigationView>(Resource.Id.activity_fluentnav_navigationview);
            _drawerLayout = _parentActivity.FindViewById<DrawerLayout>(Resource.Id.activity_fluentnav_drawerlayout);
            _navigationView.InflateMenu(Resource.Menu.menu_empty);
            int spacerCounter = 0;
            for (int i = 0; i < _menuDef.FeaturesAtPosition.Count; i++)
            {
                if (_menuDef.FeaturesAtPosition[i] == null)
                {
                    continue;
                }

                Dictionary<string, object> positionDef = _menuDef.FeaturesAtPosition[i];
                if (positionDef["type"].Equals("item"))
                {
                    string title = (string)positionDef["name"];
                    Console.WriteLine(spacerCounter + " " + i + " " + title);
                    _navigationView.Menu.Add(spacerCounter, i - spacerCounter, i + 1, title);
                }
                else if (positionDef["type"].Equals("spacer"))
                {
                    string title = (string)positionDef["name"];
                    spacerCounter += 1;
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
            string eventId = (string) _menuDef.FeaturesAtPosition[position]["eventId"];
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