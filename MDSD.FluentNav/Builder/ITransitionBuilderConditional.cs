using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder
{
    public interface ITransitionBuilderConditional<TBaseView>
    {
        ITransitionBuilderConditional<TBaseView> ElseIfNavigateTo<TView>(Func<bool> condition) where TView : TBaseView;
        IViewBuilderPlain<TBaseView> ElseNavigateTo<TView>() where TView : TBaseView;
    }
}
