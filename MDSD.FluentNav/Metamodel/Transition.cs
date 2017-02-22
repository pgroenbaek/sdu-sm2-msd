using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Metamodel
{
    public class Transition<ViewType>
    {
        public View<ViewType> TargetView { get; private set; }

        // TODO Transition type
    }
}
