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
using MDSD.FluentNav.Builder.Interfaces;
using MDSD.FluentNav.Example.Droid.Views;

namespace MDSD.FluentNav.Example.Droid
{
    [Activity(Label = "MDSD.FluentNav.Example", Icon = "@drawable/icon", MainLauncher = true, Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FluentNavAppCompatActivity
    {
        protected override void BuildNavigation(INavigationBuilder<Android.Support.V4.App.Fragment> nav)
        {
            nav
            .ViewGroup()
                .Menu()
                    .Type(MenuDrawer)
                    .Attribute("spacer", 2)
                .SubView<RedFragment>(title: "Red")
                    .Content()
                        .OnClick(Resource.Id.fragment_red_btn1)
                            .NavigateTo<BlueFragment>()
                .SubView<GreenFragment>(title: "Green")
                .SubView<YellowFragment>(title: "Yellow")
                    .Content()
                        .OnClick(Resource.Id.fragment_yellow_btn1)
                            .NavigateToIf<WhiteFragment>(() => false)
                            .ElseNavigateTo<BlackFragment>()
            .View<BlueFragment>(title: "This one is blue.")
                .Content()
            ;
            
            /*
            nav
            .TopView<GreenFragment>(title: "Example")
                .Plain()
                .OnClick(Resource.Id.fragment_green_btn1).NavigateTo<RedFragment>()
            .View<RedFragment>(title: "Red")
                .Plain()
                .OnClick(Resource.Id.fragment_red_btn1).NavigateTo<YellowFragment>()
            .View<YellowFragment>(title: "LALALA")
                .Plain();
                */
        }
    }
}

