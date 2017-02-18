using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Metamodel
{
    public class Navigation
    {
        public View CurrentView { get; private set; }

        private Dictionary<Type, View> views = new Dictionary<Type, View>();

        public Navigation()
        {
        }

    }
}
