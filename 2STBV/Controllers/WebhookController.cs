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

            var sendMessageUrl = _telegramApiUrl + "/sendMessage";
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>();

                if (RequestStorage.Storage.ContainsKey(update.message.from.id))
                {
                    string storedOTP;
                    RequestStorage.Storage.TryGetValue(update.message.from.id, out storedOTP);

                    string[] textTokens = update.message.text.Split(' ');

                    if (!textTokens[0].Equals("/auth")) return new HttpResponseMessage(HttpStatusCode.OK);

                    if (storedOTP.Equals(textTokens[1]))
                    {
                        values = new Dictionary<string, string>{
                           { "chat_id", update.message.from.id.ToString() },
                           { "text", "Authentication successful!" },
                           { "reply_to_message_id", update.message.message_id.ToString() }
                        };

                        var content = new FormUrlEncodedContent(values);

                        var response = await client.PostAsync(sendMessageUrl, content);

                        var responseString = await response.Content.ReadAsStringAsync();

                        try
                        {
                            var message = JsonConvert.DeserializeObject<Message>(responseString);
                            RequestStorage.Storage.Remove(update.message.from.id);
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        values = new Dictionary<string, string>{
                           { "chat_id", update.message.from.id.ToString() },
                           { "text", "Authentication failed. Please try again." },
                           { "reply_to_message_id", update.message.message_id.ToString() }
                        };

                        var content = new FormUrlEncodedContent(values);

                        var response = await client.PostAsync(sendMessageUrl, content);

                        var responseString = await response.Content.ReadAsStringAsync();
                    }
                }
                else
                {
                    values = new Dictionary<string, string>{
                           { "chat_id", update.message.from.id.ToString() },
                           { "text", "Sorry but I am not expecting any messages from you." },
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
