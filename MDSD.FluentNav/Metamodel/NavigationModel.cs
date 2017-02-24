using MDSD.FluentNav.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSD.FluentNav.Metamodel
{
    public class NavigationModel
    {
        public View CurrentView { get; private set; }
        public bool IsModelBuilt { get; private set; }
        
        private Dictionary<Type, View> _views;

        public NavigationModel()
        {
            IsModelBuilt = false;
            _views = new Dictionary<Type, View>();
        }

        public void AddView(View view)
        {
            if(IsModelBuilt)
            {
                return;
            }
            _views.Add(view.Type, view);
        }

        public void Initialize()
        {
            IsModelBuilt = true;
            NavigationModelValidator.Validate(this);
        }

        public Transition HandleEvent(string eventId)
        {
            if(CurrentView != null)
            {
                return null;
            }

            Transition nextTransition = CurrentView.NextTransition(eventId);
            if(nextTransition != null && _views.ContainsKey(nextTransition.TargetView))
            {
                CurrentView = _views[nextTransition.TargetView];
            }
            return nextTransition;
        }
    }
}
