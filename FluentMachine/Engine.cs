using System.Collections.Generic;
using FluentMachine.Configuration;
using FluentMachine.Exceptions;
using FluentMachine.History;
using FluentMachine.Logic;

namespace FluentMachine
{
    public class Engine
    {
        public Journal Journal = new Journal();

        private Settings _settings;

        private TransitionsSet _transitions;

        public string CurrentState { set; get; }

        #region Settings

        public Settings Map<T>() where T : struct
        {
            _settings = new Settings(this);

            return _settings.Map<T>();
        }

        public Settings Settings()
        {
            return _settings;
        }

        #endregion

        #region Transitions

        public IFrom AddTransition()
        {
            return new Transition(this);
        }

        public void AddTransition(Transition transition)
        {
            if (_transitions == null)
            {
                _transitions = new TransitionsSet();
            }

            _transitions.Add(transition);
        }
        
        public List<Transition> GetTransitions()
        {
            return _transitions;
        }

        #endregion

        public void PowerOn()
        {
            CurrentState = _settings.StartState;
        }

        public void Execute(ICommand command)
        {

            if (CurrentState == _settings.EndState)
            {
                throw new ReachedEndStateException();
            };

            _transitions.Execute(command, this);
        }
        
        public List<Transition> GetCurrentStateTransitionsList()
        {
            return GetTransitions().FindAll(t => t.StartsFrom(GetCurrentState()));
        }

        public string GetCurrentState()
        {
            return CurrentState;
        }
    }
}
