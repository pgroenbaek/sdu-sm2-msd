using MDSD.FluentNav.Validator;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Metamodel
{
    public class NavigationModel
    {
        public bool IsModelBuilt { get; private set; }
        public List<AbstractView> AllViews { get; private set; }

        public NavigationModel()
        {
            IsModelBuilt = false;
            AllViews = new List<AbstractView>();
        }

        public void Initialize()
        {
            IsModelBuilt = true;
            NavigationModelValidator.Validate(this);
        }
    }
}
