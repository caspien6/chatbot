using FluentAssertions;
using Xunit;

namespace FluentMachine.xUnitTests.PowerOnOff
{
    public class EngineShould
    {

        private readonly Engine _engine;

        public EngineShould()
        {
            _engine = new Engine();

            _engine.Map<States>()
                    .StartsWith(States.PowerOn).NeverEnding();

            _engine.AddTransition()
                    .From(States.PowerOn)
                    .To(States.PowerOff)
                    .Accept<ButtonClickCommand>();

            _engine.AddTransition()
                    .From(States.PowerOff)
                    .To(States.PowerOn)
                    .Accept<ButtonClickCommand>();

            _engine.PowerOn();
        }


        [Fact]
        public void EnginePowerOn_CurrentStateIsStartState()
        {
            _engine.GetCurrentState().Should().Be(_engine.Settings().StartState);
        }

        [Fact]
        public void ExecuteButtonClickCommand_EnginePoweredOn_StateSwitchToPowerOn()
        {
            _engine.Execute(new ButtonClickCommand());

            _engine.GetCurrentState().Should().Be(States.PowerOff);
        }

        [Fact]
        public void ExecuteButtonClickCommandTwice_EnginePoweredOn_StateSwitchToPowerOnThenToPowerOff()
        {
            _engine.Execute(new ButtonClickCommand());

            _engine.GetCurrentState().Should().Be(States.PowerOff);

            _engine.Execute(new ButtonClickCommand());

            _engine.GetCurrentState().Should().Be(States.PowerOn);
        }

    }
}
