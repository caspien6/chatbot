using FluentMachine.Logic;

namespace FluentMachine.xUnitTests.PowerOnOff
{
    public struct States
    {
        public const string PowerOn = "PowerOn";

        public const string PowerOff = "PowerOff";
    }

    public struct ButtonClickCommand : ICommand
    {
        public string Payload { get; set; }
    }

}
