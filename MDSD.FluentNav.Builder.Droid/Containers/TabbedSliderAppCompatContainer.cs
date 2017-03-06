using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace MDSD.FluentNav.Builder.Droid.Containers
{
    // TODO Unfinished, this could be an additional menu type for android, as well as tabbedbar's.
    public class TabbedSliderAppCompatContainer : Android.Support.V4.App.Fragment, ViewGroup.IOnHierarchyChangeListener
    {
        private FluentNavAppCompatActivity _parentActivity;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _parentActivity = (FluentNavAppCompatActivity)Activity;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ViewGroup rootView = (ViewGroup) inflater.Inflate(Resource.Layout.container_tabbedslider, container, false);
            // TODO Set listener properly, so that event handlers that forward buttonclicks to the metamodel are added (in OnChildViewAdded).
            //rootView.FindViewById<FrameLayout>(Resource.Id.activity_fluentnav_contentframe).SetOnHierarchyChangeListener(this);
            return rootView;
        }

        public override void OnResume()
        {
            base.OnResume();
            _parentActivity.SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.abc_ic_ab_back_mtrl_am_alpha);
            _parentActivity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            _parentActivity.SupportActionBar.SetDisplayShowHomeEnabled(true);
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

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    _parentActivity.OnBackPressed();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}