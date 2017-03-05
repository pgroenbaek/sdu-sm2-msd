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
        IViewBuilder<TBaseView> View<TView>(string title = null) where TView : TBaseView;
    }
}
