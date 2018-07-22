using FluentMachine;
using FluentMachine.Logic;
using System;

namespace Tester
{
    class Program
    {
        public delegate bool ActionDelegate(ICommand command);

        public struct Actions
        {
            public static bool SwitchOnBetween2PMAnd6PM(ICommand command)
            {
                Console.WriteLine("SwithcOnBetween2PMAnd6PM");

                return true;
            }
        }

        public struct States
        {
            public const string PowerOn = "PowerOn";

            public const string PowerOff = "PowerOff";
        }

        public struct ButtonClickCommand : ICommand
        {
            public string Payload { get; set; }
        }

        static void Main(string[] args)
        {
            var engine = new Engine();
            engine.Map<States>().StartsWith(States.PowerOn).NeverEnding();

            engine.AddTransition().From(States.PowerOff).To(States.PowerOn).Process<ButtonClickCommand>().With(Actions.SwitchOnBetween2PMAnd6PM);

            engine.AddTransition().From(States.PowerOn).To(States.PowerOff).Accept<ButtonClickCommand>();


            engine.PowerOn();
            engine.CurrentState = States.PowerOff;
            engine.Execute(new ButtonClickCommand());
            engine.Execute(new ButtonClickCommand());

            if (engine.GetCurrentState() == States.PowerOff)
            {

                Console.WriteLine("Power-Off state");
            }


            Console.ReadKey();
        }
    }
}
