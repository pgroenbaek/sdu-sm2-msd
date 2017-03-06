using MDSD.FluentNav.Builder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder.Interfaces
{
    public interface IViewGroupBuilder<TBaseView>
    {
        IMenuBuilder<TBaseView> Menu();
        IViewBuilder<TBaseView> View<TView>(string title = null) where TView : TBaseView;
    }
}
