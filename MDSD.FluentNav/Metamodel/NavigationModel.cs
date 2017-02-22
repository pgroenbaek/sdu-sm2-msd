using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Metamodel
{
    public class NavigationModel<ViewType>
    {
        public View<ViewType> CurrentView { get; private set; }

        private Dictionary<ViewType, View<ViewType>> views = new Dictionary<ViewType, View<ViewType>>();

        public NavigationModel(string name = "", string test = "")
        {

        }

    }
}
