using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder.Interfaces
{
    // This TMenuTypeEnum generic is a workaround (in combination with the IsEnum check), the Enum constraint is possibly added in future versions of C#.
    // Source: http://stackoverflow.com/a/79903
    // Source: https://github.com/dotnet/roslyn/issues/262 and referencing issues.
    // For some reason IConvertible was not available.
    public interface IMenuBuilder<TBaseView, TMenuTypeEnum> where TMenuTypeEnum : struct, IComparable, IFormattable//, IConvertible
    {
        TMenuTypeEnum MenuType(); // Not that elegant, impl of the property-get is in GenericFluentNavBuilder.
        IMenuBuilder<TBaseView, TMenuTypeEnum> MenuAttribute();
        IContentBuilder<TBaseView, TMenuTypeEnum> Content();
        IViewBuilder<TBaseView, TMenuTypeEnum> View<TView>(string title = null) where TView : TBaseView;
    }
}
