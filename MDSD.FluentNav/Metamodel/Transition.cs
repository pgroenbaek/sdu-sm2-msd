using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Metamodel
{
    public class Transition
    {
        public View TargetView { get; private set; }

        public Transition(View targetView)
        {
            TargetView = targetView;
        }
        // TODO Transition type
    }
}
