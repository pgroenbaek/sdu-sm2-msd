using MDSD.FluentNav.Builder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder.Interfaces
{
    public interface IViewBuilder<TBaseView>
    {
        IContentBuilder<TBaseView> Content();
        IViewBuilder<TBaseView> View<TView>(string title = null) where TView : TBaseView;
        IViewGroupBuilder<TBaseView> ViewGroup();
    }
}
