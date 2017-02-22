﻿using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using MDSD.FluentNav.Builder.Droid;
using MDSD.FluentNav.Builder;
using MDSD.FluentNav.Example.Droid.Views;

namespace MDSD.FluentNav.Example.Droid
{
    [Activity(Label = "MDSD.FluentNav.Example", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FluentNavAppCompatActivity
    {
        
        protected override void BuildNavigation(INavigationBuilder<Android.Support.V4.App.Fragment> nav)
        {
            nav
            .TopView<GreenFragment>(title: "Example")
                .DrawerMenu()
                .Item<RedFragment>(name: "Red")
                .Item<GreenFragment>(name: "Green")
                .Spacer(name: null)
                .Item<YellowFragment>(icon: null, name: "Yellow")
            .SubView<RedFragment>()
                .Plain()
                .OnClick(Resource.Id.fragment_red_btn1).NavigateTo<BlueFragment>()
            .SubView<YellowFragment>()
                .Plain()
                .OnClick(Resource.Id.fragment_yellow_btn1).NavigateTo<BlueFragment>()
            .SubView<BlueFragment>()
                .TabbedSlider()
                .Item<WhiteFragment>(name: "White")
                .Item<BlackFragment>(name: "Not White")
            ;

        }
    }
}

