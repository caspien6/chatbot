namespace FluentMachine.History
{
    public class JournalItem
    {
        public long Timestamp { get; private set; }

        public StateChange StateChange;

        public JournalItem(long timestamp, StateChange stateChange)
        {
            Timestamp = timestamp;

            StateChange = stateChange;

        }
    }
}
