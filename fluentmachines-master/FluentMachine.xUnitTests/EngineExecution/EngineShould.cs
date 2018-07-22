using System;
using System.Linq;
using FluentAssertions;
using FluentMachine.Exceptions;
using FluentMachine.History;
using FluentMachine.Logic;
using Xunit;

namespace FluentMachine.xUnitTests.EngineExecution
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

    public struct BadCommand : ICommand
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

    public class EngineShould
    {
        private readonly Engine _engine;

        public EngineShould()
        {
            _engine = new Engine();

            _engine.Map<States>().StartsWith(States.Open).EndsWith(States.Closed);

            _engine.AddTransition().From(States.Open).To(States.Closed).Accept<TurnRightCommand>();

            _engine.AddTransition().From(States.Closed).To(States.Open).Process<TurnRightCommand>().With(Actions.TurnRightAction);

            _engine.PowerOn();
        }

        [Fact]
        public void ThrowsAnInvalidCommandExceptionWhenExecuteABadCommand()
        {

            System.Action action = () =>
            {
                _engine.Execute(new BadCommand());
            };

            action.ShouldThrow<InvalidCommandException>();
        }

        [Fact]
        public void HaveCurrentStateEqualOpenWhenPowersOn()
        {
            _engine.GetCurrentState().Should().Be(States.Open);
        }

        [Fact]
        public void HaveCurrentStateEqualToClosedWhenExecuteTurnRightCommand()
        {
            _engine.Execute(new TurnRightCommand());

            _engine.GetCurrentState().Should().Be(States.Closed);
        }

        [Fact]
        public void ThrowsReachedEndStateExceptionWhenExecuteTurnRightCommandTwice()
        {
            _engine.Execute(new TurnRightCommand(){ Payload = "Turn right and fall to end state!" });

            System.Action action = () =>
            {
                _engine.Execute(new TurnRightCommand() { Payload = "...don't care"});
            };

            action.ShouldThrow<ReachedEndStateException>();
        }

        [Fact]
        public void AppendAJournalItemWhenExecuteTurnRightCommand()
        {
            _engine.Execute(new TurnRightCommand());

            var count = _engine.Journal.Count;
            count.Should().Be(1);

            var stateChange = _engine.Journal.GetAllItemsOrderByTimestamp().FirstOrDefault().StateChange;

            stateChange.Should().NotBeNull();

            stateChange?.From.Should().Be(States.Open);
            stateChange?.To.Should().Be(States.Closed);
            stateChange?.HasSwitched.Should().BeTrue();
            stateChange?.Command.Should().Be(typeof(TurnRightCommand).Name);
        }
    }
}
