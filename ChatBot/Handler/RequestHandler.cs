using BLL.Context;
using BLL.Models.Game;
using BLL.Repository;
using ChatBot.Models.FacebookMessageHierarchy;
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

        public RequestHandler()
        {
           
        }

        public void HandleRequest(BotRequest data)
        {
            Task.Factory.StartNew(() =>
            {
                var fact = new StoryContextFactory();
                _repository = new UserRepository(fact.CreateDbContext(new string[2]));
                RequestData(data);
            });
        }

        private void RequestData(BotRequest data)
        {
            foreach (var entry in data.entry)
            {
                foreach (var message in entry.messaging)
                {
                    if (string.IsNullOrWhiteSpace(message?.message?.text))
                        continue;

                    User u = null;

                    u = _repository.FindUser(UInt64.Parse(message.sender.id));
                    if (u == null)
                    {
                        u = _repository.CreateUser(UInt64.Parse(message.sender.id), message.sender.id);
                    }




                    if (u != null)
                    {
                        var msg = $"You said: {message.message.text} with this id: {u.Facebook_id}";
                        var response = new BotMessageResponse
                        {
                            recipient = new BotUser
                            {
                                id = u.Facebook_id.ToString()
                            },
                            message = new MessageResponse
                            {
                                text = msg
                            }
                        };
                        var json = JsonConvert.SerializeObject(response);
                        PostRaw("https://graph.facebook.com/v3.0/me/messages?access_token=EAADSmRksBeEBAOHMfZBAEHPLIo6UXugelborcpSJIaIxIfUIxd2fQzM6ADwwLOqWdJzlg8pUNZCWPB5ZAgomk1zdtmOil7L1hlOu1HLilOUrReg6ZCwQmhukZBqYN0YZBOWENQcZAw7RYRCpoC6EnjyYiLisEe9izvGhq7VdJtSuBHPaKaN3BnI", json);
                    }
                    else
                    {
                        var msg = $"You said: {message.message.text} with this id: {message.sender.id}";
                        var response = new BotMessageResponse
                        {
                            recipient = new BotUser
                            {
                                id = message.sender.id
                            },
                            message = new MessageResponse
                            {
                                text = msg
                            }
                        };
                        var json = JsonConvert.SerializeObject(response);
                        PostRaw("https://graph.facebook.com/v3.0/me/messages?access_token=EAADSmRksBeEBAOHMfZBAEHPLIo6UXugelborcpSJIaIxIfUIxd2fQzM6ADwwLOqWdJzlg8pUNZCWPB5ZAgomk1zdtmOil7L1hlOu1HLilOUrReg6ZCwQmhukZBqYN0YZBOWENQcZAw7RYRCpoC6EnjyYiLisEe9izvGhq7VdJtSuBHPaKaN3BnI", json);
                    }
                }
            }
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
