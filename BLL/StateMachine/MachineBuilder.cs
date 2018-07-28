using Stateless;
using Stateless.Graph;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.StateMachine
{
    public class ChatStoryMenu
    {
        enum Trigger
        {
            Start,
            BackToMainMenu,
            Info,
            Stories,
            ChooseStory,
            GiveName,
            GivePrimaryAttribute,
            StoryCommand

        }

        enum State
        {
            MainMenu,
            ListStory,
            CharacterCreation,
            ChooseName,
            ChoosePrimaryAttribute,
            InStory
        }

        State _state = State.MainMenu;

        StateMachine<State, Trigger> _machine;
        StateMachine<State, Trigger>.TriggerWithParameters<string> _setNameTrigger;

        string _name;

        string _callee;

        public ChatStoryMenu()
        {
            _machine = new StateMachine<State, Trigger>(() => _state, s => _state = s);

            _setNameTrigger = _machine.SetTriggerParameters<string>(Trigger.GiveName);

            _machine.Configure(State.MainMenu)
                .InternalTransition(Trigger.Info, t => Console.WriteLine())
                .InternalTransition(Trigger.Start, t => Console.WriteLine())
                .Permit(Trigger.Stories, State.ListStory);

            _machine.Configure(State.ListStory)
                .Permit(Trigger.ChooseStory, State.ChooseName)
                .Permit(Trigger.BackToMainMenu, State.MainMenu);

            _machine.Configure(State.CharacterCreation)
                .Permit(Trigger.BackToMainMenu, State.MainMenu);

            _machine.Configure(State.ChooseName)
                .SubstateOf(State.CharacterCreation)
                .Permit(Trigger.GiveName, State.ChoosePrimaryAttribute);
            _machine.Configure(State.ChoosePrimaryAttribute)
                .SubstateOf(State.CharacterCreation)
                .Permit(Trigger.GivePrimaryAttribute, State.InStory);

            _machine.Configure(State.InStory)
                .Permit(Trigger.BackToMainMenu, State.MainMenu)
                .InternalTransition(Trigger.StoryCommand, t => Console.WriteLine() );

            /*_machine.Configure(State.Ringing)
                .OnEntryFrom(_setCalleeTrigger, callee => OnDialed(callee), "Caller number to call")
                .Permit(Trigger.CallConnected, State.Connected);

            _machine.Configure(State.Connected)
                .OnEntry(t => StartCallTimer())
                .OnExit(t => StopCallTimer())
                .InternalTransition(Trigger.MuteMicrophone, t => OnMute())
                .InternalTransition(Trigger.UnmuteMicrophone, t => OnUnmute())
                .InternalTransition<int>(_setVolumeTrigger, (volume, t) => OnSetVolume(volume))
                .Permit(Trigger.LeftMessage, State.OffHook)
                .Permit(Trigger.PlacedOnHold, State.OnHold);

            _machine.Configure(State.OnHold)
                .SubstateOf(State.Connected)
                .Permit(Trigger.TakenOffHold, State.Connected)
                .Permit(Trigger.PhoneHurledAgainstWall, State.PhoneDestroyed);*/
        }


        public void OnStart()
        {
            _machine.Fire(Trigger.Start);
        }

        public void OnInfo()
        {
            _machine.Fire(Trigger.Info);
        }
        public void OnStories()
        {
            _machine.Fire(Trigger.Stories);
        }
        public void OnChoosingStory()
        {
            _machine.Fire(Trigger.ChooseStory);
        }
        public void OnGivingName()
        {
            _machine.Fire(Trigger.GiveName);
        }
        public void OnOnGivingPrimaryAttribute()
        {
            _machine.Fire(Trigger.GivePrimaryAttribute);
        }
        public void OnStoryCommand()
        {
            _machine.Fire(Trigger.StoryCommand);
        }
        public void OnBackToMainMenu()
        {
            _machine.Fire(Trigger.BackToMainMenu);
        }


    }

   /* public class PhoneCall
    {
        enum Trigger
        {
            CallDialed,
            CallConnected,
            LeftMessage,
            PlacedOnHold,
            TakenOffHold,
            PhoneHurledAgainstWall,
            MuteMicrophone,
            UnmuteMicrophone,
            SetVolume
        }

        enum State
        {
            OffHook,
            Ringing,
            Connected,
            OnHold,
            PhoneDestroyed
        }

        State _state = State.OffHook;

        StateMachine<State, Trigger> _machine;
        StateMachine<State, Trigger>.TriggerWithParameters<int> _setVolumeTrigger;

        StateMachine<State, Trigger>.TriggerWithParameters<string> _setCalleeTrigger;

        string _caller;

        string _callee;

        public PhoneCall(string caller)
        {
            _caller = caller;
            _machine = new StateMachine<State, Trigger>(() => _state, s => _state = s);

            _setVolumeTrigger = _machine.SetTriggerParameters<int>(Trigger.SetVolume);
            _setCalleeTrigger = _machine.SetTriggerParameters<string>(Trigger.CallDialed);

            _machine.Configure(State.OffHook)
                .Permit(Trigger.CallDialed, State.Ringing);

            _machine.Configure(State.Ringing)
                .OnEntryFrom(_setCalleeTrigger, callee => OnDialed(callee), "Caller number to call")
                .Permit(Trigger.CallConnected, State.Connected);

            _machine.Configure(State.Connected)
                .OnEntry(t => StartCallTimer())
                .OnExit(t => StopCallTimer())
                .InternalTransition(Trigger.MuteMicrophone, t => OnMute())
                .InternalTransition(Trigger.UnmuteMicrophone, t => OnUnmute())
                .InternalTransition<int>(_setVolumeTrigger, (volume, t) => OnSetVolume(volume))
                .Permit(Trigger.LeftMessage, State.OffHook)
                .Permit(Trigger.PlacedOnHold, State.OnHold);

            _machine.Configure(State.OnHold)
                .SubstateOf(State.Connected)
                .Permit(Trigger.TakenOffHold, State.Connected)
                .Permit(Trigger.PhoneHurledAgainstWall, State.PhoneDestroyed);
        }

        void OnSetVolume(int volume)
        {
            Console.WriteLine("Volume set to " + volume + "!");
        }

        void OnUnmute()
        {
            Console.WriteLine("Microphone unmuted!");
        }

        void OnMute()
        {
            Console.WriteLine("Microphone muted!");
        }

        void OnDialed(string callee)
        {
            _callee = callee;
            Console.WriteLine("[Phone Call] placed for : [{0}]", _callee);
        }

        void StartCallTimer()
        {
            Console.WriteLine("[Timer:] Call started at {0}", DateTime.Now);
        }

        void StopCallTimer()
        {
            Console.WriteLine("[Timer:] Call ended at {0}", DateTime.Now);
        }

        public void Mute()
        {
            _machine.Fire(Trigger.MuteMicrophone);
        }

        public void Unmute()
        {
            _machine.Fire(Trigger.UnmuteMicrophone);
        }

        public void SetVolume(int volume)
        {
            _machine.Fire(_setVolumeTrigger, volume);
        }

        public void Print()
        {
            Console.WriteLine("[{1}] placed call and [Status:] {0}", _machine, _caller);
        }

        public void Dialed(string callee)
        {
            _machine.Fire(_setCalleeTrigger, callee);
        }

        public void Connected()
        {
            _machine.Fire(Trigger.CallConnected);
        }

        public void Hold()
        {
            _machine.Fire(Trigger.PlacedOnHold);
        }

        public void Resume()
        {
            _machine.Fire(Trigger.TakenOffHold);
        }

        public string ToDotGraph()
        {
            return UmlDotGraph.Format(_machine.GetInfo());
        }
    }*/


    public class t
    {
        public static void Test()
        {
            /*var phoneCall = new PhoneCall("Lokesh");

            phoneCall.Print();
            phoneCall.Dialed("Prameela");
            phoneCall.Print();
            phoneCall.Connected();
            phoneCall.Print();
            phoneCall.SetVolume(2);
            phoneCall.Print();
            phoneCall.Hold();
            phoneCall.Print();
            phoneCall.Mute();
            phoneCall.Print();
            phoneCall.Unmute();
            phoneCall.Print();
            phoneCall.Resume();
            phoneCall.Print();
            phoneCall.SetVolume(11);
            phoneCall.Print();*/


            //var character = new State();
            // story1.Edges.Add("Karakter", character);
            //character.DisplayText = "Karakter készítés";


        }

    }
   
   
}
