﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder.Interfaces
{
    public interface INavigationBuilder<TBaseView, TMenuTypeEnum> where TMenuTypeEnum : struct, IComparable, IFormattable//, IConvertible
    {
        IViewBuilder<TBaseView, TMenuTypeEnum> View<TView>(string title = null) where TView : TBaseView;
    }
}
