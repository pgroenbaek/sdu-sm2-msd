using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder.Interfaces
{
    public interface IContentBuilder<TBaseView, TMenuTypeEnum> where TMenuTypeEnum : struct, IComparable, IFormattable//, IConvertible
    {
        ITransitionBuilder<TBaseView, TMenuTypeEnum> OnClick(int resourceId);
        IViewBuilder<TBaseView, TMenuTypeEnum> View<TView>(string title = null) where TView : TBaseView;
    }
}
