using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using MDSD.FluentNav.Builder.Interfaces;
using MDSD.FluentNav.Metamodel;
using System;
using System.Linq;
using System.Collections.Generic;
using MDSD.FluentNav.Builder.Droid.Containers;

namespace MDSD.FluentNav.Builder.Droid
{
    public abstract class FluentNavAppCompatActivity : AppCompatActivity
    {
        public const string MenuDrawer = "Drawer";

        private Android.Support.V7.Widget.Toolbar _toolbar;
        private Metamodel.Menu _appliedMenuDef;
        private NavigationModel _navModel;

        private Metamodel.View _currentView;
        private Stack<Transition> _transitionStack = new Stack<Transition>();

        private GenericFluentNavBuilder<Android.Support.V4.App.Fragment> _fluentNavBuilder = new GenericFluentNavBuilder<Android.Support.V4.App.Fragment>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_fluentnav);
            _toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.activity_fluentnav_toolbar);
            SetSupportActionBar(_toolbar);

            if (savedInstanceState == null)
            {
                BuildNavigation(_fluentNavBuilder);

                _navModel = _fluentNavBuilder.Build();
                ApplyView(_currentView);
            }
        }

        internal Android.Support.V7.Widget.Toolbar GetToolbar()
        {
            return _toolbar;
        }

        internal Metamodel.Menu GetAppliedMenuDefinition()
        {
            return _appliedMenuDef;
        }

        internal Metamodel.View GetAppliedView()
        {
            return _currentView;
        }


        /// <summary>
        ///   Override with a definition of how views should be bound together.
        /// </summary>
        /// <param name="navigation">Builder interface</param>
        protected abstract void BuildNavigation(INavigationBuilder<Android.Support.V4.App.Fragment> navigation);


        

        public override void OnBackPressed()
        {
            if (_transitionStack.Count > 0)
            {
                _currentView = _transitionStack.Peek().SourceView;
                _transitionStack.Pop();
                ApplyView(_currentView);
                return;
            }
            Finish();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            return base.OnOptionsItemSelected(item);
        }

        public void HandleEvent(string eventId)
        {
            if (_currentView == null || !_currentView.Transitions.ContainsKey(eventId))
            {
                return;
            }

            Transition nextTransition = null;
            if (_currentView.Transitions.ContainsKey(eventId))
            {
                // Evaluate from first to last condition.
                for (int i = 0; i < _currentView.Transitions[eventId].Count; i++)
                {
                    // Return the transition where the first null or true was encountered.
                    // The condition being null means either "no condition" or it corresponds to the final "else".
                    Transition t = _currentView.Transitions[eventId][i];

                    if (t.Conditional == null)
                    {
                         nextTransition = t;
                    }

                    if (t.Conditional.Invoke())
                    {
                        nextTransition = t;
                    }
                }
            }

            if (nextTransition != null)
            {
                _currentView = FindViewRecursively(null, nextTransition.TargetView);
                if (_currentView != null) {
                    _transitionStack.Push(nextTransition);
                    ApplyView(_currentView);
                }
            }
        }

        public Metamodel.View FindViewRecursively(Metamodel.View currentView, Type target)
        {
            List<Metamodel.View> results = null;
            if(currentView == null)
            {
                results = _navModel.AllViews.Where(v => v.Type == target).ToList();
            }
            else if (currentView is Metamodel.ViewGroup)
            {
                results = ((Metamodel.ViewGroup) currentView).SubViews.Where(v => v.Type == target).ToList();
            }

            if (results != null)
            {
                Metamodel.View result = results.First();
                if (result != null)
                {
                    return result;
                }

                if (currentView == null)
                {
                    foreach(Metamodel.View v in _navModel.AllViews)
                    {
                        return FindViewRecursively(v, target);
                    }
                }
                else if (currentView is Metamodel.ViewGroup)
                {
                    foreach (Metamodel.View v in ((Metamodel.ViewGroup)currentView).SubViews)
                    {
                        return FindViewRecursively(v, target);
                    }
                }
            }
            return null;
        }
        

        private void ApplyView(Metamodel.View view)
        {
            Type contentType = view.Type;
            _appliedMenuDef = view.MenuDefinition;
            
            // Set basic attributes.
            SupportActionBar.Title = view.Title;

            // Configure style of menu.
            Android.Support.V4.App.Fragment container = null;
            switch(_appliedMenuDef.MenuType)
            {
                case MenuDrawer:
                    container = new DrawerAppCompatContainer();
                    break;

                default:
                    container = new PlainAppCompatContainer();
                    break;
            }
            SupportFragmentManager
                .BeginTransaction()
                .Replace(Resource.Id.activity_fluentnav_containerframe, container)
                .DisallowAddToBackStack()
                .Commit();

            // Instantiate content fragment.
            Android.Support.V4.App.Fragment contentFragment = (Android.Support.V4.App.Fragment)Activator.CreateInstance(contentType);
            
            // Set the content frame to the specified type of fragment.
            SupportFragmentManager
                .BeginTransaction()
                .Replace(Resource.Id.activity_fluentnav_contentframe, contentFragment)
                .DisallowAddToBackStack()
                .Commit();
        }
    }
}
