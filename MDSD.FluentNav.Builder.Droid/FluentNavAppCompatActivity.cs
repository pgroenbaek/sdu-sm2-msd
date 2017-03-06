using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using MDSD.FluentNav.Builder.Interfaces;
using MDSD.FluentNav.Metamodel;
using System;
using System.Collections.Generic;

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
                ApplyView(_navModel.CurrentView);
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


        /// <summary>
        ///   Override with a definition of how views should be bound together.
        /// </summary>
        /// <param name="navigation">Builder interface</param>
        protected abstract void BuildNavigation(INavigationBuilder<Android.Support.V4.App.Fragment> navigation);



        //////////////////////////////////////
        //////////////////////////////////////
        ///   Using the built metamodel    ///
        //////////////////////////////////////
        //////////////////////////////////////
        

        // Returns true if transition stack is empty.
        public bool HandleBackPressed()
        {
            if (_transitionStack.Count > 0)
            {
                _currentView = _transitionStack.Peek().SourceView;
                _transitionStack.Pop();
                return false;
            }
            return true;
        }

        public override void OnBackPressed()
        {
            bool isEmpty = _navModel.HandleBackPressed();
            if(isEmpty)
            {
                Finish();
                return;
            }
            ApplyView(_navModel.CurrentView);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Console.WriteLine(item);
            return base.OnOptionsItemSelected(item);
        }

        public void HandleEvent(string eventId)
        {
            if (_currentView == null)
            {
                return null;
            }

            Transition nextTransition = CurrentView.NextTransition(eventId);
            if (nextTransition != null && _views.ContainsKey(nextTransition.TargetView))
            {
                CurrentView = _navModel.[nextTransition.TargetView];
                _transitionStack.Push(nextTransition);
            }
            ApplyView(_navModel.CurrentView);
        }

        internal Transition NextTransition(string eventId)
        {
            if (Transitions.ContainsKey(eventId))
            {
                // Evaluate from first to last condition.
                for (int i = 0; i < Transitions[eventId].Count; i++)
                {
                    // Return the transition where the first null or true was encountered.
                    // The condition being null means either "no condition" or it corresponds to the final "else".
                    Transition t = Transitions[eventId][i];

                    if (t.Conditional == null)
                    {
                        return t;
                    }

                    if (t.Conditional.Invoke())
                    {
                        return t;
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
            //MenuType? menuType = null;
            //Enum.TryParse<MenuType>(_appliedMenuDef.MenuType, out menuType);
            Android.Support.V4.App.Fragment container = null;
            /*switch(menuType)
            {
                case MenuType.Plain:
                    container = new PlainAppCompatContainer();
                    break;

                case MenuType.Drawer:
                    container = new DrawerAppCompatContainer();
                    break;

                case MenuType.TabbedSlider:
                    container = new TabbedSliderAppCompatContainer();
                    break;

                default:
                    return;
            }*/
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
