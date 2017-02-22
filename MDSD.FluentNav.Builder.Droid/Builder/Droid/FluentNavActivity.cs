using Android.App;
using Android.Support.V7.App;
using MDSD.FluentNav.Builder;
using MDSD.FluentNav.Metamodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.OS;

namespace MDSD.FluentNav.Builder.Droid
{
    public abstract class FluentNavActivity : AppCompatActivity, INavigationBuilder
    {
        private NavigationModel NavModel { get; }

        public FluentNavActivity()
        {
            NavModel = new NavigationModel();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            BuildNavigation(this);
        }

        protected abstract void BuildNavigation(INavigationBuilder navigation);

        public INavigationBuilder View<T : Android.Support.V7.Widget>(string id, params object[] subViews)
        {
            Type viewType = typeof(T);


            
            return this;
        }
    }
}
