using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ChatBot.Handler;
using ChatBot.Models.FacebookMessageHierarchy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacebookBotController : ControllerBase
    {
        private RequestHandler _handler;

        public FacebookBotController(RequestHandler handler)
        {
            _handler = handler;
        }

        [HttpGet]
        public async Task<ActionResult> Receive()
        {
            var query = Request.Query;

            //_logWriter.WriteLine(Request.RawUrl);

            if (query["hub.verify_token"] == "what_kind_of_url")
            {
                //string type = Request.QueryString["type"];
                var retVal = query["hub.challenge"];
                return Ok(retVal.ToString()) ;
            }
            else
            {
                return NotFound("Shits on fire");
            }
        }
        
        [ActionName("Receive")]
        [HttpPost]
        public async Task<ActionResult> ReceivePost(BotRequest data)
        {
            
            try
            {
                _handler.HandleRequest(data);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }


            return Ok();
        }
        
        

    }
}