using System.Collections.Generic;
using FluentMachine.Configuration;
using FluentMachine.Exceptions;
using FluentMachine.History;
using FluentMachine.Logic;

namespace FluentMachine
{
    public class Engine
    {
        public int Id { get; set; }

        public Journal Journal = new Journal();

        public Settings Setting { get; set; }

        public TransitionsSet _transitions { get; set; }

        public string CurrentState { set; get; }

        #region Settings

        public Settings Map<T>() where T : struct
        {
            Setting = new Settings();
            Setting.Engine = this;
            Setting.EngineId = this.Id;
            return Setting.Map<T>();
        }

        public Settings Settings()
        {
            return Setting;
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
            CurrentState = Setting.StartState;
        }

        public void Execute(ICommand command)
        {

            if (CurrentState == Setting.EndState)
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
