using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Metamodel
{
    public class GenericMenu<TMenuTypeEnum> where TMenuTypeEnum : struct, IComparable, IFormattable//, IConvertible
    {
        public TMenuTypeEnum MenuType { get; private set; }
        public Dictionary<string, object> MenuAttributes { get; private set; }

        public GenericMenu(TMenuTypeEnum menuType)
        {
            MenuType = menuType;
            MenuAttributes = new Dictionary<string, object>();
        }
    }
}
