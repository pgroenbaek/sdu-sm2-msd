using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Metamodel
{
    public class Transition
    {
        public Type TargetView { get; private set; }

        public Transition(Type targetView)
        {
            TargetView = targetView;
        }
        // TODO Transition type
    }
}
