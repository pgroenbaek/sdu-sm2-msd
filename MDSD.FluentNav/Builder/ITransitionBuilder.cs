﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Builder
{
    public interface ITransitionBuilder
    {
        IViewBuilderPlain NavigateTo<T>(); // TODO, transition animation
    }
}
