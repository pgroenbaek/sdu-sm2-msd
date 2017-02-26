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

namespace MDSD.FluentNav.Builder.Droid.Builder.Droid.Containers
{
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
            ViewGroup rootView = (ViewGroup) base.OnCreateView(inflater, container, savedInstanceState);
            rootView.SetOnHierarchyChangeListener(this);
            return rootView;
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
}