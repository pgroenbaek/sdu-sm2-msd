using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder
{
    public interface IViewBuilderPlain<TBaseView>
    {
        ITransitionBuilder<TBaseView> OnClick(int resourceId);
        IViewBuilder<TBaseView> SubView<TView>(string title = null) where TView : TBaseView;
    }
}
