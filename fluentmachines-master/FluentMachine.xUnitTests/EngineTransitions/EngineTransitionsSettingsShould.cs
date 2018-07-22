using System;
using System.Linq;
using FluentAssertions;
using FluentMachine.Exceptions;
using FluentMachine.Logic;
using Xunit;

namespace FluentMachine.xUnitTests.EngineTransitions
{
    public struct States
    {
        public const string Open = "Open";

        public const string Closed = "Closed";
    }

    public struct TurnRightCommand : ICommand
    {
        public string Payload { get; set; }
    }

    public struct Actions
    {
        public static bool TurnRightAction(ICommand command)
        {
            throw new NotImplementedException();
        }
    }

    public class EngineTransitionsSettingsShould
    {
        private readonly Engine _engine;

        public EngineTransitionsSettingsShould()
        {
            _engine = new Engine();

            _engine.Map<States>().StartsWith(States.Open).EndsWith(States.Closed);     
        }

        [Fact]
        public void ThrowInvalidStateValueExceptionWhenTransitionFromInvalidState()
        {
            System.Action setting = () =>
            {
                _engine.AddTransition().From("InvalidState");
            };

            setting.ShouldThrow<InvalidStateException>();
        }

        [Fact]
        public void ThrowInvalidStateValueExceptionWhenHasATransitionToInvalidState()
        {
            System.Action setting = () =>
            {
                _engine.AddTransition().From(States.Open).To("Invalid State");
            };

            setting.ShouldThrow<InvalidStateException>();
        }

        [Fact]
        public void HaveInputCommandAssignableToATurnRightCommandWhenHasATransitionThatAcceptIt()
        {
            _engine.AddTransition().From(States.Open).To(States.Closed).Accept<TurnRightCommand>();

            var transition = _engine.GetTransitions().First();

            transition.InputCommandType.Should().BeAssignableTo<TurnRightCommand>();
        }

        [Fact]
        public void ThrowsNotImplementedExceptionWhenProcessACommandAssociatedWithAnNotImplementedAction()
        {
            _engine.AddTransition()
                .From(States.Open)
                .To(States.Closed)
                .Process<TurnRightCommand>()
                .With(Actions.TurnRightAction);

            var transition = _engine.GetTransitions().First();

            transition.ProcessDelegate.Should().NotBeNull();

            Action action = () =>
            {
                transition.ProcessDelegate(new TurnRightCommand());
            };

            action.ShouldThrow<NotImplementedException>();

        }
    }
}
