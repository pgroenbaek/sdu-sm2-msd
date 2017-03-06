using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Metamodel
{
    // TODO Transition anim
    public class Transition
    {
        public Type TargetView { get; set; }
        public View SourceView { get; private set; }
        public Func<bool> Conditional { get; private set; }

        public Transition(Type targetView, View sourceView, Func<bool> conditional = null)
        {
            TargetView = targetView;
            SourceView = sourceView;
            Conditional = conditional;
        }
    }
}
