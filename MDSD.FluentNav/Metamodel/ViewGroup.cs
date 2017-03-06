using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Metamodel
{
    public class ViewGroup : AbstractView
    {
        public List<View> SubViews { get; private set; } = new List<View>();
    }
}
