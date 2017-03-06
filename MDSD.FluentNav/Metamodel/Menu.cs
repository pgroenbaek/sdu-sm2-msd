using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Metamodel
{
    public class Menu
    {
        public string MenuType { get; private set; }
        public Dictionary<string, object> MenuAttributes { get; private set; }

        public Menu(string menuType)
        {
            MenuType = menuType;
            MenuAttributes = new Dictionary<string, object>();
        }
    }
}
