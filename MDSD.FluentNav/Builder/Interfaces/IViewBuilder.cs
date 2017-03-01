using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder
{
    public interface IViewBuilder<TBaseView>
    {
        IViewBuilderPlain<TBaseView> Content();
        IViewBuilderMenuDrawer<TBaseView> DrawerMenu();
        IViewBuilderMenuTabbedSlider<TBaseView> TabbedSlider();
    }
}
