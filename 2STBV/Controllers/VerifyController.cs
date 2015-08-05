using _2STBV.Common.DataAccess;
using System;
using System.Web.Http;
using System.Linq;

namespace _2STBV.Bot.Controllers
{
    public class VerifyController : ApiController
    {
        // /Verification/GetToken
        public string GetToken(string userId)
        {
            var token = Guid.NewGuid().ToString("N");
            var code = Guid.NewGuid().ToString("N").Substring(0, 5);

            using (var context = new _2STBVContext())
            {
                var userTelegramAccount = new UserTelegramAccount { UserId = userId, VerificationToken = token, VerificationCode = code, Verified = false };

                context.UserTelegramAccounts.Add(userTelegramAccount);

                context.SaveChanges();
            }

            return token;
        }

        public bool VerifyVerificationCode(string userId, string code)
        {
            var codeEquals = false;
            using (var context = new _2STBVContext())
            {
                var userTelegramAccount = (from account in context.UserTelegramAccounts
                                           where account.UserId.Equals(userId)
                                           select account).FirstOrDefault();
                if (userTelegramAccount != null)
                {
                    codeEquals = userTelegramAccount.VerificationCode.Equals(code);

                    if (codeEquals)
                    {
                        userTelegramAccount.Verified = true;
                        userTelegramAccount.VerifiedOn = DateTime.Now;

                        context.Entry(userTelegramAccount).State = System.Data.Entity.EntityState.Modified;

                        context.SaveChanges();
                    }
                }
            }

            return codeEquals;
        }
    }
}
