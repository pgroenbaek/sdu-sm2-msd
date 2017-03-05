using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder.Interfaces
{
    public interface ITransitionConditionalBuilder<TBaseView, TMenuTypeEnum> where TMenuTypeEnum : struct, IComparable, IFormattable//, IConvertible
    {
        ITransitionConditionalBuilder<TBaseView, TMenuTypeEnum> ElseIfNavigateTo<TView>(Func<bool> condition) where TView : TBaseView;
        IContentBuilder<TBaseView, TMenuTypeEnum> ElseNavigateTo<TView>() where TView : TBaseView;
    }
}
