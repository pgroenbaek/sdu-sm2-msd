using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder
{
    public interface ITransitionBuilder<TBaseView>
    {
        IViewBuilderPlain<TBaseView> NavigateTo<TView>() where TView : TBaseView; // TODO, transition animation
    }
}
