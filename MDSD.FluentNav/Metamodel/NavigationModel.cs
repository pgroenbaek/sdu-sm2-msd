using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Metamodel
{
    public class NavigationModel<EMenuTypes>
    {
        public View<EMenuTypes> CurrentView { get; private set; }

        private Dictionary<Type, View<EMenuTypes>> views = new Dictionary<Type, View<EMenuTypes>>();

        public NavigationModel()
        {

        }

        public void AddView(Type view, Dictionary<string, Type> transitions, Dictionary<int, Dictionary<string, object>> menuDefinition)
        {

        }

        public void AddTransition(Type fromView, Type toView)
        {

        }


    }
}
