using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentMachine.Exceptions;

namespace FluentMachine.Configuration
{
    public class Settings
    {
        private readonly Engine _engine;

        private List<string> _states;

        public string StartState { get; private set; }

        public string EndState { get; private set; }

        public Settings(Engine engine)
        {
            _engine = engine;
        }

        #region pass-through operations Map<T>, StartsWith, EndsWith, NeverEnding

        public Settings Map<T>() where T : struct
        {

            _states = SqueezeAllConstStringFields<T>();

            return _engine.Settings();
        }

        public Settings StartsWith(string state)
        {
            CheckStateValidity(state);

            StartState = state;

            return _engine.Settings();
        }

        public Settings EndsWith(string state)
        {
            CheckStateValidity(state);

            EndState = state;

            return _engine.Settings();
        }

        public Settings NeverEnding()
        {
            EndState = null;

            return _engine.Settings();
        }

        #endregion

        public void CheckStateValidity(string state)
        {
            if (!_states.Contains(state))
            {
                throw new InvalidStateException();
            }
        }

        private static List<string> SqueezeAllConstStringFields<T>()
        {

            return (from field in typeof(T).GetFields()
                    where field.FieldType.Name.Equals("string", StringComparison.CurrentCultureIgnoreCase) && field.IsLiteral
                    select field.Name)
                .ToList();
        }

    }
}
