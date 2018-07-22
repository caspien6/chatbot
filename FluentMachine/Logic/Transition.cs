using System;
using FluentMachine.Exceptions;
using FluentMachine.History;

namespace FluentMachine.Logic
{
    public class Transition : IFrom, ITo, IAcceptOrProcess, IWith
    {
        private readonly Engine _engine;

        public ActionDelegate ProcessDelegate { get; private set; }

        public Type InputCommandType { get; private set; }

        public string InitialTransitionState { get; private set; }

        public string FinalTransitionState { get; private set; }

        #region fluent constructors (with Engine DI)

        public Transition(Engine engine)
        {
            _engine = engine;
        }

        private Transition(Engine engine, string from) : this(engine)
        {
            InitialTransitionState = from;
        }

        private Transition(Engine engine, string from, string finalTransitionState) : this(engine, from)
        {
            FinalTransitionState = finalTransitionState;
        }

        private Transition(Engine engine, string from, string finalTransitionState, Type inputCommandType) : this(engine, from, finalTransitionState)
        {
            InputCommandType = inputCommandType;
        }

        private Transition(Engine engine, string from, string finalTransitionState, Type inputCommandType, ActionDelegate processDelegateDelegate)
            : this(engine, from, finalTransitionState, inputCommandType)
        {
            ProcessDelegate = processDelegateDelegate;
        }

        #endregion

        #region fluent operations From, To, [ Accept | Process, With ]

        public ITo From(string from)
        {
            _engine.Settings().CheckStateValidity(from);

            return new Transition(this._engine, from);
        }

        public IAcceptOrProcess To(string to)
        {
            _engine.Settings().CheckStateValidity(to);

            return new Transition(this._engine, this.InitialTransitionState, to);
        }

        public void Accept<T>() where T : struct, ICommand
        {
            _engine.AddTransition(new Transition(_engine, this.InitialTransitionState, this.FinalTransitionState, typeof(T)));
        }

        public IWith Process<T>() where T : struct, ICommand
        {
            return new Transition(this._engine, this.InitialTransitionState, this.FinalTransitionState, typeof(T));

        }

        public void With(ActionDelegate actionDelegate)
        {
            _engine.AddTransition(new Transition(_engine, this.InitialTransitionState, this.FinalTransitionState, this.InputCommandType, actionDelegate));
        }

        #endregion

        public bool StartsFrom(string startState)
        {
            return string.Equals(InitialTransitionState, startState, StringComparison.CurrentCultureIgnoreCase);
        }

        public bool EndsTo(string endState)
        {
            return string.Equals(FinalTransitionState, endState, StringComparison.CurrentCultureIgnoreCase);
        }

        private bool Accepts(ICommand command)
        {
            return command.GetType() == InputCommandType;
        }

        public StateChange Execute(ICommand command)
        {
            if (!Accepts(command))
            {
                throw new InvalidCommandException();
            }

            var hasSwitched = ProcessDelegate == null || ProcessDelegate.Invoke(command);

            return new StateChange(InitialTransitionState, FinalTransitionState, hasSwitched, command.GetType().Name);
        }
    }
}