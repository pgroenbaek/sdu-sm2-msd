using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder.Interfaces
{
    public interface IMenuBuilder<TBaseView>
    {
        IMenuBuilder<TBaseView> Type(string type);
        IMenuBuilder<TBaseView> Attribute(string key, object attribute);
        IViewBuilder<TBaseView> View<TView>(string title = null) where TView : TBaseView;
        IViewBuilder<TBaseView> EndViewGroup();
    }
}
