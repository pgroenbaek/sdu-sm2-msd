using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder
{
    public interface INavigationBuilder<ViewType>
    {
        IViewBuilder<ViewType> TopView<T>(string title = null) where T : ViewType;
    }
}
