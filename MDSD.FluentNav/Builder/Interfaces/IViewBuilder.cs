using MDSD.FluentNav.Builder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder.Interfaces
{
    public interface IViewBuilder<TBaseView, TMenuTypeEnum> where TMenuTypeEnum : struct, IComparable, IFormattable//, IConvertible
    {
        IContentBuilder<TBaseView, TMenuTypeEnum> Content();
        IMenuBuilder<TBaseView, TMenuTypeEnum> Menu();
        IViewBuilder<TBaseView, TMenuTypeEnum> View<TView>(string title = null) where TView : TBaseView;
    }
}
