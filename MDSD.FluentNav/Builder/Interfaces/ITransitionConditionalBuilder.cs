using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder.Interfaces
{
    public interface ITransitionConditionalBuilder<TBaseView>
    {
        ITransitionConditionalBuilder<TBaseView> ElseIfNavigateTo<TView>(Func<bool> condition) where TView : TBaseView;
        IContentBuilder<TBaseView> ElseNavigateTo<TView>() where TView : TBaseView;
    }
}
