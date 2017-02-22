using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder
{
    public interface ITransitionBuilder<ViewType>
    {
        IViewBuilderPlain<ViewType> NavigateTo<T>() where T : ViewType; // TODO, transition animation
    }
}
