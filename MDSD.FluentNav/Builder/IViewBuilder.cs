using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder
{
    public interface IViewBuilder
    {
        IViewBuilderPlain Plain();
        IViewBuilderMenuDrawer DrawerMenu();
        IViewBuilderMenuTabbedSlider TabbedSlider();
    }
}
