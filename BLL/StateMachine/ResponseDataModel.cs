using BLL.FacebookMessageHierarchy;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.StateMachine
{
    public class ResponseDataModel
    {
        public List<QuickReply> quick_replies { get; set; }
        public string text { get; set; }
    }
}
