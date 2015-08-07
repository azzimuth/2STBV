using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace _2STBV.Web.Helpers
{
    public class _2STBVBotClient
    {
        private string _2STBVBotUrl;

        public _2STBVBotClient()
        {
            _2STBVBotUrl = ConfigurationManager.AppSettings["2STBVBotUrl"];
        }

        public async Task<string> GetVerificationToken(string userId)
        {
            string token;

            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new Dictionary<string, string> { { "userId", userId } });

                var response = await client.PostAsync(_2STBVBotUrl + "/Verification/GetToken", content);

                token = await response.Content.ReadAsStringAsync();
            }

            return token;
        }

        public async Task<bool> VerifyVerificationCode(string userId, string code)
        {
            bool status;

            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new Dictionary<string, string> { { "userId", userId }, { "code", code } });

                var response = await client.PostAsync(_2STBVBotUrl + "/Verification/Verify", content);

                var result = await response.Content.ReadAsStringAsync();

                status = bool.Parse(result);
            }

            return status;
        }
    }
}