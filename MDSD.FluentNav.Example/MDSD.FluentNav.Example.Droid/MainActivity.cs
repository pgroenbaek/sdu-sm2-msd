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
    [Activity(Label = "MDSD.FluentNav.Example", Icon = "@drawable/icon", MainLauncher = true, Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FluentNavAppCompatActivity
    {
        
        protected override void BuildNavigation(INavigationBuilder<Android.Support.V4.App.Fragment> nav)
        {
            nav
            .TopView<GreenFragment>(title: "Example")
                .DrawerMenu()
                .DrawerItem<RedFragment>(name: "Red")
                .DrawerItem<GreenFragment>(name: "Green")
                .DrawerSpacer(name: null)
                .DrawerItem<YellowFragment>(icon: null, name: "Yellow")
            .SubView<RedFragment>()
                .Plain()
                .OnClick(Resource.Id.fragment_red_btn1)
                    .NavigateTo<BlueFragment>()
            .SubView<YellowFragment>()
                .Plain()
                .OnClick(Resource.Id.fragment_yellow_btn1)
                    .NavigateTo<BlueFragment>()
            .SubView<BlueFragment>()
                .TabbedSlider()
                .TabbedItem<WhiteFragment>(name: "White")
                .TabbedItem<BlackFragment>(name: "Not White")
            ;
        }
    }
}

