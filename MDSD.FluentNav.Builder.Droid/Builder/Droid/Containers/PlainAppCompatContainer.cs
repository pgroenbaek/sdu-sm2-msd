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

namespace MDSD.FluentNav.Builder.Droid.Builder.Droid.Containers
{
    public class PlainAppCompatContainer : Android.Support.V4.App.Fragment, ViewGroup.IOnHierarchyChangeListener
    {
        private FluentNavAppCompatActivity _parentActivity;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _parentActivity = (FluentNavAppCompatActivity)Activity;
        }        

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            

            ViewGroup rootView = (ViewGroup) inflater.Inflate(Resource.Layout.container_plain, container, false);
            rootView.SetOnHierarchyChangeListener(this);
            return rootView;
        }

        // Add click listeners to buttons, when child views are added.
        public void OnChildViewAdded(View parent, View child)
        {
            for (int i = 0; i < ((ViewGroup)child).ChildCount; i++)
            {
                View childView = ((ViewGroup)child).GetChildAt(i);
                if (childView is Button)
                {
                    Button b = (Button)childView;
                    b.Click += (btnSender, btnEvent) =>
                    {
                        Console.WriteLine("CliCCCCK");
                        _parentActivity.HandleEvent(Convert.ToString(b.Id));
                    };
                }
            }
        }

        public void OnChildViewRemoved(View parent, View child)
        {

        }
    }
}