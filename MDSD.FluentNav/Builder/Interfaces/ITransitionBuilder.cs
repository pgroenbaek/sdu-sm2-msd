using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder.Interfaces
{
    public interface ITransitionBuilder<TBaseView, TMenuTypeEnum> where TMenuTypeEnum : struct, IComparable, IFormattable//, IConvertible
    {
        ITransitionConditionalBuilder<TBaseView, TMenuTypeEnum> NavigateToIf<TView>(Func<bool> condition) where TView : TBaseView;
        IContentBuilder<TBaseView, TMenuTypeEnum> NavigateTo<TView>() where TView : TBaseView; // TODO, transition animation
    }
}
