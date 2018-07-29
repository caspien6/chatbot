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
using Microsoft.Extensions.Logging;

namespace ChatBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacebookBotController : ControllerBase
    {
        private RequestHandler _handler;
        private ILogger logger;

        public FacebookBotController(RequestHandler handler, ILogger<FacebookBotController> logger)
        {
            _handler = handler;
            this.logger = logger;
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
                await _handler.HandleRequest(data);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                logger.LogError(e.StackTrace);
                return NotFound(e.Message);
            }


            return Ok();
        }

        [ActionName("DeleteUser")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(UInt64 id)
        {
            _handler.DeleteUser(id);
            return Ok();
        }



    }
}