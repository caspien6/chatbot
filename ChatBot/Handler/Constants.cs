using BLL.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBot.Handler
{
    public static class Constants
    {
        public const string INFO = "info";
        public const string BACK_TO_MAIN_MENU = "backtomenu";
        public const string LIST_STORIES = "stories";
        public const string START = "start";
        public const string CHOOSE_STORY = "choosestory";
        public const string GIVE_NAME = "name";
        public const string GIVE_PRIMARY_ATTRIBUTE = "attribute";

        public static ResponseDataModel DEFAULT_ERROR = new ResponseDataModel { text = "you are not in the correct state for this command" };
        public static ResponseDataModel NOT_AVAILABLE_ERROR = new ResponseDataModel { text = "Létező id-t adj meg! (1-...)" };
        

    }
}
