using _2STBV.Common.Classes;
using _2STBV.Util;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace _2STBV.Controllers
{
    public class AuthenticationController : ApiController
    {
        private string _telegramApiUrl;

        public AuthenticationController()
        {
            _telegramApiUrl = ConfigurationManager.AppSettings.Get("TelegramApiUrl");
        }

        public HttpResponseMessage EnqueueOTP(string token, string OTP, [FromBody]User user)
        {
            var sendMessageUrl = _telegramApiUrl + "/sendMessage";

            if (!RequestStorage.Storage.ContainsKey(user.id)) RequestStorage.Storage.Add(user.id, OTP);
            
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
