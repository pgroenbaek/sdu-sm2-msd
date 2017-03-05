﻿using MDSD.FluentNav.Metamodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Validator
{
    public class NavigationModelValidator<TMenuTypeEnum> where TMenuTypeEnum : struct, IComparable, IFormattable//, IConvertible
    {
        /// <summary>
        ///   Validates a navigation model, ensuring the following is in order:
        ///   1. All navigation targets are present as views.
        ///   2. 
        ///   
        ///   An exception is thrown if the momdel is invalid.
        /// </summary>
        /// <param name="navModel">The navigation model to validate.</param>
        /// <exception cref="ArgumentException">The model provided as an argument was invalid.</exception>
        public static void Validate(NavigationModel<TMenuTypeEnum> navModel)
        {
            // Validate point 1.
            foreach(View<TMenuTypeEnum> view in navModel._views.Values)
            {
                foreach(List<Transition<TMenuTypeEnum>> transitionList in view._transitions.Values)
                {
                    foreach(Transition<TMenuTypeEnum> t in transitionList)
                    {
                        if(!navModel._views.ContainsKey(t.TargetView))
                        {
                            throw new ArgumentException("Navigation target '" + t.TargetView.ToString() + "' was not present as a view.");
                        }
                    }
                }
            }
        }
    }
}
