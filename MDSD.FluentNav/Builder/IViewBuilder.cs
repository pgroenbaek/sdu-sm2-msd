using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder
{
    public interface IViewBuilder<ViewType>
    {
        IViewBuilderPlain<ViewType> Plain();
        IViewBuilderMenuDrawer<ViewType> DrawerMenu();
        IViewBuilderMenuTabbedSlider<ViewType> TabbedSlider();
    }
}
