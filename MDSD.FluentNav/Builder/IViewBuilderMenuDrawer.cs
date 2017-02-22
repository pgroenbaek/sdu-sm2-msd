﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder
{
    public interface IViewBuilderMenuDrawer
    {
        IViewBuilderMenuDrawer Spacer(string name = null, object icon = null);
        IViewBuilderMenuDrawer Item<T>(string name = null, object icon = null);
        IViewBuilder SubView<T>(string title = null);
    }
}
