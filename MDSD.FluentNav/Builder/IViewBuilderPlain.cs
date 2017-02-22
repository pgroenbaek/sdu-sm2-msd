using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder
{
    public interface IViewBuilderPlain
    {
        ITransitionBuilder OnClick(int resourceId);
        IViewBuilder SubView<T>(string title = null);
    }
}
