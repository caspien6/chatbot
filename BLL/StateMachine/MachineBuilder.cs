using BLL.FacebookMessageHierarchy;
using BLL.Models.Game;
using BLL.Repository;
using BLL.Repository.StoryRepository;
using Stateless;
using Stateless.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.StateMachine
{
    public class MainMenuStateMachine
    {
        public State _state = State.MainMenu;
        StateMachine<State, Trigger> _machine;
        StateMachine<State, Trigger>.TriggerWithParameters<string> _setNameTrigger;
        private IStoryRepository _storyRepository;
        private IUserRepository _userRepository;
        string _name;

        string _callee;

        public MainMenuStateMachine(IStoryRepository repository, IUserRepository userRepository)
        {
            PrepareMachine();
            _storyRepository = repository;
            _userRepository = userRepository;
        }

        public enum Trigger
        {
            Start,
            BackToMainMenu,
            Info,
            Stories,
            ChooseStory,
            //ContinueStory,
            GiveName,
            GivePrimaryAttribute,
            StoryCommand
        }

        public enum State
        {
            MainMenu,
            ListStory,
            CharacterCreation,
            ChooseName,
            ChoosePrimaryAttribute,
            InStory
        }

        private void PrepareMachine()
        {
            _machine = new StateMachine<State, Trigger>(() => _state, s => _state = s);

            //_setNameTrigger = _machine.SetTriggerParameters<string>(Trigger.GiveName);

            _machine.Configure(State.MainMenu)
                .Ignore(Trigger.BackToMainMenu)
                .InternalTransition(Trigger.Info, t => Console.WriteLine())
                .InternalTransition(Trigger.Start, t => Console.WriteLine())
                .Permit(Trigger.Stories, State.ListStory);

            _machine.Configure(State.ListStory)
                .Permit(Trigger.ChooseStory, State.ChooseName)
                .Permit(Trigger.BackToMainMenu, State.MainMenu);
                

            _machine.Configure(State.ChooseName)
                .Permit(Trigger.BackToMainMenu, State.MainMenu)
                .Permit(Trigger.GiveName, State.ChoosePrimaryAttribute);

            _machine.Configure(State.ChoosePrimaryAttribute)
                .Permit(Trigger.BackToMainMenu, State.MainMenu)
                .Permit(Trigger.GivePrimaryAttribute, State.InStory);

            _machine.Configure(State.InStory)
                .Permit(Trigger.BackToMainMenu, State.MainMenu)
                .InternalTransition(Trigger.StoryCommand, t => Console.WriteLine());

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

        public ResponseDataModel OnInfo()
        {
            _machine.Fire(Trigger.Info);
            return new ResponseDataModel { text = "Chatbot Beta version v0.4", quick_replies = new List<QuickReply> {
                new QuickReply{content_type = "text" , title =  "stories", payload = "RED"},
                new QuickReply{content_type = "text" , title =  "info", payload = "RED"},
            } };
        }
        public ResponseDataModel OnStories()
        {
            _machine.Fire(Trigger.Stories);
            var stories = _storyRepository.FindAllStory();
            string response = "Here is all the story what you can play:\n";
            var quickReply = new List<QuickReply>();

            for (int i = 0; i < stories.Count; i++)
            {
                response += $"{stories.ElementAt(i).Title}: \n {stories.ElementAt(i).Description}\n";
                quickReply.Add(new QuickReply {title = (i+1).ToString(), content_type = "text", payload = "RED" });
            }
            return new ResponseDataModel { text = response , quick_replies = quickReply};
        }

        public ResponseDataModel OnChoosingStory(User u, int index)
        {
            _machine.Fire(Trigger.ChooseStory);
            _storyRepository.CreateChosenStory(u, index-1);
            var storymsg =  "A story kiválasztva, válassz a karakterednek nevet. Ezt később nem változtathatod meg úgy válassz.";
            return new ResponseDataModel
            {
                text = storymsg,
                quick_replies = new List<QuickReply> {
                    new QuickReply{content_type = "text" , title =  "backtomenu", payload = "RED"},
                    new QuickReply{content_type = "text" , title =  "info", payload = "RED"},
                }
            };
        }

        public ResponseDataModel OnGivingName(User u, string name )
        {
            _machine.Fire(Trigger.GiveName);
            //_storyRepository.SetName(u, name);
            var storymsg = $"A név kiválasztva, {name}\nMost pedig elsődleges attribútumot kell választanod:\n 1. Strength\n 2. Agility\n 3. Intelligence";
            return new ResponseDataModel
            {
                text = storymsg,
                quick_replies = new List<QuickReply> {
                    new QuickReply{content_type = "text" , title =  "1", payload = "RED"},
                    new QuickReply{content_type = "text" , title =  "2", payload = "GREEN"},
                    new QuickReply{content_type = "text" , title =  "3", payload = "BLUE"},
                    new QuickReply{content_type = "text" , title =  "backtomenu", payload = "RED"},
                }
            };
        }
        public ResponseDataModel OnGivingPrimaryAttribute(User u, AttributeType attributeType)
        {
            _machine.Fire(Trigger.GivePrimaryAttribute);
            _storyRepository.SetPrimaryAttribute(u, attributeType);
            var storymsg = $"A kiválasztott attribútumod, {attributeType.ToString()}";
            return new ResponseDataModel
            {
                text = storymsg,
                quick_replies = new List<QuickReply> {
                    new QuickReply{content_type = "text" , title =  "backtomenu", payload = "RED"},
                }
            };
        }
        public void OnStoryCommand()
        {
            _machine.Fire(Trigger.StoryCommand);
        }
        public ResponseDataModel OnBackToMainMenu()
        {
            _machine.Fire(Trigger.BackToMainMenu);
           var msg =  "Welcome to the main menu!\nType info for version number";
            return new ResponseDataModel
            {
                text = msg,
                quick_replies = new List<QuickReply> {
                    new QuickReply{content_type = "text" , title =  "stories", payload = "RED"},
                    new QuickReply{content_type = "text" , title =  "info",  payload = "RED"},
                }
            };
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
