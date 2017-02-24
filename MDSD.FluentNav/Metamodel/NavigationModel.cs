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

        private bool _isModelBuilt;
        private Dictionary<Type, View> _views;

        public NavigationModel()
        {
            _isModelBuilt = false;
            _views = new Dictionary<Type, View>();
        }

        public void AddView(View view)
        {
            if(_isModelBuilt)
            {
                return;
            }
            _views.Add(view.Type, view);
        }

        public void Initialize()
        {
            _isModelBuilt = true;
            NavigationModelValidator.Validate(this);
        }

        public void HandleEvent(string eventId)
        {
            if(CurrentView != null)
            {
                return;
            }

            Transition nextTransition = CurrentView.NextTransition(eventId);
            if(nextTransition != null && _views.ContainsKey(nextTransition.TargetView))
            {
                CurrentView = _views[nextTransition.TargetView];
            }
        }

        public void HandleBackEvent()
        {

        }
    }
}
