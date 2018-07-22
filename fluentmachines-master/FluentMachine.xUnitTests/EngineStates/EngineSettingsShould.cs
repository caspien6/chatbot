using System;
using FluentAssertions;
using FluentMachine.Configuration;
using FluentMachine.Exceptions;
using Xunit;

namespace FluentMachine.xUnitTests.EngineStates
{
    public struct States
    {
        public const string Dummy = "Dummy";
    }

    public class EngineSettingsShould
    {
        private readonly Engine _engine;

        public EngineSettingsShould()
        {
            _engine = new Engine();
        }
        
        [Fact]
        public void ThrowInvalidStateValueExceptionWhenStartsWithAnInvalidState()
        {
            Action setting = () =>
            {
                _engine.Map<States>().StartsWith("InvalidState");
            };

            setting.ShouldThrow<InvalidStateException>();
        }

        [Fact]
        public void ThrowInvalidStateValueExceptionWhenEndsWithAnInvalidState()
        {
            Action setting = () =>
            {
                _engine.Map<States>().EndsWith("InvalidState");
            };

            setting.ShouldThrow<InvalidStateException>();
        }

        [Fact]
        public void HaveStartAndEndStatesAccordinglyToSettings()
        {
            var settings = _engine.Map<States>().StartsWith(States.Dummy).EndsWith(States.Dummy);

            settings.StartState.Should().Be(States.Dummy);
            settings.EndState.Should().Be(States.Dummy);
        }

        [Fact]
        public void HaveNullEndStateWhenNeverEnding()
        {
            var settings = _engine.Map<States>().StartsWith(States.Dummy).NeverEnding();

            settings.StartState.Should().Be(States.Dummy);
            settings.EndState.Should().BeNullOrEmpty();
        }

    }
}
