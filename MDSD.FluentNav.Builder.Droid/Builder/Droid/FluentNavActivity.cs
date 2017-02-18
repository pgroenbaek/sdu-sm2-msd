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
    public abstract class FluentNavActivity : AppCompatActivity, INavigation
    {
        private Navigation Navigation { get; }

        public FluentNavActivity()
        {
            Navigation = new Navigation();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Build(this);
        }

        protected abstract void Build(INavigation navigation);

        public void View()
        {

        }
    }
}
