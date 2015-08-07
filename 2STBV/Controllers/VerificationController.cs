using _2STBV.Common.DataAccess;
using System;
using System.Web.Http;
using System.Linq;

namespace _2STBV.Bot.Controllers
{
    public class VerificationController : ApiController
    {
        // /Verification/GetToken
        public string GetToken(string userId)
        {
            var token = Guid.NewGuid().ToString("N");
            //var code = Guid.NewGuid().ToString("N").Substring(0, 5);
            using (var context = new _2STBVContext())
            {
                var userTelegramAccount = new UserTelegramAccount { UserId = userId, VerificationToken = token, Verified = false };

                context.UserTelegramAccounts.Add(userTelegramAccount);

                context.SaveChanges();
            }

            return token;
        }

        public bool VerifyVerificationCode(string userId, string code)
        {
            var codeIsValid = false;
            using (var context = new _2STBVContext())
            {
                var userTelegramAccount = (from account in context.UserTelegramAccounts
                                           where account.UserId.Equals(userId)
                                           select account).FirstOrDefault();
                if (userTelegramAccount != null)
                {
                    codeIsValid = userTelegramAccount.VerificationCode.Equals(code) && DateTime.Now > userTelegramAccount.VerificationCodeExpiration;

                    if (codeIsValid)
                    {
                        userTelegramAccount.Verified = true;
                        context.Entry(userTelegramAccount).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                    }
                }
            }

            return codeIsValid;
        }
    }
}
