using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder.Interfaces
{
    public interface IMenuBuilder<TBaseView>
    {
        IMenuBuilder<TBaseView> MenuType(int type);
        IMenuBuilder<TBaseView> MenuAttribute(string key, string attribute);
        IContentBuilder<TBaseView> Content();
        IViewBuilder<TBaseView> View<TView>(string title = null) where TView : TBaseView;
    }
}
