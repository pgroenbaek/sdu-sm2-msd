using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder.Interfaces
{
    public interface ITransitionBuilder<TBaseView>
    {
        ITransitionConditionalBuilder<TBaseView> NavigateToIf<TView>(Func<bool> condition) where TView : TBaseView;
        IContentBuilder<TBaseView> NavigateTo<TView>() where TView : TBaseView; // TODO, transition animation
    }
}
