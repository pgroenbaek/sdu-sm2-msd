using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder.Interfaces
{
    public interface IContentBuilder<TBaseView>
    {
        ITransitionBuilder<TBaseView> OnClick(int resourceId);
        IViewBuilder<TBaseView> SubView<TView>(string title = null) where TView : TBaseView;
        IViewBuilder<TBaseView> View<TView>(string title = null) where TView : TBaseView;
        IViewGroupBuilder<TBaseView> ViewGroup();
    }
}
