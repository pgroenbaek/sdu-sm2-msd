﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder
{
    public interface IViewBuilderMenuTabbedSlider<TBaseView>
    {
        IViewBuilderMenuTabbedSlider<TBaseView> TabbedItem<TView>(string name = null, object icon = null) where TView : TBaseView;
        IViewBuilder<TBaseView> View<TView>(string title = null) where TView : TBaseView;
    }
}