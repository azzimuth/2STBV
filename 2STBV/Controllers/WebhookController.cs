using _2STBV.Common.Classes;
using _2STBV.Util;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace _2STBV.Controllers
{
    public class WebhookController : ApiController
    {
        private ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string _telegramApiUrl;

        public WebhookController()
        {
            _telegramApiUrl = ConfigurationManager.AppSettings.Get("TelegramApiUrl");
        }

        [ResponseType(typeof(Message))]
        public async Task<HttpResponseMessage> Post(string token, [FromBody] Update update)
        {
            var secret = ConfigurationManager.AppSettings.Get("TelegramSecurityToken");

            if (!secret.Equals(token))
            {
                _log.ErrorFormat("Intrusion attempt! Got token id {0}", token);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            
            string[] textTokens = update.message.text.Split(' ');
            
            //return alwasy HTTP200 so that Telegram doesn't retry sending the same request again
            if (!textTokens[0].StartsWith("/")) return new HttpResponseMessage(HttpStatusCode.OK);
            
            var sendMessageUrl = _telegramApiUrl + "/sendMessage";
            var values = new Dictionary<string, string>();
            
            switch(textTokens[0]){
                case ("/start"):
                    using (var context = new _2STBVContext(), var client = new HttpClient())
	                {
	                    var userTelegramAccount = (from account in context.UserTelegramAccounts
								   where account.VerificationToken.Equals(textTokens[1])
								   select account).FirstOrDefault();
						if (userTelegramAccount != null)
		                {
		                    userTelegramAccount.VerificationCode = Guid.NewGuid().ToString("N").Substring(0,5);
		                    userTelegramAccount.VerificationCodeExpiration = DateTime.Now.AddMinutes(10);
		                    userTelegramAccount.TelegramUserId = update.message.from.id;
		                    context.Entry(userTelegramAccount).State = System.Data.Entity.EntityState.Modified;
				            context.SaveChanges();
		                }
		                values = new Dictionary<string, string>{
                           { "chat_id", update.message.from.id.ToString() },
                           { "text", "Your verification code: " + userTelegramAccount.VerificationCode },
                           { "reply_to_message_id", update.message.message_id.ToString() }
                        };

                        var content = new FormUrlEncodedContent(values);

                        var response = await client.PostAsync(sendMessageUrl, content);

                        var responseString = await response.Content.ReadAsStringAsync();
	                }
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
