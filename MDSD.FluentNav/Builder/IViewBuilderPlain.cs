using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder
{
    public interface IViewBuilderPlain<ViewType>
    {
        ITransitionBuilder<ViewType> OnClick(int resourceId);
        IViewBuilder<ViewType> SubView<T>(string title = null) where T : ViewType;
    }
}
