using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder
{
    public interface INavigationBuilder<TBaseView>
    {
        IViewBuilder<TBaseView> TopView<TView>(string title = null) where TView : TBaseView;
    }
}
