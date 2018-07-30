using BLL.Context;
using BLL.FacebookMessageHierarchy;
using BLL.Models.Game;
using BLL.Repository;
using BLL.StateMachine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ChatBot.Handler
{
    public class RequestHandler
    {
        private IUserRepository _repository;
        private IServiceScopeFactory _scopeFactory;
        private MainMenuStateMachine _stateMachine = null;
        private ILogger _logger;

        public RequestHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public void DeleteUser(UInt64 id)
        {
            Task.Factory.StartNew(() =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    _repository = scope.ServiceProvider.GetService<IUserRepository>();
                    _repository.DeleteUser(id);
                }
            });
        }

        public async Task HandleRequest(BotRequest data)
        {
            await Task.Factory.StartNew(() =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    _repository = scope.ServiceProvider.GetService<IUserRepository>();
                    _stateMachine = scope.ServiceProvider.GetService<MainMenuStateMachine>();
                    _logger = scope.ServiceProvider.GetService<ILogger<RequestHandler>>();
                    RequestData(data);
                }
            });
        }

        private void RequestData(BotRequest data)
        {
            _logger.LogInformation("Begin data processing");
            foreach (var entry in data.entry)
            {
                _logger.LogInformation("Entry");
                foreach (var message in entry.messaging)
                {
                    _logger.LogInformation("Message");
                    if (string.IsNullOrWhiteSpace(message?.message?.text))
                        continue;
                    

                    User u = null;

                    u = _repository.FindUser(UInt64.Parse(message.sender.id));
                    if (u == null)
                    {
                        u = _repository.CreateUser(UInt64.Parse(message.sender.id), message.sender.id);
                    }
                    _stateMachine._state = u.SavedState;

                    var msg = GetResponseMessage(message, u);
                    u.SavedState = _stateMachine._state;
                    _repository.UpdateUser(u);

                    /*if (u != null)
                    {
                        PrepareMessage(msg, u.Facebook_id.ToString());
                    }
                    else
                    {
                        PrepareMessage(msg, message.sender.id);
                    }*/
                    
                }
            }
        }

        private string GetResponseMessage(BotMessageReceivedRequest message, User user)
        {
            switch (message.message.text.ToLower().Trim())
            {
                case Constants.INFO:
                    if (user.SavedState == MainMenuStateMachine.State.MainMenu)
                    {
                        return _stateMachine.OnInfo();
                    }
                    else
                    {
                        return "you are not in the correct state for this command";
                    }
                case Constants.LIST_STORIES:
                    if (user.SavedState == MainMenuStateMachine.State.MainMenu)
                    {
                        var responseData = _stateMachine.OnStories();
                        PrepareMessageWithQuickReply(responseData.text, responseData.quick_replies, user.Facebook_id.ToString());
                        return "";
                    }
                    else
                    {
                        return "you are not in the correct state for this command";
                    }
                case Constants.BACK_TO_MAIN_MENU:
                    return _stateMachine.OnBackToMainMenu();
                default:
                    return DecideStoryCommand(message, user);
            }
        }

        private string DecideStoryCommand(BotMessageReceivedRequest message, User user)
        {
            if (user.SavedState == MainMenuStateMachine.State.ListStory)
            {
                int index;
                if ( Int32.TryParse(message.message.text, out index) )
                {
                    return _stateMachine.OnChoosingStory(user, index).text;
                }
                else
                {
                    return "Létező id-t adj meg! (1-...)";
                }
            }
            return "";

        }

        private void PrepareMessageWithQuickReply(string message,List<QuickReply> quickReplies ,string facebook_id)
        {
            var msg = $"{message}\n id ({facebook_id})";
            var response = new BotMessageResponse
            {
                recipient = new BotUser
                {
                    id = facebook_id
                },
                message = new MessageResponse
                {
                    text = msg,
                    quick_replies = quickReplies
                },
            };
            var json = JsonConvert.SerializeObject(response);
            PostRaw("https://graph.facebook.com/v3.0/me/messages?access_token=EAADSmRksBeEBAOHMfZBAEHPLIo6UXugelborcpSJIaIxIfUIxd2fQzM6ADwwLOqWdJzlg8pUNZCWPB5ZAgomk1zdtmOil7L1hlOu1HLilOUrReg6ZCwQmhukZBqYN0YZBOWENQcZAw7RYRCpoC6EnjyYiLisEe9izvGhq7VdJtSuBHPaKaN3BnI", json);

        }

        private void PrepareMessage(string message, string facebook_id)
        {
            var msg = $"{message}\n id ({facebook_id})";
            var response = new BotMessageResponse
            {
                recipient = new BotUser
                {
                    id = facebook_id
                },
                message = new MessageResponse
                {
                    text = msg,
                    quick_replies = new List<QuickReply> { new QuickReply {content_type = "text", payload = "Red", title = "backtomenu" } }
                },
            };
            var json = JsonConvert.SerializeObject(response);
            PostRaw("https://graph.facebook.com/v3.0/me/messages?access_token=EAADSmRksBeEBAOHMfZBAEHPLIo6UXugelborcpSJIaIxIfUIxd2fQzM6ADwwLOqWdJzlg8pUNZCWPB5ZAgomk1zdtmOil7L1hlOu1HLilOUrReg6ZCwQmhukZBqYN0YZBOWENQcZAw7RYRCpoC6EnjyYiLisEe9izvGhq7VdJtSuBHPaKaN3BnI", json);
            
        }

        private string PostRaw(string url, string data)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "POST";
            using (var requestWriter = new StreamWriter(request.GetRequestStream()))
            {
                requestWriter.Write(data);
            }

            var response = (HttpWebResponse)request.GetResponse();
            if (response == null)
                throw new InvalidOperationException("GetResponse returns null");

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                return sr.ReadToEnd();
            }
        }

    }
}
