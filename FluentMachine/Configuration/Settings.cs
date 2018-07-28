using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentMachine.Exceptions;

namespace FluentMachine.Configuration
{
    public class Settings
    {
        public int Id { get; set; }

        public int EngineId { get; set; }

        public Engine Engine { get; set; }

        public List<string> _states { get; set; }

        public string StartState { get; private set; }

        public string EndState { get; private set; }


        #region pass-through operations Map<T>, StartsWith, EndsWith, NeverEnding

        public Settings Map<T>() where T : struct
        {

            _states = SqueezeAllConstStringFields<T>();

            return Engine.Settings();
        }

        public Settings StartsWith(string state)
        {
            CheckStateValidity(state);

            StartState = state;

            return Engine.Settings();
        }

        public Settings EndsWith(string state)
        {
            CheckStateValidity(state);

            EndState = state;

            return Engine.Settings();
        }

        public Settings NeverEnding()
        {
            EndState = null;

            return Engine.Settings();
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
