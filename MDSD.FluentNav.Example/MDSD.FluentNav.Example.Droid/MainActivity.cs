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
            .BeginViewGroup()
                // If C# supported enum-constraints in generics, i could have enforced a menutype-parameter here in the Menu()-call 
                // based on something specified in the subclass like "Android.Support.V4.App.Fragment" is currently. Would have been a lot nicer to look at.
                .Menu()
                    .Type(MenuDrawer)
                    .Attribute("spacer", 2)
                .View<RedFragment>(title: "Red")
                    .Content()
                        .OnClick(Resource.Id.fragment_red_btn1)
                            .NavigateTo<BlueFragment>()
                .View<GreenFragment>(title: "Green")
            .EndViewGroup()
            .View<BlueFragment>(title: "This one is blue.")
                .Content()
                    .OnClick(Resource.Id.fragment_blue_btn1)
                        .NavigateTo<YellowFragment>()
            .View<YellowFragment>(title: "Yellow")
                .Content()
                    .OnClick(Resource.Id.fragment_yellow_btn1)
                        .NavigateToIf<WhiteFragment>(() => false)
                        .ElseNavigateTo<BlackFragment>()
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

