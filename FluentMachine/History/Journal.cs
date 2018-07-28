using System;
using System.Collections.Generic;
using System.Linq;
using FluentMachine.Logic;

namespace FluentMachine.History
{
    public class Journal : List<JournalItem>
    {
        public int Id { get; set; }

        public void AddStateChange(string from, string to, bool hasSwitched, ICommand command)
        {
            AddStateChange(new StateChange(from, to, hasSwitched, command.GetType().Name));
        }

        public void AddStateChange(StateChange stateChange)
        {
            Add(new JournalItem(GetUtcTimestampInMilliseconds(), stateChange));
        }

        public static long GetUtcTimestampInMilliseconds()
        {
            var ts = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1));

            return (long) ts.TotalMilliseconds;
        }

        public List<JournalItem> GetAllItemsOrderByTimestamp()
        {
            return this.OrderBy(item => item.Timestamp).ToList();
        }
    }
}
