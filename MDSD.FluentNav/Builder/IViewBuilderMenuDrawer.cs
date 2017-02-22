﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder
{
    public interface IViewBuilderMenuDrawer<ViewType>
    {
        IViewBuilderMenuDrawer<ViewType> Spacer(string name = null, object icon = null);
        IViewBuilderMenuDrawer<ViewType> Item<T>(string name = null, object icon = null) where T : ViewType;
        IViewBuilder<ViewType> SubView<T>(string title = null) where T : ViewType;
    }
}