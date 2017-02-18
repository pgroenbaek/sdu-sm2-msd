using Android.Support.V7.App;
using MDSD.FluentNav.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDSD.FluentNav.Builder.Droid
{
    public abstract class FluentNavActivity : AppCompatActivity
    {
        protected abstract void Build();
    }
}
