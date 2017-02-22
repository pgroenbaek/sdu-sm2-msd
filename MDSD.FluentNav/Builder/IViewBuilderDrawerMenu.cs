using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder
{
    public interface IViewBuilderDrawerMenu
    {
        IViewBuilderDrawerMenu Spacer(string name = null, object icon = null);
        IViewBuilderDrawerMenu Item<T>(string name = null, object icon = null);
    }
}
