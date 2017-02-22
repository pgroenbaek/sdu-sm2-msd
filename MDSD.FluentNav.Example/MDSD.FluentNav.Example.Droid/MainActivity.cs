using System;

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
    public class MainActivity : FluentNavActivity
    {
        int test;

        protected override void BuildNavigation(INavigationBuilder nav)
        {
            nav
            .TopView<GreenFragment>(title)
                .MenuDrawer()
                .Item(name, icon).OnClick().NavigateTo<RedFragment>()
                .Item(name, icon).OnClick().NavigateTo<GreenFragment>()
                .Spacer(name = null)
                .Item(name, icon).OnClick().NavigateTo<YellowFragment>()
            .SubView<RedFragment>()
                .Plain()
                .OnClick(Resource.Id.fragment_red_btn1).NavigateTo<BlueFragment>()
            .SubView<YellowFragment>()
                .Plain()
                .OnClick(Resource.Id.fragment_red_btn1).NavigateTo<BlueFragment>()
            .SubView<BlueFragment>()
                .TabbedSlider()
                .Item<WhiteFragment>(name, icon)
                .Item<BlackFragment>(name, icon)
            ;

        }
    }
}

