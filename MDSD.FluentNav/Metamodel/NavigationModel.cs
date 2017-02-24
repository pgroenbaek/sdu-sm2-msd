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

        private Dictionary<Type, View> _views;

        public NavigationModel()
        {
            _views = new Dictionary<Type, View>();
        }

        public void AddView(View view)
        {
            _views.Add(view.Type, view);
        }

        public void HandleEvent(string eventId)
        {

        }
    }
}
