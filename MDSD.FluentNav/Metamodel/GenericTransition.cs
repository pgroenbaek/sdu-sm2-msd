using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Metamodel
{
    public class Transition<TMenuTypeEnum> where TMenuTypeEnum : struct, IComparable, IFormattable//, IConvertible
    {
        public Type TargetView { get; set; }
        public View<TMenuTypeEnum> SourceView { get; internal set; }
        public Func<bool> Conditional { get; internal set; }

        public Transition(Type targetView, Func<bool> conditional = null)
        {
            TargetView = targetView;
            Conditional = conditional;
        }
        // TODO Transition anim
    }
}
