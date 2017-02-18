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

namespace MDSD.FluentNav.Example.Droid
{
    [Activity(Label = "MDSD.FluentNav.Example", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FluentNavActivity
    {
        protected override void Build(INavigation navigation)
        {
            navigation.
                View()
            ;
        }
    }
}

