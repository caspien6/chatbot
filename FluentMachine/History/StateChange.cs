using FluentMachine.Logic;

namespace FluentMachine.History
{
    public class StateChange
    {
        public string From { get; private set; }

        public string To { get; private set; }

        public bool HasSwitched { get; private set; }

        public string Command { get; private set; }

        public StateChange(string from, string to, bool hasSwitched, string command)
        {
            From = from;

            To = to;

            HasSwitched = hasSwitched;

            Command = command;
        }
    }
}
