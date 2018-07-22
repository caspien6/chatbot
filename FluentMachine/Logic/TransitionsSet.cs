using System.Collections.Generic;

namespace FluentMachine.Logic
{
    public class TransitionsSet : List<Transition>
    {
        public void Execute(ICommand command, Engine engine)
        {
            var filteredTransitions = engine.GetCurrentStateTransitionsList();

            foreach (var t in filteredTransitions)
            {
                var stateChange = t.Execute(command);

                engine.Journal.AddStateChange(stateChange);

                if (!stateChange.HasSwitched) continue;

                engine.CurrentState = t.FinalTransitionState;

                break;
            }

        }
    }
}
